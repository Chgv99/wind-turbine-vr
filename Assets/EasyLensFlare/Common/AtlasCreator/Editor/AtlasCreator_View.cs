using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GR
{
    public class AtlasCreator_View
    {
        const float MENU_HEIGHT = 30f;

        public UnityEngine.Object LoadPathObject { get; set; }
        public Action<Texture> OnImageElementPanelDrop { get; set; }
        Vector2 mCacheImagePanelScrollValue;
        AtlasCreator_ImageMenuVO mImageMenuVO;
        AtlasCreator_ImageElementVO[] mImageElementVOArray;
        AtlasCreator_ImageArgumentVO mImageArgumentVO;
        AtlasCreator_AtlasPanelVO[] mAtlasPanelVOArray;


        public AtlasCreator_View()
        {
            mImageMenuVO = new AtlasCreator_ImageMenuVO();
            mImageElementVOArray = new AtlasCreator_ImageElementVO[0];
            mImageArgumentVO = new AtlasCreator_ImageArgumentVO();
            mAtlasPanelVOArray = new AtlasCreator_AtlasPanelVO[0];
        }

        public void DrawGUI()
        {
            RefreshMenuPanel(mImageMenuVO);
            RefreshImagePanel(mImageElementVOArray);
            RefreshArgumentPanel(mImageArgumentVO);
            RefreshAtlasPanel(mAtlasPanelVOArray);
        }

        public void UpdateImageMenuVO(AtlasCreator_ImageMenuVO imageMenuVO)
        {
            mImageMenuVO = imageMenuVO;
        }

        public void UpdateImageElementVO(AtlasCreator_ImageElementVO[] imageElementVOArray)
        {
            mImageElementVOArray = imageElementVOArray;
        }

        public void UpdateImageArgumentVO(AtlasCreator_ImageArgumentVO imageArgumentVO)
        {
            mImageArgumentVO = imageArgumentVO;
        }

        public void UpdateAtlasVO(AtlasCreator_AtlasPanelVO[] atlasPanelVOArray)
        {
            mAtlasPanelVOArray = atlasPanelVOArray;
        }

        void RefreshMenuPanel(AtlasCreator_ImageMenuVO arg)
        {
            var rootRect = new Rect(0, 0, Screen.width, MENU_HEIGHT);
            GUI.Box(rootRect, "");

            rootRect.position += Vector2.one * 5f;
            rootRect.size = rootRect.size - Vector2.one * 10f;
            GUILayout.BeginArea(rootRect);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Build", GUILayout.Width(100f), GUILayout.Height(16f)))
            {
                arg.OnBuildBtnClicked();
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Width(270f));

            EditorGUILayout.LabelField("Resolution - ", GUILayout.Width(80f));

            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.LabelField("x: ", GUILayout.Width(20f));
                arg.BuildResolutionW = EditorGUILayout.IntField(arg.BuildResolutionW, GUILayout.Width(70f));
                EditorGUILayout.LabelField("y: ", GUILayout.Width(20f));
                arg.BuildResolutionH = EditorGUILayout.IntField(arg.BuildResolutionH, GUILayout.Width(70f));

                if (changeScope.changed)
                    arg.OnBuildResolutionParamUpdate(arg);
            }

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("New", GUILayout.Width(100f), GUILayout.Height(16f)))
            {
                arg.OnNewBtnClicked();
            }
            if (GUILayout.Button("Save", GUILayout.Width(100f), GUILayout.Height(16f)))
            {
                arg.OnSaveBtnClicked();
            }
            if (GUILayout.Button("Load", GUILayout.Width(100f), GUILayout.Height(16f)))
            {
                arg.OnLoadBtnClicked();
            }

            LoadPathObject = EditorGUILayout.ObjectField(LoadPathObject, typeof(UnityEngine.Object), true, GUILayout.Width(100f), GUILayout.Height(18f));

            var fontStyle = new GUIStyle();
            fontStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            fontStyle.fontSize = 13;
            fontStyle.alignment = TextAnchor.LowerLeft;
            GUILayout.Label("<--- Please drop Doc data to that", fontStyle);

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        void RefreshImagePanel(AtlasCreator_ImageElementVO[] contents)
        {
            var heightOffset = MENU_HEIGHT + 5f;
            var height = Screen.height - heightOffset;
            height = height / 100f * 40f;
            var rootRect = new Rect(0, heightOffset, Screen.width / 100f * 20f, height);
            GUI.Box(rootRect, "");

            var fontStyle = new GUIStyle();
            fontStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            fontStyle.fontSize = 17;
            fontStyle.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(rootRect.x - 2f, rootRect.center.y, rootRect.width, 40f), "Please Drop \nImage to That", fontStyle);

            mCacheImagePanelScrollValue = GUI.BeginScrollView(rootRect, mCacheImagePanelScrollValue
                , new Rect(0f, 0f, rootRect.width * 0.8f, Mathf.Max(contents.Length * 20f, rootRect.height) + 30f), false, true);

            for (int i = 0; i < contents.Length; i++)
            {
                var item = contents[i];
                var buttonRect = new Rect(2f, 3f + 23f * i, rootRect.width * 0.75f, 20);
                var closeButtonRect = new Rect(buttonRect.xMax + 3f, buttonRect.y, rootRect.width * 0.12f, 20f);

                var imageNameStyle = new GUIStyle(GUI.skin.button);

                if (item.IsHightlight)
                {
                    imageNameStyle.fontStyle = FontStyle.Bold;
                }

                if (GUI.Button(buttonRect, item.ImageName, imageNameStyle))
                {
                    item.OnClicked?.Invoke(item);
                }

                if (GUI.Button(closeButtonRect, "x", imageNameStyle))
                {
                    item.OnDel?.Invoke(item);
                }
            }

            if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (Event.current.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    var dragItem = DragAndDrop.objectReferences[0];

                    if (dragItem is Texture)
                        OnImageElementPanelDrop?.Invoke(dragItem as Texture);
                }
                Event.current.Use();
            }

            GUI.EndScrollView();
        }

        void RefreshArgumentPanel(AtlasCreator_ImageArgumentVO arg)
        {
            var heightOffset = MENU_HEIGHT + 5f;
            var height = Screen.height - heightOffset;
            heightOffset += height / 100f * 40f;
            heightOffset += 10f;
            height = Screen.height - heightOffset;
            var rootRect = new Rect(0, heightOffset, Screen.width / 100f * 20f, height);
            GUI.Box(rootRect, "");

            GUILayout.BeginArea(rootRect);
            EditorGUILayout.Space();

            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("x:", GUILayout.MaxWidth(50f));
                arg.X = EditorGUILayout.FloatField(arg.X);
                EditorGUILayout.EndHorizontal();
                arg.X = EditorGUILayout.Slider(arg.X, 0f, 1f);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("y:", GUILayout.MaxWidth(50f));
                arg.Y = EditorGUILayout.FloatField(arg.Y);
                EditorGUILayout.EndHorizontal();
                arg.Y = EditorGUILayout.Slider(arg.Y, 0f, 1f);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("width:", GUILayout.MaxWidth(50f));
                arg.Width = EditorGUILayout.FloatField(arg.Width);
                EditorGUILayout.EndHorizontal();
                arg.Width = EditorGUILayout.Slider(arg.Width, 0f, 1f);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("height:", GUILayout.MaxWidth(50f));
                arg.Height = EditorGUILayout.FloatField(arg.Height);
                EditorGUILayout.EndHorizontal();
                arg.Height = EditorGUILayout.Slider(arg.Height, 0f, 1f);

                if (changeScope.changed)
                {
                    arg.OnValueChange?.Invoke(arg);
                }
            }

            GUILayout.EndArea();
        }

        void RefreshAtlasPanel(AtlasCreator_AtlasPanelVO[] contents)
        {
            var heightOffset = MENU_HEIGHT + 5f;
            var widthOri = Screen.width / 100f * 20f + 10f;
            var widthLength = Screen.width - widthOri;
            var rootRect = new Rect(widthOri, heightOffset, widthLength, Screen.height - heightOffset);
            GUI.Box(rootRect, "");
            var canvasRect = new Rect(rootRect.x + 10f, rootRect.y + 15f, rootRect.width - 20f, rootRect.height - 55f);
            GUI.Box(canvasRect, "");

            for (int i = 0; i < contents.Length; i++)
            {
                var atlasElementItem = contents[i];

                var rect = atlasElementItem.CoordInfo;
                rect.x = canvasRect.x + rect.x * canvasRect.width;
                rect.y = canvasRect.y + rect.y * canvasRect.height;
                rect.width = rect.width * canvasRect.width;
                rect.height = rect.height * canvasRect.height;

                var rect_border = new Rect(rect.x - 2f, rect.y - 2f, rect.width + 4f, rect.height + 4f);
                var rectTop = new Rect(rect_border.x, rect_border.y, rect_border.width - 3f, 2f);
                var rectBottom = new Rect(rect_border.x, rect_border.yMax - 4f, rect_border.width - 3f, 2f);
                var rectLeft = new Rect(rect_border.x, rect_border.y + 2f, 2f, rect_border.height - 4f);
                var rectRight = new Rect(rect_border.xMax - 4f, rect_border.y + 2f, 2f, rect_border.height - 4f);

                if (atlasElementItem.IsHightlight)
                {
                    var cacheBackgroundColor = GUI.color;
                    GUI.color = new Color(cacheBackgroundColor.r, cacheBackgroundColor.g, cacheBackgroundColor.b, 0.4f);
                    GUI.DrawTexture(rectTop, Texture2D.whiteTexture);
                    GUI.DrawTexture(rectBottom, Texture2D.whiteTexture);
                    GUI.DrawTexture(rectLeft, Texture2D.whiteTexture);
                    GUI.DrawTexture(rectRight, Texture2D.whiteTexture);
                    GUI.color = cacheBackgroundColor;
                }
                else
                {
                    var cacheBackgroundColor = GUI.color;
                    GUI.color = new Color(cacheBackgroundColor.r, cacheBackgroundColor.g, cacheBackgroundColor.b, 0.2f);
                    GUI.DrawTexture(rectTop, Texture2D.whiteTexture);
                    GUI.DrawTexture(rectBottom, Texture2D.whiteTexture);
                    GUI.DrawTexture(rectLeft, Texture2D.whiteTexture);
                    GUI.DrawTexture(rectRight, Texture2D.whiteTexture);
                    GUI.color = cacheBackgroundColor;
                }

                GUI.DrawTexture(rect, atlasElementItem.TexInfo);
            }
        }
    }
}
