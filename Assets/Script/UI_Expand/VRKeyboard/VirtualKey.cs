using UnityEngine;
using System.Collections;

namespace MFramework
{

    [System.Serializable]
    public class VirtualKey
    {
        public string m_NormalCharacter;            //normalCharacter
        public string m_ShiftCharacter;   //char When Press Shift
        public KeyCode m_CharStateCode;  //CharState Code
        public KeyCode m_NumberStateCode;

        public string m_NumberCharacter;  //NumberState
        public string m_KeyImage = "";
        public bool m_Pressed;
        public bool m_Special;
    }
}
