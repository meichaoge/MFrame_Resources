using UnityEngine;
using System.Collections;
using System;


namespace MFramework
{

    /// <summary>
    /// 弹框级别
    /// </summary>
    public enum MessageBoxLevel
    {
        Lower = 0,         //最低
        Normal,              //普通的通知类型
        NormalHigh,      //确认消息级别
        High,                //系统级别  如返回登录界面等 
        Top              //最高 掉线、退出应用等
    }

    public class BaseMessageBox : MonoBehaviour, IMessageBox
    {
        protected MessageBoxCallback handle; //回调
        protected MessageBoxResquest request;  //传入参数
        protected MessageBoxResponse response = new MessageBoxResponse();//响应的操作
        public MessageBoxLevel BoxLevel;//{ protected set; get; }  //弹框级别
        public int MsgBoxID; //{ protected set; get; }  //弹框ID

        protected virtual void Awake()
        {
      //      UIManagerModel.GetInstance().LaugangeChanEvent += ShowMsg;
            HideBoxView();
        }

        protected virtual void Start()
        {

        }

        protected virtual void OnDestroy()
        {
//            UIManagerModel.GetInstance().LaugangeChanEvent -= ShowMsg;
        }

        /// <summary>
        /// 隐藏MessageBox的显示项
        /// </summary>
        protected virtual void HideBoxView()
        {

        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show(MessageBoxResquest _request, MessageBoxCallback _listener, int id = -1)
        {
            handle = _listener;
            request = _request;
            MsgBoxID = id;
        }

        protected virtual void ShowMsg()
        {


        }


        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void Hide() { }
        /// <summary>
        /// 面板的按钮点击事件
        /// </summary>
        /// <param name="paramater"></param>
        protected virtual void OnViewButtonClick(object paramater) { }
        /// <summary>
        /// 获得优先级
        /// </summary>
        /// <returns></returns>
        public MessageBoxLevel GetBoxLevel()
        {
            if (request != null)
                return request.m_BoxLevel;
            return MessageBoxLevel.Normal;
        }

        //重置显示其他的内容
        public virtual void Reset()
        {
            HideBoxView();
        }


    }
}







