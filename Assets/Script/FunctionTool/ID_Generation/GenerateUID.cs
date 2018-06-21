using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 生成指定长度的ID
    /// </summary>
    public class GenerateUID
    {
        private static string[] Keys = new string[]
        {
        "A","B","C","D",  "E","F","G","H",  "I","J","K","L",  "M","N","O","P",  "Q","R","S","T",  "U","V","W","X",  "Y","Z",
        "a","b","c","d",  "e","f","g","h",  "i","j","k","l",  "m","n","o","p",  "q","r","s","t",  "u","v","w","x",  "y","z",
        "0","1",  "2","3","4","5","6","7","8","9",
        };


        /// <summary>
        /// 生成32位的Id
        /// </summary>
        /// <returns></returns>
        public static string GetUID32()
        {
            StringBuilder result = new StringBuilder();
            for (int dex = 0; dex < 32; ++dex)
            {
                int value = Mathf.RoundToInt(Random.value * (Keys.Length - 1));
                result.Append(Keys[value]);
            }
            //   Debug.Log(result.ToString());
            return result.ToString();
        }
        /// <summary>
        /// 生成64位的Id
        /// </summary>
        /// <returns></returns>
        public static string GetUID64()
        {
            StringBuilder result = new StringBuilder();
            for (int dex = 0; dex < 64; ++dex)
            {
                int value = Mathf.RoundToInt(Random.value * (Keys.Length - 1));
                result.Append(Keys[value]);
            }
            //      Debug.Log(result.ToString());
            return result.ToString();
        }


    }
}