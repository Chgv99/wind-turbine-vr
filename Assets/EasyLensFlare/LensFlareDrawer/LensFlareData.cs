using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    public class LensFlareData
    {
        /// <summary>
        /// Consider VR render situation.
        /// </summary>
        public Func<Vector2> GetViewportPosition { get; set; }
        public float Intensity { get; set; }
        public float ScaleMultiple { get; set; }
        public int ElementConfiguratorID { get; set; }
        /// <summary>
        /// This is dynamic binding object.
        /// </summary>
        public LensFlareElementGroup CacheElementGroup { get; set; }
    }
}
