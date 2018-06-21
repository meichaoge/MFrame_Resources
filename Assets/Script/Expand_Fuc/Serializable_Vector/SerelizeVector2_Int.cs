using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{

    /// <summary>
    ///  �Զ����װUnity.Vector2, ����Unity.vector2 �޷����л� ,��Ϊ��������
    /// </summary>
    [Serializable]
    public class SerelizeVector2_Int 
    {
        public int X;
        public int Y;

        public SerelizeVector2_Int(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public SerelizeVector2_Int(Vector3 vec)
        {
            X = (int)vec.x;
            Y = (int)vec.y;
        }

        public SerelizeVector2_Int(Vector2 vec)
        {
            X = (int)vec.x;
            Y = (int)vec.y;
        }

        public static Vector2 SerelizeVector2_Float2Vector2(SerelizeVector2_Int vec)
        {
            if (vec == null) return Vector2.zero;
            return new Vector2(vec.X, vec.Y);
        }

        public override string ToString()
        {
            return "X=" + X + "   Y=" + Y;
        }

    }
}
