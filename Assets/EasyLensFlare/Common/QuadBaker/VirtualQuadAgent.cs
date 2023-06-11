using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    /// <summary>
    /// 单个虚拟面片对象，与QuadBakerContainer组合使用。
    /// </summary>
    public class VirtualQuadAgent : MonoBehaviour
    {
        public enum ERegistMode { Manual, StartDestroy, EnableDisable }
        public enum EUpdateMode { Update, LateUpdate, Manual }
        //可以选择3种注册模式，手动或OnEnable时注册到管理器或Awake进行注册。
        public ERegistMode registMode = ERegistMode.Manual;
        public EUpdateMode updateMode = EUpdateMode.Manual;
        public QuadBakerContainer quadBakerContainer;
        //在QuadBakerContainer中开启更新顶点色后，顶点色会在Update中更新。
        public Color vertexColor = Color.white;
        //在QuadBakerContainer中开启更新UV后，UV会在Update中更新。
        public Vector2 uvP1 = new Vector2(0f, 0f);
        public Vector2 uvP2 = new Vector2(1f, 0f);
        public Vector2 uvP3 = new Vector2(0f, 1f);
        public Vector2 uvP4 = new Vector2(1f, 1f);

        bool mIsInitialized;
        QuadBaker.VirtualQuad mVirtualQuad;
        public bool IsInitialized { get { return mIsInitialized; } }


        public void Initialization()
        {
            mVirtualQuad = quadBakerContainer.RegistVirtualQuadPoint(transform);
            Update();

            mIsInitialized = true;
        }

        public void ManualUpdate()
        {
            ExecUpdate();
        }

        public void Release()
        {
            quadBakerContainer.UnregistVirtualQuadPoint(transform);
            mIsInitialized = false;
        }

        void OnEnable()
        {
            if (registMode != ERegistMode.EnableDisable) return;

            Initialization();
        }

        void Start()
        {
            if (registMode != ERegistMode.StartDestroy) return;

            Initialization();
        }

        void Update()
        {
            if (updateMode == EUpdateMode.Update)
                ExecUpdate();
        }

        void LateUpdate()
        {
            if (updateMode == EUpdateMode.LateUpdate)
                ExecUpdate();
        }

        void OnDisable()
        {
            if (registMode != ERegistMode.EnableDisable) return;

            Release();
        }

        void OnDestroy()
        {
            if (registMode != ERegistMode.StartDestroy) return;

            Release();
        }

        void ExecUpdate()
        {
            if (mVirtualQuad != null)
            {
                mVirtualQuad.VertexColor = vertexColor;
                mVirtualQuad.UVPoint1 = uvP1;
                mVirtualQuad.UVPoint2 = uvP2;
                mVirtualQuad.UVPoint3 = uvP3;
                mVirtualQuad.UVPoint4 = uvP4;
            }
        }
    }
}
