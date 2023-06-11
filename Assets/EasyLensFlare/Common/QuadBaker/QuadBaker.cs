using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    /// <summary>
    /// 面片更新生成等操作的核心类
    /// 规则：
    /// (2(uv(0,1))-----(3(uv(1,1))
    /// |           |
    /// (0(uv(0,0))-----(1(uv(1,0))
    /// </summary>
    public class QuadBaker
    {
        public class VirtualQuad
        {
            public Vector3 ModelPoint1 { get; set; }
            public Vector3 ModelPoint2 { get; set; }
            public Vector3 ModelPoint3 { get; set; }
            public Vector3 ModelPoint4 { get; set; }

            public Vector2 UVPoint1 { get; set; }
            public Vector2 UVPoint2 { get; set; }
            public Vector2 UVPoint3 { get; set; }
            public Vector2 UVPoint4 { get; set; }

            public Color VertexColor { get; set; }


            public void Init()
            {
                UVPoint1 = new Vector2(0, 0);
                UVPoint2 = new Vector2(1, 0);
                UVPoint3 = new Vector2(0, 1);
                UVPoint4 = new Vector2(1, 1);

                ModelPoint1 = new Vector3(0, 0, 0);
                ModelPoint2 = new Vector3(1, 0, 0);
                ModelPoint3 = new Vector3(1, 1, 0);
                ModelPoint4 = new Vector3(0, 1, 0);

                VertexColor = Color.white;
            }
        }

        int mVirtualQuadIDCounter;
        bool mVirtualQuadIsDirty;

        Vector3[] mVerticesArray;
        Color[] mVerticesColorArray;
        Vector2[] mUvArray;
        Mesh mCacheMesh;
        List<VirtualQuad> mVirtualQuadList;

        public bool UpdateVertexPosition { get; set; }
        public bool UpdateVertexColor { get; set; }
        public bool UpdateUv { get; set; }
        public List<VirtualQuad> VirtualQuadList { get { return mVirtualQuadList; } }
        public Mesh Mesh { get { return mCacheMesh; } }


        public void Init()
        {
            mCacheMesh = new Mesh();
            mCacheMesh.name = "QuadBaker Mesh";
            mVirtualQuadList = new List<VirtualQuad>();

            UpdateVertexPosition = true;
            UpdateVertexColor = true;
            UpdateUv = true;

            UpdateMeshInfo();
        }

        public int AddVirtualQuad(VirtualQuad virtualQuad)
        {
            var id = mVirtualQuadIDCounter;

            mVirtualQuadList.Add(virtualQuad);

            mVirtualQuadIsDirty = true;
            mVirtualQuadIDCounter++;

            return id;
        }

        public void RemoveVirtualQuad(QuadBaker.VirtualQuad virtualQuad)
        {
            mVirtualQuadList.Remove(virtualQuad);

            mVirtualQuadIsDirty = true;
        }

        public void Clear()
        {
            mCacheMesh.Clear();
            mVirtualQuadList.Clear();
        }

        public void Update()
        {
            if (mVirtualQuadIsDirty)
                UpdateMeshInfo();

            if (UpdateVertexPosition)
            {
                if (mVerticesArray == null)
                    mVerticesArray = mCacheMesh.vertices;

                for (int i = 0, iMax = mVirtualQuadList.Count; i < iMax; i++)
                {
                    var virtualQuad = mVirtualQuadList[i];

                    var ori = i * 4;
                    mVerticesArray[ori + 0] = virtualQuad.ModelPoint1;
                    mVerticesArray[ori + 1] = virtualQuad.ModelPoint2;
                    mVerticesArray[ori + 2] = virtualQuad.ModelPoint3;
                    mVerticesArray[ori + 3] = virtualQuad.ModelPoint4;
                }
                mCacheMesh.vertices = mVerticesArray;
                mCacheMesh.RecalculateBounds();
            }

            if (UpdateVertexColor)
            {
                if (mVerticesColorArray == null)
                    mVerticesColorArray = mCacheMesh.colors;

                for (int i = 0, iMax = mVirtualQuadList.Count; i < iMax; i++)
                {
                    var virtualQuad = mVirtualQuadList[i];

                    var ori = i * 4;
                    mVerticesColorArray[ori + 0] = virtualQuad.VertexColor;
                    mVerticesColorArray[ori + 1] = virtualQuad.VertexColor;
                    mVerticesColorArray[ori + 2] = virtualQuad.VertexColor;
                    mVerticesColorArray[ori + 3] = virtualQuad.VertexColor;
                }
                mCacheMesh.colors = mVerticesColorArray;
            }

            if (UpdateUv)
            {
                if (mUvArray == null)
                    mUvArray = mCacheMesh.uv;

                for (int i = 0, iMax = mVirtualQuadList.Count; i != iMax; i++)
                {
                    var virtualQuad = mVirtualQuadList[i];
                    var offset = i * 4;
                    mUvArray[offset] = virtualQuad.UVPoint1;
                    mUvArray[offset + 1] = virtualQuad.UVPoint2;
                    mUvArray[offset + 2] = virtualQuad.UVPoint3;
                    mUvArray[offset + 3] = virtualQuad.UVPoint4;
                }
                mCacheMesh.uv = mUvArray;
            }
        }

        void UpdateMeshInfo()
        {
            mCacheMesh.triangles = new int[0];

            mCacheMesh.vertices = new Vector3[mVirtualQuadList.Count * 4];
            mVerticesArray = new Vector3[mVirtualQuadList.Count * 4];

            var uvs = new Vector2[mVirtualQuadList.Count * 4];
            mUvArray = new Vector2[mVirtualQuadList.Count * 4];
            for (int i = 0, iMax = mVirtualQuadList.Count; i != iMax; i++)
            {
                var virtualQuad = mVirtualQuadList[i];
                var offset = i * 4;
                uvs[offset] = virtualQuad.UVPoint1;
                uvs[offset + 1] = virtualQuad.UVPoint2;
                uvs[offset + 2] = virtualQuad.UVPoint3;
                uvs[offset + 3] = virtualQuad.UVPoint4;
            }
            mCacheMesh.uv = uvs;

            var vertexColors = new Color[mVirtualQuadList.Count * 4];
            mVerticesColorArray = new Color[mVirtualQuadList.Count * 4];

            for (int i = 0, iMax = mVirtualQuadList.Count; i < iMax; i++)
            {
                var virtualQuad = mVirtualQuadList[i];
                var offset = i * 4;
                vertexColors[offset] = virtualQuad.VertexColor;
                vertexColors[offset + 1] = virtualQuad.VertexColor;
                vertexColors[offset + 2] = virtualQuad.VertexColor;
                vertexColors[offset + 3] = virtualQuad.VertexColor;
            }
            mCacheMesh.colors = vertexColors;

            var triangles = new int[mVirtualQuadList.Count * 6];
            for (int i = 0, iMax = mVirtualQuadList.Count; i != iMax; i++)
            {
                var offset = i * 6;
                var vertexOffset = i * 4;

                triangles[offset] = vertexOffset + 0;
                triangles[offset + 1] = vertexOffset + 2;
                triangles[offset + 2] = vertexOffset + 3;
                triangles[offset + 3] = vertexOffset + 0;
                triangles[offset + 4] = vertexOffset + 3;
                triangles[offset + 5] = vertexOffset + 1;
            }
            mCacheMesh.triangles = triangles;

            mCacheMesh.RecalculateBounds();
            mCacheMesh.RecalculateNormals();
            mCacheMesh.RecalculateTangents();

            mVirtualQuadIsDirty = false;
        }
    }
}
