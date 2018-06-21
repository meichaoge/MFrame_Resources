using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MFramework
{

    public enum InputContenType
    {
        Standard,    //any char 
        Number,     //just Number
        Character,  //just char
        NumberAndCharacter,  //number+char 
        Customer    //CustomerSelfDefine

    }
    /// <summary>
    /// Customer Input Check Rule 
    /// </summary>
    /// <param name="inputStr">current Input Char </param>
    /// <param name="previousInput">Already Input String</param>
    /// <returns></returns>
    public delegate bool CustomerDefineInputCheckHandle(string inputStr, ref string previousInput, out string errorMsg, ref List<object> errorMsgParameter);

    [System.Serializable]
    public class VRInputRequest
    {
        public InputContenType m_InputContenType;
        [Range(0, 20)]
        public int m_Limite_Max = 0;  //input char Number Limit
        [Range(0, 20)]
        public int m_Limite_Min = 0;  //input char Number Limit
        public string m_PreviousInput = "";  //previous input 
        public string m_RealInput = "";

        public bool m_PasswordType = false;
        public CustomerDefineInputCheckHandle m_InputCheckHandle;  //only user When is Customer InputContenType

        public Vector3 m_KeyBoardOffeset_PC = new Vector3(0, -0.27f, 0.1f);  //键盘的位置
        public Vector3 m_KeyBoardOffeset_Gear = new Vector3(0, -0.27f, 0.1f);  //键盘的位置

        public Vector3 m_KeyBoardAngle = Vector3.zero;  //旋转

        public Vector3 m_KeyBordShowPosition; //KeyBoardShow position
    }
}
