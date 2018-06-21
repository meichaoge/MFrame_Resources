using UnityEngine;
using System.Collections;

namespace MFramework
{
    /// <summary>
    /// Helper Class To Set Tip Box Trans
    /// </summary>
    public class UIShowTipHelper : MonoBehaviour
    {
        /// <summary>
        /// 是否跟随当前对象位置
        /// </summary>
        public bool KeyBoardFollowTarget = false; 
        public Vector3 ShowOffSet = Vector3.zero;
        public Vector3 ShowAngle = Vector3.zero;
        public Vector3 ShowScale = Vector3.one;

        private void OnMouseUpAsButton()
        {
           
        }
    }

}