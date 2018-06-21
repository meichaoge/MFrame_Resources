using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Style
{


    #region Label
    private static GUIStyle m_Label_Style_Middle = null;
    public static GUIStyle  Label_Style_Middle
    {
        get
        {
            if(m_Label_Style_Middle == null)
            {
                m_Label_Style_Middle = new GUIStyle();
                m_Label_Style_Middle.alignment = TextAnchor.MiddleCenter;
                m_Label_Style_Middle.normal.textColor = Color.black;
            }
            return m_Label_Style_Middle;
        }
    }


    private static GUIStyle m_Label_Style_Title = null;
    public static GUIStyle Label_Style_Title
    {
        get
        {
            if (m_Label_Style_Title == null)
            {
                m_Label_Style_Title = new GUIStyle();
                m_Label_Style_Title.alignment = TextAnchor.MiddleCenter;
                m_Label_Style_Title.normal.textColor = Color.green;
                m_Label_Style_Title.fontSize = 30;
            }
            return m_Label_Style_Title;
        }
    }
    #endregion

    #region TextLabel
    private static GUIStyle m_TextField_Style_MiddleLeft = null;
    public static GUIStyle TextField_Style_MiddleLeft
    {
        get
        {
            if (m_TextField_Style_MiddleLeft == null)
            {
                m_TextField_Style_MiddleLeft = new GUIStyle();
                m_TextField_Style_MiddleLeft.alignment = TextAnchor.MiddleLeft;
                m_TextField_Style_MiddleLeft.normal.textColor = Color.white;
                m_TextField_Style_MiddleLeft.fontSize = 24;
             //   m_TextField_Style_MiddleLeft.
             //**需要设置背景TODO
            }
            return m_TextField_Style_MiddleLeft;
        }
    }
    #endregion

}
