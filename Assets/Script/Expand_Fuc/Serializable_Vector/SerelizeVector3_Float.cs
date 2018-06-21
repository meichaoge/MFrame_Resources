using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace MFramework
{
    /// <summary>
    /// 自定义封装Unity.Vector3, 由于Unity.vector3无法序列化
    /// </summary>
    [Serializable]
    public class SerelizeVector3_Float 
    {
        public float X;
        public float Y;
        public float Z;


        public SerelizeVector3_Float(float x)
        {
            X = x;
            Debug.Log("ddddddddddddddddddddddddddddddddd");
        }

        public SerelizeVector3_Float(float x=0,float y = 0,float z=0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public SerelizeVector3_Float(Vector3 vec)
        {
            X = vec.x;
            Y = vec.y;
            Z = vec.z;
        }

        /// <summary>
        /// SerelizeVector3_Float 转换成Unity.Vector3
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector3 SerelizeVector3_Float2Vector3(SerelizeVector3_Float vec)
        {
            if (vec == null) return Vector3.zero;
            return new Vector3(vec.X, vec.Y, vec.Z);
        }

        public Vector3 SerelizeVector3_Float2Vector3()
        {
            return new Vector3(X, Y, Z);
        }



        public override string ToString()
        {
            return "X=" + X + "   Y=" + Y + "  Z=" + Z;
        }


    }
}
