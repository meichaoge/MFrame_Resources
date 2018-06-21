using UnityEngine;
using System.Collections;

namespace MFramework
{

    /// <summary>
    /// 委托回调接口
    /// </summary>
    /// <param name="resp"></param>
    public delegate void MessageBoxCallback(MessageBoxResponse resp); //定义消息框的操作回调委托

    /// <summary>
    /// 消息框接口
    /// </summary>
    public interface IMessageBox
    {
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="_infor">要显示的消息框信息</param>
        /// <param name="_listener">回调</param>
        void Show(MessageBoxResquest _resquest, MessageBoxCallback _hander, int id = -1);

        /// <summary>
        /// 关闭或者隐藏消息框
        /// </summary>
        void Hide();

    }




    /// <summary>
    /// 提示框内部信息参数
    /// </summary>
    public class MessageBoxResquest
    {
        public enum MessageBoxButton
        { //要显示的内容的Button
            YES,
            YESNO,
            YESNOCANCEL
        }
        /// <summary>
        /// 消息框类型
        /// </summary>
        public enum MessageType
        {
            None,
            Information,                //信息
            SystemBroadcast,       //系统广播框    默认是一直显示
            InputBox,                     //输入框
            TipsBox,                   //提示框
            ProgressBar,                //进度条
            SystemWait,                //模态等待界面
            VRBrocard,              //公告板
            SystemTip               //系统提示框 正前方弹出几秒后消失
        }
        public MessageType m_MSGType;
        public MessageBoxLevel m_BoxLevel = MessageBoxLevel.Normal;  //优先级 高优先级可能会关闭低优先级 相同优先级一般只显示一个

        #region Box属性
        public Transform m_Parent = null; //父节点
        public Vector3 m_WorldPosition = Vector3.zero;  //显示的位置
        public Vector3 m_Angle = Vector3.zero;  //显示的位置的角度
        public Vector3 m_Scale = Vector3.one;  //缩放系数
        public bool m_IsModel = false;  //是否是模态对话框
        public float m_DispearTime = 0;  //自动消失时间  默认不自动消息

        #endregion

        #region 中间显示内容 部分可以为空
        public string m_Title;  //标题栏
        public string m_MessageInfor; //弹出信息
        public object[] m_MsgParameter = new object[0];  //弹出框参数
        public TextAnchor m_MsgTextAnchor = TextAnchor.MiddleCenter; //文字对齐方式

        #region InforBox
        public string m_MessageImage;  //弹出信息对应的图片默认的图标 或者网络图片  为“”标示不显示
        #endregion

        #region InputBox
        public bool m_multilineInpt = false;  //是否是多行输入
        public string m_InputFilePlaceholder;  //输入框内部的提示信息
        #endregion

        #region SystemBrocard
        public bool m_Highpriority = false;  //默认按照调用时间顺序显示 为true 则优先显示
        #endregion

        #region VRBrocard 
        public int m_MaxMsgCout = 50;  //显示的最大消息数量
        public bool m_AutoClip = false; //是否以文本显示区域自动弹出最上层的消息 当为true时 m_MaxMsgCout无效
        #endregion

        #region ProgressBar
        public float m_Progress;   //0-1 //进度
        #endregion

        #endregion

        #region Button 选项
        public bool m_ShowClose = false;  //是否显示关闭按钮
        public MessageBoxButton m_MSGBtn;  //要显示的按钮
        public string m_CloseButtonName;   //按钮close显示内容
        public string m_ButtonName1 = "100028";   //按钮1显示内容 确定
        public string m_ButtonName2 = "100023";   //按钮2显示内容  取消
        public string m_ButtonName3;   //按钮2显示内容

        #endregion

        public MessageBoxResponse m_DefaultResponse;  //默认的操作 自动关闭时候自动回复

        //*********这里的字段只定义要显示的内容以及显示的方式 不考虑具体的业务功能
    }

    /// <summary>
    /// 消息框的操作反馈
    /// </summary>
    public class MessageBoxResponse
    {
        public bool m_IsSure;   //确定或者取消(否)
        public string m_InputStr;  //输入

        public MessageBoxResponse()
        {
            m_IsSure = false;
            m_InputStr = "";
        }

        public MessageBoxResponse(bool _operate)
        {
            m_IsSure = _operate;
        }

        public MessageBoxResponse(bool _operate, string _input)
        {
            m_IsSure = _operate;
            m_InputStr = _input;
        }

    }
}

