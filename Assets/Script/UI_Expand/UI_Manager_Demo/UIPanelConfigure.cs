using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#region  修改记录
//2017/7/7  增加 UIPanelLevel,并改成可序列化资源ScriptableObject  ，去掉  ParentPanel
#endregion


namespace MFramework    
{
    [System.Serializable]
    public class UIPanelConfgInfor
    {
        public string PanelName;
        public string PanelPath;
        public UIPanelLevel m_UIPanelLevel = UIPanelLevel.FixedDirectionUI; //UI层级
        public bool IsParentRoot = true;  //父节点是否是根节点
    }




}


