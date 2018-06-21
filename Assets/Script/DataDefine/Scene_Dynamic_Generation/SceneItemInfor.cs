using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{

    [System.Serializable]
    //配置文件中每一个对象的信息
    public class SceneItemInfor
    {
        public SerelizeVector3_Float m_Position; //位置
        public SerelizeVector3_Float m_LocalAngle;  //角度
        public SerelizeVector3_Float m_LocalScale;  //缩放
        public SerelizeVector3_Float m_Size;    //大小
        public string m_SourcePath;  //Prefab 资源相对于resource 路径

    }


    ///******编辑器下使用Asset配置信息
    [System.Serializable]
    //每一种类型的对象的配置信息
    public class SceneItemInfor_EachType
    {
        public string ItemName;
        public List<SceneItemInfor> ItemConfgInfor = new List<SceneItemInfor>();

        public override string ToString()
        {
            string msg = "ItemName="+ ItemName;
            foreach (var item in ItemConfgInfor)
            {
                msg += "m_Position=" + item.m_Position.ToString();
                msg += "m_LocalAngle=" + item.m_LocalAngle.ToString();
                msg += "m_LocalScale=" + item.m_LocalScale.ToString();
                msg += "m_Size=" + item.m_Size.ToString();
                msg += item.m_SourcePath;
            }
            return msg;
        }

    }

    [System.Serializable]
    //每一个场景中所有对象的配置信息
    public class SceneItemGenarationInfor
    {
        public SceneTypeEnum CurScene;
        public List<SceneItemInfor_EachType> AllSceneItemType = new List<SceneItemInfor_EachType>();

        public override string ToString()
        {
            string msg = "CurScene=" + CurScene;
            foreach (var item in AllSceneItemType)
            {
                msg += item.ToString();
            }
            return msg;
        }
    }

    //场景类型
    public enum SceneTypeEnum
    {
        StartScene = -1, //启动
        MainLobby = 0, //主大厅
        Concert = 1, // 演唱会
        PartyRoom = 2,    //聊天室
        Panorama360 = 3, //全景 360 
        Antiquaty = 4,   //古风
        Conference = 5,   //发布会
        Athena = 6,   //雅典娜

        Login = 7, //登录
        Guide = 100, //新手村
    }



}