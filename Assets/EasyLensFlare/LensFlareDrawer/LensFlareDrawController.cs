using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GR
{
    /// <summary>
    /// Control the draw of lens dirty
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public sealed class LensFlareDrawController : MonoBehaviour
    {
        static LensFlareDrawController mInstance;
        public static LensFlareDrawController Instance { get { return mInstance; } }

        [SerializeField]
        LensFlareElementGroupConfigurator[] lensFlareElementGroupConfiguratorArray = null;
        List<LensFlareData> mLensFlareDataList = null;

        LensFlareDrawer mLensFlareDrawer;


        void Awake()
        {
            mInstance = this;
        }

        void OnEnable()
        {
            mLensFlareDataList = new List<LensFlareData>(16);
            mLensFlareDrawer = new LensFlareDrawer();
            mLensFlareDrawer.Initialization(mLensFlareDataList, lensFlareElementGroupConfiguratorArray);
        }

        void OnDisable()
        {
            mLensFlareDrawer?.Release();

            mInstance = null;
        }

        public LensFlareData RegistLensFlareData()
        {
            var result = new LensFlareData();

            mLensFlareDataList.Add(result);

            return result;
        }

        public bool UnregistLensFlareData(LensFlareData data)
        {
            var result = mLensFlareDataList.Remove(data);

            return result;
        }

    }
}
