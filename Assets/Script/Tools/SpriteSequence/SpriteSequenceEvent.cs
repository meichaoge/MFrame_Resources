using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    ///  序列帧执行序列
    /// </summary>
    public class SpriteSequenceEvent
    {

        public int m_HashCode;
        public UnityEngine.UI.Image m_Img;
        public BaseSpriteSequenceInfor m_SequenceInfor;

        /// <summary>
        /// 执行序列帧获取prite 并设置
        /// </summary>
        /// <returns>当前序列帧是否已经停止或者结束</returns>
        public void DoSequence()
        {
            if (m_Img == null || m_SequenceInfor == null) return;
            if (m_SequenceInfor.GetSpriteOneByOne())
            {
                if (m_SequenceInfor.CurrentSprite != null)
                    m_Img.sprite = m_SequenceInfor.CurrentSprite;
            }
        }

        //public bool CheckNeedStop()
        //{
        //    if (m_Img == null || m_SequenceInfor == null) return true;

        //    if (m_SequenceInfor.IsStopSequence) return true;
        //    return false;
        //}
    }
}