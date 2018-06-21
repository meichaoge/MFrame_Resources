using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    //**********需要继续完善TODO   圆形布局
    public class CircularScrollLayout : MonoBehaviour
    {
        public Transform[] AllKeyPointTrans;
        [SerializeField]
        protected  int ShowCount = 4;
        [SerializeField]
        protected float m_TweenTime = 1f;
        public int MiddleSelectIndex = 0;  //正中间的元素

        protected bool m_IsInitialed = false;
        protected int m_FirstShowDataIndex = 0; //The First Point Show DataIndex

        #region 临时数据
        protected List<GameObject> m_AllDataBase;
        protected Vector3[] m_InitialShowPoints;
        [SerializeField]
        protected bool isComplete = true;

        #endregion

        #region Mono Frame
        protected virtual void Awake()
        {
            if (AllKeyPointTrans == null || AllKeyPointTrans.Length == 0)
            {
                Debug.LogError("Not Setting AllKeyPointTrans");
                m_IsInitialed = false;
                return;
            }
        }

        protected virtual void Start()
        {
            m_InitialShowPoints = new Vector3[AllKeyPointTrans.Length];
            for (int dex = 0; dex < AllKeyPointTrans.Length; ++dex)
            {  //Record Initialed Position
                m_InitialShowPoints[dex] = AllKeyPointTrans[dex].localPosition;
            }
        }

        #endregion
        /// <summary>
        /// 设置数据源显示
        /// </summary>
        /// <param name="data"></param>
        public void SetbindDataForShow(List<GameObject> data)
        {
            m_AllDataBase = new List<GameObject>(data);
            m_IsInitialed = true;
        }

        public virtual void Show()
        {
            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);

            DefaultView();
        }

        public virtual void Hide()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        protected virtual void DefaultView()
        {
            if (m_IsInitialed == false)
            {
                Debug.LogError("CircularScroll is Not Initialed ");
                return;
            }

            //int dex = 0;
            //for (dex = 0; dex < m_AllDataBase.Count; ++dex)
            //{
            //    if (m_AllDataBase[dex].GetComponent<SeleteRoleItem>().RoleID == LoginModel.CurrentPlayerInfor.BodyId)
            //        break;
            //}//if

            //var obj = m_AllDataBase[dex];
            //m_AllDataBase.RemoveAt(dex);
            //m_AllDataBase.Insert(1, obj);


            m_FirstShowDataIndex = 0;
            isComplete = true;
            for (int dex = 0; dex < AllKeyPointTrans.Length; ++dex)
            {
                if (dex < AllKeyPointTrans.Length)
                {
                    ShowPoint(dex, m_FirstShowDataIndex + dex);
                }
                else
                {
                    AllKeyPointTrans[dex].gameObject.SetActive(false);
                }
            }//for 

            MiddleSelectIndex = 1;
        }

        public void ChangeRoleView(bool isScrollLeft)
        {
            if (isComplete == false) return;
            isComplete = false;
            EventCenter.GetInstance().DelayDoEnumerator(m_TweenTime + Time.deltaTime, () =>
            {
                isComplete = true;
                //**   RoleSelectScript.ChoseRoleModel(middleRoleModelID);
            });
          
            if (isScrollLeft)
            {//
                OnScrollLeft();
                m_FirstShowDataIndex++;
            }
            else
            {
                OnScrollRight();
                m_FirstShowDataIndex--;
            }//esle

         //   Log4Helper.Debug("ChangeRoleView  Complete  isScrollLeft=" + isScrollLeft + " MiddleSelectIndex=" + MiddleSelectIndex);
        }


        protected virtual void OnScrollLeft()
        {
        //    transform.DORotate(new Vector3(0, 360 / AllKeyPointTrans.Length + transform.localEulerAngles.y, 0), m_TweenTime).OnStart(() =>
        //    {
        //        // ShowPoint(ShowCount + currentIndex, ShowCount + currentIndex);
        //    }).OnComplete(() =>
        //    {
        //        //  AllKeyPointTrans[GetTransIndex(currentIndex)].gameObject.SetActive(false);
        //        MiddleSelectIndex++;
        //        //      MiddleSelectIndex = GetTransIndex(MiddleSelectIndex);
        //        // middleRoleModelID = AllKeyPointTrans[MiddleSelectIndex].GetChild(0).GetComponent<SeleteRoleItem>().RoleID;
        //        //   Log4Helper.Debug("Complete !!!!  MiddleSelectIndex Roid=" + middleRoleModelID);

        //    });
        }

        protected virtual void OnScrollRight()
        {
            //transform.DORotate(new Vector3(0, -360 / AllKeyPointTrans.Length + transform.localEulerAngles.y, 0), m_TweenTime).OnStart(() =>
            //{
            //    //      ShowPoint(currentIndex - 1, currentIndex - 1);
            //}).OnComplete(() =>
            //{
            //    //      Log4Helper.Debug("currentIndex + ShowCount - 1=" + (currentIndex + ShowCount - 1));
            //    // AllKeyPointTrans[GetTransIndex(currentIndex + ShowCount - 1)].gameObject.SetActive(false);

            //    MiddleSelectIndex--;
            //    //  MiddleSelectIndex = GetTransIndex(MiddleSelectIndex);
            //    //    middleRoleModelID = AllKeyPointTrans[MiddleSelectIndex].GetChild(0).GetComponent<SeleteRoleItem>().RoleID;
            //    //     Log4Helper.Debug("Complete !!!!  MiddleSelectIndex Roid=" + middleRoleModelID);
            //});
        }




        protected Transform operateTrans;
        protected void ShowPoint(int transIndex, int dataIndex)
        {
            transIndex = GetTransRealIndex(transIndex);
            //Debug.Log("BBBB   previousValue="+ previousValue+ "   transIndex=" + transIndex);

            if (AllKeyPointTrans[transIndex].gameObject.activeSelf == false)
                AllKeyPointTrans[transIndex].gameObject.SetActive(true);

            dataIndex = GetDataRealIndex(dataIndex);

            while (AllKeyPointTrans[transIndex].childCount > 0)
            { //Clear Previous
                operateTrans = AllKeyPointTrans[transIndex].GetChild(0);
                operateTrans.SetParent(transform);
                if (operateTrans.gameObject.activeSelf)
                    operateTrans.gameObject.SetActive(false);
            }//while
            operateTrans = m_AllDataBase[dataIndex].transform;  //RoleModel
                                                                // Debug.Log(AllShowRolePoints[dataIndex].gameObject.name + "   ::" + transIndex + ":::" + dataIndex + "                      " + operateTrans.name);

            operateTrans.SetTransLocalState(Vector3.one, Vector3.zero, Vector3.zero, true, AllKeyPointTrans[transIndex]);
            //operateTrans.SetParent(AllKeyPointTrans[transIndex]);
            //operateTrans.localPosition = Vector3.zero;
            //operateTrans.localRotation = Quaternion.identity;
            //operateTrans.localScale = Vector3.one;
            operateTrans.gameObject.SetActive(true);

        }
        /// <summary>
        /// 根据参数索引获取实际的映射索引
        /// </summary>
        /// <param name="dex"></param>
        /// <returns></returns>
        protected int GetTransRealIndex(int dex)
        {
            if (dex > AllKeyPointTrans.Length - 1)
                dex = dex % (AllKeyPointTrans.Length);

            if (dex < 0)
            {
                dex = Mathf.Abs(dex) % (AllKeyPointTrans.Length);
                dex = -1 * dex + AllKeyPointTrans.Length;
                dex = dex % (AllKeyPointTrans.Length); //必须加上否则会出现dex=AllShowRolePoints.Length
            }
            return dex;
        }
        /// <summary>
        /// 根据参数索引获取实际的映射索引
        /// </summary>
        /// <param name="dex"> 当前虚拟的数据索引</param>
        /// <returns></returns>
        protected int GetDataRealIndex(int dex)
        {
            if (dex < 0)
            {
                dex = Mathf.Abs(dex) % (m_AllDataBase.Count);
                dex = -1 * dex + m_AllDataBase.Count;
            }
            if (dex > m_AllDataBase.Count - 1)
                dex = dex % (m_AllDataBase.Count);

            return dex;
        }


    }
}
