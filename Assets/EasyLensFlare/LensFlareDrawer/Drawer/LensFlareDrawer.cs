using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GR
{
    public class LensFlareDrawer
    {
        float CAMERA_OFFSET = 0.3f;

        LensFlareElementGroup[] mLensFlareElementGroupArray;
        List<LensFlareData> mLensFlareDataList;

        Transform mQuadBakerRootTransform;

        List<QuadBakerContainer> mQuadBakerContainerList;

        Camera mCacheMainCamera;

        bool mCacheNearclipValueIsInit;
        float mCacheNearclipWidth;
        float mCacheNearclipHeight;

        CommandBuffer mDrawMeshCommandBuffer;

        Camera CacheMainCamera
        {
            get
            {
                if (mCacheMainCamera == null)
                    mCacheMainCamera = Camera.main;

                return mCacheMainCamera;
            }
        }


        public void Initialization(List<LensFlareData> lensFlareDataList, LensFlareElementGroupConfigurator[] configuratorArray)
        {
            mQuadBakerContainerList = new List<QuadBakerContainer>(32);

            mLensFlareDataList = lensFlareDataList;

            mLensFlareElementGroupArray = new LensFlareElementGroup[configuratorArray.Length];
            for (int i = 0; i < mLensFlareElementGroupArray.Length; i++)
                mLensFlareElementGroupArray[i] = new LensFlareElementGroup(configuratorArray[i]);

            mDrawMeshCommandBuffer = new CommandBuffer();

            InitAtlasLensFlare();

            LensFlare_RenderImageCallbackController.Instance.RegistCallback(ELensFlareRenderImageCallbackOrder.RenderImageLate,
                OnRenderImageCallback);
        }

        public void Release()
        {
            LensFlare_RenderImageCallbackController.Instance.UnregistCallback(ELensFlareRenderImageCallbackOrder.RenderImageLate,
                OnRenderImageCallback);

            if (mQuadBakerRootTransform)
                UnityEngine.Object.Destroy(mQuadBakerRootTransform.gameObject);

            mLensFlareElementGroupArray = null;

            mQuadBakerContainerList.Clear();

            if (mDrawMeshCommandBuffer != null)
            {
                mDrawMeshCommandBuffer.Release();
                mDrawMeshCommandBuffer = null;
            }

            mCacheNearclipValueIsInit = false;
        }

        void UpdateQuads()
        {
            if (!mCacheNearclipValueIsInit)
            {
                var p0 = ConvertPosition(new Vector3(0f, 0f, 0f));
                var p1 = ConvertPosition(new Vector3(1f, 0f, 0f));
                var p2 = ConvertPosition(new Vector3(0f, 1f, 0f));

                mCacheNearclipWidth = Vector3.Distance(p0, p1);
                mCacheNearclipHeight = Vector3.Distance(p0, p2);

                mCacheNearclipValueIsInit = true;
            }

            mDrawMeshCommandBuffer.Clear();

            UpdateAtlasLensFlare();
        }

        void InitAtlasLensFlare()
        {
            mQuadBakerRootTransform = new GameObject("LensFlare_QuadBaker_Root(Dynamic)").transform;
            mQuadBakerRootTransform.localPosition = new Vector3(0f, 0f, CacheMainCamera.nearClipPlane + CAMERA_OFFSET);
            mQuadBakerRootTransform.localRotation = Quaternion.identity;
            mQuadBakerRootTransform.localScale = Vector3.one;

            for (int i = 0; i < mLensFlareElementGroupArray.Length; i++)
            {
                var lensFlareElementGroup = mLensFlareElementGroupArray[i];

                var quadBakerContainerGO = new GameObject("QuadBakerContainer_" + lensFlareElementGroup.Configurator.name);
                quadBakerContainerGO.transform.SetParent(mQuadBakerRootTransform);
                quadBakerContainerGO.AddComponent<MeshFilter>();
                var meshRenderer = quadBakerContainerGO.AddComponent<MeshRenderer>();
                meshRenderer.material = new Material(Shader.Find("Hidden/LensFlareDrawerQuadShader"));
                meshRenderer.material.color = Color.white;
                meshRenderer.material.SetTexture("_MainTex", lensFlareElementGroup.Configurator.atlasMainTexture);
                meshRenderer.enabled = false;

                var quadBakerContainer = quadBakerContainerGO.AddComponent<QuadBakerContainer>();

                quadBakerContainer.updateOrder = QuadBakerContainer.EUpdateOrder.Manual;

                lensFlareElementGroup.Pool = new LensFlare_SimplePool<VirtualQuadAgent>(32, () =>
                {
                    var quadBakerAgentGO = new GameObject("QuadBakerAgent");
                    quadBakerAgentGO.transform.SetParent(mQuadBakerRootTransform);
                    quadBakerAgentGO.transform.localPosition = Vector3.zero;
                    quadBakerAgentGO.transform.localRotation = Quaternion.identity;
                    quadBakerAgentGO.transform.localScale = Vector3.zero;

                    var quadBakerAgent = quadBakerAgentGO.AddComponent<VirtualQuadAgent>();
                    quadBakerAgent.quadBakerContainer = quadBakerContainer;

                    quadBakerAgent.updateMode = VirtualQuadAgent.EUpdateMode.Manual;

                    quadBakerAgent.Initialization();
                    quadBakerAgent.vertexColor = Color.clear;

                    return quadBakerAgent;
                });
                lensFlareElementGroup.PoolTakedList = new List<VirtualQuadAgent>(32);

                mQuadBakerContainerList.Add(quadBakerContainer);
            }
        }

        void UpdateAtlasLensFlare()
        {
            for (int i = 0, iMax = mLensFlareElementGroupArray.Length; i < iMax; i++)
            {
                var lensFlareElementGroup = mLensFlareElementGroupArray[i];

                lensFlareElementGroup.LensDirtyTotalExpectIntensity = 0f;

                for (int j = 0, jMax = lensFlareElementGroup.PoolTakedList.Count; j < jMax; j++)
                {
                    var poolTakedItem = lensFlareElementGroup.PoolTakedList[j];

                    poolTakedItem.vertexColor = Color.clear;
                    poolTakedItem.transform.localScale = Vector3.zero;
                    poolTakedItem.ManualUpdate();

                    lensFlareElementGroup.Pool.Despawn(poolTakedItem);
                }
                lensFlareElementGroup.PoolTakedList.Clear();
            }

            for (int i = 0; i < mLensFlareDataList.Count; i++)
            {
                var lensFlareData = mLensFlareDataList[i];

                if (lensFlareData.CacheElementGroup == null)
                {
                    for (int groupArrayIndex = 0; groupArrayIndex < mLensFlareElementGroupArray.Length; groupArrayIndex++)
                    {
                        var currentGroupItem = mLensFlareElementGroupArray[groupArrayIndex];

                        if (currentGroupItem.Configurator.id == lensFlareData.ElementConfiguratorID)
                        {
                            lensFlareData.CacheElementGroup = currentGroupItem;
                            break;
                        }
                    }
                }

                var cacheElementGroup = lensFlareData.CacheElementGroup;
                var elementConfigurator = cacheElementGroup.Configurator;

                var imageElementConfInfoList = elementConfigurator.atlasDocData.imageElementInfoList;
                var elementAttachConfInfoArray = elementConfigurator.elementAttachInfoArray;

                cacheElementGroup.LensDirtyTotalExpectIntensity += lensFlareData.Intensity * elementConfigurator.lensDirtyContributeMultiplePerLensData;

                var lensDirtyImageName = elementConfigurator.lensDirtyImageName;

                for (int imageElementIndex = 0, imageElementMax = imageElementConfInfoList.Count
                    ; imageElementIndex < imageElementMax; imageElementIndex++)
                {
                    var imageElementItem = imageElementConfInfoList[imageElementIndex];

                    var attachInfo = elementAttachConfInfoArray[imageElementIndex];
                    if (attachInfo.elementName == lensDirtyImageName)
                        continue;

                    var virtualQuadAgent = lensFlareData.CacheElementGroup.Pool.Spawn();

                    VirtualQuadAgentUVMapping(virtualQuadAgent, imageElementItem.rectInfo);
                    //uv set.

                    var positionOffset = new Vector2(attachInfo.offsetScale.x, attachInfo.offsetScale.y);
                    var scaleOffset = new Vector2(attachInfo.offsetScale.z, attachInfo.offsetScale.w);
                    var elementPosition = LensFlareUtil.GetElementPositionReferenceByOffsetWeight(
                        lensFlareData.GetViewportPosition() + positionOffset
                        , new Vector2(0.5f, 0.5f), attachInfo.offsetWeight);

                    virtualQuadAgent.vertexColor = Color.white * lensFlareData.Intensity
                       * attachInfo.intensity * elementConfigurator.lensFlareGlobalIntensityMultiple;
                    //default color * dynamic intensity * static intensity * global static intensity.

                    virtualQuadAgent.transform.position = ConvertPosition(elementPosition);
                    virtualQuadAgent.transform.LookAt(CacheMainCamera.transform.position, CacheMainCamera.transform.up);
                    virtualQuadAgent.transform.localScale = new Vector3(1f * scaleOffset.x, 1f * scaleOffset.y, 1f) * lensFlareData.ScaleMultiple;
                    //position set.

                    virtualQuadAgent.ManualUpdate();

                    lensFlareData.CacheElementGroup.PoolTakedList.Add(virtualQuadAgent);
                }
            }

            for (int i = 0, iMax = mLensFlareElementGroupArray.Length; i < iMax; i++)
            {
                var lensFlareElementGroup = mLensFlareElementGroupArray[i];
                var configurator = lensFlareElementGroup.Configurator;

                if (lensFlareElementGroup.LensDirtyTotalExpectIntensity > lensFlareElementGroup.LensDirtyTotalIntensity)
                {
                    lensFlareElementGroup.LensDirtyTotalIntensity = Mathf.Lerp(lensFlareElementGroup.LensDirtyTotalIntensity
                        , lensFlareElementGroup.LensDirtyTotalExpectIntensity, Time.deltaTime * configurator.lensDirtyShowTweenSpeed);
                }
                else
                {
                    lensFlareElementGroup.LensDirtyTotalIntensity = Mathf.Lerp(lensFlareElementGroup.LensDirtyTotalIntensity
                       , lensFlareElementGroup.LensDirtyTotalExpectIntensity, Time.deltaTime * configurator.lensDirtyFadeTweenSpeed);
                }

                if (lensFlareElementGroup.LensDirtyTotalIntensity > 0f)
                {
                    var lensDirtyImageName = configurator.lensDirtyImageName;
                    if (!string.IsNullOrEmpty(lensDirtyImageName))
                    {
                        var elementAttachInfoArray = lensFlareElementGroup.Configurator.elementAttachInfoArray;
                        var lensDirtyTextureIndex = -1;
                        for (int attachInfoIndex = 0; attachInfoIndex < elementAttachInfoArray.Length; attachInfoIndex++)
                        {
                            var elementAttachInfo = elementAttachInfoArray[attachInfoIndex];

                            if (elementAttachInfo.elementName == lensDirtyImageName)
                            {
                                lensDirtyTextureIndex = attachInfoIndex;
                                break;
                            }
                        }

                        var lensDirtyConfInfo = configurator.atlasDocData.imageElementInfoList[lensDirtyTextureIndex];

                        var lensDirtyVirtualQuadAgent = lensFlareElementGroup.Pool.Spawn();

                        VirtualQuadAgentUVMapping(lensDirtyVirtualQuadAgent, lensDirtyConfInfo.rectInfo);

                        lensDirtyVirtualQuadAgent.vertexColor = Color.white * lensFlareElementGroup.LensDirtyTotalIntensity
                            * configurator.lensDirtyGlobalIntensityMultiple;

                        lensDirtyVirtualQuadAgent.transform.position = ConvertPosition(new Vector3(0.5f, 0.5f));
                        lensDirtyVirtualQuadAgent.transform.LookAt(CacheMainCamera.transform.position, CacheMainCamera.transform.up);
                        lensDirtyVirtualQuadAgent.transform.localScale = new Vector3(mCacheNearclipWidth, mCacheNearclipHeight, 1f);

                        lensDirtyVirtualQuadAgent.ManualUpdate();

                        lensFlareElementGroup.PoolTakedList.Add(lensDirtyVirtualQuadAgent);
                    }
                }
            }

            for (int i = 0, iMax = mQuadBakerContainerList.Count; i < iMax; i++)
            {
                var quadBakerContainerItem = mQuadBakerContainerList[i];
                quadBakerContainerItem.ManualUpdateQuadBaker();
                mDrawMeshCommandBuffer.DrawMesh(quadBakerContainerItem.QuadBaker.Mesh
                    , quadBakerContainerItem.transform.localToWorldMatrix
                    , quadBakerContainerItem.MeshRenderer.sharedMaterial);
            }
        }

        void OnRenderImageCallback(LensFlareRenderImageContext context, RenderTexture src, RenderTexture dst)
        {
            UpdateQuads();

            if (!context.GraphicsBlitFlag)
            {
                Graphics.Blit(src, dst);
                context.GraphicsBlitFlag = true;
            }

            Graphics.ExecuteCommandBuffer(mDrawMeshCommandBuffer);
        }

        Vector3 ConvertPosition(Vector3 viewportPoint)
        {
            var nearClipOffset = CacheMainCamera.nearClipPlane + CAMERA_OFFSET;
            return CacheMainCamera.ViewportToWorldPoint(new Vector3(viewportPoint.x, viewportPoint.y, nearClipOffset));
        }

        void VirtualQuadAgentUVMapping(VirtualQuadAgent agent, Rect rect)
        {
            agent.uvP1 = new Vector2(rect.x, 1 - (rect.y + rect.height));
            agent.uvP2 = new Vector2(rect.x + rect.width, 1 - (rect.y + rect.height));
            agent.uvP3 = new Vector2(rect.x, 1 - rect.y);
            agent.uvP4 = new Vector2(rect.x + rect.width, 1 - rect.y);
        }
    }
}
