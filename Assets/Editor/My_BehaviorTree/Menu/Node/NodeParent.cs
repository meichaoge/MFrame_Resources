using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 节点的父类
    /// </summary>
    [System.Serializable]
    public class NodeParent
    {
        /// <summary>
        /// 节点字段信息
        /// </summary>
        public class NodeFileInfor
        {
            public NodeAttribute m_NodeAttribute;
            public FieldInfo m_FieldInfo;
            public NodeActionParent m_Script;
            public Type m_RealType;

            public NodeFileInfor(NodeAttribute attribute, FieldInfo field, NodeActionParent script, Type type)
            {
                m_NodeAttribute = attribute;
                m_FieldInfo = field;
                m_Script = script;
                m_RealType = type;
            }
        }


        /// <summary>
        /// 节点连线信息
        /// </summary>
        public class NodeLineInfor
        {
            public FieldInfo m_NodeFieldInfor;
            public NodeInOutEnum m_LineType;
            public bool m_IsSignalConnected;

            public List<NodeLineInfor> m_ConnectNodeLine = new List<NodeLineInfor>();  //所有关联的节点
            public Vector2 m_Pos = Vector2.zero;
            public NodeParent m_BelongNode = null; //所属于的节点

            protected static HashSet<string> m_AllNodeLinePointID = new HashSet<string>(); //记录所有使用过的ID
            public string NodeLinePointID {
                get;
                private set;
            } //标识该节点连线的ID


            public NodeLineInfor(FieldInfo field, NodeInOutEnum fieldType,bool isSignalConnect, NodeParent belongto)
            {
                m_NodeFieldInfor = field;
                m_LineType = fieldType;
                m_IsSignalConnected = isSignalConnect;
                m_BelongNode = belongto;
                NodeLinePointID = GenerateUID.GetUID64(); //获得64位的ID；
                while (m_AllNodeLinePointID.Contains(NodeLinePointID))
                {
                    NodeLinePointID = GenerateUID.GetUID64(); //获得64位的ID；
                }
            }


            public static void  TryAddLineConnect(NodeLineInfor begin, NodeLineInfor end)
            {
                if (begin == null || end == null)
                {
                    //Debug.Log("TryAddLineConnect Fail, " + (begin == null) + " :  " + (end == null));
                    return;
                }

                //if(begin.m_LineType== NodeInOutEnum.None|| begin.m_LineType == NodeInOutEnum.In)
                //    return; //起点不是输出节点
                //if (end.m_LineType != NodeInOutEnum.InOut || end.m_LineType != NodeInOutEnum.In)
                //    return; //起点不是输入节点

                for (int dex=0;dex< begin. m_ConnectNodeLine.Count;++dex)
                {
                    if(begin.m_ConnectNodeLine[dex]==end)
                        return; //已经包含了这个连线
                }

                for (int dex = 0; dex < end.m_ConnectNodeLine.Count; ++dex)
                {
                    if (end.m_ConnectNodeLine[dex] == begin)
                        return; //已经包含了这个连线
                }

                begin.m_ConnectNodeLine.Add(end);
                end.m_ConnectNodeLine.Add(begin);

                LinConnectInfor connected = new LinConnectInfor(begin,end);
                BehaviorTreeEditorMenu.RecordConnectedLine(connected);
            }


            /// <summary>
            /// 检测当前连线点是否可以作为连出节点
            /// </summary>
            /// <param name="nodeLinePoint"></param>
            /// <returns></returns>
            public static bool CheckCanLineOut(NodeLineInfor nodeLinePoint)
            {
                //if (nodeLinePoint.m_LineType == NodeInOutEnum.None || nodeLinePoint.m_LineType == NodeInOutEnum.In)
                //{
                //    Debug.Log("CheckCanLineOut  False Not Right LineType:" + nodeLinePoint.m_LineType);
                //    return false;
                //}
                if (nodeLinePoint.m_IsSignalConnected)
                {
                    if (nodeLinePoint.m_ConnectNodeLine.Count > 1)
                    {
                        Debug.LogError("CheckCanLineOut To Much Line COnnect");
                        return false;
                    }
                    if (nodeLinePoint.m_ConnectNodeLine.Count == 1)
                    {
                        Debug.LogError("CheckCanLineOut Allready Connect");
                        return false;  //已经连线了
                    }
                }
                return true;
            }

            /// <summary>
            /// 检测当前连线点是否可以作为连入节点
            /// </summary>
            /// <param name="nodeLinePoint"></param>
            /// <returns></returns>
            public static bool CheckCanLineIn(NodeLineInfor nodeLinePoint)
            {
                //if (nodeLinePoint.m_LineType == NodeInOutEnum.None || nodeLinePoint.m_LineType == NodeInOutEnum.Out)
                //    return false;
                if (nodeLinePoint.m_IsSignalConnected)
                {
                    if (nodeLinePoint.m_ConnectNodeLine.Count > 1)
                    {
                        Debug.LogError("CheckCanLineOut To Much Line COnnect");
                        return false;
                    }
                    if (nodeLinePoint.m_ConnectNodeLine.Count == 1)
                        return false;  //已经连线了
                }
                return true;
            }

        }


        public class LinConnectInfor
        {
            public NodeLineInfor m_BeginNodeLintPoint;
            public NodeLineInfor m_EndNodeLintPoint;
           // public string m_LineID;

            public LinConnectInfor(NodeLineInfor @from, NodeLineInfor @to)
            {
                m_BeginNodeLintPoint = @from;
                m_EndNodeLintPoint = @to;
             //   m_LineID = @from.NodeLinePointID; //取起点的ID 作为线标识
            }
        }



        protected readonly List<NodeFileInfor> m_AllNodeFiled = new List<NodeFileInfor>();  //所有公开的标有NodeAttribute 特性的字段
        public float m_NodeBaseWidth = 200; //基础宽度
        protected float m_NodeTitleHeight = 50; //每一个节点有一个title
        protected float m_NodeWidthScale = 1; //由于不同类型对应使用不同的宽度
        protected float m_NodeHeightScale = 1; //由于不同类型对应使用不同的高度
        protected bool m_IsInitialed = false;
        public Vector2 m_Pos;
        protected Dictionary<FieldInfo, NodeLineInfor> m_AllNodeLinesInfor = new Dictionary<FieldInfo, NodeLineInfor>(); //所有的连线点信息
        Dictionary<FieldInfo, Rect> m_AllLinPoint = new Dictionary<FieldInfo, Rect>(); //所有的连线点
        public static NodeLineInfor m_SelectNodeLineBegin = null;  //任意时刻选择的连线点
        public static NodeLineInfor m_SelectNodeLineEnd = null;  //任意时刻选择的连线点

        protected string m_NodeBgImg = "Assets/Editor/EditorResources/ActionNodeItembg.png";
        public bool IsSelectNodeLinePoint { get; protected set; }  //光标点击时候是否在连线点上
        public bool IsSelectNode { get; protected set; }  //光标点击时候是否在节点上且不在连线点上


        public float GetNodeWidth()
        {
            return m_NodeBaseWidth * m_NodeWidthScale;
        }
        public float GetNodeHeight()
        {
            return m_NodeTitleHeight + (Constants.NodeFieldItemHeight + Constants.NodeFileItemSpace) * m_AllNodeFiled.Count;
        }


        public NodeParent(NodeActionParent scriptObj)
        {
            FieldInfo[] allfields = scriptObj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in allfields)
            {
                NodeAttribute attribute = Attribute.GetCustomAttribute(field, typeof(NodeAttribute)) as NodeAttribute;
                if (attribute != null)
                {
                    object Value = field.GetValue(scriptObj);
                    if (Value == null)
                    {
                        field.SetValue(scriptObj, attribute.DefaultValue);  //如果没有设置默认值则使用特性中定义的默认值
                        //Debug.Log(">>>>  " + field.Name);
                    }

                    if (attribute.InOut != NodeInOutEnum.None)
                    {
                        NodeLineInfor nodeLine = new NodeLineInfor(field, attribute.InOut, attribute.SignalConnect, this);
                        m_AllNodeLinesInfor.Add(field, nodeLine);
                    }//记录所有的节点

                }
            }
        }


        #region  绘制
        /// <summary>
        /// 绘制当前节点
        /// </summary>
        public virtual void DrawNode(EditorRecordNode recordNode)
        {
            if (m_IsInitialed == false)
            {
                m_IsInitialed = true;
                //Debug.Log("DrawNode >>>  Initial");
                GetAllNodeField(recordNode.m_ActionScript);
            }
            Rect rect = new Rect(m_Pos.x, m_Pos.y, GetNodeWidth(), GetNodeHeight());
            //  Debug.Log("DrawNode rectrect= " + rect);
            GUI.DrawTexture(rect, EditorImageHelper.GetImageByPath(m_NodeBgImg));
            //Debug.Log("xxx zz  " + m_NodeBaseWidth * m_NodeWidthScale);
            GUILayout.BeginArea(rect);
            GUILayout.BeginVertical();
            #region Title
            GUILayout.Label(new GUIContent(recordNode.m_ActionScript.m_BehaviorActionAttribute.ActionType.Name), Style.Label_Style_Title,
                GUILayout.Width(GetNodeWidth() - Constants.NodeFieldOffsetBounder), GUILayout.Height(m_NodeTitleHeight));

            #endregion

            #region Node Field
            GUILayout.BeginArea(new Rect(0, m_NodeTitleHeight, rect.width, rect.height - m_NodeTitleHeight));
            float startPos = 0;
            foreach (var item in m_AllNodeFiled)
            {
                DrawNodeField(item, startPos);
                GUILayout.Space(Constants.NodeFileItemSpace);
                startPos += Constants.NodeFileItemSpace + GetNodeHeight(item.m_NodeAttribute) * Constants.NodeFieldItemHeight;
            }
            GUILayout.EndArea();
            #endregion

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        /// <summary>
        /// 根据节点的字段类型调用对应的绘制接口
        /// </summary>
        /// <param name="item"></param>
        void DrawNodeField(NodeFileInfor item, float pos)
        {
            switch (item.m_NodeAttribute.ValueType)
            {
                case NodeValueTypeEnum.Int:
                    NodeDrawer_Int.Instance.DrawNodeField(item, this, pos);
                    break;
                case NodeValueTypeEnum.Float:
                    NodeDrawer_Float.Instance.DrawNodeField(item, this, pos);
                    break;
                case NodeValueTypeEnum.String:
                    NodeDrawer_String.Instance.DrawNodeField(item, this, pos);
                    break;
            }
        }

        /// <summary>
        /// 获取当前节点显示的宽度比例
        /// </summary>
        /// <param name="attribute"></param>
        protected void GetNodeWidth(NodeAttribute attribute)
        {
            if (attribute == null) return;
            float scale_width = 1;
            switch (attribute.ValueType)
            {
                case NodeValueTypeEnum.Int:
                case NodeValueTypeEnum.Float:
                    scale_width = 1;
                    break;
                case NodeValueTypeEnum.String:
                case NodeValueTypeEnum.Vector2:
                    scale_width = 1.5f;
                    break;
                case NodeValueTypeEnum.Vector3:
                case NodeValueTypeEnum.Vector4:
                case NodeValueTypeEnum.Matrix3x3:
                case NodeValueTypeEnum.Matrix4x4:
                case NodeValueTypeEnum.Color3:
                case NodeValueTypeEnum.Color4:
                    scale_width = 2;
                    break;

                default:
                    Debug.Log("GetNodeWidth Not Define" + attribute.ValueType);
                    scale_width = 1;
                    break;
            }

            if (m_NodeWidthScale < scale_width)
                m_NodeWidthScale = scale_width;
        }

        /// <summary>
        /// 获取当前节点显示的高度比例
        /// </summary>
        /// <param name="attribute"></param>
        protected float GetNodeHeight(NodeAttribute attribute)
        {
            if (attribute == null) return 0;
            float scale_height = 1;
            switch (attribute.ValueType)
            {
                case NodeValueTypeEnum.Int:
                case NodeValueTypeEnum.Float:
                case NodeValueTypeEnum.String:
                    scale_height = 1;
                    break;
                case NodeValueTypeEnum.Vector2:
                case NodeValueTypeEnum.Vector3:
                case NodeValueTypeEnum.Vector4:
                    scale_height = 2f;
                    break;
                case NodeValueTypeEnum.Matrix3x3:
                    scale_height = 3f;
                    break;
                case NodeValueTypeEnum.Matrix4x4:
                    scale_height = 4f;
                    break;
                case NodeValueTypeEnum.Color3:
                case NodeValueTypeEnum.Color4:
                    scale_height = 1;
                    break;

                default:
                    Debug.Log("GetNodeWidth Not Define" + attribute.ValueType);
                    scale_height = 1;
                    break;
            }

            return scale_height;
        }

        #endregion

        #region 遍历获取当前类型的所有指定的字段以便于绘制

        /// <summary>
        /// 获取所有标记为NodeAttribute 特性的字段
        /// </summary>
        protected virtual void GetAllNodeField(NodeActionParent scriptAction)
        {
            m_AllNodeFiled.Clear();
            FieldInfo[] allfields = scriptAction.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in allfields)
            {
                NodeAttribute attribute = Attribute.GetCustomAttribute(field, typeof(NodeAttribute)) as NodeAttribute;
                if (attribute != null)
                {
                    Debug.Log("GetAllNodeField   " + attribute.Name + "  " + field.Name);
                    GetNodeWidth(attribute);
                    m_NodeHeightScale += GetNodeHeight(attribute);
                    NodeFileInfor infor = new NodeFileInfor(attribute, field, scriptAction, scriptAction.GetType());
                    m_AllNodeFiled.Add(infor);
                }

            }
        }
        #endregion


        #region 当前Node 的属性

        public object GetDefaultValueSetting(FieldInfo field, NodeParent node)
        {
            return null;
        }

        #endregion

        /// <summary>
        /// 根据字段获取关联的连线点
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public NodeLineInfor GetNodeLineInforOfField(FieldInfo field)
        {
            NodeLineInfor infor = null;

            if (m_AllNodeLinesInfor.TryGetValue(field, out infor))
            {
                return infor;
            }
            Debug.LogError("GetNodeLineInforOfField Fail " + field.Name);
            return null;
        }



        /// <summary>
        /// 更新当前节点是否选中/是否选择连线点的状态
        /// </summary>
        /// <param name="mousPos"></param>
        public void UpdateNodeSelectState(Vector2 mousPos, bool isGetStartPoint)
        {
            Rect rect = new Rect(m_Pos.x, m_Pos.y, GetNodeWidth(), GetNodeHeight());
            IsSelectNode = rect.Contains(mousPos);  //是否在节点的显示区域中
            if (IsSelectNode == false) return;

            foreach (var item in m_AllLinPoint)
            {
                //Debug.Log("item " + item.Value + " :" + item.Value.Contains(mousPos) + "   mousPos" + mousPos);
                if (item.Value.Contains(mousPos))
                {
                    NodeLineInfor currentNodeLine = GetNodeLineInforOfField(item.Key);

                    #region     获得起点
                    if (isGetStartPoint)
                    {
                        if (m_SelectNodeLineBegin == null)
                        {
                            m_SelectNodeLineEnd = null;
                            if (NodeLineInfor.CheckCanLineOut(currentNodeLine))
                            {
                                m_SelectNodeLineBegin = currentNodeLine;
                                IsSelectNodeLinePoint = true;
                                IsSelectNode = false; //说明当前选中的是节点的连线点
                                //Debug.Log("Select Lint Point");
                                return;
                            } //当前点可以作为连出节点
                        }
                    }
                    #endregion

                    #region 获取终点
                    if (isGetStartPoint == false)
                    {
                        if(m_SelectNodeLineBegin==null)
                            return;

                        if (currentNodeLine.m_BelongNode == m_SelectNodeLineBegin.m_BelongNode)
                        {
                            m_SelectNodeLineEnd = null;
                            return;  //起点和终点不允许是同一个节点
                        }
                        else
                        {
                            if (NodeLineInfor.CheckCanLineIn(currentNodeLine))
                            {
                                m_SelectNodeLineEnd = currentNodeLine;
                                IsSelectNodeLinePoint = true;
                                IsSelectNode = false; //说明当前选中的是节点的连线点
                                                      //  Debug.Log("Select Lint Point");
                                return;
                            }//可以作为连入节点
                        }
                    }
                    #endregion

                }
            }//foreach
            IsSelectNodeLinePoint = false;
        }





        public void OnClickLineNode(FieldInfo field)
        {
            //Debug.Log("mmmmm"+field.Name);
        }

        public void RecordLinePoint(FieldInfo field, Rect rect)
        {
            Rect rectInfor = new Rect();
            if (m_AllLinPoint.TryGetValue(field, out rectInfor))
            {
                m_AllLinPoint[field] = rect;
            }
            else
            {
                m_AllLinPoint.Add(field, rect);
            }
        }

        /// <summary>
        /// 节点的项坐标系转成节点坐标系 项坐标系是在标题下开始的
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Vector2 NodeFieldItem2NodeSpace(Vector2 pos)
        {
            pos.y += m_NodeTitleHeight;
            return pos;
        }

        /// <summary>
        /// 节点坐标系转成编辑器的坐标系
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Vector2 NodeSpace2EditorWinSpace(Vector2 pos)
        {
            pos.y += m_Pos.y;  //更新节点的真实位置 转到EditorWindow坐标系下
            pos.x += m_Pos.x;
            return pos;
        }


        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="pos"></param>
        public void MoveNode(Vector2 pos)
        {
            m_Pos += pos;
        }

    }
}