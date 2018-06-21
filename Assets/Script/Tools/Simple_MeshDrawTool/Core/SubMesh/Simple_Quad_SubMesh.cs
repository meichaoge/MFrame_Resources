using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    public class Simple_Quad_SubMesh : BaseSubMesh
    {
        protected override BaseMeshPoint CreateFirstMeshPoint(Vector3 point)
        {
            return new QuadPoint(point);
        }

        protected override BaseMeshPoint CreateSubKeyPoint(BaseMeshPoint previous, Vector3 currentPoint, float widthRatio = 1)
        {
            QuadPoint keyPoint = new QuadPoint(currentPoint);
            Vector3 opDir = Vector3.Cross(((previous as QuadPoint).GetQuadLeft() - previous.Point_Center), (currentPoint - previous.Point_Center)); //垂直于面Previous 和current的向量
            Vector3 currentDir = Vector3.Cross(opDir, (previous.Point_Center - currentPoint)).normalized; //当前点的方向线
            keyPoint.CenterLeftDirNor = currentDir;
            return keyPoint;
        }

        public override void AddPoint(Vector3 point)
        {
            //base.AddPoint(Point);
            if (m_AllSubKeyPathpoints.Count == 0)
            {
                QuadPoint _firstPoint = CreateFirstMeshPoint(point) as QuadPoint;
                m_AllSubKeyPathpoints.Add(_firstPoint);
                return;
            }

            QuadPoint previousQuaPoint = m_AllSubKeyPathpoints[m_AllSubKeyPathpoints.Count - 1] as QuadPoint; //上一个Key路径点
            Vector3 direction = point - previousQuaPoint.Point_Center; //两个Key路径点的向量

            if (m_AllSubKeyPathpoints.Count == 1)
            {  //第一个点的方向有第二个点决定
                previousQuaPoint.CenterLeftDirNor = new Vector3(-1, 0, 0);
                m_AllSubMeshPoints.Add(previousQuaPoint.GetQuadRight());
                m_AllSubMeshPoints.Add(previousQuaPoint.GetQuadLeft());
            }



            Vector3 controllPoint1 = VectorExpand.GetPointPerpendicularTo(previousQuaPoint.GetQuadLeft(), previousQuaPoint.Point_Center, point);  //获得垂直于上一个QuadPoint 点且在direction 平面的单位控制点
            List<QuadPoint> subQuadPoint = new List<QuadPoint>(); // 子节点Point

            for (int dex = 0; dex < previousQuaPoint.SubsectionCount; dex++)
            {
                Vector3 _subPoint = Curve.CalculateBezier(previousQuaPoint.Point_Center, point, controllPoint1, (dex + 1) * 1f / (previousQuaPoint.SubsectionCount));  //根据二阶贝塞尔曲线获得 一个分割点
                QuadPoint currentSubPoint;
                int previousStartIndex = m_AllSubMeshPoints.Count - 2;
                if (dex == 0)
                    currentSubPoint = CreateSubKeyPoint(previousQuaPoint, _subPoint) as QuadPoint;
                else
                    currentSubPoint = CreateSubKeyPoint(subQuadPoint[subQuadPoint.Count - 1], _subPoint) as QuadPoint;

                subQuadPoint.Add(currentSubPoint);
                m_AllSubTrangles.Add(previousStartIndex);
                m_AllSubTrangles.Add(previousStartIndex + 1);
                m_AllSubMeshPoints.Add(currentSubPoint.GetQuadRight());
                m_AllSubMeshPoints.Add(currentSubPoint.GetQuadLeft());
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1);  //上一个Right,上一个Left,当前的Left

                m_AllSubTrangles.Add(previousStartIndex);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 2);  //上一个Right,当前的Left，但情感的Left

                if (dex == previousQuaPoint.SubsectionCount - 1)
                    m_AllSubKeyPathpoints.Add(currentSubPoint);

            }


        }



    }




    [System.Serializable]
    public class QuadPoint : BaseMeshPoint
    {
        public Vector3 CenterLeftDirNor;  //中心diam向左的单位方向向量
        public int SubsectionCount = 1; //两个点被分割的段数

        public QuadPoint(Vector3 point) : base(point)
        {
        }

        public Vector3 GetQuadLeft()
        {
            return Point_Center + LineWidth / 2f * CenterLeftDirNor;
        }

        public Vector3 GetQuadRight()
        {
            return Point_Center - LineWidth / 2f * CenterLeftDirNor;
        }

    }
}