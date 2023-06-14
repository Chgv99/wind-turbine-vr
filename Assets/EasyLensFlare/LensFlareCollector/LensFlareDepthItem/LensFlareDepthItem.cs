using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GR
{
    /// <summary>
    /// Judging occlusion through depth information
    /// </summary>
    public class LensFlareDepthItem : LensFlareCollectorBase
    {
        public float depthTextureFadeEps = 0.003f;

        int mCacheDepthTexFrameCount;
        Texture2D mCacheDepthTex2D;
        Material mGetDepthMat;


        protected override void OnEnable()
        {
            base.OnEnable();

            mGetDepthMat = new Material(Shader.Find("Hidden/LensFlareDepthItem_GetDepthShader"));

            CacheMainCamera.depthTextureMode |= DepthTextureMode.Depth;

            LensFlare_RenderImageCallbackController.Instance.RegistCallback(ELensFlareRenderImageCallbackOrder.RenderImage,
              OnRenderImageCallback);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            LensFlare_RenderImageCallbackController.Instance.UnregistCallback(ELensFlareRenderImageCallbackOrder.RenderImage,
                OnRenderImageCallback);

            if (LensFlareDrawController.Instance && mLensFlareData != null)
                LensFlareDrawController.Instance.UnregistLensFlareData(mLensFlareData);

            if (mCacheDepthTex2D != null)
            {
                UnityEngine.Object.Destroy(mCacheDepthTex2D);
                mCacheDepthTex2D = null;
            }
        }

        void OnRenderImageCallback(LensFlareRenderImageContext context, RenderTexture src, RenderTexture dst)
        {
            if (!base.MainCameraFrustumPlanesCheck())
            {
                mLensFlareData.Intensity = 0f;
                return;
            }

            if (base.AngleLimitCheck())
            {
                mLensFlareData.Intensity = 0f;
                return;
            }

            if (mCacheDepthTexFrameCount != Time.frameCount)
            {
                var cacheRenderTextureActive = RenderTexture.active;
                var depthRT = RenderTexture.GetTemporary(src.descriptor);
                Graphics.Blit(src, depthRT, mGetDepthMat);

                if (mCacheDepthTex2D == null)
                    mCacheDepthTex2D = new Texture2D(depthRT.width, depthRT.height);
                mCacheDepthTex2D.ReadPixels(new Rect(0, 0, depthRT.width, depthRT.height), 0, 0);
                mCacheDepthTex2D.Apply();
                RenderTexture.ReleaseTemporary(depthRT);
                RenderTexture.active = cacheRenderTextureActive;
                mCacheDepthTexFrameCount = Time.frameCount;
            }

            var viewPortPos = CacheMainCamera.WorldToViewportPoint(transform.position, Camera.MonoOrStereoscopicEye.Mono);
            viewPortPos.z /= CacheMainCamera.farClipPlane;

            var depthTexCol = mCacheDepthTex2D.GetPixel(Mathf.FloorToInt(viewPortPos.x * mCacheDepthTex2D.width)
                , Mathf.FloorToInt(viewPortPos.y * mCacheDepthTex2D.height));

            if (viewPortPos.z - depthTexCol.a > depthTextureFadeEps)
            {
                mLensFlareData.Intensity = Mathf.Lerp(mLensFlareData.Intensity, 0f, base.fadeSpeedMultiple * Time.deltaTime);
            }
            else
            {
                mLensFlareData.Intensity = Mathf.Lerp(mLensFlareData.Intensity, 1f, base.showSpeedMultiple * Time.deltaTime);
            }
            var distanceAttenuation = base.DistanceAttenuationProcess(mLensFlareData);
            base.IntensityProcess(mLensFlareData, distanceAttenuation);
            base.ScaleProcess(mLensFlareData, distanceAttenuation);
        }
    }
}
