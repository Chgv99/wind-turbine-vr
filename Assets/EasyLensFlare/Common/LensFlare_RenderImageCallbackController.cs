using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OnRenderImageCallback = System.Action<GR.LensFlareRenderImageContext
    , UnityEngine.RenderTexture, UnityEngine.RenderTexture>;

namespace GR
{
    public enum ELensFlareRenderImageCallbackOrder { RenderImage, RenderImageLate }

    public sealed class LensFlareRenderImageContext
    {
        public bool GraphicsBlitFlag { get; set; }
    }

    public sealed class LensFlare_RenderImageCallbackController
    {
        public sealed class LensFlare_RenderImageCallback : MonoBehaviour
        {
            public Action<RenderTexture, RenderTexture> onRenderImage;


            [ImageEffectOpaque]
            void OnRenderImage(RenderTexture src, RenderTexture dst)
            {
                onRenderImage(src, dst);
            }
        }

        static LensFlare_RenderImageCallbackController mInstance;
        public static LensFlare_RenderImageCallbackController Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new LensFlare_RenderImageCallbackController();

                return mInstance;
            }
        }

        static Camera mSpecifiedCamera;
        LensFlareRenderImageContext mContext;
        LensFlare_RenderImageCallback mLensFlare_RenderImageCallback;
        List<OnRenderImageCallback> mOnRenderImageCallbackList;
        List<OnRenderImageCallback> mOnRenderImageLaterCallbackList;


        public static void SetSpecifiedCamera(Camera specifiedCamera)
        {
            mSpecifiedCamera = specifiedCamera;
        }

        public void RegistCallback(ELensFlareRenderImageCallbackOrder order, OnRenderImageCallback callbackFunc)
        {
            InitializationCheck();

            switch (order)
            {
                case ELensFlareRenderImageCallbackOrder.RenderImage:
                    mOnRenderImageCallbackList.Add(callbackFunc);
                    break;
                case ELensFlareRenderImageCallbackOrder.RenderImageLate:
                    mOnRenderImageLaterCallbackList.Add(callbackFunc);
                    break;
                default:
                    break;
            }

            UnityComponentCheck();
        }

        public void UnregistCallback(ELensFlareRenderImageCallbackOrder order, OnRenderImageCallback callbackFunc)
        {
            switch (order)
            {
                case ELensFlareRenderImageCallbackOrder.RenderImage:
                    mOnRenderImageCallbackList.Remove(callbackFunc);
                    break;
                case ELensFlareRenderImageCallbackOrder.RenderImageLate:
                    mOnRenderImageLaterCallbackList.Remove(callbackFunc);
                    break;
                default:
                    break;
            }

            UnityComponentCheck();
        }

        void InitializationCheck()
        {
            if (mContext == null)
                mContext = new LensFlareRenderImageContext();

            if (mOnRenderImageCallbackList == null)
                mOnRenderImageCallbackList = new List<OnRenderImageCallback>(16);

            if (mOnRenderImageLaterCallbackList == null)
                mOnRenderImageLaterCallbackList = new List<OnRenderImageCallback>(16);
        }

        void UnityComponentCheck()
        {
            //---------------------------------
            for (int i = mOnRenderImageCallbackList.Count - 1; i >= 0; i--)
            {
                var item = mOnRenderImageCallbackList[i];

                if (item == null)
                    mOnRenderImageCallbackList.RemoveAt(i);
            }

            for (int i = mOnRenderImageLaterCallbackList.Count - 1; i >= 0; i--)
            {
                var item = mOnRenderImageLaterCallbackList[i];

                if (item == null)
                    mOnRenderImageLaterCallbackList.RemoveAt(i);
            }
            //Ensure list element is not null.

            if (!mLensFlare_RenderImageCallback)
            {
                if (mOnRenderImageCallbackList.Count > 0 || mOnRenderImageLaterCallbackList.Count > 0)
                {
                    var dstCamera = mSpecifiedCamera ?? Camera.main;
                    if (dstCamera)
                    {
                        mLensFlare_RenderImageCallback = dstCamera.gameObject.AddComponent<LensFlare_RenderImageCallback>();
                        mLensFlare_RenderImageCallback.onRenderImage = OnRenderImageCallback;
                    }
                }
            }
            else
            {
                if (mOnRenderImageCallbackList.Count == 0 && mOnRenderImageLaterCallbackList.Count == 0)
                {
                    UnityEngine.Object.Destroy(mLensFlare_RenderImageCallback);
                }
            }
        }

        void OnRenderImageCallback(RenderTexture src, RenderTexture dst)
        {
            mContext.GraphicsBlitFlag = false;

            for (int i = 0; i < mOnRenderImageCallbackList.Count; i++)
            {
                var item = mOnRenderImageCallbackList[i];

                item.Invoke(mContext, src, dst);
            }

            for (int i = 0; i < mOnRenderImageLaterCallbackList.Count; i++)
            {
                var item = mOnRenderImageLaterCallbackList[i];

                item.Invoke(mContext, src, dst);
            }

            if (!mContext.GraphicsBlitFlag)
                Graphics.Blit(src, dst);
        }
    }
}
