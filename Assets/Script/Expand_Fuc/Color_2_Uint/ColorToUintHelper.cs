using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Color 类型与Uint 转换
public static class ColorToUintHelper
{
    //Color 转成Uint 存储和表示
    public static uint Color2Uint(this Color @From)
    {
        return BitConverter.ToUInt32(new byte[] {
            (byte) (@From.r*255),
            (byte) (@From.g*255),
            (byte) (@From.b*255),
            (byte) (@From.a*255),
        }, 0);
    }


    public static int Color2Int(this Color @From)
    {
        return (int)@From.Color2Uint();
    }

    //Uint To Color
    public static Color Uint2Color(this uint @From)
    {
        byte[] color = BitConverter.GetBytes(@From);
        return new Color(color[0] * 1f / 255, color[1] * 1f / 255, color[2] * 1f / 255, color[3] * 1f / 255);
    }


}
