                                                                                    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{

    public static class VectorExpand
    {

        //��ô�ֱ������P1p2������p1p2,p1p3������ͬһƽ��ĵ�����(�����Point1)
        //�൱����֪һ��ƽ���������� ������һ�������һ��������
        public static Vector3 GetPointPerpendicularTo(Vector3 Point1, Vector3 point2, Vector3 point3, bool IsPosition = true)
        {
            Vector3 P1p2 = point2 - Point1; //P1p2 ����
            Vector3 P1P3 = point3 - Point1; //����p1p3

            Vector3 P1P4CrossP1p2p3 = Vector3.Cross(P1p2, P1P3);  //��ֱ��p1p2 ,p1p3������ �õ�����p4
            Vector3 P1P5CrossP1p2 = Vector3.Cross(P1P4CrossP1p2p3, P1p2); //��ֱ��p4 �Լ�p1p2��
            P1P5CrossP1p2 = P1P5CrossP1p2.normalized;
            if (IsPosition)
                return P1P5CrossP1p2 + Point1;  //��������
            else
                return P1P5CrossP1p2; //����һ����λ��������
        }
    }
}
