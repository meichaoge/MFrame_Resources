using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;

namespace MFramework
{

    public delegate void VRKeyInputHandle(string _inputStr, bool finishInput, string errorMsg, params object[] errorMsgParameter);  //输入回调
    public class KeyBoard : ViewBaseFU_EX
    {
        /// <summary>
        /// 虚拟键盘的输入事件
        /// </summary>
        public enum VRKeyboardEvent
        {
            ForceCloseKeyBoard,
            KeyBoardStateNotify,    //开始 或者结束 输入
        }

        public static KeyBoard Instance { private set; get; }

        #region UI

        public Canvas m_Canvas;
        public Image m_InputBG;//输入背景框
        public Vector2 m_Origin;
        public Vector2 m_Spacing;
        public Vector3 m_KeyTranslation;
        public Vector2 m_ItemSpace = new Vector2(1, 1);

        bool m_CapsShift = false;
        bool m_NormalInputState = true;  //Not NumberInput
        #endregion

        VRKeyInputHandle handleInput = null;   //the Input Handle
        VRInputRequest inputRequest;
        string appendChar;
        string result = "";
        string errorMsg = "";
        List<object> errorMsgParameter = new List<object>();

        public VirtualKeyState keyboardState { private set; get; } //KeyBoard State
        public List<VirtualKeyButton> allVirtualKey = new List<VirtualKeyButton>();  //All Key
        [HideInInspector]
        public VirtualKeyButton m_OperateKeyButton = null;
        private VRInputField m_VRInputFieldScript = null;


      

        #region Mono
        protected override void Awake()
        {
            Instance = this;
            keyboardState = VirtualKeyState.Normal;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GlobalEntity.GetInstance().RemoveListener(VRKeyboardEvent.ForceCloseKeyBoard, CloseKeyboard);

            handleInput = null;
            errorMsg = "";
            errorMsgParameter.Clear();
            if (m_VRInputFieldScript != null)
                m_VRInputFieldScript.TransformListenerToolScript.RemoveListener(TransformListenerTool.ListenTransformChangeEventEnum.LocalPosition, FollowTarget_Pos);  //开始跟随对象
#if UNITY_EDITOR
            if (m_VRInputFieldScript != null)
                m_VRInputFieldScript.TransformListenerToolScript.RemoveListener(TransformListenerTool.ListenTransformChangeEventEnum.Rotation, FollowTarget_Rotation);  //开始跟随对象
#endif
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            GlobalEntity.GetInstance().AddListener(VRKeyboardEvent.ForceCloseKeyBoard, CloseKeyboard);
        }

        #endregion


        #region Create KeyBoard

        //static int index = 2;
        //// create a single key
        //void CreateKey(string normalChar, string shiftedChar, string numberChar, KeyCode normalCode, KeyCode numberCode, Vector2 pos, Vector2 scale, Transform parent, bool special = false, string imageName = "")
        //{
        //    VirtualKey key = new VirtualKey(); //Record State
        //    key.m_NormalCharacter = normalChar;
        //    key.m_ShiftCharacter = shiftedChar;
        //    key.m_NumberCharacter = numberChar;

        //    key.m_CharStateCode = normalCode;
        //    key.m_NumberStateCode = numberCode;

        //    key.m_Pressed = false;
        //    key.m_Special = special;
        //    key.m_KeyImage = imageName;

        //    // create button
        //    GameObject obj = (GameObject)Object.Instantiate(m_KeyButtonPrefab);
        //    obj.name = normalChar;
        //    obj.transform.SetParent(parent, false);
        //    obj.transform.localScale = Vector3.one;

        //    RectTransform rectrans = obj.GetComponent<RectTransform>();
        //    //Debug.Log(obj.name + "  " + pos);
        //    rectrans.anchoredPosition = pos;
        //    rectrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_Spacing.x * scale.x);  //按照参数的值设置RectTransform 大小
        //    rectrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_Spacing.y * scale.y);
        //    obj.GetComponent<UIEffect>().PointerClickEvent += ButtonPressed;  //注册点击事件

        //    VirtualKeyButton button = obj.GetComponent<VirtualKeyButton>();
        //    button.m_Index = index;
        //    index++;
        //    button.InitilKey(key); //初始化
        //    allVirtualKey.Add(button);
        //}

