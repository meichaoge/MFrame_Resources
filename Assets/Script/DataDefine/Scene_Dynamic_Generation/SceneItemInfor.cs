using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{

    [System.Serializable]
    //�����ļ���ÿһ���������Ϣ
    public class SceneItemInfor
    {
        public SerelizeVector3_Float m_Position; //λ��
        public SerelizeVector3_Float m_LocalAngle;  //�Ƕ�
        public SerelizeVector3_Float m_LocalScale;  //����
        public SerelizeVector3_Float m_Size;    //��С
        public string m_SourcePath;  //Prefab ��Դ�����resource ·��

    }


    ///******�༭����ʹ��Asset������Ϣ
    [System.Serializable]
    //ÿһ�����͵Ķ����������Ϣ
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
    //ÿһ�����������ж����������Ϣ
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

    //��������
    public enum SceneTypeEnum
    {
        StartScene = -1, //����
        MainLobby = 0, //������
        Concert = 1, // �ݳ���
        PartyRoom = 2,    //������
        Panorama360 = 3, //ȫ�� 360 
        Antiquaty = 4,   //�ŷ�
        Conference = 5,   //������
        Athena = 6,   //�ŵ���

        Login = 7, //��¼
        Guide = 100, //���ִ�
    }



}