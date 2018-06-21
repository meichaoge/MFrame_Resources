using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


namespace MFramework
{

    /// <summary>
    /// 警告或者错误提示框
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UIMessageBox : BaseMessageBox
    {
        public Text m_Title;
        public Image m_BoxImage;  //类型标示图片
        public Text m_Message;
        public UIEffect m_CloseUIEffect;
        public Text m_CloseText;
        public UIEffect m_UIEffect1;
        public Text m_UIEffect1Text;
        public UIEffect m_UIEffect2;
        public Text m_UIEffect2Text;
        public UIEffect m_UIEffect3;
        public Text m_UIEffect3Text;
        [SerializeField]
        private GridLayoutGroup gridLayout;

        public static Vector3 ShowDefaultPosition = new Vector3(0, 0.5f, 0.7f);//默认的显示位置
        float duration;  //倒计时

        RectTransform canvas;
        [Tooltip("面板初始的大小")]
        [SerializeField]
        Vector2 initialSize = new Vector2(580, 320);
        [Tooltip("消息显示区域一行字的高度")]
        [SerializeField]
        float initialHeight = 41f;

        protected override void Awake()
        {
            base.Awake();
            BoxLevel = MessageBoxLevel.Normal;  //弹框级别
            canvas = transform.GetComponent<RectTransform>();
            initialSize = canvas.sizeDelta;

            if (m_UIEffect1 != null)
                m_UIEffect1.PointerUpEvent += OnViewButtonClick;  //注册事件
            if (m_UIEffect2 != null)
                m_UIEffect2.PointerUpEvent += OnViewButtonClick;  //注册事件
            if (m_UIEffect3 != null)
                m_UIEffect3.PointerUpEvent += OnViewButtonClick;  //注册事件
        }
        protected override void HideBoxView()
        {
            base.HideBoxView();
            if (m_CloseUIEffect != null) m_CloseUIEffect.gameObject.SetActive(false);
            if (m_UIEffect1 != null) m_UIEffect1.gameObject.SetActive(false);
            if (m_UIEffect2 != null) m_UIEffect2.gameObject.SetActive(false);
            if (m_UIEffect3 != null) m_UIEffect3.gameObject.SetActive(false);
            if (m_BoxImage != null) m_BoxImage.gameObject.SetActive(false);
            m_Title.text = m_Message.text = "";
        }
        public override void Show(MessageBoxResquest _request, MessageBoxCallback _listener, int id = -1)
        {
            base.Show(_request, _listener, id);
            if (_request == null) { Hide(); return; }
            BoxLevel = _request.m_BoxLevel;

            m_Message.alignment = _request.m_MsgTextAnchor;

            SetButtonStateBaseOnType(_request.m_MSGBtn);
            if(_request.m_MSGBtn== MessageBoxResquest.MessageBoxButton.YES)
                _request.m_IsModel = true;   //自动为模态

            ShowMsg();

            #region AutoHide

            if (_request.m_DispearTime != 0)
            {
                duration = _request.m_DispearTime; //持续时间
                InvokeRepeating("UpdateTickView", 1, 1); //自动更新
                m_UIEffect1Text.text += (int)(_request.m_DispearTime + 0.5f);
                gridLayout.cellSize = new Vector2(306, 76);
                StartCoroutine(AutoHide(_request.m_DispearTime));
            }
            else
                gridLayout.cellSize = new Vector2(214, 86);
            #endregion

            transform.SetParent(EventCenter.GetInstance().m_UIRoot);
            if (_request.m_WorldPosition != ShowDefaultPosition)
                transform.position = _request.m_WorldPosition;  //自定义位置
            else
                transform.localPosition = ShowDefaultPosition;

            transform.localPosition = _request.m_WorldPosition;
            transform.localRotation = Quaternion.identity;
            EventCenter.GetInstance().AddUpdateEvent(FollowCamera, EventCenter.UpdateRate.NormalFrame);

        }

        public override void Hide()
        {
            EventCenter.GetInstance().RemoveUpdateEvent(FollowCamera, EventCenter.UpdateRate.NormalFrame);
            CancelInvoke("UpdateTickView");
            MessageBoxModel.GetInstance().RemoveMessageBox(MsgBoxID, BoxLevel);

            if (MessageBox.MsgBoxPool != null)
                MessageBox.MsgBoxPool.Recycle(this, MessageBoxResquest.MessageType.Information);


            if (handle != null)
            { //默认所有的响应操作全在这里被调用 方便统一管理
              //*****后面需要考虑 有些面板关闭时不会处理消息 而是强制等待用户响应
                handle(response);  //处理事件
            }
        }


        protected override void ShowMsg()
        {
            if (request == null) return;
            switch (request.m_MSGBtn)
            {
                case MessageBoxResquest.MessageBoxButton.YES:
                    #region Yes
                    if (m_UIEffect1Text != null)
                      //  m_UIEffect1Text.text = LauguageTool.Ins.GetText(request.m_ButtonName1);  //默认显示“确定”
                    m_UIEffect1Text.text =request.m_ButtonName1;  //默认显示“确定”
                    #endregion
                    break;
                case MessageBoxResquest.MessageBoxButton.YESNO:
                    #region Yesno
                    gridLayout.cellSize = new Vector2(306, 76);
                    if (m_UIEffect1Text != null)
                    {//YES
                        //m_UIEffect1Text.text = LauguageTool.Ins.GetText(request.m_ButtonName1);  //默认显示“确定”
                        m_UIEffect2Text.text = request.m_ButtonName1;

                    }
                    if (m_UIEffect2Text != null)
                    {//NO
                     //   m_UIEffect2Text.text = LauguageTool.Ins.GetText(request.m_ButtonName2);  //默认显示"取消”
                        m_UIEffect2Text.text = request.m_ButtonName2;
                    }
                    #endregion
                    break;
                case MessageBoxResquest.MessageBoxButton.YESNOCANCEL:
                    #region YesnoCancel
                    if (m_UIEffect1Text != null)
                    {//YES
                     //   m_UIEffect1Text.text = string.IsNullOrEmpty(request.m_ButtonName1) ? "Yes" : LauguageTool.Ins.GetText(request.m_ButtonName1);  //默认显示“确定”
                        m_UIEffect1Text.text = string.IsNullOrEmpty(request.m_ButtonName1) ? "Yes" : request.m_ButtonName1;  //默认显示“确定”
                    }

                    if (m_UIEffect2Text != null)
                    {//NO
                        m_UIEffect1Text.text = string.IsNullOrEmpty(request.m_ButtonName1) ? "Yes" : request.m_ButtonName2;  //默认显示“quxiao”

                        //   m_UIEffect2Text.text = string.IsNullOrEmpty(request.m_ButtonName2) ? "No" : LauguageTool.Ins.GetText(request.m_ButtonName2);  //默认显示"quxiao”
                    }

                    if (m_UIEffect3Text != null)
                    {//Cancel
                        m_UIEffect3Text.text = string.IsNullOrEmpty(request.m_ButtonName3) ? "Cancel" : request.m_ButtonName3;  //默认显示“确定”
                    }

                    #endregion
                    break;
            }

            //    string msg = LauguageTool.Ins.GetText(request.m_MessageInfor);
            string msg =request.m_MessageInfor;
            if (request.m_MsgParameter != null && request.m_MsgParameter.Length > 0)
                m_Message.text = string.Format(msg, request.m_MsgParameter);
            else
                m_Message.text = msg;

            if (m_Message != null && canvas != null)
            {
                m_Message.rectTransform.sizeDelta = new Vector2(m_Message.rectTransform.sizeDelta.x, m_Message.preferredHeight);
                canvas.sizeDelta = initialSize + new Vector2(0, m_Message.preferredHeight - initialHeight);  //自动调整大小
            }

            ////Set Canvs position
            //EventCenter.Instance.CurrentFameLastDoAction(() =>
            //{
            //    if (m_Message != null && canvas != null)
            //        canvas.sizeDelta = initialSize + new Vector2(0, m_Message.rectTransform.sizeDelta.y - initialHeight);  //自动调整大小
            //});
        }

        void SetButtonStateBaseOnType(MessageBoxResquest.MessageBoxButton type)
        {
            if (m_UIEffect1.gameObject.activeSelf == false)
                m_UIEffect1.gameObject.SetActive(true);

            if (type == MessageBoxResquest.MessageBoxButton.YES)
            {
                if (m_UIEffect2.gameObject.activeSelf )
                    m_UIEffect2.gameObject.SetActive(false);
                if (m_UIEffect3.gameObject.activeSelf )
                    m_UIEffect3.gameObject.SetActive(false);
                return;
            }

            if (type == MessageBoxResquest.MessageBoxButton.YESNO)
            {
                if (m_UIEffect2.gameObject.activeSelf==false)
                    m_UIEffect2.gameObject.SetActive(true);

                if (m_UIEffect3.gameObject.activeSelf)
                    m_UIEffect3.gameObject.SetActive(false);
                return;
            }

            if (type == MessageBoxResquest.MessageBoxButton.YESNOCANCEL)
            {
                if (m_UIEffect2.gameObject.activeSelf == false)
                    m_UIEffect2.gameObject.SetActive(true);

                if (m_UIEffect3.gameObject.activeSelf == false)
                    m_UIEffect3.gameObject.SetActive(true);

                return;
            }
     
        }


        protected override void OnViewButtonClick(object paramater)
        {
            switch ((paramater as GameObject).name)
            {
                case "YES":
                    response.m_IsSure = true;
                    break;
                case "NO":
                    response.m_IsSure = false;
                    break;
                case "CANCEL":
                    response.m_IsSure = false;
                    break;
                case "CLOSE":
                    response.m_IsSure = false;
                    break;
            }//switch
            StopCoroutine("AutoHide");
            Hide();
        }

        /// <summary>
        /// 当设置了MessageBoxResquest.m_DispearTime时。如果过了一段时间不操作自动处理消息
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        IEnumerator AutoHide(float time)
        {
            yield return new WaitForSeconds(time);
            if (response != null && request.m_DefaultResponse != null)
                response.m_IsSure = request.m_DefaultResponse.m_IsSure; //使用默认的操作
            Hide();
        }



        void UpdateTickView()
        {
            m_UIEffect1Text.text = request.m_ButtonName1;
            duration -= 1;
        }

        /// <summary>
        /// 在水平方向上跟随主相机 垂直方向上不跟随
        /// </summary>
        void FollowCamera()
        {
            TransFormHelper.FollowCameraHorizontal(transform, 0.5f);
        }

        //在切换场景时打开则会出现这种情况 
        protected override void OnDestroy()
        {
            if (m_UIEffect1 != null)
                m_UIEffect1.PointerUpEvent -= OnViewButtonClick;  //取消监听事件
            if (m_UIEffect2 != null)
                m_UIEffect2.PointerUpEvent -= OnViewButtonClick;  //取消监听事件
            if (m_UIEffect3 != null)
                m_UIEffect3.PointerUpEvent -= OnViewButtonClick;  //取消监听事件
            if (m_CloseUIEffect != null)
                m_CloseUIEffect.PointerUpEvent -= OnViewButtonClick; //关闭
            EventCenter.GetInstance().RemoveUpdateEvent(FollowCamera, EventCenter.UpdateRate.NormalFrame);

            CancelInvoke("UpdateTickView");
            base.OnDestroy();
        }

        public override void Reset()
        {
            base.Reset();
            EventCenter.GetInstance().RemoveUpdateEvent(FollowCamera, EventCenter.UpdateRate.NormalFrame);
            CancelInvoke("UpdateTickView");
        }


    }
}
