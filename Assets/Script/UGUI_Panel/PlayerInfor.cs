using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MFramework.UGUI
{
    public class PlayerInfor : UIViewBase
    {
        #region UI
        private Image m_imgHeadImage;
        private TMPro.TextMeshProUGUI m_txtPlayerName;

        #endregion

        #region Frame
        protected override void Awake()
        {
            base.Awake();
            this.InitView();
        }

        private void InitView()
        {
            Image imgHeadImage = transform.Find("HeadImage").gameObject.GetComponent<Image>();
            TMPro.TextMeshProUGUI txtPlayerName = transform.Find("PlayerName").gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            //**
            m_imgHeadImage = imgHeadImage;
            m_txtPlayerName = txtPlayerName;

        }
        #endregion
    }
}