using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MFramework.EditorExpand
{


#if UNITY_EDITOR
    /// <summary>
    /// 标识所有的UI上标记Raycast 的对象
    /// </summary>
    public class EShowRayCastImage : MonoBehaviour
    {
        void Start()
        {

        }

        static Vector3[] fourCorners = new Vector3[4];
        void OnDrawGizmos()
        {
            foreach (MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>())
            {
                if (g.raycastTarget)
                {
                    RectTransform rectTransform = g.transform as RectTransform;
                    rectTransform.GetWorldCorners(fourCorners);
                    Gizmos.color = Color.blue;
                    for (int i = 0; i < 4; i++)
                        Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);

                }
            }
        }
    }
#endif

}