using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    public interface IAtlasCreator_Builder
    {
        void BuildToDisk(string savePath, AtlasCreator_DocData docData);
    }
}
