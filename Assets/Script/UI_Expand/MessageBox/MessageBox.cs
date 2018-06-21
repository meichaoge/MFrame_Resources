using UnityEngine;

namespace MFramework
{
    public static class MessageBox
    {
        private static MseeageBoxPool _MsgBoxPool;
        public static MseeageBoxPool MsgBoxPool
        {
            get
            {
                if (_MsgBoxPool == null)
                {
                    _MsgBoxPool = BaseObjectPool.GetPoolByType(PoolType.MsgBox, ConstDefine.UIMsgBoxResourcePath, 0) as MseeageBoxPool;
                    if (_MsgBoxPool == null)
                    {
                        Debug.LogError("Get Pool Fail " + PoolType.MsgBox);
                    }//if
                }//if
                return _MsgBoxPool;
            }
        }



        /// <summary>
        /// 一个只读的模态信息提示框  有一个确定按钮 并且有超时自动选择功能
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">显示内容</param>
        /// <param name="handle">回调接口</param>
        /// <param name="request">可以设置当前类型提示框的其他属性 某些设置可能无效 默认为空</param>
        public static void Alert(string title, string msg, MessageBoxCallback handle, MessageBoxResquest request)
        {//一个只读的信息提示框  有一个确定按钮

            if (request == null) return;
            request.m_MSGType = MessageBoxResquest.MessageType.Information;
            request.m_MSGBtn = MessageBoxResquest.MessageBoxButton.YES;
            request.m_Title = title;
            request.m_MessageInfor = msg;

            MessageBoxModel.GetInstance().ShowMessageBox(request, handle);
        }

        /// <summary>
        /// 显示一个确认信息框  有两个按钮
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="handle">回调</param>
        /// <param name="request">其他额外设置</param>
        public static void Confirm(string title, string msg, MessageBoxCallback handle, MessageBoxResquest request)
        {
            if (request == null) return;
            request.m_MSGType = MessageBoxResquest.MessageType.Information;
            request.m_MSGBtn = MessageBoxResquest.MessageBoxButton.YESNO;
            request.m_Title = title;
            request.m_MessageInfor = msg;

            MessageBoxModel.GetInstance().ShowMessageBox(request, handle);

        }

        /// <summary>
        /// 显示一个输入框 有两个按钮和一个输入框接口输入
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        /// <param name="multiline">是否是多行文本框 默认是单行</param>
        /// <param name="defaultStr">输入框默认显示的文本</param>
        public static void GetInput(string title, string msg, MessageBoxCallback handle, MessageBoxResquest request, bool multiline = false, string defaultStr = "")
        {
            if (request == null) return;

            request.m_MSGType = MessageBoxResquest.MessageType.InputBox;
            request.m_MSGBtn = MessageBoxResquest.MessageBoxButton.YESNO;
            request.m_Title = title;
            request.m_MessageInfor = msg;
            request.m_multilineInpt = multiline; //是否多行
            request.m_InputFilePlaceholder = defaultStr; //默认显示的输入

            Show(request, handle);
        }

        /// <summary>
        /// 显示一个自动滚动的进度条 它不能定义一个时间间隔自动关闭
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        public static void Progress(float progress, MessageBoxCallback handle, MessageBoxResquest request)
        {
            if (request == null) return;
            request.m_MSGType = MessageBoxResquest.MessageType.ProgressBar;
            request.m_Progress = progress;

            Show(request, handle);
        }

        /// <summary>
        /// 显示一个模态系统等待框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="waitTime">事件必须大于0</param>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        public static void SystemWait(string msg, float waitTime, MessageBoxCallback handle, MessageBoxResquest request)
        {
            if (request == null) return;
            request.m_MSGType = MessageBoxResquest.MessageType.SystemWait;
            request.m_MessageInfor = msg;
            request.m_DispearTime = waitTime;
            request.m_IsModel = true;

            Show(request, handle);
        }

        /// <summary>
        /// 显示一个水平拉伸提示框 无法操作并且会自动消失
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="despearTime">自动消失时间</param>
        /// <param name="parent">父节点</param>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        /// <param name="msgParameter">消息显示区域的参数设置</param>
        public static void Tips(string msg, float dispearTime, Transform parent, MessageBoxCallback handle, MessageBoxResquest request, params object[] msgParameter)
        {
            if (request == null) return;
            request.m_MSGType = MessageBoxResquest.MessageType.TipsBox;
            request.m_MessageInfor = msg;
            request.m_MsgParameter = msgParameter;
            request.m_DispearTime = dispearTime;
            request.m_Parent = parent;
            Show(request, handle);
        }

        /// <summary>
        /// 显示一个垂直拉伸的提示框 几秒后自动消失
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="dispearTime"></param>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        public static void SystemTip(string msg, float dispearTime, MessageBoxCallback handle, MessageBoxResquest request)
        {
            //if (request == null) return;
            if (request == null) request = new MessageBoxResquest();
            request.m_MSGType = MessageBoxResquest.MessageType.SystemTip;
            request.m_MessageInfor = msg;
            request.m_DispearTime = dispearTime;
            Show(request, handle);
        }

