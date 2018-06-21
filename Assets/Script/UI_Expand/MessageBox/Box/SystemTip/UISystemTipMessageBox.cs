using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MFramework
{

    public class UISystemTipMessageBox : BaseMessageBox
    {

        public RectTransform m_TopCanvasTrans;
        public Text m_TipMessage;

        private bool isShowIng = false;

        RectTransform canvas;
        Vector2 initialSize = new Vector2(300, 58);
        float initialHeight = 38;

        protected override void Awake()
        {
            base.Awake();
            BoxLevel = MessageBoxLevel.Lower;  //弹框级别
            canvas = GetComponent<RectTransform>();
            initialSize = canvas.sizeDelta;
        }


        protected override void HideBoxView()
        {
            base.HideBoxView();
        }

        public override void Show(MessageBoxResquest _request, MessageBoxCallback _listener, int id = -1)
        {
            base.Show(_request, _listener);
            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);
            if (_request == null)
            {
                Hide();
                return;
            };
            m_TopCanvasTrans.SetParent(EventCenter.GetInstance().m_UIRoot);  //父节点
            m_TopCanvasTrans.localScale = Vector3.one / 1000;
            m_TopCanvasTrans.localPosition = _request.m_WorldPosition;  //要显示的位置
            m_TopCanvasTrans.localEulerAngles = _request.m_Angle;  //要显示的位置

            if (_request.m_DispearTime == 0)
            {
                _request.m_DispearTime = 0;  //默认显示
            }
            if (isShowIng == false)
            {
                StartCoroutine(AutoHide(_request.m_DispearTime));
            }
            else
            {//正在显示
                StopCoroutine("AutoHide");
                StartCoroutine(AutoHide(_request.m_DispearTime));
            }
            m_TipMessage.text = _request.m_MessageInfor;


            EventCenter.GetInstance().AddUpdateEvent(AutoFillSize, EventCenter.UpdateRate.DelayOneFrame);
            EventCenter.GetInstance().AddUpdateEvent(FaceToCamera, EventCenter.UpdateRate.DelayOneFrame);
        }

        public override void Hide()
        {
            base.Hide();
            EventCenter.GetInstance().RemoveUpdateEvent(AutoFillSize, EventCenter.UpdateRate.DelayOneFrame);
            EventCenter.GetInstance().RemoveUpdateEvent(FaceToCamera, EventCenter.UpdateRate.DelayOneFrame);
            StopCoroutine("AutoHide");
            GameObject.Destroy(gameObject);
        }

        IEnumerator AutoHide(float time)
        {
            yield return new WaitForSeconds(time);
            Hide();
        }

        /// <summary>
        /// 自适应大小
        /// </summary>
        void AutoFillSize()
        {
            if (m_TipMessage != null && canvas != null)
            {
                canvas.sizeDelta = initialSize + new Vector2(0, m_TipMessage.rectTransform.sizeDelta.y - initialHeight);  //自动调整大小
            }
        }

        /// <summary>
        /// 面向摄像机
        /// </summary>
        void FaceToCamera()
        {
            m_TopCanvasTrans.position = Camera.main.transform.position + Camera.main.transform.forward * 0.6f;
            m_TopCanvasTrans.LookAt(Camera.main.transform);
            m_TopCanvasTrans.Rotate(0, 180, 0);
        }



    }
}
