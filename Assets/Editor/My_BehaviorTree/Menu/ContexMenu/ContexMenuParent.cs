using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 上下文菜单形式的弹框 一般由右键唤出
    /// </summary>
    public  class ContexMenuParent : MenuParent
    {

        public MenuParent m_ParentAttachMenu; //该菜单附着的父菜单
        public bool IsActive { get; protected set; }  //是否处于激活状态 

        #region 构造函数
        public ContexMenuParent(float x, float y, float width, float height, MenuAnchor anchor, string titleName) :
            base(x, y, width, height, anchor, titleName)
        {
            IsActive = false;
        }

        public ContexMenuParent(Rect rect, MenuAnchor anchor, string titleName):
            base(rect, anchor, titleName)
        {
            IsActive = false;
        }
        #endregion

        public override void DrawMenu()
        {
            if(m_ParentAttachMenu==null)
            {
                Debug.LogError("ContexMenuParent  DrawMenu Fail,No Parent  " + m_MenuName);
                return;
            }

            //if (IsActive == false)
            //{
            //    Debug.Log("处于非激活状态"+ m_MenuName);
            //    return;
            //}

            base.DrawMenu();
        }


        public override bool IsInside(Vector2 pos)
        {
            if (IsActive == false) return false;
            return base.IsInside(pos);
        }

        /// <summary>
        /// 显示或者隐藏Contex 
        /// </summary>
        /// <param name="trySetActive"></param>
        public virtual void ShowOrHideContex(bool trySetActive)
        {
            if (trySetActive == IsActive) return;

            IsActive = trySetActive;
            if(trySetActive)
            {
                Debug.Log("ShowOrHideContex  激活");
            }
        }

        public virtual void UpdateShowPosition(Vector2 pos)
        {

        }


    }
}