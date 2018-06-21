using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 只执行一次的循环
    /// </summary>
    public class SingleLoopSpriteSequenceInfor : BaseSpriteSequenceInfor
    {

        #region 创建
        public SingleLoopSpriteSequenceInfor(string[] spritesPath, string name, StopSpriteSequence condition, bool clearPrevious)
            : base(spritesPath, name, condition, clearPrevious)
        {
            IsLoopSchema = false;
        }

        public SingleLoopSpriteSequenceInfor(IEnumerable<Sprite> sprites, string name, StopSpriteSequence condition, bool clearPrevious)
           : base(sprites, name, condition, clearPrevious)
        {
            IsLoopSchema = false;
        }

        #endregion

        #region 遍历
        public override bool GetSpriteOneByOne()
        {
            if (IsForcePauseLoop ) return false; //被强制暂停中

            if (IsStopSequence)
            {
                SetCurrentSprite_ByIndex(m_RecordIndex);
                return false;  //停止了动画（已经播放完成）
            } //结束循环

            if (m_AllSourceSprite.Count == 0)
            {
                EmptySource();
                return true;
            }
            //if(CurrentSprite!=null)
            //Debug.Log(">>>>>>>>"+CurrentSprite.name+ "  m_RecordIndex="+ m_RecordIndex);

            CheckFirstStart();
            if (CheckSequenceBreak()) return false;
            ++m_RecordIndex;

            NormalLoop();
            OneLoopEndCheck();

            return true;
        }

        protected override bool CheckSequenceBreak()
        {
            //return base.CheckSequenceBreake();
            if (CheckCondition())
            {
                CurrentSprite = m_AllSourceSprite[m_RecordIndex];
                IsStopSequence = true;
                if (BreakAction != null)
                    BreakAction();
                return true;
            }
            return false;
        }

        protected override void OneLoopEndCheck()
        {
            //base.OneLoopEndCheck();
            if (m_RecordIndex < m_AllSourceSprite.Count - 1)
            {
                IsTheEndOfThisLoop = false;
                return;
            }
            if (m_RecordIndex > m_AllSourceSprite.Count - 1)
                m_RecordIndex = -1;
            IsTheEndOfThisLoop = true;


            Debug.Log("Out Of Range " + m_RecordIndex + "   Stop Sequence");
            IsStopSequence = true;
            IsForcePauseLoop = true;
            SetCurrentSprite_End();

            if (EndAction != null)
                EndAction();
        }
        #endregion

        #region 继续序列帧
        protected override bool CheckCanSequenceLoop()
        {
            if (m_AllSourceSprite.Count == 0) return false;
            if (m_RecordIndex < m_AllSourceSprite.Count - 1)
            {
                SetCurrentSprite_ByIndex(m_RecordIndex + 1);
                IsStopSequence = false;
                return true;
            }
            return false;
        }
        #endregion

        #region 重新开始序列帧动画
        public override void ReStartSequence()
        {
            base.ReStartSequence();
            m_RecordIndex = -1;
            IsStopSequence = false;
            IsTheEndOfThisLoop = false;
            SetCurrentSprite_First();
        }
        #endregion

        #region 强制停止序列帧
        public override void ForcePauseSequence() {
            IsForcePauseLoop = true;
        }
        #endregion

        #region 恢复暂停
        public override void RecoveryPauseSequence()
        {
            IsForcePauseLoop = false;
        }
        #endregion

        //#region 停止一个序列帧 
        //public override void StopSequence() {

        //}
        //#endregion


    }
}