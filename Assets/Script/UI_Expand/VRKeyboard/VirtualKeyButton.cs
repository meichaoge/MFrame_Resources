using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MFramework
{

    public enum VirtualKeyState
    {
        Normal,
        NormalShift,
        Number
    }

    public class VirtualKeyButton : MonoBehaviour
    {
        public string m_DefaultImage;
        public Image m_Image;
        public Image m_ExtraImage;
        public Text m_KeyName;
        public VirtualKey virtualKeyInfor;
        bool NeedShow = true;
        public int m_Index;

        Vector3 initialLocalPosition;
        Vector3 initialLocalScale;

        UIEffect effect;

        void Awake()
        {
            effect = GetComponent<UIEffect>();
            effect.PointerUpEvent += KeyBoard.Instance.ButtonPressed;
        }


        void OnDestroy()
        {
            if (NeedShow)
            {
                effect.PointerEnterEvent -= OnPointerEnter;
                effect.PointerExitEvent -= OnPointerExit;
            }
            effect.PointerUpEvent -= KeyBoard.Instance.ButtonPressed;
        }


        public void InitilKey(VirtualKey key)
        {
            initialLocalPosition = transform.localPosition;
            initialLocalScale = transform.localScale;
            virtualKeyInfor = key;
            //   m_KeyName.text = key.m_NormalCharacter;
            Show(VirtualKeyState.Normal);  //初始显示正常的
            if (virtualKeyInfor.m_CharStateCode == KeyCode.Numlock)
            {
                m_Image.enabled = false;
                m_ExtraImage.enabled = true;
            }
        }

        void Start()
        {
            if (NeedShow)
            {
                effect.PointerEnterEvent += OnPointerEnter;
                effect.PointerExitEvent += OnPointerExit;
            }
        }


        public void Show(VirtualKeyState state)
        {
            switch (state)
            {
                case VirtualKeyState.Normal:
                    //  m_KeyName.text = LauguageTool.Ins.GetText(virtualKeyInfor.m_NormalCharacter);
                    m_KeyName.text = virtualKeyInfor.m_NormalCharacter;
                    break;
                case VirtualKeyState.NormalShift:
                    m_KeyName.text = virtualKeyInfor.m_ShiftCharacter;
                 //   m_KeyName.text = LauguageTool.Ins.GetText(virtualKeyInfor.m_ShiftCharacter);
                    break;
                case VirtualKeyState.Number:
                    m_KeyName.text = virtualKeyInfor.m_NumberCharacter;
                  //  m_KeyName.text = LauguageTool.Ins.GetText(virtualKeyInfor.m_NumberCharacter);
                    break;
            }
            SpecialShow(state);
        }


        void SpecialShow(VirtualKeyState state)
        {
            if (virtualKeyInfor.m_CharStateCode == KeyCode.Numlock)
            {
                if (state == VirtualKeyState.Number)
                {
                    m_KeyName.text = "";
                    m_ExtraImage.gameObject.SetActive(true);
                    //       Image.sprite = SpriteHelper.Instance.GetSpriteByName(virtualKeyInfor.m_KeyImage);
                }
                else
                {
                    m_KeyName.text = "123";
                    m_ExtraImage.gameObject.SetActive(false);
                    //    m_ExtraImage.sprite = SpriteHelper.Instance.GetSpriteByName(m_DefaultImage);
                }
            }
        }



        //Get KeyCode
        public KeyCode GetKeyCode(VirtualKeyState state)
        {
            KeyCode code = KeyCode.None;
            if (virtualKeyInfor == null) return code;
            switch (state)
            {
                case VirtualKeyState.Normal:
                    code = virtualKeyInfor.m_CharStateCode;
                    break;
                case VirtualKeyState.NormalShift:
                    code = virtualKeyInfor.m_CharStateCode;
                    break;
                case VirtualKeyState.Number:
                    code = virtualKeyInfor.m_NumberStateCode;
                    break;
            }
            return code;
        }

        //Get KeyChar
        public string GetKeyChar(VirtualKeyState state)
        {
            string charName = "";
            if (virtualKeyInfor == null) return charName;
            if (virtualKeyInfor.m_CharStateCode == KeyCode.Space) return " ";
            switch (state)
            {
                case VirtualKeyState.Normal:
                    charName = virtualKeyInfor.m_NormalCharacter;
                    break;
                case VirtualKeyState.NormalShift:
                    charName = virtualKeyInfor.m_ShiftCharacter;
                    break;
                case VirtualKeyState.Number:
                    charName = virtualKeyInfor.m_NumberCharacter;
                    break;
            }
            return charName;
        }


        void OnPointerEnter(GameObject parameter)
        {
            KeyBoard.Instance.m_OperateKeyButton = this;
            transform.SetAsLastSibling();
        }

        void OnPointerExit(GameObject parameter)
        {

            transform.SetSiblingIndex(m_Index);
            KeyBoard.Instance.m_OperateKeyButton = null;
        }



    }
}
