using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    public class AtlasCreator_ImageMenuVO
    {
        public Action OnBuildBtnClicked { get; set; }
        public int BuildResolutionW { get; set; }
        public int BuildResolutionH { get; set; }
        public Action<AtlasCreator_ImageMenuVO> OnBuildResolutionParamUpdate { get; set; }

        public Action OnNewBtnClicked { get; set; }
        public Action OnSaveBtnClicked { get; set; }
        public Action OnLoadBtnClicked { get; set; }
    }
}
