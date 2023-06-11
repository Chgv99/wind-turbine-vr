using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    public class AtlasCreator_DocData : ScriptableObject
    {
        [Serializable]
        public class ImageElementInfo
        {
            public Rect rectInfo;
            public Texture linkedTexture;
        }

        public Vector2Int resolution;
        public List<ImageElementInfo> imageElementInfoList;
    }
}
