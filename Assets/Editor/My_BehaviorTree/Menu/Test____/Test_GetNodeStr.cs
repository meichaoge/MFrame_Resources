using MFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 方便测试的脚本
    /// </summary>
    public class Test_GetNodeStr : MonoBehaviour
    {

        /// <summary>
        /// 遍历所有带有特定标签的脚本 获取设置的Action路径
        /// </summary>
        public static List<string> GetAllActionNode()
        {
            List<string> m_AllActionAttribute = new List<string>();

            //*******遍历获取程序集并获得所有标有指定特性的类
            //**这里获得所有标记由BehaviorActionAttribute 的Action 类
            IEnumerable<Type> availableTypes = AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(type => type.GetTypes());
            foreach (Type type in availableTypes)
            {
                foreach (BehaviorActionAttribute attribute in Attribute.GetCustomAttributes(type).OfType<BehaviorActionAttribute>())
                {
                    //      Debug.Log(attribute.ActionArrangement);
                    Debug.Log("GetAllActionNode : " + type.Name);

                    m_AllActionAttribute.Add(attribute.ActionArrangement);
                }
            }
            return m_AllActionAttribute;


            #region 测试数据
            m_AllActionAttribute.Add("生物/苹果x");
            m_AllActionAttribute.Add("生物/植物");
            m_AllActionAttribute.Add("生物/动物");
            m_AllActionAttribute.Add("生物/植物/蔬菜/白菜");
            m_AllActionAttribute.Add("生物/动物/宠物/猫");

            m_AllActionAttribute.Add("生物/动物");

            m_AllActionAttribute.Add("生物/植物/蔬菜/萝卜");
            m_AllActionAttribute.Add("生物");

            m_AllActionAttribute.Add("生物/动物/宠物/狗");
            m_AllActionAttribute.Add("生物/植物/水果/苹果");
            m_AllActionAttribute.Add("生物/动物/野生/老虎");

            m_AllActionAttribute.Add("生物/植物/水果/苹果");
            m_AllActionAttribute.Add("生物/植物/水果/火龙果");
            m_AllActionAttribute.Add("生物/植物/水果/香蕉");
            m_AllActionAttribute.Add("生物/植物/水果/西瓜");

            m_AllActionAttribute.Add("生物/人/白人");
            m_AllActionAttribute.Add("生物/人/黑人");
            m_AllActionAttribute.Add("生物/人/黄种人");

            m_AllActionAttribute.Add("生物/太阳系/地球");
            m_AllActionAttribute.Add("生物/太阳系/火星");
            m_AllActionAttribute.Add("生物/太阳系/天王星");
            m_AllActionAttribute.Add("生物/太阳系/海王星");
            m_AllActionAttribute.Add("生物/太阳系/冥王星");
            m_AllActionAttribute.Add("生物/太阳系/木星");

            m_AllActionAttribute.Add("生物/城市/大连");
            m_AllActionAttribute.Add("生物/城市/深圳");
            m_AllActionAttribute.Add("生物/城市/上海");
            m_AllActionAttribute.Add("生物/城市/南京");
            m_AllActionAttribute.Add("生物/城市/扬州");
            m_AllActionAttribute.Add("生物/城市/成都");
            m_AllActionAttribute.Add("生物/城市/东莞");
            m_AllActionAttribute.Add("生物/城市/岳阳");
            m_AllActionAttribute.Add("生物/城市/洛阳");
            m_AllActionAttribute.Add("生物/城市/西藏");

            m_AllActionAttribute.Add("生物/学科/数学");
            m_AllActionAttribute.Add("生物/学科/语文");
            m_AllActionAttribute.Add("生物/学科/物理");
            m_AllActionAttribute.Add("生物/学科/化学");
            m_AllActionAttribute.Add("生物/学科/地理");
            m_AllActionAttribute.Add("生物/学科/哲学");

            #endregion

        }


        public static List<object> GetAllActionNode2()
        {
            List<object> m_AllActionAttribute = new List<object>();

            //*******遍历获取程序集并获得所有标有指定特性的类
            //**这里获得所有标记由BehaviorActionAttribute 的Action 类
            IEnumerable<Type> availableTypes = AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(type => type.GetTypes());
            foreach (Type type in availableTypes)
            {
                foreach (BehaviorActionAttribute attribute in Attribute.GetCustomAttributes(type).OfType<BehaviorActionAttribute>())
                {
                    //      Debug.Log(attribute.ActionArrangement);
                    Debug.Log("GetAllActionNode : " + type.Name);
                    TreeViewNodeInfor_ActionNode nodeInfor = new TreeViewNodeInfor_ActionNode(attribute.ActionArrangement, type, attribute);
                    m_AllActionAttribute.Add(nodeInfor);
                }
            }
            return m_AllActionAttribute;

        }



    }
}
