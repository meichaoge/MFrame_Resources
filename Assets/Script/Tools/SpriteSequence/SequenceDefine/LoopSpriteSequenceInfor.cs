using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 需要使用者自己注意中断条件和结束条件是否都需要，这里没有处理这种同时存在的情况
    /// </summary>
    public class LoopSpriteSequenceInfor : BaseSpriteSequenceInfor
    {
        private float m_LastTimeRecordTime_Break = 0f; //上一次break 暂停时间
        public bool IsWaitingTick { private set; get; } //标识是否正在等待中
        public bool IsWaitingTick_End { private set; get; } //标识是否在一次循环后处于等待
        public float DelayTimeAfterBreak { private set; get; } //一次中断后多久自动启动下一轮

        private float m_LastTimeRecordTime_End = 0f;
        public float DelayTimeAfterEnd { private set; get; } //完成一次循环多久自动启动下一轮

        #region 创建
        public LoopSpriteSequenceInfor(string[] spritesPath, string name, StopSpriteSequence condition, bool clearPrevious, float delayTimeBreak,float delayTimeEnd)
            : base(spritesPath, name, condition, clearPrevious)
        {
            IsLoopSchema = true;
            DelayTimeAfterBreak = delayTimeBreak;
            DelayTimeAfterEnd = delayTimeEnd;
            IsWaitingTick = false;
        }

        public LoopSpriteSequenceInfor(IEnumerable<Sprite> sprites, string name, StopSpriteSequence condition, bool clearPrevious, float delayTimeBreak, float delayTimeEnd)
           : base(sprites, name, condition, clearPrevious)
        {
            IsLoopSchema = true;
            DelayTimeAfterBreak = delayTimeBreak;
            DelayTimeAfterEnd = delayTimeEnd;
            IsWaitingTick = false;
        }

        #endregion

        #region  遍历获取
        public override bool GetSpriteOneByOne()
        {
            if (IsForcePauseLoop) return false; //暂停中

            if (IsWaitingTick)
            {//中断或者末尾
                CheckCanSequenceLoop();
                return false;
            }

            if (IsWaitingTick_End)
            {
                CheckCanSequenceLoop_End();
                return false;
            }
            if (m_AllSourceSprite.Count == 0)
            {
                EmptySource();
                return true;
            }

            CheckFirstStart();
            if (CheckSequenceBreak()) return false;
            ++m_RecordIndex;

            NormalLoop();
            OneLoopEndCheck();
            return true;
        }

        protected override bool CheckSequenceBreak()
        {
            if (CheckCondition())
            {
                CurrentSprite = m_AllSourceSprite[m_RecordIndex];
                if (BreakAction != null)
                    BreakAction();

                if (DelayTimeAfterBreak >= 0)
                {
                    m_LastTimeRecordTime_Break = Time.time; //中断成立开始计时
                    IsWaitingTick = true;
                    return true;
                }
                else
                {
                    IsWaitingTick = false;
                    return false;
                }//中断成立但是设置的中断时间小于=0  继续运行
            } //中断循环成立
            return false;
        }

        protected override void OneLoopEndCheck()
        {
            //  base.OneLoopEndCheck();
            if (m_RecordIndex < m_AllSourceSprite.Count - 1)
            {
                IsTheEndOfThisLoop = false;
                return;
            }
            if (m_RecordIndex > m_AllSourceSprite.Count - 1)
                m_RecordIndex = -1;


            IsTheEndOfThisLoop = true;
            m_RecordIndex = -1;
            if (EndAction != null)
                EndAction();
            if (DelayTimeAfterEnd > 0)
            {
                m_LastTimeRecordTime_End = Time.time;
                IsWaitingTick_End = true;
            }
            else
            {
                IsWaitingTick_End = false;
            }
            return;
        }

        #endregion

        #region 继续序列帧
        //不需要这个自己不断检测
        public override bool ContinueSequence()
        {
            if (IsForcePauseLoop) IsForcePauseLoop = false;
            return false;
        }

        protected override bool CheckCanSequenceLoop()
        {
            if (IsWaitingTick == false || DelayTimeAfterBreak <= 0) return true;

            if (Time.time - m_LastTimeRecordTime_Break >= DelayTimeAfterBreak)
            {
                Debug.Log("This Loop Break Waiting Over"); //
                m_LastTimeRecordTime_Break = Time.time;
                IsWaitingTick = false;
                IsTheEndOfThisLoop = false;
                ResetSequence();
                return true;
            }
            IsWaitingTick = true;
          //  Debug.Log("Waiting Remain .. " + (Time.time - m_LastTimeRecordTime_Break)+ "  >> DelayTimeAfterBreak="+ DelayTimeAfterBreak);
            return false;
        }

        /// <summary>
        /// 检测到一次循环之后是否可以开始新的循环
        /// </summary>
        /// <returns></returns>
        protected bool CheckCanSequenceLoop_End()
        {
            if (IsTheEndOfThisLoop == false || DelayTimeAfterEnd <= 0 || IsWaitingTick_End == false) return true;

            if (Time.time - m_LastTimeRecordTime_End >= DelayTimeAfterEnd)
            {
//                Debug.Log("This Loop End Waiting Over"); //
                m_LastTimeRecordTime_End = Time.time;
                IsWaitingTick_End = false;
                ResetSequence();
                return true;
            }
            IsWaitingTick_End = true;
           // Debug.Log("WaitingLoop End Remain .. " + (Time.time - m_LastTimeRecordTime_End) + "     >>DelayTimeAfterEnd="+ DelayTimeAfterEnd);
            return false;
        }

        protected override void ResetSequence()
        {
            IsStopSequence = false;
            //必须更新状态否则下次检测条件可能还是成立
            if (m_RecordIndex <= m_AllSourceSprite.Count - 1)
            {
                SetCurrentSprite_ByIndex(m_RecordIndex + 1);
            }
            else
            {
                SetCurrentSprite_End();
            }
        }
        #endregion

        #region 重新开始序列帧动画
        public override void ReStartSequence()
        {
            base.ReStartSequence();
            m_LastTimeRecordTime_Break = Time.time;
            m_RecordIndex = -1;
            IsStopSequence = false;  
            IsTheEndOfThisLoop = false;
            IsWaitingTick = false; //恢复中断
            IsWaitingTick_End = false;
            SetCurrentSprite_First();
        }
        #endregion

        #region 强制停止序列帧
        public override void ForcePauseSequence()
        {
            IsForcePauseLoop = true;
        }
        #endregion

        #region 恢复暂停
        public override void RecoveryPauseSequence() {
            IsForcePauseLoop = false;
        }
        #endregion
    }
}