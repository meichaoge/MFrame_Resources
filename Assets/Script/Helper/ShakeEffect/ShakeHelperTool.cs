using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace MFramework.Tween
{
    /// <summary>
    /// 震屏效果帮助类  
    /// </summary>
    public class ShakeHelperTool : Singleton_Static<ShakeHelperTool>
    {
        protected override void InitialSingleton() {  }


        /// <summary>
        /// 震屏特效
        /// </summary>
        /// <param name="curve1"></param>
        /// <param name="curve2"></param>
        /// <param name="target"></param>
        /// <param name="distance">最远震动偏移距离</param>
        /// <param name="tweenTime"></param>
        /// <param name="onCompleteAction"></param>
        public Sequence ShakePosition(AnimationCurve curve1,  Transform target, Vector3 distance, float tweenTime, System.Action<Transform> onCompleteAction)
        {
            Sequence seque = DOTween.Sequence();
            Vector3 m_RecordPosition = target.position;
            float shakeValue1 = 0;
            Tweener move1 = DOTween.To(() => shakeValue1, x => shakeValue1 = x, 1, tweenTime ).SetEase(curve1).OnUpdate(() =>
            {
                target.position = m_RecordPosition + distance * shakeValue1;
            }).OnComplete(() =>
            {
                //Debug.LogInfor("ShakePosition   Step1");
                shakeValue1 = 1;

                target.position = m_RecordPosition;  //恢复位置
                if (onCompleteAction != null)
                    onCompleteAction(target);
            });

            seque.Join(move1);//.Join(move2);
            return seque;
        }



        /// <summary>
        /// 震屏特效
        /// </summary>
        /// <param name="curve1"></param>
        /// <param name="target"></param>
        /// <param name="distance">最远震动偏移距离</param>
        /// <param name="tweenTime"></param>
        /// <param name="onCompleteAction"></param>
        public Sequence ShakeLocalPosition(AnimationCurve curve1, Transform target, Vector3 distance, float tweenTime, System.Action<Transform> onCompleteAction)
        {
            Sequence seque = DOTween.Sequence();
            Vector3 m_RecordLocalPostion = target.localPosition;
            float shakeValue = 0;
            Tweener move1 = DOTween.To(() => shakeValue, x => shakeValue = x, 1, tweenTime ).SetEase(curve1).OnUpdate(() =>
            {
                target.localPosition = m_RecordLocalPostion + distance * shakeValue;
            }).OnComplete(() =>
            {
                //Debug.LogInfor("ShakeLocalPosition   Step1");
                shakeValue = 1;
                target.localPosition = m_RecordLocalPostion;  //恢复位置
                if (onCompleteAction != null)
                    onCompleteAction(target);
            });

            seque.Join(move1); //.Join(move2);
            return seque;
        }


        /// <summary>
        /// 震屏特效
        /// </summary>
        /// <param name="curve1"></param>
        /// <param name="target"></param>
        /// <param name="distance">最远震动偏移距离</param>
        /// <param name="tweenTime"></param>
        /// <param name="delayTime">延迟多久执行  由于Sequence 无法添加延时</param>
        /// <param name="onCompleteAction"></param>
        public Sequence ShakeLocalPosition(AnimationCurve curve1, Transform target, Vector3 distance, float tweenTime,float delayTime, System.Action<Transform> onCompleteAction)
        {
            Sequence seque = DOTween.Sequence();
            Vector3 m_RecordLocalPostion = target.localPosition;
            float shakeValue = 0;
            Tweener move1 = DOTween.To(() => shakeValue, x => shakeValue = x, 1, tweenTime ).SetEase(curve1).OnUpdate(() =>
            {
                target.localPosition = m_RecordLocalPostion + distance * shakeValue;
            }).OnComplete(() =>
            {
                //Debug.LogInfor("ShakeLocalPosition   Step1");
                shakeValue = 1;
                target.localPosition = m_RecordLocalPostion;  //恢复位置
                if (onCompleteAction != null)
                    onCompleteAction(target);
            }).SetDelay(delayTime);

            seque.Join(move1); //.Join(move2);
            return seque;
        }


    }
}
