using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace MFramework
{
    /// <summary>
    /// ���ı��༭
    /// </summary>
    public class ConfgRichTextTool
    {
        //����ɸ��ı���ʽ
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