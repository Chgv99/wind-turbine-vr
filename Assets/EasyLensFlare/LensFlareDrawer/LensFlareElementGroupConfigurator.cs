using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    /// <summary>
    /// Lens flare Configurator info.
    /// </summary>
    [CreateAssetMenu(fileName = "LensFlareElementGroupConfigurator", menuName = "GR/LensFlareElementGroupConfigurator")]
    public class LensFlareElementGroupConfigurator : ScriptableObject
    {
        [Serializable]
        public class ElementAttachInfo
        {
            public string elementName;
            public float intensity = 1f;
            public float offsetWeight = 1f;
            public Vector4 offsetScale = new Vector4(0f, 0f, 1f, 1f);
        }

        public int id;
        public Texture atlasMainTexture;
        public AtlasCreator_DocData atlasDocData;

        public float lensDirtyGlobalIntensityMultiple = 3f;
        public float lensDirtyShowTweenSpeed = 6f;
        public float lensDirtyFadeTweenSpeed = 8f;
        public float lensDirtyContributeMultiplePerLensData = 0.8f;
        public float lensFlareGlobalIntensityMultiple = 1f;

        public string lensDirtyImageName;
        public ElementAttachInfo[] elementAttachInfoArray;


        void OnValidate()
        {
            if (atlasDocData != null)
            {
                var elementList = atlasDocData.imageElementInfoList;
                if (elementList != null && elementList.Count > 0)
                {
                    var attachInfoArray = elementAttachInfoArray;
                    if (attachInfoArray == null || attachInfoArray.Length != elementList.Count)
                    {
                        attachInfoArray = new ElementAttachInfo[elementList.Count];
                    }
                    for (int i = 0; i < attachInfoArray.Length; i++)
                    {
                        if (attachInfoArray[i] == null)
                            attachInfoArray[i] = new ElementAttachInfo();

                        if (elementList[i].linkedTexture)
                            attachInfoArray[i].elementName = elementList[i].linkedTexture.name;
                    }
                    elementAttachInfoArray = attachInfoArray;
                }
            }

        }
    }
}
