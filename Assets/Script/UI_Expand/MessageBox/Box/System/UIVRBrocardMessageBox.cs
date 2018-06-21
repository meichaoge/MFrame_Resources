using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



namespace MFramework
{

    /// <summary>
    /// 注意文本框的对其方式以及添加了Content SizeFitter 自适应大小
    /// </summary>
    public class UIVRBrocardMessageBox : BaseMessageBox
    {
        public List<string> m_AllSystemMessage = new List<string>();
        public Text m_MessageText;


        private Vector2 defaultSize;
        protected override void Awake()
        {
            base.Awake();
            defaultSize = m_MessageText.rectTransform.parent.GetComponent<RectTransform>().sizeDelta;  //初始大小
        }

        void OnEnable()
        {
            m_MessageText.text = "";
        }

        public override void Show(MessageBoxResquest _request, MessageBoxCallback _listener, int id = -1)
        {
            base.Show(_request, _listener);
            if (_request == null) return;
            if (string.IsNullOrEmpty(_request.m_MessageInfor) == false)
            {
                m_AllSystemMessage.Add(_request.m_MessageInfor);//压入消息
                                                                // Debug.Log("新来的消息长度 " + (_request.m_MessageInfor).Length);
                m_MessageText.text += "\n" + _request.m_MessageInfor;

                if (_request.m_AutoClip)
                { //自动裁减  弹出最上层的消息 ***需要优化
                    StartCoroutine(AutoSize());  //*********不能直接在这里使用While循环 否则会在一帧内执行完 弹出所有的输入
                }
                else
                {
                    if (m_AllSystemMessage.Count > _request.m_MaxMsgCout && _request.m_MaxMsgCout > 0)
                    {  //对于超过数量的消息自动弹出最上层的消息
                        string _oldStr = m_AllSystemMessage[0];  //取第一个元素
                        _oldStr = _oldStr + "\n";
                        string _newStr = m_MessageText.text;
                        int totalLength = _newStr.Length;
                        int deleteIndex = _oldStr.Length;
                        //   Debug.Log(_oldStr + "弹出的长度是 " + deleteIndex);
                        m_MessageText.text = _newStr.Substring(deleteIndex, totalLength - deleteIndex);
                        m_AllSystemMessage.RemoveAt(0);
                    }
                }

            }
        }

        /// <summary>
        /// 自动弹出超出文本区域的文本 这个需要优化 不能直接完全弹出整个第一个消息
        /// TODO
        /// </summary>
        /// <returns></returns>
        IEnumerator AutoSize()
        {
            while (m_MessageText.rectTransform.sizeDelta.y > defaultSize.y)
            { //显示的内容超过原始大小
                if (m_AllSystemMessage.Count > 0)
                {
                    string _oldStr = m_AllSystemMessage[0];  //取第一个元素
                    _oldStr = "\n" + _oldStr;
                    string _newStr = m_MessageText.text;
                    int totalLength = _newStr.Length;
                    int deleteIndex = _oldStr.Length;
                    //  Debug.Log(_oldStr + "弹出的长度是 " + deleteIndex);
                    m_MessageText.text = _newStr.Substring(deleteIndex, totalLength - deleteIndex);
                    m_AllSystemMessage.RemoveAt(0);
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    yield break;
                }
            }
        }


        public override void Hide()
        {
            base.Hide();
            gameObject.SetActive(false);
        }


    }
}