        //// create a row of keys
        //void CreateKeyRow(string chars, string shiftedChars, string numberChar, KeyCode[] charaCodes, KeyCode[] numberCodes, Vector2 pos, float offset, Transform parent, bool special = false, string imageName = "menu_KeyBorad")
        //{
        //    pos.x += offset * m_Spacing.x;
        //    for (int i = 0; i < chars.Length; i++)
        //    {
        //        CreateKey(chars[i].ToString(), shiftedChars[i].ToString(), numberChar[i].ToString(), charaCodes[i], numberCodes[i], pos, Vector2.one, parent, special, imageName);
        //        pos.x += m_Spacing.x + m_ItemSpace.x;
        //    }
        //}

        //// create a whole keyboard
        //void CreateKeyboard()
        //{
        //    Vector2 pos = m_Origin;
        //    pos.y -= m_Spacing.y;
        //    GameObject Up = CommonOperate.CreateObjWhithName("Up");

        //    CreateKeyRow(
        //        "qwertyuiop",
        //        "QWERTYUIOP",
        //        "1234567890",
        //        new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P },
        //        new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 },
        //        pos,
        //        0.5f,
        //        Up.transform
        //    );
        //    pos.x += m_ItemSpace.x;
        //    CreateKey("", "", "", KeyCode.Backspace, KeyCode.Backspace, pos + new Vector2(m_Spacing.x * 11f, 0.0f), new Vector2(1.9f, 1.0f), Up.transform, true, "menu_Close");
        //    pos.y -= m_Spacing.y + m_ItemSpace.y;

        //    GameObject Middle = CommonOperate.CreateObjWhithName("Middle");
        //    CreateKeyRow(
        //        "asdfghjkl",
        //        "ASDFGHJKL",
        //        "@!?.,_/+-",
        //        new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L },
        //          new KeyCode[] { KeyCode.At, KeyCode.Exclaim, KeyCode.Question, KeyCode.Period, KeyCode.Comma, KeyCode.Underscore, KeyCode.Slash, KeyCode.Plus, KeyCode.Minus },
        //        pos,
        //        0.7f,
        //         Middle.transform
        //        );

        //    CreateKey("", "", "", KeyCode.Clear, KeyCode.Clear, pos + new Vector2(m_Spacing.x * 11f, 0.0f), new Vector2(1.9f, 1.0f), Middle.transform, true, "menu_Keyboard2");
        //    pos.y -= m_Spacing.y + m_ItemSpace.y;
        //    pos = new Vector2(m_Origin.x + 0.5f * m_Spacing.x, pos.y);
        //    CreateKey("", "", "", KeyCode.LeftShift, KeyCode.LeftShift, pos, new Vector2(1, 1.0f), Middle.transform, true, "menu_keyboard_Shift");  //添加的Shift键位z
        //    pos.x += m_ItemSpace.x;



        //    GameObject Down = CommonOperate.CreateObjWhithName ("Down");
        //    CreateKeyRow(
        //        "zxcvbnm",
        //        "ZXCVBNM",
        //        "&$()%#*",
        //        new KeyCode[] { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M },
        //          new KeyCode[] { KeyCode.Ampersand, KeyCode.Dollar, KeyCode.LeftParen, KeyCode.RightParen, KeyCode.F13, KeyCode.Hash, KeyCode.Asterisk },
        //        //************ KeyCode.F13 present % for This Time
        //        pos,
        //        1.0f,
        //        Down.transform
        //        );
        //    pos += new Vector2(m_Spacing.x * 8 + m_ItemSpace.x, 0);
        //    CreateKey("", "", "", KeyCode.Space, KeyCode.Space, pos, new Vector2(1f, 1.0f), Down.transform, true, "space");  //添加的Space键位z
        //    pos += new Vector2(m_Spacing.x + m_ItemSpace.x, 0);
        //    CreateKey("", "", "", KeyCode.Numlock, KeyCode.Numlock, pos, new Vector2(1f, 1.0f), Down.transform, true, "menu_Keyboard_Number");  //添加的切换键位z
        //    pos += new Vector2(m_Spacing.x * 1.5f + m_ItemSpace.x, 0);
        //    CreateKey("", "", "", KeyCode.Return, KeyCode.Return, pos, new Vector2(1.9f, 1.0f), Down.transform, true, "menu_Keyboard2");  //添加的Enter键位z
        //    pos.y -= m_Spacing.y + m_ItemSpace.y;


