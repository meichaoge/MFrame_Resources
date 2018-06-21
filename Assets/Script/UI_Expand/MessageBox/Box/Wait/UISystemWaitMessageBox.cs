using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MFramework
{

    /// <summary>
    /// 弹出模态等待匡  
    /// </summary>
    public class UISystemWaitMessageBox : BaseMessageBox
    {
        public Text m_Message;
        public Image m_WaitingImage;
        float rotateSpeed = 10f;  //旋转角度


        RectTransform canvas;
        Vector2 initialSize = new Vector2(514, 224);
        float initialHeight = 41f;

        protected override void Awake()
        {
            base.Awake();
            BoxLevel = MessageBoxLevel.High;  //弹框级别
            canvas = transform.GetComponent<RectTransform>();
            if (canvas == null) Debug.LogError("Miss Canvas");
            else
            {
                initialSize = canvas.sizeDelta;
            }
        }

        protected override void HideBoxView()
        {
            base.HideBoxView();
        }

        public override void Show(MessageBoxResquest _request, MessageBoxCallback _listener, int id = -1)
        {
            base.Show(_request, _listener);
            if (_request == null) { Hide(); return; }
            _request.m_IsModel = true;   //等待框一定是模态的

            //MessageBoxModel.GetInstance().ModelMessageBoxRegest(this, true);

            if (m_Message != null)
                m_Message.text = string.IsNullOrEmpty(_request.m_MessageInfor) ? "" : _request.m_MessageInfor;  //默认显示“”

            EventCenter.GetInstance().AddUpdateEvent(AutoFillSize, EventCenter.UpdateRate.DelayTwooFrame);
            EventCenter.GetInstance().AddUpdateEvent(Waitting, EventCenter.UpdateRate.DelayTwooFrame);  //自旋转

            if (_request.m_DispearTime <= 0)
            {
                Debug.Log("等待时间太短!");
                _request.m_DispearTime = 1f;
            }

            StartCoroutine(AutoWait(_request.m_DispearTime));   //进入等待模式
            transform.SetParent(EventCenter.GetInstance().m_UIRoot);
            transform.localPosition = new Vector3(0, 0, 0.5f);
            transform.localRotation = Quaternion.identity;

        }

        public override void Hide()
        {
            EventCenter.GetInstance().RemoveUpdateEvent(AutoFillSize, EventCenter.UpdateRate.DelayTwooFrame);
            EventCenter.GetInstance().RemoveUpdateEvent(Waitting, EventCenter.UpdateRate.DelayTwooFrame);  //自旋转

            //     MessageBoxModel.GetInstance().ModelMessageBoxRegest(this, false);
            StopCoroutine("AutoWait");
        }


        IEnumerator AutoWait(float time)
        {
            yield return new WaitForSeconds(time);
            if (handle != null)
            { //默认所有的响应操作全在这里被调用 方便统一管理
                handle(response);  //处理事件
            }
            Hide();
        }


        /// <summary>
        /// 自适应大小
        /// </summary>
        void AutoFillSize()
        {
            if (m_Message != null && canvas != null)
            {
                canvas.sizeDelta = initialSize + new Vector2(0, m_Message.rectTransform.sizeDelta.y - initialHeight);  //自动调整大小
            }
        }

        void Waitting()
        {
            if (m_WaitingImage != null)
                m_WaitingImage.transform.Rotate(0, 0, rotateSpeed);  //自动旋转
        }



    }
}
