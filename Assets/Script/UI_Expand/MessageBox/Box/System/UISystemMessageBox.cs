using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

namespace MFramework
{

    public class UISystemMessageBox : BaseMessageBox
    {
        //*********************需要重新实现***************TODO
        public RectMask2D m_Mask;
        public Text m_MessageInfor1;  //要显示的内容
        public Text m_MessageInfor2;  //要显示的内容
        float tweenTime = 5f;
        float sizeOfEachChar = 20; //每一个字符占据的单位大小
        public static UISystemMessageBox Instance;
        // private Stack<string> systemInfor = new Stack<string>();  //要显示的内容
        private List<string> systemInfor = new List<string>();  //要显示的内容



        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }


        void OnEnable()
        {
            InvokeRepeating("SystemBrocadcast", 0f, tweenTime * 0.51f);  //这里间隔时间必须大于tweenTime / 2
            InvokeRepeating("FaceToCamera", 0f, 1 / 30);
        }

        void OnDisable()
        {
            CancelInvoke("SystemBrocadcast");
            CancelInvoke("FaceToCamera");
        }

        protected override void HideBoxView()
        {
            base.HideBoxView();
        }


        public override void Show(MessageBoxResquest _request, MessageBoxCallback _listener, int id = -1)
        {
            base.Show(_request, _listener);
            if (_request == null) return;
            if (string.IsNullOrEmpty(_request.m_MessageInfor) == false)
            {
                if (_request.m_Highpriority)
                { //优先显示
                    Debug.Log("优先显示");
                    List<string> _newList = new List<string>(systemInfor.Count + 1);
                    _newList.Add(_request.m_MessageInfor);  //压入数据
                    for (int _index = 0; _index < systemInfor.Count; _index++)
                    {
                        _newList.Add(systemInfor[_index]);
                    }//for
                    systemInfor.Clear();
                    systemInfor = _newList;
                }//if
                else
                { //顺序显示
                    systemInfor.Add(_request.m_MessageInfor);  //压入数据
                }
            }
            transform.position = _request.m_WorldPosition;
        }

        public override void Hide()
        {
            base.Hide();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示系统广播
        /// </summary>
        void SystemBrocadcast()
        {
            if (systemInfor.Count == 0)
            { //当消息已经广播完 则停止检测
              //CancelInvoke("SystemBrocadcast");
                return;
            }
            float _MessageLength = 0f;
            float _MessageMoveLength = 0f;
            RectTransform _operateRect = m_MessageInfor1.GetComponent<RectTransform>();
            if (_operateRect.position.x >= 0f)
            { //m_MessageInfor1 为当前显示的页面
            }
            else
            { //m_MessageInfor1 在显示过程中 不可用
                _operateRect = m_MessageInfor2.GetComponent<RectTransform>();
                if (_operateRect.position.x >= 0f)
                {  //m_MessageInfor2 可以操作
                   //  Debug.Log("当前项2 可以操作");
                }
                else
                {
                    //  Debug.Log("等待..");
                    _operateRect = null;   //标示当前调用不可用需要等待下一次被调用
                }
            }

            if (_operateRect != null && systemInfor.Count > 0)
            {
                int _showLength = systemInfor[0].Length;   //默认每一字站位sizeOfEachChar
                                                           // Debug.Log("要显示的内容的长度是 " + _showLength);

                _MessageLength = Mathf.Max(_showLength * sizeOfEachChar, m_Mask.rectTransform.sizeDelta.x / 2); //最小长度是mask 一半长度
                _MessageMoveLength = Mathf.Max(_showLength * sizeOfEachChar, m_Mask.rectTransform.sizeDelta.x) + _MessageLength / 2;  //移动的距离 最小是mask长度
                _operateRect.sizeDelta = new Vector2(_MessageLength, _operateRect.sizeDelta.y); //设置Message长度
                _operateRect.GetComponent<Text>().text = systemInfor[0];
                systemInfor.RemoveAt(0);
                MoveText(_operateRect, _MessageMoveLength);
            }
        }

        /// <summary>
        /// 走马灯效果
        /// </summary>
        /// <param name="_ope"></param>
        /// <param name="_moveLength"></param>
        void MoveText(RectTransform _ope, float _moveLength)
        {
            _ope.DOLocalMoveX(-1 * _moveLength, tweenTime).OnComplete(() =>
            {
                _ope.localPosition = new Vector3(_ope.sizeDelta.x, _ope.localPosition.y, _ope.localPosition.z);
            });
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        void FaceToCamera()
        {
            transform.position = request.m_WorldPosition;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

    }
}
