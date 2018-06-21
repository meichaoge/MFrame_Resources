using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    public class NodeDrawerParent
    {
        protected string m_NotConnectLinePath = "Assets/Editor/EditorResources/WhitePortOutline.png";
        protected string m_ConnectingLinePath = "Assets/Editor/EditorResources/WhitePortSolid.png";

   

        /// <summary>
        /// 绘制一个节点的字段
        /// </summary>
        /// <param name="fieldInfor">字段信息</param>
        /// <param name="belongNode">所属的节点对象</param>
        public virtual void DrawNodeField(NodeParent.NodeFileInfor fieldInfor, NodeParent belongNode, float sartPosY)
        {

        }

        protected bool DrawInOutPutLinePoint(NodeParent.NodeFileInfor fieldInfor, NodeParent belongNode, float sartPosY, System.Action<FieldInfo> onClickLineNode)
        {
            if (fieldInfor.m_NodeAttribute.InOut == NodeInOutEnum.None) return false;
            NodeParent.NodeLineInfor lineInfor = belongNode.GetNodeLineInforOfField(fieldInfor.m_FieldInfo);
            if (lineInfor == null) return false;
            Rect rect = new Rect(0, sartPosY, Constants.NodeLinePointSize.x, Constants.NodeLinePointSize.y);

            //**记录当前线点在节点编辑区域的坐标
            lineInfor.m_Pos = belongNode.NodeSpace2EditorWinSpace(belongNode.NodeFieldItem2NodeSpace(new Vector2(rect.x,rect.y))); //BehaciorEditor Win 坐标系下坐标

            belongNode.RecordLinePoint(fieldInfor.m_FieldInfo, new Rect(lineInfor.m_Pos.x, lineInfor.m_Pos.y, rect.width, rect.height)); //记录

            if (lineInfor.m_ConnectNodeLine.Count == 0)
            {
                GUI.DrawTexture(rect, EditorImageHelper.GetImageByPath(m_NotConnectLinePath));

                //if (GUI.Button(rect, new GUIContent("", NoConnectImg)))
                //{
                //    if (onClickLineNode != null)
                //        onClickLineNode(fieldInfor.m_FieldInfo);
                //}

                //if(  GUILayout.Button(new GUIContent("", NoConnectImg), GUILayout.Width(Constants.NodeLinePointSize.x), GUILayout.Height(Constants.NodeLinePointSize.y)))
                //  {
                //      if (onClickLineNode != null)
                //          onClickLineNode(fieldInfor.m_FieldInfo);
                //  }
                // // GUI.DrawTexture(new Rect(0, 0, Constants.NodeLinePointSize.x, Constants.NodeLinePointSize.y), NoConnectImg);
            }
            else
            {
                GUI.DrawTexture(rect, EditorImageHelper.GetImageByPath(m_ConnectingLinePath));

                //if (GUILayout.Button(new GUIContent("", ConnectedImg), GUILayout.Width(Constants.NodeLinePointSize.x), GUILayout.Height(Constants.NodeLinePointSize.y)))
                //{
                //    if (onClickLineNode != null)
                //        onClickLineNode(fieldInfor.m_FieldInfo);
                //}
                //   GUI.DrawTexture(new Rect(0, 0, Constants.NodeLinePointSize.x, Constants.NodeLinePointSize.y), ConnectedImg);
            }
            return true;
        }

    }

    public class NodeDrawer_Int : NodeDrawerParent
    {
        protected static NodeDrawer_Int m_Instance = null;
        public static NodeDrawer_Int Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new NodeDrawer_Int();
                return m_Instance;
            }
        }

        public override void DrawNodeField(NodeParent.NodeFileInfor fieldInfor, NodeParent belongNode, float sartPosY)
        {
            if (fieldInfor == null) return;
            GUILayout.BeginHorizontal();
            DrawInOutPutLinePoint(fieldInfor, belongNode, sartPosY, belongNode.OnClickLineNode);
                GUILayout.Label("", GUILayout.Width(Constants.NodeLinePointSize.x));
            GUILayout.Label(new GUIContent(fieldInfor.m_NodeAttribute.Name), Style.Label_Style_Middle,
                GUILayout.Width(Constants.NodeFieldAttributeNameWidth), GUILayout.Height(Constants.NodeFieldItemHeight));

            object fileldValue = fieldInfor.m_FieldInfo.GetValue(fieldInfor.m_Script);
            if (fileldValue == null)
            {
                Debug.LogError("DrawNodeField Fail,Field " + fieldInfor.m_FieldInfo.Name + " Not Initialed or Value is Null");
                return;
            }
            //Debug.Log("fieldInfor.m_Node" + fieldInfor.m_Node+ "   fieldInfor.m_FieldInfo"+ fieldInfor.m_FieldInfo);
            //Debug.Log("xxxx " + fieldInfor.m_FieldInfo.GetValue(fieldInfor.m_Node));
            string inputVlue = GUILayout.TextField(fileldValue.ToString(), GUILayout.Height(Constants.NodeFieldItemHeight),
                GUILayout.Width(belongNode.GetNodeWidth() - Constants.NodeFieldAttributeNameWidth - Constants.NodeFieldOffsetBounder));
            //string inputVlue = GUI.TextField(new Rect(Constants.NodeFieldAttributeNameWidth, 0, belongNode.GetNodeWidth() - Constants.NodeFieldAttributeNameWidth, Constants.NodeFieldItemHeight),
            //    fileldValue.ToString());
            //Debug.Log("xx " + Convert.ToInt32(inputVlue));
            //  inputVlue = Convert.ToInt32(inputVlue).ToString();
            long tryValue = Convert.ToInt64(inputVlue);
            if (tryValue >= int.MaxValue || tryValue <= int.MinValue)
                return;  //越界
            fieldInfor.m_FieldInfo.SetValue(fieldInfor.m_Script, Convert.ToInt32(inputVlue));

            GUILayout.EndHorizontal();

        }
    }

    public class NodeDrawer_Float : NodeDrawerParent
    {
        protected static NodeDrawer_Float m_Instance = null;
        public static NodeDrawer_Float Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new NodeDrawer_Float();
                return m_Instance;
            }
        }
        public override void DrawNodeField(NodeParent.NodeFileInfor fieldInfor, NodeParent belongNode, float sartPosY)
        {
            if (fieldInfor == null) return;
            GUILayout.BeginHorizontal();
            DrawInOutPutLinePoint(fieldInfor, belongNode, sartPosY, belongNode.OnClickLineNode);
                GUILayout.Label("", GUILayout.Width(Constants.NodeLinePointSize.x));
            GUILayout.Label(new GUIContent(fieldInfor.m_NodeAttribute.Name), Style.Label_Style_Middle,
                GUILayout.Width(Constants.NodeFieldAttributeNameWidth), GUILayout.Height(Constants.NodeFieldItemHeight));

            object fileldValue = fieldInfor.m_FieldInfo.GetValue(fieldInfor.m_Script);
            if (fileldValue == null)
            {
                Debug.LogError("DrawNodeField Fail,Field " + fieldInfor.m_FieldInfo.Name + " Not Initialed or Value is Null");
                return;
            }
            //Debug.Log("fieldInfor.m_Node" + fieldInfor.m_Node+ "   fieldInfor.m_FieldInfo"+ fieldInfor.m_FieldInfo);
            //Debug.Log("xxxx " + fieldInfor.m_FieldInfo.GetValue(fieldInfor.m_Node));
            string inputVlue = GUILayout.TextField(fileldValue.ToString(), GUILayout.Height(Constants.NodeFieldItemHeight),
                GUILayout.Width(belongNode.GetNodeWidth() - Constants.NodeFieldAttributeNameWidth - Constants.NodeFieldOffsetBounder));
            //string inputVlue = GUI.TextField(new Rect(Constants.NodeFieldAttributeNameWidth, 0, belongNode.GetNodeWidth() - Constants.NodeFieldAttributeNameWidth, Constants.NodeFieldItemHeight),
            //    fileldValue.ToString());
            fieldInfor.m_FieldInfo.SetValue(fieldInfor.m_Script, (float)(Convert.ToDouble(inputVlue)));

            GUILayout.EndHorizontal();
        }
    }

    public class NodeDrawer_String : NodeDrawerParent
    {
        protected static NodeDrawer_String m_Instance = null;
        public static NodeDrawer_String Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new NodeDrawer_String();
                return m_Instance;
            }
        }
        public override void DrawNodeField(NodeParent.NodeFileInfor fieldInfor, NodeParent belongNode, float sartPosY)
        {
            if (fieldInfor == null)
                return;

            GUILayout.BeginHorizontal();

            DrawInOutPutLinePoint(fieldInfor, belongNode, sartPosY, belongNode.OnClickLineNode);
                GUILayout.Label("", GUILayout.Width(Constants.NodeLinePointSize.x));
            GUILayout.Label(new GUIContent(fieldInfor.m_NodeAttribute.Name), Style.Label_Style_Middle,
                GUILayout.Width(Constants.NodeFieldAttributeNameWidth), GUILayout.Height(Constants.NodeFieldItemHeight));
            object fileldValue = fieldInfor.m_FieldInfo.GetValue(fieldInfor.m_Script);

            if (fileldValue == null)
            {
                Debug.LogError("DrawNodeField Fail,Field " + fieldInfor.m_FieldInfo.Name + " Not Initialed or Value is Null");
                return;
            }
            //Debug.Log("fieldInfor.m_Node" + fieldInfor.m_Node+ "   fieldInfor.m_FieldInfo"+ fieldInfor.m_FieldInfo);
            //Debug.Log("xxxx " + fieldInfor.m_FieldInfo.GetValue(fieldInfor.m_Node));
            string inputVlue = GUILayout.TextField(fileldValue.ToString(), GUILayout.Height(Constants.NodeFieldItemHeight),
                GUILayout.Width(belongNode.GetNodeWidth() - Constants.NodeFieldAttributeNameWidth - Constants.NodeFieldOffsetBounder));
            //string inputVlue = GUI.TextField(new Rect(Constants.NodeFieldAttributeNameWidth, 0, belongNode.GetNodeWidth() - Constants.NodeFieldAttributeNameWidth, Constants.NodeFieldItemHeight),
            fieldInfor.m_FieldInfo.SetValue(fieldInfor.m_Script, inputVlue);
            GUILayout.EndHorizontal();

        }
    }



}