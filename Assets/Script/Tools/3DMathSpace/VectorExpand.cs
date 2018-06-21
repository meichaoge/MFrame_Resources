                                                                                    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{

    public static class VectorExpand
    {

        //获得垂直于向量P1p2，且于p1p2,p1p3向量在同一平面的点坐标(相对于Point1)
        //相当于已知一个平面上三个点 求另外一个点组成一个长方形
        public static Vector3 GetPointPerpendicularTo(Vector3 Point1, Vector3 point2, Vector3 point3, bool IsPosition = true)
        {
            Vector3 P1p2 = point2 - Point1; //P1p2 向量
            Vector3 P1P3 = point3 - Point1; //向量p1p3

            Vector3 P1P4CrossP1p2p3 = Vector3.Cross(P1p2, P1P3);  //垂直于p1p2 ,p1p3的向量 得到向量p4
            Vector3 P1P5CrossP1p2 = Vector3.Cross(P1P4CrossP1p2p3, P1p2); //垂直于p4 以及p1p2中
            P1P5CrossP1p2 = P1P5CrossP1p2.normalized;
            if (IsPosition)
                return P1P5CrossP1p2 + Point1;  //返回坐标
            else
                return P1P5CrossP1p2; //返回一个单位方向向量
        }
    }
}
