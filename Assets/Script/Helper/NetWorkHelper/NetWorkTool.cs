using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFramework;
using System;

namespace MFramework.NetWork
{
    /// <summary>
    /// ����������λ
    /// </summary>
    public enum NetDataEnum
    {
        B,
        KB,
        M,
        G,
        T,
        P
    }


    /// <summary>
    /// ����״̬������
    /// </summary>
    public class NetWorkTool : Singleton_Static<NetWorkTool>
    {
        private NetWorkStateEnum m_CurNetWorkStateEnum = NetWorkStateEnum.Unknow;
        public NetWorkStateEnum CurNetWorkStateEnum
        {
            get { return m_CurNetWorkStateEnum; }

            private set
            {
                if (value != m_CurNetWorkStateEnum)
                {
                    m_CurNetWorkStateEnum = value;
                    switch (m_CurNetWorkStateEnum)
                    {
                        case NetWorkStateEnum.NetNotValible:
                            if (OnNetWorkNotValibleEvent != null) OnNetWorkNotValibleEvent();
                            break;
                        case NetWorkStateEnum.UseWifiNet:
                            if (OnNetWorkNotValibleEvent != null) OnUseWifiNetEvent();
                            break;
                        case NetWorkStateEnum.UseFlowDataNet:
                            if (OnNetWorkNotValibleEvent != null) OnUseFlowDataNetEvent();
                            break;
                        default:
                            Debug.Log("Not Define " + m_CurNetWorkStateEnum);
                            break;
                    }//���绷���ı�
                }
            }
        } //��ǰ�����绷��


        public enum NetWorkStateEnum
        {
            Unknow,  //�޷�Ԥ֪
            NetNotValible,//���粻����
            UseWifiNet,  //Wifi ����
            UseFlowDataNet, //ʹ������
        }



        public Action OnNetWorkNotValibleEvent;
        public Action OnUseWifiNetEvent;
        public Action OnUseFlowDataNetEvent;

        protected override void InitialSingleton() { }

        /// <summary>
        /// �Ƿ���ʹ��Wifi ����
        /// </summary>
        /// <param name="callback"></param>
        public bool IsUsingWifiNet()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("IsUsingWifiNet False.... NetWork NotConnect");
                return false;
            }

            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                Debug.Log("IsUsingWifiNet false .... Use flow Data ..");
                return true;
            }
            Debug.Log("IsUsingWifiNet true     .... Use Wifi Data ..");
            return false;
        }

        /// <summary>
        /// �Ƿ�����ʹ������
        /// </summary>
        /// <returns></returns>
        public bool IsUsingFlowDataNet()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("IsUsingFlowDataNet False.... NetWork NotConnect");
                return false;
            }

            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                Debug.Log("IsUsingFlowDataNet false .... Use Wifi Data ..");
                return false;
            }

            Debug.Log("IsUsingFlowDataNet true .... Use flow Data ..");
            return false;
        }


        #region  ��������ʾ��ʽת��

        /// <summary>
        /// ������������ݴ�С������ ������ȷ������������ʾ��
        /// </summary>
        /// <param name="byteSize">���ݴ�С</param>
        /// <param name="dataEnum">���ݵ�λ</param>
        /// <param name="isUpdaRounding">true�Ƿ�������ȡ��,false ʱ��ֱ�ӳ���1024 ȡ�� </param>
        public void GetNetDataDesciption(ref int byteSize, ref NetDataEnum dataEnum, bool isUpdaRounding)
        {
            if (byteSize < 1024) return;
            if (isUpdaRounding)
                byteSize = (byteSize + 1023) / 1024;
            else
                byteSize = byteSize / 1024;
            dataEnum = (NetDataEnum)(dataEnum + 1);

            GetNetDataDesciption(ref byteSize, ref dataEnum, isUpdaRounding);
        }

        /// <summary>
        /// ������������ݴ�С������ ������ȷ������������ʾ��
        /// </summary>
        /// <param name="byteSize">���ݴ�С</param>
        /// <param name="dataEnum">���ݵ�λ</param>
        /// <param name="accuracy">��ʾ����ֵ��ȷ��С������漸λ</param>
        public void GetNetDataDesciption(ref float byteSize, ref NetDataEnum dataEnum,uint accuracy=2)
        {
            if (byteSize < 1024)
            {
                byteSize = (int)(Mathf.Pow(10, accuracy) * byteSize) / Mathf.Pow(10, accuracy);
                return;
            }
            byteSize = byteSize / 1024;
            dataEnum = (NetDataEnum)(dataEnum + 1);

            GetNetDataDesciption(ref byteSize, ref dataEnum, accuracy);
        }
        #endregion


    }
}
