using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MFramework
{

    public class InputCheckRule
    {

        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool IsNumber(string msg)
        {
            Regex rex = new Regex(@"^\d+$");
            return rex.IsMatch(msg);
        }
        /// <summary>
        /// 判断是否是字符
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool IsChar(string msg)
        {
            //Regex reg1 = new Regex(@"^[A-Za-z0-9]+$");
            Regex reg1 = new Regex(@"^[A-Za-z]+$");
            return reg1.IsMatch(msg);
        }

        //UserNameInputField Check Rule
        public static bool CheckUserNameRule(string inputStr, ref string previousInput, out string errorMsg, ref List<object> errorMsgParameter)
        {
            bool result;
            errorMsg = "";
            if (previousInput.Length == 0)
            {//First Char Shoud Be Char 
                if (IsChar(inputStr) || inputStr == "_")
                    result = true;
                else
                {
                    result = false;
                    //   errorMsg = LauguageTool.Ins.GetText("900000"); //第一个字符必须是字母或者下划线
                    errorMsg ="900000"; //第一个字符必须是字母或者下划线
                }
            }
            else
            {
                if (IsChar(inputStr) || IsNumber(inputStr) || inputStr == "_")
                    result = true;
                else
                {
                    result = false;
                    // errorMsg = LauguageTool.Ins.GetText("900001"); //请输入数字或者字母或者下划线
                    errorMsg = "900001"; //请输入数字或者字母或者下划线
                }
            }
            return result;
        }



    }
}
