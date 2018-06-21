using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace MFramework
{
    /// <summary>
    /// 富文本编辑
    /// </summary>
    public class ConfgRichTextTool
    {
        //编码成富文本形式
        public static string BuildStringToHtml(string color, string msg)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<color=");
            builder.Append(color);
            builder.Append(">");
            builder.Append(msg);
            builder.Append("</color>");

            return builder.ToString();
        }

    }
}