        //}
        #endregion

        /// <summary>
        /// 创建虚拟键盘并绑定到UImanager
        /// </summary>
        public void InitialVrKeyBoard()
        {
            keyboardState = VirtualKeyState.Normal;  //KeyBoard State
            m_NormalInputState = true;
            m_CapsShift = false;

            m_InputBG.enabled = false;
            gameObject.SetActive(false);
        }

        #region GetInput

        /// <summary>
        /// Start To Use Virtual Keyboard Get input
        /// </summary>
        /// <param name="_handle"></param>
        /// <param name="realTimeInput"></param>
        public void GetInput(VRInputField vrInput, VRInputRequest _inputRequest, VRKeyInputHandle _handle)
        {
            handleInput = _handle;
            inputRequest = _inputRequest;
            result = inputRequest.m_RealInput;  //自动保留之前输入的信息

            //Set KeyBoard Position
            gameObject.SetActive(true);
            m_VRInputFieldScript = vrInput;
            transform.localEulerAngles = _inputRequest.m_KeyBoardAngle;
            FollowTarget_Pos(vrInput.transform);
            vrInput.TransformListenerToolScript.AddEventListenner(TransformListenerTool.ListenTransformChangeEventEnum.LocalPosition, FollowTarget_Pos);  //开始跟随对象
#if UNITY_EDITOR
            vrInput.TransformListenerToolScript.AddEventListenner(TransformListenerTool.ListenTransformChangeEventEnum.Rotation, FollowTarget_Rotation);  //开始跟随对象
#endif

            GlobalEntity.GetInstance().Dispatch<bool>(VRKeyboardEvent.KeyBoardStateNotify, true);

            m_CapsShift = false;
            m_NormalInputState = true;
            appendChar = "";  //Reset

            if (m_OperateKeyButton != null)
                m_OperateKeyButton.transform.SetSiblingIndex(m_OperateKeyButton.m_Index);

            ShowKeyView();
            m_InputBG.enabled = false;
        }

        public void ButtonPressed(GameObject keyObj)
        {
            VirtualKeyButton button = keyObj.GetComponent<VirtualKeyButton>();
            errorMsg = "";
            errorMsgParameter.Clear();
            if (button != null)
            {
                ChangeKeyBoardState(button);  //Get Set Keyboard State
                ShowKeyView();

                if (button.GetKeyCode(VirtualKeyState.Normal) == KeyCode.Escape)
                {
                    #region Escape
                    CloseKeyboard();
                    return;
                    #endregion
                }  //关闭按钮

                if (button.GetKeyCode(VirtualKeyState.Normal) == KeyCode.Return)
                {
                    #region Enter
                    if (handleInput != null)
                        handleInput(result, true, errorMsg, errorMsgParameter.ToArray());  //返回输入信息

                    GlobalEntity.GetInstance().Dispatch<bool>(VRKeyboardEvent.KeyBoardStateNotify, false);
                    gameObject.SetActive(false);
                    return;
                    #endregion
                }

                if (button.GetKeyCode(VirtualKeyState.Normal) == KeyCode.Backspace)//&& m_DisplayText.text.Length > 0)
                { // remove last char
                    #region Backspace 回退
                    //   appendChar = "\b"; //回车
                    if (result.Length > 0)
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                    #endregion
                }
                else if (button.GetKeyCode(VirtualKeyState.Normal) == KeyCode.Clear)
                {
                    #region Clear 
                    appendChar = "";
                    result = "";
                    #endregion
                }
                else if (button.GetKeyCode(VirtualKeyState.Normal) == KeyCode.Numlock)
                {  //Numlock
                    appendChar = "";
                }
                else
                {
                    #region 其他
                    int len = Encoding.Default.GetBytes(result).Length;  //获取编码长度 区分汉字和字母
                    if (len >= inputRequest.m_Limite_Max && inputRequest.m_Limite_Max != 0)
                    {
                        result = result.Substring(0, inputRequest.m_Limite_Max);
                        errorMsg = "900007";  //>输入字符应少于{0}个
                        errorMsgParameter.Add(inputRequest.m_Limite_Max);
                    }
                    else
                    {
                        if (button.GetKeyCode(VirtualKeyState.Normal) == KeyCode.Space)
                            appendChar = " ";
                        else
                            appendChar = button.GetKeyChar(keyboardState);

                        #region Check Input string type
                        switch (inputRequest.m_InputContenType)
                        {
                            case InputContenType.Standard:
                                result += appendChar;
                                break;
                            case InputContenType.Number:
                                if (InputCheckRule.IsNumber(appendChar))
                                {
                                    result += appendChar;
                                }
                                else
                                {
                                    errorMsg = "900004"; //输入不是数字
                                }
                                break;
                            case InputContenType.Character:
                                if (InputCheckRule.IsChar(appendChar))
                                {
                                    result += appendChar;
                                }
                                else
                                {
                                    errorMsg = "900005"; //输入不是字母
                                }
                                break;
                            case InputContenType.NumberAndCharacter:
                                if (InputCheckRule.IsChar(appendChar) || InputCheckRule.IsNumber(appendChar))
                                {
                                    result += appendChar;
                                }
                                else
                                {
                                    errorMsg = "900006"; //输入不是字母或者数字
                                }
                                break;
                            case InputContenType.Customer:   //customerDefine
                                if (inputRequest.m_InputCheckHandle != null)
                                {
                                    if (inputRequest.m_InputCheckHandle(appendChar, ref result, out errorMsg, ref errorMsgParameter))
                                    {
                                        result += appendChar;
                                    }
                                }
                                else
                                {
                                    result += appendChar;
                                    errorMsg = "Miss Customer Check Input Rule!!!! ";
                                }
                                break;
                        }
                        #endregion
                    }
                    #endregion
                }

                if (handleInput != null)
                    handleInput(result, false, errorMsg, errorMsgParameter.ToArray());  //Return the input Result

            }
        }

