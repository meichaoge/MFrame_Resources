using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    public class Simple_Point_SubMesh : BaseSubMesh
    {
        protected override BaseMeshPoint CreateFirstMeshPoint(Vector3 point)
        {
            return null;
        }

        protected override BaseMeshPoint CreateSubKeyPoint(BaseMeshPoint previous, Vector3 currentPoint, float widthRatio = 1)
        {
            return null;
        }

        public override void AddPoint(Vector3 Point)
        {
            // base.AddPoint(Point);
            BaseMeshPoint meshPoint = new BaseMeshPoint(Point);
            m_AllSubKeyPathpoints.Add(meshPoint);
            m_AllSubMeshPoints.Add(Point);
            if (m_AllSubMeshPoints.Count <= 2) return;

            m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 3);
            m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1);
            m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 2);

        }




    }
}