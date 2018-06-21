using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    ////中断检测条件
    public delegate bool StopSpriteSequence();
    public abstract class BaseSpriteSequenceInfor
    {
        #region  数据 事件 状态
        public string AnimationName { get; protected set; }  //帧动画名称
        protected List<Sprite> m_AllSourceSprite = new List<Sprite>();  //帧动画源文件

        //***序列帧执行时的事件
        public System.Action BeginAction { get; set; } //开始播放事件
        public System.Action PlayingSequqnceAction { get; set; } //正在播放事件
        public System.Action EndAction { get; set; } //结束本次循环事件
        public System.Action BreakAction { get; set; } //当设置 m_StopSpriteSequenceEvent 并执行检测成立中断本次循环的事件

        protected event StopSpriteSequence m_StopSpriteSequenceEvent;  //中断检测事件
        protected int m_RecordIndex = -1; //当前执行序列使用的精灵索引 -1 标识没有开始或者结束
                                          //状态标识
     //   public bool IsSequenceRuning { protected set; get; } //标识是否是在正常运转(单循环结束时以及Loop被强制暂停时为true)
        public bool IsLoopSchema { protected set; get; } //标识是否是循环模式
        public Sprite CurrentSprite { protected set; get; } //当前正在使用的Sprite

        //内部维护状态
        protected bool IsStopSequence {  set; get; } //标识是否停止当前序列帧动画
        protected bool IsTheEndOfThisLoop {  set; get; } //标识是否完成本次循环

        //是否被强制暂停了
        //SingleLoopSpriteSequenceInfor 结束一次循环或者中途被Pause
        //LoopSpriteSequenceInfor 被Pause
        protected bool IsForcePauseLoop { set; get;  }

        #endregion

        #region 创建序列帧
        public BaseSpriteSequenceInfor(string[] spritesPath, string name, StopSpriteSequence condition, bool clearPrevious)
        {
            SpriteSequenceInitial(name, condition, clearPrevious);

            if (spritesPath == null || spritesPath.Length == 0) return;
            for (int dex = 0; dex < spritesPath.Length; ++dex)
            {
                //    Sprite spr = ResourceMgr.instance.LoadSprite(spritesPath[dex]);
                Sprite spr = Resources.Load<Sprite>(spritesPath[dex]);
                m_AllSourceSprite.Add(spr);
            }
        }

        public BaseSpriteSequenceInfor(IEnumerable<Sprite> sprites, string name, StopSpriteSequence condition, bool clearPrevious)
        {
            SpriteSequenceInitial(name, condition, clearPrevious);
            if (sprites == null)
            {
                Debug.LogError("CreateSpriteAnimation Sprites Should Not Be Null");
            }
            else
            {
                foreach (var item in sprites)
                {
                    m_AllSourceSprite.Add(item);
                }
            }
        }


        protected virtual void SpriteSequenceInitial(string name, StopSpriteSequence condition, bool clearPrevious)
        {
            AnimationName = name;
            IsStopSequence = false;
            if (clearPrevious)
            {
                m_AllSourceSprite.Clear();
                m_RecordIndex = -1;
            }

            if (condition != null)
                m_StopSpriteSequenceEvent += condition;
        }

        #endregion

        #region  设置当前使用的精灵
        /// <summary>
        /// 获取第一个精灵
        /// </summary>
        protected void SetCurrentSprite_First()
        {
            if (m_AllSourceSprite.Count > 0)
            {
                CurrentSprite = m_AllSourceSprite[0];
            }
            else
            {
                CurrentSprite = null;
            }
        }
        /// <summary>
        /// 获取最后一个精灵
        /// </summary>
        protected void SetCurrentSprite_End()
        {
            if (m_AllSourceSprite.Count != 0)
                CurrentSprite = m_AllSourceSprite[m_AllSourceSprite.Count - 1];
            else
                CurrentSprite = null;
        }
        /// <summary>
        /// 根据当前的索引获取精灵
        /// </summary>
        protected void SetCurrentSprite_ByIndex(int dex)
        {
            if (dex != -1)
            {
    //            Debug.Log("Name;"+AnimationName+"dex=" + dex+" ;; "+ m_AllSourceSprite.Count);
                CurrentSprite = m_AllSourceSprite[dex];
            }
            else
            {
                CurrentSprite = null;
            }
        }

        #endregion

        #region   取序列帧中某一帧精灵
        public abstract bool GetSpriteOneByOne();
        /// <summary>
        /// 中断事件监测
        /// </summary>
        /// <returns></returns>
        protected bool CheckCondition()
        {
            if (m_StopSpriteSequenceEvent != null && m_StopSpriteSequenceEvent()) return true;
            return false;
        }

        /// <summary>
        /// 帧动画源文件为空
        /// </summary>
        protected virtual void EmptySource()
        {
            m_RecordIndex = -1;
            CurrentSprite = null;
            IsStopSequence = true;
            if (EndAction != null)
                EndAction();
            IsForcePauseLoop = true;
        }

        /// <summary>
        /// 第一次启动或者新的循环
        /// </summary>
        protected virtual void CheckFirstStart()
        {
            if (m_RecordIndex == -1 && m_AllSourceSprite.Count != 0)
            {
                if (BeginAction != null)
                    BeginAction();

                SetCurrentSprite_First();
                IsStopSequence = false;
            }
        }

        /// <summary>
        /// 中断检测
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckSequenceBreak();


        /// <summary>
        /// 重置状态
        /// </summary>
        protected virtual void ResetSequence()
        {
            m_RecordIndex = -1;
            IsStopSequence = false;
        }

        /// <summary>
        /// 当前循环到最末尾一张
        /// </summary>
        /// <returns></returns>
        protected virtual void OneLoopEndCheck()
        {

        }

        /// <summary>
        /// 正常循环
        /// </summary>
        /// <returns></returns>
        protected virtual void NormalLoop()
        {
            IsForcePauseLoop = false;
            CurrentSprite = m_AllSourceSprite[m_RecordIndex];
            if (PlayingSequqnceAction != null)
                PlayingSequqnceAction();
        }

        #endregion


        #region   继续序列帧
        public virtual bool ContinueSequence()
        {
            if (IsForcePauseLoop ) IsForcePauseLoop = false;
            return CheckCanSequenceLoop();
        }

        protected abstract bool CheckCanSequenceLoop();


        #endregion

        #region  重新开始序列帧动画
        public virtual void ReStartSequence() {
            IsForcePauseLoop = false; //恢复暂停
        }
        #endregion

        #region 强制停止序列帧
        public abstract void ForcePauseSequence();
        #endregion

        #region 恢复暂停
        public abstract void RecoveryPauseSequence();
        #endregion

        #region 停止一个序列帧 
       public void StopAndClearSequence()
        {
            m_AllSourceSprite.Clear();
        }
        #endregion

    }
}