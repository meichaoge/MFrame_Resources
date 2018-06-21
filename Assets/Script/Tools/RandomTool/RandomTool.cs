using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{
    /// <summary>
    /// Helper Tool For Get Random Value
    /// </summary>
    public class RandomTool
    {
        public static int[] GetRandomInt(int @From, int @To, int ResultLength = 3)
        {
            List<int> result = new List<int>();
            if (ResultLength < 1) return new int[0];
            result.Add(Random.Range(@From, @To));
            while (result.Count < ResultLength)
            {
                int randomValue = Random.Range(@From, @To);
                if (randomValue != result[result.Count - 1])
                    result.Add(randomValue);
            }

            return result.ToArray();
        }

        public static float[] GetRandomFloat(float @From, float @To, int ResultLength = 3)
        {
            List<float> result = new List<float>();
            if (ResultLength < 1) return new float[0];
            result.Add(Random.Range(@From, @To));
            while (result.Count < ResultLength)
            {
                float randomValue = Random.Range(@From, @To);
                if (Mathf.Approximately(randomValue, result[result.Count - 1]) == false)
                    result.Add(randomValue);
            }

            return result.ToArray();
        }
    }
}
