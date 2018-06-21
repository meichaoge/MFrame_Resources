using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    public class Constants 
    {
        public static Vector2 BehaviorTreeWinMinSize = new Vector2(800,600);   //行为树编辑器最小大小
        public static float NodeParametersWindowWidth = 300f; //节点参数框宽度
        public static Vector2 ContexNodeActionMenuSize = new Vector2(300, 300);//编辑区右键节点菜单栏大小

        public const  float ToolBarHeight = 20; //工具栏高度

        public static float NodeFieldAttributeNameWidth = 60; //每个节点的字段名长度
        public static float NodeFieldItemHeight = 30; // 每个节点的字段高度
        public static float NodeFieldOffsetBounder = 10;  //节点距离边界的距离
        public static float NodeFileItemSpace = 5; //每个节点两项之间的间距
        public static Vector2 NodeLinePointSize = new Vector2(30, 30); //连线点


       


    }
}