using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#region  �޸ļ�¼
//2017/7/7  ���� UIPanelLevel,���ĳɿ����л���ԴScriptableObject  ��ȥ��  ParentPanel
#endregion


namespace MFramework    
{
    [System.Serializable]
    public class UIPanelConfgInfor
    {
        public string PanelName;
        public string PanelPath;
        public UIPanelLevel m_UIPanelLevel = UIPanelLevel.FixedDirectionUI; //UI�㼶
        public bool IsParentRoot = true;  //���ڵ��Ƿ��Ǹ��ڵ�
    }




}


