using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    public class LensFlareRaycastItem : LensFlareCollectorBase
    {
        const float HIT_COUNTER_MAX = 3f;

        public LayerMask layerMask = ~0;
        float mHitCounter;


        protected override void OnEnable()
        {
            base.OnEnable();

            mHitCounter = 0f;
        }

        void Update()
        {
            var isObstacle = false;

            if (!base.MainCameraFrustumPlanesCheck())
                isObstacle = true;

            if (!isObstacle)
                isObstacle = Physics.Linecast(transform.position, CacheMainCamera.transform.position, layerMask);

            if (base.AngleLimitCheck())
            {
                isObstacle = true;
            }

            if (isObstacle)
            {
                mHitCounter = Mathf.Clamp01(mHitCounter - Time.deltaTime * base.fadeSpeedMultiple);
            }
            else
            {
                mHitCounter = Mathf.Min(HIT_COUNTER_MAX, mHitCounter + Time.deltaTime * base.showSpeedMultiple);
            }

            mLensFlareData.Intensity = Mathf.Clamp01(mHitCounter);

            var distanceAttenuation = base.DistanceAttenuationProcess(mLensFlareData);

            base.IntensityProcess(mLensFlareData, distanceAttenuation);
            base.ScaleProcess(mLensFlareData, distanceAttenuation);

        }
    }
}
