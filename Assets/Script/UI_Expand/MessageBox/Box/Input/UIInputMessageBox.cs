//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//namespace MFramework
//{

//    /// <summary>
//    /// 显示一个输入框 提示框
//    /// </summary>
//    public class UIInputMessageBox : BaseMessageBox
//    {
//        public Text m_Title;  //标题
//        public Text m_InputTip;  //输入提示框
//        public VRInputField m_InputField;
//        public Text m_Placeholder;  //提示等

//        public UIEffect m_CloseUIEffect;
//        public Text m_CloseText;
//        public UIEffect m_UIEffect1;
//        public Text m_UIEffect1Text;
//        public UIEffect m_UIEffect2;
//        public Text m_UIEffect2Text;

//        protected override void HideBoxView()
//        {
//            base.HideBoxView();
//            if (m_CloseUIEffect != null) m_CloseUIEffect.gameObject.SetActive(false);
//            if (m_UIEffect1 != null) m_UIEffect1.gameObject.SetActive(false);
//            if (m_UIEffect2 != null) m_UIEffect2.gameObject.SetActive(false);
//        }


//        public override void Show(MessageBoxResquest _reque, MessageBoxCallback _listener, int id = -1)
//        {
//            base.Show(_reque, _listener);
//            if (_reque == null) return;

//            #region 显示的内容

//            if (m_Title != null)
//                m_Title.text = string.IsNullOrEmpty(_reque.m_Title) ? "InputInfor" : _reque.m_Title;  //默认显示“系统错误”
//            if (_reque.m_ShowClose)
//            {
//                m_CloseUIEffect.gameObject.SetActive(true);
//                m_CloseUIEffect.PointerUpEvent += OnViewButtonClick; //关闭
//                if (m_CloseText != null)
//                    m_CloseText.text = string.IsNullOrEmpty(_reque.m_CloseButtonName) ? "Close" : _reque.m_CloseButtonName;  //默认显示“”
//            }
//            if (m_InputTip != null)
//            {
//                m_InputTip.text = string.IsNullOrEmpty(_reque.m_MessageInfor) ? "请输入 ...." : _reque.m_MessageInfor;
//            }
//            if (m_Placeholder != null)
//            {
//                m_Placeholder.text = string.IsNullOrEmpty(_reque.m_InputFilePlaceholder) ? "Enter...." : _reque.m_InputFilePlaceholder;
//            }

//            //    Debug.Log("m_MSGBtn " + _reque.m_MSGBtn);
//            switch (_reque.m_MSGBtn)
//            {
//                case MessageBoxResquest.MessageBoxButton.YESNO:
//                    #region Yesno
//                    m_UIEffect1.gameObject.SetActive(true);
//                    m_UIEffect2.gameObject.SetActive(true);
//                    if (m_UIEffect1Text != null)
//                    {//YES
//                        m_UIEffect1Text.text = string.IsNullOrEmpty(_reque.m_ButtonName1) ? "确定" : _reque.m_ButtonName1;  //默认显示“确定”
//                    }
//                    if (m_UIEffect1 != null)
//                    {
//                        m_UIEffect1.PointerUpEvent += OnViewButtonClick;  //注册事件
//                    }
//                    if (m_UIEffect2Text != null)
//                    {//NO
//                        m_UIEffect2Text.text = string.IsNullOrEmpty(_reque.m_ButtonName2) ? "取消" : _reque.m_ButtonName2;  //默认显示“确定”
//                    }
//                    if (m_UIEffect2 != null)
//                    {
//                        m_UIEffect2.PointerUpEvent += OnViewButtonClick;  //注册事件
//                    }
//                    #endregion
//                    break;
//            }
//            #endregion

//            transform.SetParent(EventCenter.GetInstance().m_UIRoot);  //挂载在UI主节点上
//            transform.localPosition = new Vector3(0, 0.5f, 0.5f);
//            transform.localRotation = Quaternion.identity;  //要显示的位置

//            //   transform.position = _reque.m_WorldPosition; 
//        }

//        public override void Hide()
//        {
//            base.Hide();
//            if (m_UIEffect1 != null)
//                m_UIEffect1.PointerUpEvent -= OnViewButtonClick;  //取消监听事件
//            if (m_UIEffect2 != null)
//                m_UIEffect2.PointerUpEvent -= OnViewButtonClick;  //取消监听事件
//            if (m_CloseUIEffect != null)
//                m_CloseUIEffect.PointerUpEvent -= OnViewButtonClick; //关闭

//            if (handle != null)
//            { //默认所有的响应操作全在这里被调用 方便统一管理
//              //*****后面需要考虑 有些面板关闭时不会处理消息 而是强制等待用户响应
//                handle(response);  //处理事件
//            }
//            gameObject.SetActive(false);
//        }

//        protected override void OnViewButtonClick(object paramater)
//        {
//            base.OnViewButtonClick(paramater);
//            switch ((paramater as GameObject).name)
//            {
//                case "YES":
//                    response.m_IsSure = true;
//                    response.m_InputStr = m_InputField.text; //输入内容
//                    break;
//                case "NO":
//                    response.m_IsSure = false;
//                    break;
//                //case "CANCEL":
//                //    response.m_IsSure = false;
//                //    break;
//                case "CLOSE":
//                    response.m_IsSure = false;
//                    break;
//            }//switch
//            Hide();
//        }


//    }
//}