        /// <summary>
        /// 系统滚动条广播 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="highpriority">是否是高优先级</param>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        public static void SystemBrocard(string msg, bool highpriority, MessageBoxCallback handle, MessageBoxResquest request)
        {
            if (request == null) return;
            request.m_MSGType = MessageBoxResquest.MessageType.SystemBroadcast;
            request.m_MessageInfor = msg;
            request.m_Highpriority = highpriority;
            Show(request, handle);
        }

        /// <summary>
        /// VR 公告广播
        /// </summary>
        /// <param name="msg">要显示的消息</param>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        public static void VRBrocard(string msg, MessageBoxCallback handle, MessageBoxResquest request)
        {
            if (request == null) return;
            request.m_MSGType = MessageBoxResquest.MessageType.VRBrocard;
            request.m_MessageInfor = msg;
            Show(request, handle);
        }




        /// <summary>
        /// 自定义消息框  不能被用户调用触发，只能被消息系统内部调用
        /// </summary> 
        /// <param name="request"></param>
        /// <param name="handle"></param>
        public static BaseMessageBox Show(MessageBoxResquest request, MessageBoxCallback handle, int id = -1)
        {
            BaseMessageBox result = null;
            GameObject _boxResource = null;
            switch (request.m_MSGType)
            {
                case MessageBoxResquest.MessageType.Information:
                    #region 弹出提示框
                    if (MsgBoxPool != null)
                        result = MsgBoxPool.GetInstance("UIMessageBox", MessageBoxResquest.MessageType.Information, null, (messageBox) =>
                        {
                            //Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAA " + messageBox.gameObject.name + " ::" + messageBox.BoxLevel);
                            messageBox.Show(request, handle, id);
                        });
                    break;
                #endregion
                case MessageBoxResquest.MessageType.InputBox:
                    #region 弹出输入框
                    _boxResource = Resources.Load<GameObject>("Prefabs/UI/MessageBox/UIInputMessageBox");
                    if (_boxResource != null)
                    {
                        GameObject box = GameObject.Instantiate(_boxResource);
                        IMessageBox _imessage = box.GetComponent<IMessageBox>();
                        _imessage.Show(request, handle);
                    }
                    #endregion
                    break;
                case MessageBoxResquest.MessageType.SystemBroadcast:
                    #region 弹出走马灯效果
                    if (UISystemMessageBox.Instance == null)
                    {
                        _boxResource = Resources.Load<GameObject>("Prefabs/UI/MessageBox/UISystemBroadcast");
                        if (_boxResource != null)
                        {
                            GameObject box = GameObject.Instantiate(_boxResource);
                            IMessageBox _imessage = box.GetComponent<IMessageBox>();
                            _imessage.Show(request, handle);
                        }
                    }
                    else
                    {
                        UISystemMessageBox.Instance.Show(request, handle);
                    }

                    #endregion
                    break;
                case MessageBoxResquest.MessageType.TipsBox:
                    #region 弹出提示框
                    ////_boxResource = Resources.Load<GameObject>("Prefabs/UI/MessageBox/UITipBox");
                    ////if (_boxResource != null)
                    ////{
                    ////    GameObject box = GameObject.Instantiate(_boxResource);
                    ////    result = box.GetComponent<BaseMessageBox>();
                    ////    result.Show(request, handle);
                    ////}

                    #region 弹出提示框 
                    if (MsgBoxPool != null)
                        result = MsgBoxPool.GetInstance("UITipBox", MessageBoxResquest.MessageType.TipsBox, null, (tipBox) => { tipBox.Show(request, handle, id); });  //对象池中没有类型标示获得的池对象类型不对
                    #endregion
                    #endregion
                    break;
                case MessageBoxResquest.MessageType.ProgressBar:
                    #region 弹出进度匡
                    _boxResource = Resources.Load<GameObject>("Prefabs/UI/MessageBox/UIProgressBox");
                    if (_boxResource != null)
                    {
                        GameObject box = GameObject.Instantiate(_boxResource);
                        IMessageBox _imessage = box.GetComponent<IMessageBox>();
                        _imessage.Show(request, handle);
                    }

                    #endregion
                    break;
                case MessageBoxResquest.MessageType.SystemWait:
                    #region 弹出系统等待框
                    _boxResource = Resources.Load<GameObject>("Prefabs/UI/MessageBox/UISystemWaitBox");
                    if (_boxResource != null)
                    {
                        GameObject box = GameObject.Instantiate(_boxResource);
                        IMessageBox _imessage = box.GetComponent<IMessageBox>();
                        _imessage.Show(request, handle);
                    }
                    #endregion
                    break;
                case MessageBoxResquest.MessageType.VRBrocard:
                    #region 显示公告板内容
                    MessageBoxModel.GetInstance().ShowVRBrocardMessage(request, handle);
                    #endregion
                    break;
                case MessageBoxResquest.MessageType.SystemTip:
                    #region 显示系统提示框
                    _boxResource = Resources.Load<GameObject>("Prefabs/UI/MessageBox/UISystemTip");
                    if (_boxResource != null)
                    {
                        GameObject box = GameObject.Instantiate(_boxResource);
                        IMessageBox _imessage = box.GetComponent<IMessageBox>();
                        _imessage.Show(request, handle);
                    }
                    #endregion
                    break;
            }

            return result;
        }




    }
}
