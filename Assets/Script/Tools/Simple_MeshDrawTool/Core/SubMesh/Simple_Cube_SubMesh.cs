using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    public class Simple_Cube_SubMesh : BaseSubMesh
    {
        protected override BaseMeshPoint CreateFirstMeshPoint(Vector3 point)
        {
            return new Quad3DPoint(point);
        }

        protected override BaseMeshPoint CreateSubKeyPoint(BaseMeshPoint previous, Vector3 currentPoint, float widthRatio = 1)
        {
            Quad3DPoint point = new Quad3DPoint(currentPoint);
            Vector3 opDir = Vector3.Cross((previous as Quad3DPoint).CenterLeftDirNor, (currentPoint - previous.Point_Center)); //垂直于面Previous 和current的向量
            Vector3 currentDir = Vector3.Cross(opDir, (previous.Point_Center - currentPoint)).normalized; //当前点的方向线
            point.CenterDownUptDirNor = opDir.normalized;
            point.CenterLeftDirNor = currentDir;
            return point;
        }

        public override void AddPoint(Vector3 point)
        {
            //base.AddPoint(point);

            if (m_AllSubKeyPathpoints.Count == 0)
            {
                Quad3DPoint _firstPoint = CreateFirstMeshPoint(point) as Quad3DPoint;
                m_AllSubKeyPathpoints.Add(_firstPoint);
                m_AllSubTrangles.Clear();
                m_AllSubMeshPoints.Clear();
                return;
            }

            Quad3DPoint previousQuaPoint = m_AllSubKeyPathpoints[m_AllSubKeyPathpoints.Count - 1] as Quad3DPoint; //上一个Key路径点
            Vector3 direction = point - previousQuaPoint.Point_Center; //两个Key路径点的向量


            if (m_AllSubKeyPathpoints.Count == 1)
            { //处理第一个Key点
                //Debug.Log("Deal With The First Point");
                previousQuaPoint.CenterDownUptDirNor = Vector3.Cross(new Vector3(-1, 0, 0), direction).normalized;
                previousQuaPoint.CenterLeftDirNor = Vector3.Cross(direction, previousQuaPoint.CenterDownUptDirNor).normalized;

                m_AllSubMeshPoints.Add(previousQuaPoint.GetQuadUpRight());   //Up Right
                m_AllSubMeshPoints.Add(previousQuaPoint.GetQuadUpLeft());    //Up left
                m_AllSubMeshPoints.Add(previousQuaPoint.GetQuadDownRight());   //Down Right
                m_AllSubMeshPoints.Add(previousQuaPoint.GetQuadDownLeft());    //Down Left

                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 2);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 3);  //下三角

                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 2);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 3);  //
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 4);  //上三角
            }

            List<Quad3DPoint> subQuadPoint = new List<Quad3DPoint>(); // 子节点Point
            if (m_AllSubKeyPathpoints.Count >= 2)
            {
                Vector3 KeyPathDir1 = m_AllSubKeyPathpoints[m_AllSubKeyPathpoints.Count - 1].Point_Center - m_AllSubKeyPathpoints[m_AllSubKeyPathpoints.Count - 2].Point_Center; //上两个KeyPath Point 方向
                Vector3 KeyPathDir2 = point - m_AllSubKeyPathpoints[m_AllSubKeyPathpoints.Count - 1].Point_Center;  //上一个KeyPath 点到当前KeyPath Point 的方向
                float angle = Vector3.Angle(KeyPathDir1, KeyPathDir2);
                Debug.Log("angle=" + angle);
                //if (angle >= 89.5f)
                //{   //需要绘制一个三角锥 而不是长方体
                //    if (point.y < AllKeyPathpoints[AllKeyPathpoints.Count - 1].Point_Center.y)
                //    { //在下面
                //    }
                //    else
                //    {

                //    }
                //    return;
                //}//if
            }//if

            //获得左右两个面曲线的控制点
            Vector3 controllPointLeftRight = VectorExpand.GetPointPerpendicularTo(previousQuaPoint.GetQuadUpLeft() - new Vector3(0, previousQuaPoint.LineHeight / 2f, 0), previousQuaPoint.Point_Center, point);  //获得垂直于上一个QuadPoint 点且在direction 平面的单位控制点
            for (int dex = 0; dex < previousQuaPoint.SubsectionCount; dex++)
            {
                Vector3 _subPoint = Curve.CalculateBezier(previousQuaPoint.Point_Center, point, controllPointLeftRight, (dex + 1) * 1f / (previousQuaPoint.SubsectionCount));  //根据二阶贝塞尔曲线获得 一个分割点
                Quad3DPoint currentSubPoint;
                int previousStartIndex = m_AllSubMeshPoints.Count - 4;
                if (dex == 0)
                    currentSubPoint = CreateSubKeyPoint(previousQuaPoint, _subPoint) as Quad3DPoint;
                else
                    currentSubPoint = CreateSubKeyPoint(subQuadPoint[subQuadPoint.Count - 1], _subPoint) as Quad3DPoint;

                subQuadPoint.Add(currentSubPoint);

                #region Up Plane
                m_AllSubTrangles.Add(previousStartIndex);
                m_AllSubTrangles.Add(previousStartIndex + 1);
                m_AllSubMeshPoints.Add(currentSubPoint.GetQuadUpRight());
                m_AllSubMeshPoints.Add(currentSubPoint.GetQuadUpLeft());
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1); //上面 左三角

                m_AllSubTrangles.Add(previousStartIndex);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1); //上面 右三角
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 2);
                #endregion

                #region Down Plane
                m_AllSubMeshPoints.Add(currentSubPoint.GetQuadDownRight());
                m_AllSubMeshPoints.Add(currentSubPoint.GetQuadDownLeft());

                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 2);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1);
                m_AllSubTrangles.Add(previousStartIndex + 2); //下面 右倒三角以显示

                m_AllSubTrangles.Add(previousStartIndex + 2);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1);
                m_AllSubTrangles.Add(previousStartIndex + 3); ; //下面左倒三角以显示
                #endregion

                #region LeftPlane
                m_AllSubTrangles.Add(previousStartIndex + 1);
                m_AllSubTrangles.Add(previousStartIndex + 3);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1); //左侧 到下三角

                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 3);
                m_AllSubTrangles.Add(previousStartIndex + 1); //左侧倒上三角

                #endregion

                #region Right Plane
                m_AllSubTrangles.Add(previousStartIndex);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 4);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 2);

                m_AllSubTrangles.Add(previousStartIndex);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 2);
                m_AllSubTrangles.Add(previousStartIndex + 2);

                #endregion

                #region Forward Plane
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 2);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 4);   //前倒 下三角


                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 1);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 4);
                m_AllSubTrangles.Add(m_AllSubMeshPoints.Count - 3);   //前倒 上三角

                #endregion

                if (dex == previousQuaPoint.SubsectionCount - 1)
                {
                    m_AllSubKeyPathpoints.Add(currentSubPoint);
                    //Debug.Log("Add Key Path " + dex);
                }
            }
        }

    }


    [System.Serializable]
    public class Quad3DPoint : BaseMeshPoint
    {
        public float LineHeight = 2f; //Mesh厚度
        public Vector3 CenterLeftDirNor; //中心点向左的单位向量
        public Vector3 CenterDownUptDirNor; //中心点向左的单位向量
        public int SubsectionCount = 5; //两个点被分割的段数


        public Quad3DPoint(Vector3 point) : base(point)
        {

        }

        public Vector3 GetQuadUpRight()
        {
            return Point_Center + LineHeight / 2f * CenterDownUptDirNor - LineWidth / 2f * CenterLeftDirNor;
        }

        public Vector3 GetQuadUpLeft()
        {
            return Point_Center + LineHeight / 2f * CenterDownUptDirNor + LineWidth / 2f * CenterLeftDirNor;
        }

        public Vector3 GetQuadDownRight()
        {
            return Point_Center - LineHeight / 2f * CenterDownUptDirNor - LineWidth / 2f * CenterLeftDirNor;
        }
        public Vector3 GetQuadDownLeft()
        {
            return Point_Center - LineHeight / 2f * CenterDownUptDirNor + LineWidth / 2f * CenterLeftDirNor;
        }


    }
}