using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{

    /// <summary>
    /// 自定义封装Unity.Vector2 由于Unity.vector2无法序列化
    /// </summary>
    [Serializable]
    public class SerelizeVector2_Float
    {
        public float X;
        public float Y;

        public SerelizeVector2_Float(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
        }

        public SerelizeVector2_Float(Vector2 vec)
        {
            X = vec.x;
            Y = vec.y;
        }

        public static Vector2 SerelizeVector2_Float2Vector2(SerelizeVector2_Float vec)
        {
            if (vec == null) return Vector2.zero;
            return new Vector2(vec.X, vec.Y);
        }

        public Vector2 SerelizeVector2_Float2Vector2()
        {
            return new Vector2(X, Y);
        }

        public override string ToString()
        {
            return "X=" + X + "   Y=" + Y ;
        }
    }
}
