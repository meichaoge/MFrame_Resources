using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{
    /// <summary>
    /// 自定义封装Unity.Vector3, 由于Unity.vector3无法序列化 ,简化为整形数据
    /// </summary>
    [Serializable]
    public class SerelizeVector3_Int
    {
        public int X;
        public int Y;
        public int Z;


        public SerelizeVector3_Int(int x = 0, int y = 0, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public SerelizeVector3_Int(Vector3 vec)
        {
            X = (int)vec.x;
            Y = (int)vec.y;
            Z = (int)vec.z;
        }

        /// <summary>
        /// SerelizeVector3_Float 转换成Unity.Vector3
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public Vector3 SerelizeVector3_Float2Vector3(SerelizeVector3_Float vec)
        {
            if (vec == null) return Vector3.zero;
            return new Vector3(vec.X, vec.Y, vec.Z);
        }

    }
}
