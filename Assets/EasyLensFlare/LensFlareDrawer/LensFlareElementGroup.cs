using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    /// <summary>
    /// Lens flare data info.
    /// </summary>
    public class LensFlareElementGroup
    {
        public float LensDirtyTotalIntensity { get; set; }
        public float LensDirtyTotalExpectIntensity { get; set; }
        public LensFlare_SimplePool<VirtualQuadAgent> Pool { get; set; }
        public List<VirtualQuadAgent> PoolTakedList { get; set; }
        public LensFlareElementGroupConfigurator Configurator { get; set; }


        public LensFlareElementGroup(LensFlareElementGroupConfigurator configurator)
        {
            Configurator = configurator;
        }
    }
}
