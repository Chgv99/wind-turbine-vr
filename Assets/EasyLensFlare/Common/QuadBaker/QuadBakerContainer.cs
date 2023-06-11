using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    /// <summary>
    /// QuadBaker的扩展，在Unity场景中放置一个QuadBakerContainer即可作为Mesh的容器。
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class QuadBakerContainer : MonoBehaviour
    {
        public enum EUpdateOrder { Update, LateUpdate, Manual }

        public struct VirtualQuadCSStruct
        {
            public Vector3 Position1;
            public Vector3 Position2;
            public Vector3 Position3;
            public Vector3 Position4;

            public Matrix4x4 VirtualQuadMatrix;
        }

        public struct VirtualQuadPointInfo
        {
            public Transform TransformPoint { get; set; }
            public QuadBaker.VirtualQuad VirtualQuad { get; set; }
        }

        public bool updateVertexPosition = true;
        public bool updateVertexColor = true;
        public bool updateUv = true;
        public EUpdateOrder updateOrder = EUpdateOrder.Update;

        List<VirtualQuadPointInfo> mVirtualQuadPointInfoList;
        QuadBaker mQuadBaker;

        MeshFilter mMeshFilter;
        MeshRenderer mMeshRenderer;

        public QuadBaker QuadBaker { get { return mQuadBaker = mQuadBaker ?? new QuadBaker(); } }
        public MeshFilter MeshFilter { get { return mMeshFilter; } }
        public MeshRenderer MeshRenderer { get { return mMeshRenderer; } }


        void Awake()
        {
            mVirtualQuadPointInfoList = new List<VirtualQuadPointInfo>(256);

            QuadBaker.Init();

            mMeshFilter = GetComponent<MeshFilter>();
            mMeshRenderer = GetComponent<MeshRenderer>();

            mMeshFilter.mesh = QuadBaker.Mesh;
        }

        public QuadBaker.VirtualQuad RegistVirtualQuadPoint(Transform transform)
        {
            var virtualQuad = new QuadBaker.VirtualQuad();
            virtualQuad.Init();

            mVirtualQuadPointInfoList.Add(new VirtualQuadPointInfo() { TransformPoint = transform, VirtualQuad = virtualQuad });

            mQuadBaker.AddVirtualQuad(virtualQuad);

            return virtualQuad;
        }

        public void UnregistVirtualQuadPoint(Transform transform)
        {
            var virtualQuadPointInfo = mVirtualQuadPointInfoList.Find(m => m.TransformPoint == transform);

            mVirtualQuadPointInfoList.Remove(virtualQuadPointInfo);
            mQuadBaker.RemoveVirtualQuad(virtualQuadPointInfo.VirtualQuad);
        }

        public void ManualUpdateQuadBaker()
        {
            UpdateQuadBaker();
        }

        void Update()
        {
            if (updateOrder == EUpdateOrder.Update)
                UpdateQuadBaker();
        }

        void LateUpdate()
        {
            if (updateOrder == EUpdateOrder.LateUpdate)
                UpdateQuadBaker();
        }

        void UpdateQuadBaker()
        {
            UnityEngine.Profiling.Profiler.BeginSample("[QuadBaker] Translate QuadBaker Matrix");

            UpdateMatrixTranslate();

            UnityEngine.Profiling.Profiler.EndSample();

            QuadBaker.UpdateVertexColor = updateVertexColor;
            QuadBaker.UpdateVertexPosition = updateVertexPosition;
            QuadBaker.UpdateUv = updateUv;

            UnityEngine.Profiling.Profiler.BeginSample("[QuadBaker] Update Vertex And Vertex Color");
            QuadBaker.Update();
            UnityEngine.Profiling.Profiler.EndSample();
        }

        void UpdateMatrixTranslate()
        {
            var selfWorldToLocalMatrix = transform.worldToLocalMatrix;

            UnityEngine.Profiling.Profiler.BeginSample("Core");
            for (int i = 0, iMax = mVirtualQuadPointInfoList.Count; i < iMax; i++)
            {
                var quadTransform = mVirtualQuadPointInfoList[i].TransformPoint;

                var quadLocalToWorldMatrix = quadTransform.localToWorldMatrix;

                var virtualQuad = mQuadBaker.VirtualQuadList[i];
                var v = quadLocalToWorldMatrix.MultiplyPoint3x4(new Vector3(-0.5f, -0.5f, 0f));
                virtualQuad.ModelPoint1 = selfWorldToLocalMatrix.MultiplyPoint3x4(v);
                v = quadLocalToWorldMatrix.MultiplyPoint3x4(new Vector3(0.5f, -0.5f, 0f));
                virtualQuad.ModelPoint2 = selfWorldToLocalMatrix.MultiplyPoint3x4(v);
                v = quadLocalToWorldMatrix.MultiplyPoint3x4(new Vector3(-0.5f, 0.5f, 0f));
                virtualQuad.ModelPoint3 = selfWorldToLocalMatrix.MultiplyPoint3x4(v);
                v = quadLocalToWorldMatrix.MultiplyPoint3x4(new Vector3(0.5f, 0.5f, 0f));
                virtualQuad.ModelPoint4 = selfWorldToLocalMatrix.MultiplyPoint3x4(v);
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}
