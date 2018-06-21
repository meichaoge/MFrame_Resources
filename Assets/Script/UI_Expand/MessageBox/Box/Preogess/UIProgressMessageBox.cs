using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace MFramework
{

    public class UIProgressMessageBox : BaseMessageBox
    {
        public Text m_Title;
        public Text m_MessageTip;
        public Image m_ProgressBar;
        public Text m_Progress;


        protected override void HideBoxView()
        {
            base.HideBoxView();
        }

        public override void Show(MessageBoxResquest _request, MessageBoxCallback _listener, int id = -1)
        {
            base.Show(_request, _listener);
            if (_request == null) { Hide(); return; }
            if (m_ProgressBar != null)
            {
                m_ProgressBar.fillAmount = _request.m_Progress;
                m_Progress.text = (int)(_request.m_Progress * 100) + "%";
                if (Mathf.Abs(_request.m_Progress - 1) <= 0.01f)
                {
                    if (_listener != null)
                    {
                        _listener(response);
                    }
                    Hide();
                }
            }
            if (m_Title != null)
            {
                m_Title.text = _request.m_Title;
            }
            if (m_MessageTip != null)
            {
                m_MessageTip.text = _request.m_MessageInfor;  //提示信息
            }
        }

        public override void Hide()
        {
            base.Hide();
            Destroy(gameObject);
        }





    }
}
