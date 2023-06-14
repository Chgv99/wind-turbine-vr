using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    public static class AtlasCreator_BuilderUtil
    {
        public static Texture2D GenAtlas(int width, int height, AtlasCreator_DocData docData)
        {
            var mat = new Material(Shader.Find("Hidden/AtlasCreator_BuilderCombine"));
            var texture_A = new RenderTexture(width, height, 0);
            texture_A.Create();
            var texture_B = new RenderTexture(width, height, 0);
            texture_B.Create();
            var texture_final = default(RenderTexture);

            for (int i = 0; i < docData.imageElementInfoList.Count; i++)
            {
                var imageElement = docData.imageElementInfoList[i];

                var cacheWrapMode = imageElement.linkedTexture.wrapMode;
                imageElement.linkedTexture.wrapMode = TextureWrapMode.Clamp;

                mat.SetTexture("_AddTex", imageElement.linkedTexture);
                mat.SetVector("_AddTexRect", new Vector4(imageElement.rectInfo.x
                    , imageElement.rectInfo.y, imageElement.rectInfo.width, imageElement.rectInfo.height));

                Graphics.Blit(texture_A, texture_B, mat);

                imageElement.linkedTexture.wrapMode = cacheWrapMode;

                texture_final = texture_B;

                var temp = texture_B;
                texture_B = texture_A;
                texture_A = temp;
            }

            var tex2D = new Texture2D(width, height);
            RenderTexture.active = texture_final;
            tex2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            RenderTexture.active = null;
            tex2D.Apply();

            texture_A.Release();
            texture_B.Release();

            return tex2D;
        }
    }
}