        #endregion

        #region Input Check
        void ShowKeyView()
        {
            if (m_NormalInputState == false)
            {
                keyboardState = VirtualKeyState.Number;
            }
            else
            {
                if (m_CapsShift)
                    keyboardState = VirtualKeyState.NormalShift;
                else
                    keyboardState = VirtualKeyState.Normal;
            }
            //Debug.Log("keyboardState " + keyboardState);
            for (int _index = 0; _index < allVirtualKey.Count; _index++)
            {
                allVirtualKey[_index].Show(keyboardState);
            }
        }

        void ChangeKeyBoardState(VirtualKeyButton button)
        {
            if (button.GetKeyCode(VirtualKeyState.Normal) == KeyCode.LeftShift)  //Shift
                m_CapsShift = !m_CapsShift;
            if (button.GetKeyCode(VirtualKeyState.Normal) == KeyCode.Numlock) //Input Change
                m_NormalInputState = !m_NormalInputState;

        }

        #endregion



#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                CloseKeyboard();
        }
#endif

        void FollowTarget_Pos(Transform target)
        {
            Debug.Log("FollowTarget_Pos");
#if UNITY_ANDROID && !UNITY_EDITOR
            transform.DOMove(target.TransformPoint(inputRequest.m_KeyBoardOffeset_Gear), 0.05f);
         //   transform.position = target.TransformPoint(inputRequest.m_KeyBoardOffeset_Gear);
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            transform.DOMove(target.TransformPoint(inputRequest.m_KeyBoardOffeset_PC), 0.05f);
            //    transform.position = target.TransformPoint(inputRequest.m_KeyBoardOffeset_PC);
#endif
        }


        void FollowTarget_Rotation(Transform target)
        {
            Debug.Log("FollowTarget_Rotation");
#if UNITY_ANDROID && !UNITY_EDITOR
            transform.localEulerAngles = inputRequest.m_KeyBoardAngle;
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            transform.localEulerAngles = inputRequest.m_KeyBoardAngle;
#endif
        }


        void CloseKeyboard()
        {
            if (gameObject.activeSelf == false) return;

            if (handleInput != null)
                handleInput(result, true, errorMsg, errorMsgParameter.ToArray());  //返回输入信息


            GlobalEntity.GetInstance().Dispatch<bool>(VRKeyboardEvent.KeyBoardStateNotify, false);
            gameObject.SetActive(false);
        }

    }
}
