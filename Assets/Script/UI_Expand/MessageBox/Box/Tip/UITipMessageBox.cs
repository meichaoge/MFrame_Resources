using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace MFramework
{

    /// <summary>
    /// 水平拉伸提示信息
    /// </summary>
    public class UITipMessageBox : BaseMessageBox
    {

        public RectTransform m_TopCanvasTrans;
        public Text m_TipMessage;

        RectTransform m_TipBoxRectrans;
        [SerializeField]
        Vector2 initialSize = new Vector2(121, 70);
        [SerializeField]
        float initialWidth = 17;


        private static List<UITipMessageBox> allUITipMessageBoxList = new List<UITipMessageBox>();

        protected override void Awake()
        {
            base.Awake();
            BoxLevel = MessageBoxLevel.Lower;  //弹框级别
            m_TipBoxRectrans = transform.GetOrAddCompont<RectTransform>();
            initialSize = m_TipBoxRectrans.sizeDelta;
      //      UIManagerModel.GetInstance().LaugangeChanEvent += ShowMsg;
        }

        protected override void OnDestroy()
        {
       //     UIManagerModel.GetInstance().LaugangeChanEvent -= ShowMsg;
            base.OnDestroy();
        }

        public override void Show(MessageBoxResquest _request, MessageBoxCallback _listener, int id = -1)
        {
            base.Show(_request, _listener, id);
            UITipMessageBox previous = null;
            if (CheckExitAndShowing(this, out previous))
            {  //There is one Tipe Exit
                if (previous != null)
                {
                    if (previous.gameObject.activeSelf)
                        previous.Hide();
                    else
                    {
                        allUITipMessageBoxList.Remove(previous);

                        if (MessageBox.MsgBoxPool != null)
                            MessageBox.MsgBoxPool.Recycle(previous, MessageBoxResquest.MessageType.TipsBox, (tips) => { tips.Reset(); });
                    }
                }//if
            }

            allUITipMessageBoxList.Add(this);

            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);
            if (_request == null)
            {
                Hide();
                return;
            };
            m_TopCanvasTrans.SetParent(_request.m_Parent);  //父节点
            m_TopCanvasTrans.localScale = _request.m_Scale; //缩放
            m_TopCanvasTrans.localPosition = _request.m_WorldPosition;  //要显示的位置
            m_TopCanvasTrans.localEulerAngles = _request.m_Angle;  //要显示的位置

            if (_request.m_DispearTime >= 0)
                StartCoroutine(AutoHide(_request.m_DispearTime));

            ShowMsg();

        }


        protected override void ShowMsg()
        {
            if (request == null) return;
//            string msg = LauguageTool.Ins.GetText(request.m_MessageInfor);
            string msg = request.m_MessageInfor;

            if (request.m_MsgParameter != null && request.m_MsgParameter.Length > 0)
                m_TipMessage.text = string.Format(msg, request.m_MsgParameter);
            else
                m_TipMessage.text = msg;

            m_TipMessage.rectTransform.sizeDelta = new Vector2(m_TipMessage.preferredWidth, m_TipMessage.rectTransform.sizeDelta.y);
            m_TopCanvasTrans.sizeDelta = initialSize + new Vector2(m_TipMessage.preferredWidth - initialWidth, 0);  //自动调整大小
            m_TopCanvasTrans.localPosition = request.m_WorldPosition;  //要显示的位置
            m_TopCanvasTrans.localEulerAngles = request.m_Angle;  //要显示的位置

            ////Set Canvs position
            //EventCenter.Instance.CurrentFameLastDoAction(() =>
            //{
            //    m_TopCanvasTrans.sizeDelta = initialSize + new Vector2(m_TipMessage.rectTransform.sizeDelta.x - initialWidth, 0);  //自动调整大小
            //    m_TopCanvasTrans.localPosition = request.m_WorldPosition;  //要显示的位置
            //    m_TopCanvasTrans.localEulerAngles = request.m_Angle;  //要显示的位置
            //});
        }

        public override void Hide()
        {
            base.Hide();
            StopCoroutine("AutoHide");
            allUITipMessageBoxList.Remove(this);

            if (MessageBox.MsgBoxPool != null)
                MessageBox.MsgBoxPool.Recycle(this, MessageBoxResquest.MessageType.TipsBox, (tips) => { tips.Reset(); });
        }

        public override void Reset()
        {
            base.Reset();
            handle = null;
            request = null;
            response = null;
            StopCoroutine("AutoHide");
            gameObject.SetActive(false);
        }
        IEnumerator AutoHide(float time)
        {
            yield return new WaitForSeconds(time);
            Hide();
        }


        //Find if this object already show one tipMsg
        static bool CheckExitAndShowing(UITipMessageBox current, out UITipMessageBox _previous)
        {
            for (int _dex = 0; _dex < allUITipMessageBoxList.Count; ++_dex)
            {
                if (allUITipMessageBoxList[_dex].gameObject != null)
                {
                    if (allUITipMessageBoxList[_dex] != null && allUITipMessageBoxList[_dex].request != null && allUITipMessageBoxList[_dex].
                                request.m_Parent == current.request.m_Parent)
                    {
                        _previous = allUITipMessageBoxList[_dex];
                        return true;
                    }//if
                }
            }
            _previous = null;
            return false;
        }

        public static void ClearAllTips()
        {
            for (int _dex = 0; _dex < allUITipMessageBoxList.Count; ++_dex)
            {
                if (allUITipMessageBoxList[_dex].gameObject != null)
                {
                    allUITipMessageBoxList[_dex].gameObject.SetActive(true);
                    allUITipMessageBoxList[_dex].Hide();
                }
            }
        }

        public static void ClearBindTip(Transform bindTrans)
        {
            MseeageBoxPool Pool = BaseObjectPool.GetPoolByType(PoolType.MsgBox, ConstDefine.UIMsgBoxResourcePath, 0) as MseeageBoxPool;
            if (Pool == null) { return; }

            for (int _dex = 0; _dex < allUITipMessageBoxList.Count; ++_dex)
            {
                if (allUITipMessageBoxList[_dex] != null && allUITipMessageBoxList[_dex].request != null && allUITipMessageBoxList[_dex].request.m_Parent == bindTrans)
                {
                    if (allUITipMessageBoxList[_dex].gameObject.activeSelf)
                    {
                        allUITipMessageBoxList[_dex].Hide();
                    }//if
                    else
                    {
                        Pool.Recycle(allUITipMessageBoxList[_dex], MessageBoxResquest.MessageType.TipsBox, (tips) => { tips.Reset(); });
                    }
                }//if
            }
        }

        public static void ClearPreviousTip(Transform bindTrans) { }



    }
}
