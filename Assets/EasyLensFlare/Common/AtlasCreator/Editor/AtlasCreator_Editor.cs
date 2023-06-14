using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace GR
{
    public class AtlasCreator_Editor : EditorWindow
    {
        static EditorWindow mWindowHandle;

        AtlasCreator_View mView;
        AtlasCreator_DocData mCurrentDocData;
        AtlasCreator_DocData.ImageElementInfo mCurrentSelectionImageElement;


        [MenuItem("Tools/GR/LensFlare Atlas Tool")]
        public static void Setup()
        {
            if (mWindowHandle != null)
            {
                mWindowHandle.Close();
                return;
            }
            mWindowHandle = GetWindow(typeof(AtlasCreator_Editor));
            mWindowHandle.titleContent = new GUIContent("LensFlare Atlas Tool");
        }

        void OnGUI()
        {
            if (mView == null)
            {
                mCurrentDocData = ScriptableObject.CreateInstance<AtlasCreator_DocData>();
                mCurrentDocData.imageElementInfoList = new List<AtlasCreator_DocData.ImageElementInfo>(32);

                mCurrentDocData.resolution = new Vector2Int(1024, 1024);

                mView = new AtlasCreator_View();
                mView.OnImageElementPanelDrop = tex =>
                {
                    mCurrentDocData.imageElementInfoList.Add(new AtlasCreator_DocData.ImageElementInfo()
                    {
                        rectInfo = new Rect(0f, 0f, 0.2f, 0.2f),
                        linkedTexture = tex,
                    });

                    UpdateImageElementVO(mCurrentDocData);
                    UpdateAtlasVO(mCurrentDocData);
                    UpdateImageArgumentVO(mCurrentDocData, 0, 0, 0, 0);
                };

                mView.UpdateImageMenuVO(new AtlasCreator_ImageMenuVO()
                {
                    OnBuildBtnClicked = this.OnBuildBtnClicked,
                    BuildResolutionW = mCurrentDocData.resolution.x,
                    BuildResolutionH = mCurrentDocData.resolution.y,
                    OnBuildResolutionParamUpdate = OnBuildResolutionParamUpdate,
                    OnNewBtnClicked = this.OnNewBtnClicked,
                    OnSaveBtnClicked = this.OnSaveBtnClicked,
                    OnLoadBtnClicked = this.OnLoadBtnClicked,
                });
            }

            mView.DrawGUI();
        }

        void OnBuildBtnClicked()
        {
            var path = EditorUtility.SaveFolderPanel("Build", "", "LensFlareAtlas");

            var docPath = System.IO.Path.Combine(path, "LensFlareDoc.asset");
            if (File.Exists(docPath))
                File.Delete(docPath);
            var relDocPath = GetRelativeAssetPath(docPath);
            AssetDatabase.CreateAsset(mCurrentDocData, relDocPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var builder = CreateTypeInstanceForInterfaces<IAtlasCreator_Builder>();
            if (builder.Length == 1 && builder[0] is DefaultAtlasCreator_Builder)
            {
                builder[0].BuildToDisk(path, mCurrentDocData);
            }
            else
            {
                var targetBuilder = builder.FirstOrDefault(m => !(m is DefaultAtlasCreator_Builder));
                if (targetBuilder != null)
                    targetBuilder.BuildToDisk(path, mCurrentDocData);
            }
        }

        void OnNewBtnClicked()
        {
            mCurrentDocData = ScriptableObject.CreateInstance<AtlasCreator_DocData>();
            mCurrentDocData.imageElementInfoList = new List<AtlasCreator_DocData.ImageElementInfo>(32);
            mView.LoadPathObject = null;

            UpdateImageElementVO(mCurrentDocData);
            UpdateAtlasVO(mCurrentDocData);
            UpdateImageArgumentVO(mCurrentDocData, 0, 0, 0, 0);
        }

        void OnSaveBtnClicked()
        {
            var path = "";
            var isExist = false;
            if (mView.LoadPathObject != null && EditorUtility.DisplayDialog("Tip", "You sure use current object to save?", "ok"))
            {
                path = AssetDatabase.GetAssetPath(mView.LoadPathObject);
                isExist = true;
            }
            else
            {
                path = EditorUtility.SaveFilePanel("Save Config", "", "LensFlareAtlas", "asset");
            }

            if (!string.IsNullOrEmpty(path))
            {
                path = GetRelativeAssetPath(path);
                if (isExist)
                    AssetDatabase.DeleteAsset(path);

                AssetDatabase.CreateAsset(mCurrentDocData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        void OnLoadBtnClicked()
        {
            var path = "";
            if (mView.LoadPathObject != null)
            {
                path = AssetDatabase.GetAssetPath(mView.LoadPathObject);
            }
            else
            {
                path = EditorUtility.OpenFilePanel("Load Config", "", "asset");
            }

            mCurrentDocData = Instantiate(AssetDatabase.LoadAssetAtPath<AtlasCreator_DocData>(path));
            UpdateImageElementVO(mCurrentDocData);
            UpdateAtlasVO(mCurrentDocData);
            UpdateImageArgumentVO(mCurrentDocData, 0, 0, 0, 0);
        }

        void OnImageElementLabelClicked(AtlasCreator_ImageElementVO sender)
        {
            mCurrentSelectionImageElement = sender.UserData as AtlasCreator_DocData.ImageElementInfo;

            var rectInfo = mCurrentSelectionImageElement.rectInfo;
            UpdateImageArgumentVO(mCurrentDocData, rectInfo.x, rectInfo.y, rectInfo.width, rectInfo.height);
            UpdateImageElementVO(mCurrentDocData);
            UpdateAtlasVO(mCurrentDocData);
        }

        void OnImageElementLabelDelClicked(AtlasCreator_ImageElementVO sender)
        {
            var delItem = sender.UserData as AtlasCreator_DocData.ImageElementInfo;

            mCurrentDocData.imageElementInfoList.Remove(delItem);
            UpdateImageElementVO(mCurrentDocData);
            UpdateAtlasVO(mCurrentDocData);
        }

        void UpdateAtlasVO(AtlasCreator_DocData docData)
        {
            mView.UpdateAtlasVO(docData.imageElementInfoList.Select(m =>
            {
                return new AtlasCreator_AtlasPanelVO()
                {
                    CoordInfo = m.rectInfo,
                    IsHightlight = m == mCurrentSelectionImageElement,
                    TexInfo = m.linkedTexture,
                    UserData = m,
                };
            }).ToArray());
        }

        void UpdateImageElementVO(AtlasCreator_DocData docData)
        {
            mView.UpdateImageElementVO(docData.imageElementInfoList.Select(m =>
            {
                return new AtlasCreator_ImageElementVO()
                {
                    ImageName = m.linkedTexture.name,
                    OnClicked = OnImageElementLabelClicked,
                    OnDel = OnImageElementLabelDelClicked,
                    IsHightlight = m == mCurrentSelectionImageElement,
                    UserData = m,
                };
            }).ToArray());
        }

        void UpdateImageArgumentVO(AtlasCreator_DocData docData, float defaultX, float defaultY, float defaultWidth, float defaultHeight)
        {
            if (mCurrentSelectionImageElement == null) return;

            mView.UpdateImageArgumentVO(new AtlasCreator_ImageArgumentVO()
            {
                X = defaultX,
                Y = defaultY,
                Width = defaultWidth,
                Height = defaultHeight,
                OnValueChange = (arg) =>
                {
                    if (mCurrentSelectionImageElement != null)
                    {
                        var cacheRectInfo = mCurrentSelectionImageElement.rectInfo;
                        cacheRectInfo.x = arg.X;
                        cacheRectInfo.y = arg.Y;
                        cacheRectInfo.width = arg.Width;
                        cacheRectInfo.height = arg.Height;
                        mCurrentSelectionImageElement.rectInfo = cacheRectInfo;

                        UpdateAtlasVO(docData);
                    }
                },
            });
        }

        void OnBuildResolutionParamUpdate(AtlasCreator_ImageMenuVO sender)
        {
            mCurrentDocData.resolution.x = sender.BuildResolutionW;
            mCurrentDocData.resolution.y = sender.BuildResolutionH;
        }

        string GetRelativeAssetPath(string _fullPath)
        {
            _fullPath = GetRightFormatPath(_fullPath);
            int idx = _fullPath.IndexOf("Assets");
            string assetRelativePath = _fullPath.Substring(idx);
            return assetRelativePath;
        }

        string GetRightFormatPath(string _path)
        {
            return _path.Replace("\\", "/");
        }

        TInterface[] CreateTypeInstanceForInterfaces<TInterface>()
        {
            var typeEble = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()
                .Where(m => m.GetInterface(typeof(TInterface).Name) != null && !m.IsAbstract));
            var result = typeEble.ToArray();
            return Array.ConvertAll(result, m => (TInterface)Activator.CreateInstance(m));
        }
    }
}
