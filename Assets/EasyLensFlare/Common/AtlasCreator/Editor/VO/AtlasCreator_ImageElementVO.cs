using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    public class AtlasCreator_ImageElementVO
    {
        public string ImageName { get; set; }
        public bool IsHightlight { get; set; }
        public object UserData { get; set; }
        public Action<AtlasCreator_ImageElementVO> OnClicked { get; set; }
        public Action<AtlasCreator_ImageElementVO> OnDel { get; set; }
    }
}
