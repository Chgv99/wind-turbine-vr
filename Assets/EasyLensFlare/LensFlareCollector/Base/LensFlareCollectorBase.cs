using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    public class LensFlareCollectorBase : MonoBehaviour
    {
        [Tooltip("id of 'LensFlareElementGroupConfigurator' class")]
        public int elementConfiguratorID;
        [Tooltip("Global conf intensity > element conf intensity > dynamic intensity(this)")]
        public float intensityMultiple = 1f;
        [Tooltip("Tween show speed multiple relative to Time.deltaTime")]
        public float showSpeedMultiple = 3f;
        [Tooltip("Tween fade speed multiple relative to Time.deltaTime")]
        public float fadeSpeedMultiple = 3f;
        [Tooltip("Camera frustum check bounds size")]
        public Vector3 boundsSize = new Vector3(1f, 1f, 1f);
        [Tooltip("Distance attenuation function toggle")]
        public bool enableDistanceAttenuation = false;
        [Tooltip("Max distance attenuation range")]
        public float distanceAttenuationRange = 3f;

        [Tooltip("Attach angle limit toggle")]
        public bool enableAngleLimit;
        [Range(0f, 180f)]
        [Tooltip("LensFlare lighting in limit angle")]
        public float angleLimit = 45f;

        [Tooltip("Enable distance scale")]
        public bool enableDistanceScale;
        public Vector2 distanceScaleValue;

        protected LensFlareData mLensFlareData;

        Plane[] mCachePlaneArray;

        Camera mCacheMainCamera;
        protected Camera CacheMainCamera { get { return mCacheMainCamera ?? (mCacheMainCamera = Camera.main); } }


        /// <summary>
        /// LensFlare collector initialization method.
        /// </summary>
        protected virtual void OnEnable()
        {
            mLensFlareData = LensFlareDrawController.Instance.RegistLensFlareData();
            mLensFlareData.ElementConfiguratorID = elementConfiguratorID;
            mLensFlareData.GetViewportPosition = GetViewPortPosition;
        }

        /// <summary>
        /// LensFlare collector deinitialization method.
        /// </summary>
        protected virtual void OnDisable()
        {
            if (LensFlareDrawController.Instance && mLensFlareData != null)
                LensFlareDrawController.Instance.UnregistLensFlareData(mLensFlareData);
        }

        /// <summary>
        /// Camera frustum clip bounds gizmos or override.
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, boundsSize);
        }

        /// <summary>
        /// Return true has Camera Frustum contain otherwise return false.
        /// </summary>
        protected virtual bool MainCameraFrustumPlanesCheck()
        {
            if (mCachePlaneArray == null)
                mCachePlaneArray = new Plane[6];

            GeometryUtility.CalculateFrustumPlanes(Camera.main, mCachePlaneArray);
            var isContain = GeometryUtility.TestPlanesAABB(mCachePlaneArray, new Bounds(transform.position, boundsSize));

            return isContain;
        }

        /// <summary>
        /// Return true has limit angle outside otherwise return false.
        /// </summary>
        protected virtual bool AngleLimitCheck()
        {
            var result = false;

            if (enableAngleLimit)
            {
                var dir = (transform.position - CacheMainCamera.transform.position).normalized;
                var dot = Vector3.Dot(dir, CacheMainCamera.transform.forward);
                var angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                if (angle > angleLimit)
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// Return lensflare relative to main camera distance factor default situation return 1.
        /// </summary>
        protected virtual float DistanceAttenuationProcess(LensFlareData lensFlareData)
        {
            var result = 1f;

            if (enableDistanceAttenuation)
            {
                var attenurationFactor = 1f - Mathf.Clamp01(Vector3.Distance(CacheMainCamera.transform.position, transform.position)
                    / distanceAttenuationRange);

                result = attenurationFactor;
            }

            return result;
        }

        protected virtual void IntensityProcess(LensFlareData lensFlareData, float distanceAttenuation)
        {
            lensFlareData.Intensity *= distanceAttenuation;
            lensFlareData.Intensity *= intensityMultiple;
        }

        protected virtual void ScaleProcess(LensFlareData lensFlareData, float distanceAttenuation)
        {
            if (enableDistanceScale)
            {
                lensFlareData.ScaleMultiple = Mathf.Lerp(distanceScaleValue.x, distanceScaleValue.y, distanceAttenuation);
            }
            else
            {
                lensFlareData.ScaleMultiple = 1f;
            }
        }

        protected virtual Vector2 GetViewPortPosition()
        {
            return CacheMainCamera.WorldToViewportPoint(transform.position);
        }
    }
}
