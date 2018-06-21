using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 以一定的角度范围进行扇形布局 并可以规定两个扇形区域之间呈现一个指定角度的夹角
    /// 根据数据的数量会进行计算 调整两个扇形之间的角度
    /// </summary>
    public class SectorLayout : MonoBehaviour
    {
        /// <summary>
        /// 扇形布局点
        /// </summary>
        public class SectorLayoutPointInfor
        {
            public Vector3 m_WorldPos;
            public Vector3 m_RalativePos;
            public float m_Angle;  //确保旋转后中心线对准原点

            public SectorLayoutPointInfor()
            {
                m_RalativePos= m_WorldPos = Vector3.zero;
                m_Angle = 0;
            }
        }

        #region  UI Refence
        [SerializeField]
        private Transform m_SectorLayoutCenterTrans;  //中心点

        public Transform SectorLayoutCenterTrans { get { return m_SectorLayoutCenterTrans; } }
        #endregion

        #region  属性
        [SerializeField]
        [Range(0, 500)]
        private float m_Radius = 50;   //半径
        public float Radiu { get { return m_Radius; } }

        [SerializeField]
        [Range(1, 180)]
        private float m_MaxLayoutAngle = 60;  //以中心向两边最大的展开角度

        [SerializeField]
        [Range(1, 90)]
        [Tooltip("Min Angle Between Two Sector Point")]
        private float m_AngleOffset_Min = 5f;  //两个扇形区域之间最小的夹角

        [SerializeField]
        [Range(1, 90)]
        [Tooltip("Max Angle Between Two Sector Point")]
        private float m_AngleOffset_Max = 40; //两个扇形区域之间最大的夹角

        List<SectorLayoutPointInfor> m_AllSectorLayoutPoints = new List<SectorLayoutPointInfor>();  //所有划分出来的点
        #endregion

        /// <summary>
        /// 获得扇形布局的点的数据 要求根据这个点生成的对象的中心点必须是(0.5f,0)
        /// </summary>
        /// <param name="pointCount"></param>
        /// <returns></returns>
        public List<SectorLayoutPointInfor> GetSectorLayout(uint pointCount)
        {
            m_AllSectorLayoutPoints.Clear();
            if (pointCount % 2 == 0)
            {
                //Debug.LogInfor("偶数布局");
                CaculateSectorLayoutPoint(pointCount / 2, false, false); //左侧
                CaculateSectorLayoutPoint(pointCount / 2, false, true); //右侧
            }
            else
            {
                //  Debug.LogInfor("奇数布局");
                CaculateSectorLayoutPoint((pointCount - 1) / 2, true, false);
                SectorLayoutPointInfor middlePoint = new SectorLayoutPointInfor();
                middlePoint.m_RalativePos = new Vector3(0, m_Radius, 0);
                middlePoint.m_WorldPos = m_SectorLayoutCenterTrans.position + middlePoint.m_RalativePos;
                m_AllSectorLayoutPoints.Add(middlePoint); //记录当前生成的中心点
                CaculateSectorLayoutPoint((pointCount - 1) / 2, true, true);
            }
            return m_AllSectorLayoutPoints;
        }

        /// <summary>
        /// 计算扇形布局点的位置
        /// </summary>
        /// <param name="count">需要计算的点的数量</param>
        /// <param name="isStartFromMiddle">是否从正中间点开始计算 (当数据是奇数个时候为true,偶数个应该为false)</param>
        /// <param name="isLeftToRight">中心店右边的布局是true ,左边布是false</param>
        private void CaculateSectorLayoutPoint(uint count, bool isStartFromMiddle, bool isLeftToRight)
        {
            if (count == 0) return;
            List<SectorLayoutPointInfor> allSectorPoints = new List<SectorLayoutPointInfor>();  //当前创建出来的点

            #region 计算两个扇形区域之间的夹角 并限定在指定[m_AngleOffset_Min,m_AngleOffset_Max] 之间
            float angleOffset = m_MaxLayoutAngle / count;  //初步计算出来两个扇形区域之间的角度
            //   Debug.LogInfor("angleOffset=" + angleOffset);
            //使得两个扇形区域之间的角度范围被限制住在[m_AngleOffset_Min,m_AngleOffset_Max]
            if (m_AngleOffset_Min < m_AngleOffset_Max)
                angleOffset = Mathf.Clamp(angleOffset, m_AngleOffset_Min, m_AngleOffset_Max);
            else
                angleOffset = Mathf.Clamp(angleOffset, m_AngleOffset_Max, m_AngleOffset_Min);
            #endregion

            for (int dex = 1; dex <= count; ++dex)
            {
                #region 根据点的索引计算每个点再扇形布局上的坐标以及需要旋转的角度
                float curOffsetAngle = 0;
                if (isStartFromMiddle)
                    curOffsetAngle = dex * angleOffset;
                else
                    curOffsetAngle = dex * angleOffset - angleOffset / 2f; //为了确保两个点之间的夹角相同这里必须减去半个夹角才能确保中间的两个点之间也是相差 angleOffset

                SectorLayoutPointInfor trans = new SectorLayoutPointInfor();
                if (isLeftToRight)
                    trans.m_Angle = -1 * curOffsetAngle;
                else
                    trans.m_Angle = curOffsetAngle;

                curOffsetAngle = Mathf.Deg2Rad * curOffsetAngle; //转换成弧度
                if (isLeftToRight)
                    trans.m_RalativePos = new Vector3(Mathf.Sin(curOffsetAngle), Mathf.Cos(curOffsetAngle), 0) * m_Radius;
                else
                    trans.m_RalativePos = new Vector3(-1 * Mathf.Sin(curOffsetAngle), Mathf.Cos(curOffsetAngle), 0) * m_Radius;
                trans.m_WorldPos = trans.m_RalativePos + m_SectorLayoutCenterTrans.position;

                //if (isLeftToRight)
                //    trans.m_WorldPos = m_SectorLayoutCenterTrans.position + new Vector3(Mathf.Sin(curOffsetAngle), Mathf.Cos(curOffsetAngle), 0) * m_Radius;
                //else
                //    trans.m_WorldPos = m_SectorLayoutCenterTrans.position + new Vector3(-1 * Mathf.Sin(curOffsetAngle), Mathf.Cos(curOffsetAngle), 0) * m_Radius;
                #endregion

                allSectorPoints.Add(trans); //记录当前生成的点
            }

            if (isLeftToRight == false)
                allSectorPoints.Reverse();  //确保记录的数据是从左向右一次添加的
            m_AllSectorLayoutPoints.AddRange(allSectorPoints);
        }

    }
}
