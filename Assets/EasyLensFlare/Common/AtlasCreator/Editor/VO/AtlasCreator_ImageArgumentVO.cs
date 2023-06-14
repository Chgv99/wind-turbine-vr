using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    public class AtlasCreator_ImageArgumentVO
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Action<AtlasCreator_ImageArgumentVO> OnValueChange { get; set; }
    }
}
