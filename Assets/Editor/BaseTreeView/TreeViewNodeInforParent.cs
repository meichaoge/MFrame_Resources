using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MFramework
{
    /// <summary>
    /// 树形结构节点的信息基类  包含当前节点用于构建树形结构的重要信息
    /// </summary>
    public class TreeViewNodeInfor
    {
        public string m_Arrangement; //以/分割的路径层次结构


        public TreeViewNodeInfor(string arrangement)
        {
            m_Arrangement = arrangement;
        }


    }
}