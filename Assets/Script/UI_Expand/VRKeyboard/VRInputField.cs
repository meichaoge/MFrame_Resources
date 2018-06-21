using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MFramework
{

    /// <summary>
    /// 虚拟输入框  扩展标准输入框
    /// </summary>
    [AddComponentMenu("UI/VRInputField")]
    [RequireComponent(typeof(TransformListenerTool))]
    public class VRInputField : InputField
    {
        public delegate void OnFinishInputHandle(GameObject sender, string input, string errorMsg, params object[] errorMsgParameter);
        public OnFinishInputHandle handleInput;  //虚拟键盘的输入事件委托
        [SerializeField]
        public Text para;
        [SerializeField]
        public Text m_ShowText;

        public UIEffect ShowPassword = null;
        public VRInputRequest vRInputRequest = new VRInputRequest();
        private string realPassWord = ""; //the realInpt when is Password Type
        public bool ValueAccessible { private set; get; } //Use For Flag Whether The Input Value Real Worth
        public TransformListenerTool TransformListenerToolScript { private set; get; }

        protected override void Awake()
        {
            base.Awake();
            ValueAccessible = true;
            m_ShowText.text = "";
            if (ShowPassword != null)
                ShowPassword.PointerUpEvent += OnShowPasswordClick;

        }

        protected override void OnDestroy()
        {
            if (ShowPassword != null)
                ShowPassword.PointerUpEvent -= OnShowPasswordClick;
        }
        protected override void OnEnable()
        {
            if (TransformListenerToolScript == null)
                TransformListenerToolScript = GetComponent<TransformListenerTool>();
        }

        protected override void OnDisable()
        {
            ValueAccessible = true;
        }



        public override void OnPointerClick(PointerEventData eventData)
        {
            vRInputRequest.m_PreviousInput = m_ShowText.text;

            if (vRInputRequest.m_Limite_Min > vRInputRequest.m_Limite_Max)
                vRInputRequest.m_Limite_Min = vRInputRequest.m_Limite_Max;
            if (vRInputRequest.m_Limite_Min < 0)
                vRInputRequest.m_Limite_Min = 0;

            vRInputRequest.m_RealInput = realPassWord;

#if UNITY_ANDROID && !UNITY_EDITOR
        vRInputRequest.m_KeyBordShowPosition = transform.TransformPoint(vRInputRequest.m_KeyBoardOffeset_Gear); //From trans localposition to world position
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            vRInputRequest.m_KeyBordShowPosition = transform.TransformPoint(vRInputRequest.m_KeyBoardOffeset_PC); //From trans localposition to world position
#endif
            KeyBoard.Instance.GetInput(this,vRInputRequest, GetVRInput);

        }

        /// <summary>
        /// 获取输入
        /// </summary>
        /// <param name="_inputStr"></param>
        /// <param name="finishInput"></param>
        void GetVRInput(string _inputStr, bool finishInput, string errorMsg = "", params object[] errorMsgParameter)
        {
            m_ShowText.text = _inputStr;
            text = _inputStr;//由于使用非标准的输入框，因此这里必须对text 赋值，否则外界获取时为空

            if (string.IsNullOrEmpty(_inputStr))
                para.gameObject.SetActive(true);
            else
                para.gameObject.SetActive(false);

            ChangeShowTextView(_inputStr, finishInput);

            if (string.IsNullOrEmpty(errorMsg) == false)
            { //Fail Input 
                if (handleInput != null)
                    handleInput(gameObject, m_ShowText.text, errorMsg, errorMsgParameter);
                return;
            }

            if (finishInput)
            {
                para.gameObject.SetActive(false);
                ValueAccessible = (_inputStr.Length >= vRInputRequest.m_Limite_Min);
            }
            else
                ValueAccessible = false;


            if (handleInput != null)
                handleInput(gameObject, m_ShowText.text, errorMsg, errorMsgParameter);


        }

        void ChangeShowTextView(string currentInput, bool finishInput)
        {
            if (vRInputRequest.m_PasswordType)
            {
                if (string.IsNullOrEmpty(currentInput) == false)
                {
                    realPassWord = currentInput;  //Save RealText
                    string PasswordTextView = "";
                    if (finishInput)
                        PasswordTextView = new string('*', currentInput.Length);
                    else
                    {
                        PasswordTextView = new string('*', currentInput.Length - 1);
                        PasswordTextView += realPassWord[currentInput.Length - 1];
                    }
                    m_ShowText.text = PasswordTextView;
                }//if
                else
                {
                    realPassWord = "";
                }//else
            }//if
            else
            {
                realPassWord = currentInput;  //Normal State
            }//esle
        }


        public void SetTipText(string msg)
        {
            para.gameObject.SetActive(true);
            para.text = msg;
            realPassWord = m_ShowText.text = "";  //输入框不显示内容
        }

        /// <summary>
        /// 设置VR输入框显示的内容，由于无法直接获取text的值
        /// </summary>
        /// <param name="str"></param>
        public void SetViewText(string str)
        {
            if (m_ShowText != null)
            {
                para.gameObject.SetActive(false);
                m_ShowText.text = str;
                ChangeShowTextView(str, true);
            }
            else
            {
                if (string.IsNullOrEmpty(m_ShowText.text) == false)
                    para.gameObject.SetActive(false);

            }
        }
        /// <summary>
        /// 获取输入的值
        /// </summary>
        /// <returns></returns>
        public string GetViewText()
        {
            if (m_ShowText == null)
                return null;
            if (vRInputRequest.m_PasswordType)
                return realPassWord;
            return m_ShowText.text;
        }

        public bool CheckInputStringValible()
        {
            if (m_ShowText.text != "" && m_ShowText.text.Length >= vRInputRequest.m_Limite_Min && m_ShowText.text.Length <= vRInputRequest.m_Limite_Max)
                return true;
            return false;
        }

        public string GetRealText()
        {
            //if (vRInputRequest.m_PasswordType)
            return realPassWord;
        }

        void OnShowPasswordClick(GameObject parameter)
        {
            if (parameter.name == "ShowPassword")
            {
                vRInputRequest.m_PasswordType = !(vRInputRequest.m_PasswordType);
                this.SetViewText(GetRealText());
            }
        }


        public bool CheckInputLengthVarible(out string errorMsg, out object[] errorMsgParameter)
        {
            if (m_ShowText.text.Length < vRInputRequest.m_Limite_Min || (m_ShowText.text.Length > vRInputRequest.m_Limite_Max && vRInputRequest.m_Limite_Max != 0))
            {
                errorMsg = "900002"; //请输入 min ~到max 个字符
                errorMsgParameter = new object[] { vRInputRequest.m_Limite_Min, vRInputRequest.m_Limite_Max };
                return false;
            }
            errorMsg = "";
            errorMsgParameter = new object[0];
            return true;
        }






    }
}
