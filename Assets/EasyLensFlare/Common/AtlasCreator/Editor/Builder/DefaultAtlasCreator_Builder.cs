using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GR
{
    public class DefaultAtlasCreator_Builder : IAtlasCreator_Builder
    {
        public void BuildToDisk(string savePath, AtlasCreator_DocData docData)
        {
            if (!Directory.Exists(savePath))
            {
                Debug.LogError("Just support save to directory!");
                return;
            }

            var json = JsonUtility.ToJson(docData);
            var saveTex = AtlasCreator_BuilderUtil.GenAtlas(docData.resolution.x, docData.resolution.y, docData);

            File.WriteAllText(Path.Combine(savePath, "atlas.json"), json);
            File.WriteAllBytes(Path.Combine(savePath, "atlas.png"), saveTex.EncodeToPNG());
            AssetDatabase.Refresh();
        }
    }
}
