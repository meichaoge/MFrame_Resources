using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MFramework
{
    /// <summary>
    /// 管理生成的MessageBox 按照一定顺序依次弹出
    /// </summary>
    public class MessageBoxModel : Singleton_Static<MessageBoxModel>
    {
        #region 消息系统  、公告板
        //public List<UIVRBrocardMessageBox> m_AllVRBrocard { private set; get; }  //系统公告板
        #endregion

        public static Dictionary<MessageBoxLevel, List<MsgBox>> AllPopMsgBoxCache { private set; get; } //所有的将要显示的Box缓存
        static int BoxIDRecord = 0;
        public int TotalBoxWaitingCot { private set; get; } //All The Box To be Show Count
        public bool IsShowingMsgBox { private set; get; } //To Identy Whether is Shoing Msg
        public MsgBox CurShowingMsgBox { private set; get; }
        public string m_SystemNotify; //场景提示信息  每次加载完场景会去检测



        #region Initial 
        protected override void InitialSingleton()
        {
            InitialModelData();
            //GlobalEntity.GetInstance().AddListener<string>(SceneStateController.mEvent.loadingSwitchNotify, OnSceneChange); //Listen SceneChange
            //GlobalEntity.GetInstance().AddListener(SceneStateController.mEvent.loadingOverNotify, OnSceneLoadOver);  //场景切换完成
            //GlobalEntity.GetInstance().AddListener<string>(SceneStateController.mEvent.loadingSwitchNotify, DestroyAllModelMsgBox); //监听场景切换逻辑
            //GlobalEntity.GetInstance().AddListener(BeanVRUIEvent.switchAccount, ClearLocalData);   //监听切换账号
        }


        void InitialModelData()
        {
            AllPopMsgBoxCache = new Dictionary<MessageBoxLevel, List<MsgBox>>();
            AllPopMsgBoxCache.Add(MessageBoxLevel.Top, new List<MsgBox>());
            AllPopMsgBoxCache.Add(MessageBoxLevel.High, new List<MsgBox>());
            AllPopMsgBoxCache.Add(MessageBoxLevel.NormalHigh, new List<MsgBox>());
            AllPopMsgBoxCache.Add(MessageBoxLevel.Normal, new List<MsgBox>());
            AllPopMsgBoxCache.Add(MessageBoxLevel.Lower, new List<MsgBox>());


            TotalBoxWaitingCot = 0;
            CurShowingMsgBox = null;
            IsShowingMsgBox = false;
        }

        #endregion


        /// <summary>
        /// Post Request To Dic And Select The Special One To Show
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callBack"></param>
        public void ShowMessageBox(MessageBoxResquest request, MessageBoxCallback callBack)
        {
            if (request == null) return;
            MsgBox _box = new MsgBox(request, callBack);

            if (AllPopMsgBoxCache.ContainsKey(request.m_BoxLevel) == false)
                AllPopMsgBoxCache[request.m_BoxLevel] = new List<MsgBox>();

            //***TopLevel Msg Need TODO
            AllPopMsgBoxCache[request.m_BoxLevel].Add(_box);

            ++TotalBoxWaitingCot;
            ++BoxIDRecord; //Idetify One Box
            GetMessageBoxToShow(_box);
        }

        public void RemoveMessageBox(int id, MessageBoxLevel boxLevel)
        {
            if (AllPopMsgBoxCache.ContainsKey(boxLevel) == false)
            {
                Debug.LogError("Can't Get This Type Of Box " + boxLevel + " id=" + id);
                return;
            }

            if (AllPopMsgBoxCache[boxLevel].Count == 0)
            {
                Debug.LogError("Can't Get This Type Of Box,There is No Record " + boxLevel + " id=" + id);
                return;
            }

            if (AllPopMsgBoxCache[boxLevel][0].BoxID != id)
            {
                Debug.LogError("The Wrong ID Of MSG " + AllPopMsgBoxCache[boxLevel][0].BoxID + "  ID" + id);
                return;
            }
            AllPopMsgBoxCache[boxLevel].RemoveAt(0); //Delete The First One
            --TotalBoxWaitingCot;
            IsShowingMsgBox = false;
            CurShowingMsgBox = null;
            GetMessageBoxToShow(null);
        }

        //Select One Special Box To Show Base On Level And Then Order
        void GetMessageBoxToShow(MsgBox _NewAddBox)
        {
            if (IsShowingMsgBox)
            {
                // Debug.Debug((_NewAddBox == null) + "   GetMessageBoxToShow");
                if (_NewAddBox != null && _NewAddBox.request.m_BoxLevel > CurShowingMsgBox.request.m_BoxLevel)
                { //Show Higher Level Box And Reset Previous
                    Debug.Log("Add New MsgBox which BoxLevel is Higher than current,Rest Previous Then Show This " + _NewAddBox.BoxID);
                    _NewAddBox.BaseMessageBoxObj = CurShowingMsgBox.BaseMessageBoxObj; //Save Reference
                    CurShowingMsgBox.BaseMessageBoxObj.Reset(); //Reset Previous View
                    CurShowingMsgBox.BaseMessageBoxObj.Show(_NewAddBox.request, _NewAddBox.callBack, _NewAddBox.BoxID); //Show New View
                    CurShowingMsgBox = _NewAddBox; //Update Record
                    return;
                }//if

                Debug.Log((_NewAddBox == null) + " isShowingMsgBox" + CurShowingMsgBox.BoxID + " ;;MSG " + CurShowingMsgBox.request.m_MessageInfor);
                return;
            }


            //   Debug.Debug("PopMessageBox ..............");

            if (AllPopMsgBoxCache[MessageBoxLevel.Top] != null && AllPopMsgBoxCache[MessageBoxLevel.Top].Count > 0)
            {
                CurShowingMsgBox = AllPopMsgBoxCache[MessageBoxLevel.Top][0];
                Debug.Log("PopMessageBox . Top  .............");
                PopMessageBox();
                return;
            }
            if (AllPopMsgBoxCache[MessageBoxLevel.High] != null && AllPopMsgBoxCache[MessageBoxLevel.High].Count > 0)
            {
                CurShowingMsgBox = AllPopMsgBoxCache[MessageBoxLevel.High][0];
                Debug.Log("PopMessageBox . High  .............");

                PopMessageBox();
                return;
            }

            if (AllPopMsgBoxCache[MessageBoxLevel.NormalHigh] != null && AllPopMsgBoxCache[MessageBoxLevel.NormalHigh].Count > 0)
            {
                CurShowingMsgBox = AllPopMsgBoxCache[MessageBoxLevel.NormalHigh][0];
                Debug.Log("PopMessageBox . NormalHigh  .............");

                PopMessageBox();
                return;
            }

            if (AllPopMsgBoxCache[MessageBoxLevel.Normal] != null && AllPopMsgBoxCache[MessageBoxLevel.Normal].Count > 0)
            {
                CurShowingMsgBox = AllPopMsgBoxCache[MessageBoxLevel.Normal][0];
                Debug.Log("PopMessageBox . Normal  .............");

                PopMessageBox();
                return;
            }
            if (AllPopMsgBoxCache[MessageBoxLevel.Lower] != null && AllPopMsgBoxCache[MessageBoxLevel.Lower].Count > 0)
            {
                CurShowingMsgBox = AllPopMsgBoxCache[MessageBoxLevel.Lower][0];
                Debug.Log("PopMessageBox . Lower  .............");

                PopMessageBox();
                return;
            }

            Debug.Log("There is No MessageBox ..............");


            IsShowingMsgBox = false; //No Msg To Show
            CurShowingMsgBox = null;
            TotalBoxWaitingCot = 0;
        }


        void PopMessageBox()
        {
            Debug.Log("Msg=" + CurShowingMsgBox.request.m_MessageInfor + "  BoxID=  " + CurShowingMsgBox.BoxID);
            CurShowingMsgBox.BaseMessageBoxObj = MessageBox.Show(CurShowingMsgBox.request, CurShowingMsgBox.callBack, CurShowingMsgBox.BoxID);
            IsShowingMsgBox = true;
        }



        /// <summary>
        /// 创建当前场景中的系统公告板
        /// </summary>
        void CreateVRBrocard()
        {
            //m_AllVRBrocard.Clear();

            //Dictionary<string, List<SceneObjCfg>> currentCfg =  UIManagerModel.GetInstance().GetSceneCfg(RoomModel.GetIns().CurRoomType); //获得当前场景中的配置
            //if (currentCfg == null)
            //{
            //    return;
            //}
            //List<SceneObjCfg> _cfg = null;
            //if (currentCfg.TryGetValue("VRBrocard", out _cfg))
            //{
            //    GameObject _brocardRes = Resources.Load<GameObject>(_cfg[0].m_SourcePath);
            //    if (_brocardRes != null)
            //    {
            //        for (int _index = 0; _index < _cfg.Count; _index++)
            //        {
            //            GameObject go = GameObject.Instantiate(_brocardRes);
            //            go.transform.localScale = _cfg[_index].m_LocalScale;
            //            go.transform.position = _cfg[_index].m_Position;
            //            go.transform.eulerAngles = _cfg[_index].m_LocalAngle;
            //            m_AllVRBrocard.Add(go.GetComponent<UIVRBrocardMessageBox>());  //添加到列表中
            //        }
            //        _cfg.Clear();
            //    }//if
            //    else
            //    {
            //        Debug.LogError("Source path Wrong!" + _cfg[0].m_SourcePath);
            //        _cfg.Clear();
            //        return;
            //    }//else
            //}//if

            //#region Realse Data
            //var dictionaryEnum = currentCfg.GetEnumerator();
            //while (dictionaryEnum.MoveNext())
            //{
            //    dictionaryEnum.Current.Value.Clear();
            //}
            //currentCfg.Clear();
            //#endregion
        }

        void DestroySceneBrocard(string _newSecneName)
        {
            //if (m_AllVRBrocard == null) return;
            //for (int _index = 0; _index < m_AllVRBrocard.Count; _index++)
            //{
            //    GameObject.Destroy(m_AllVRBrocard[_index].gameObject);
            //}
            //m_AllVRBrocard.Clear();
        }

        /// <summary>
        /// 销毁模态框
        /// </summary>
        void DestroyAllModelMsgBox(string _newSecneName)
        {
            DestroySceneBrocard(_newSecneName);
            //for (int _index = 0; _index < allModelMSGBox.Count; _index++)
            //{
            //    GameObject.Destroy(allModelMSGBox[_index].gameObject);
            //}
            //allModelMSGBox.Clear();
        }

        /// <summary>
        /// 显示系统公告板消息
        /// </summary>
        /// <param name="request">消息请求</param>
        /// <param name="handle">消息回调</param>
        public void ShowVRBrocardMessage(MessageBoxResquest request, MessageBoxCallback handle)
        {
            //if (m_AllVRBrocard.Count < 1)
            //{
            //    return;
            //}
            //for (int _index = 0; _index < m_AllVRBrocard.Count; _index++)
            //{
            //    if (m_AllVRBrocard[_index] != null)
            //    {
            //        m_AllVRBrocard[_index].Show(request, handle);
            //    }
            //}
        }

        ///// <summary>
        ///// 所有的模态系统弹框的注册事件  注册前需要检测 CanPopModelMessageBox
        ///// </summary>
        ///// <param name="_msgBox"></param>
        ///// <param name="_isShow">是打开还是关闭</param>
        //public void ModelMessageBoxRegest(BaseMessageBox _msgBox, bool _isShow)
        //{
        //    if (_isShow)
        //    {
        //        allModelMSGBox.Add(_msgBox);
        //        UIManagerModel.GetInstance().ForbiddenUI(true);  //禁用UI
        //    }
        //    else
        //    {
        //        allModelMSGBox.Remove(_msgBox);
        //        GameObject.Destroy(_msgBox.gameObject); //销毁
        //        if (allModelMSGBox.Count < 1)
        //        {
        //            UIManagerModel.GetInstance().ForbiddenUI(false);  //禁用UI
        //        }
        //        else
        //        {
        //            Debug.Log("还有" + allModelMSGBox.Count + "个没有处理");
        //        }
        //    }
        //}



        ///// <summary>
        ///// 检测是否可以弹出模态框
        ///// </summary>
        ///// <param name="_box"></param>
        ///// <param name="hideSameLevel">是否隐藏相同层显示当前层</param>
        ///// <returns></returns>
        //public bool CanPopModelMessageBox(MessageBoxLevel level, bool hideSameLevel)
        //{
        //    for (int _index = 0; _index < allModelMSGBox.Count; _index++)
        //    {
        //        if (hideSameLevel)
        //        { //隐藏同级别的
        //            if (allModelMSGBox[_index].GetBoxLevel() == level)
        //            {
        //                allModelMSGBox[_index].Hide(); //隐藏同级别的
        //            }
        //            else if (allModelMSGBox[_index].GetBoxLevel() > level)
        //            { //有更高级别的弹框
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            if (allModelMSGBox[_index].GetBoxLevel() >= level)
        //            {
        //                return false;  //有相同级别或者更高的其他模态框
        //            }
        //        }//esle
        //    }
        //    return true;

        //}



        #region 场景切换 /切换账号
        void OnSceneChange(string _newSecneName)
        {
            UITipMessageBox.ClearAllTips();
        }
        void OnSceneLoadOver()
        {
            Debug.Log("OnSceneLoadOver..... MessageBoxModel");
            if (!string.IsNullOrEmpty(m_SystemNotify))
            {
                MessageBoxResquest resque = new MessageBoxResquest();
                resque.m_WorldPosition = UIMessageBox.ShowDefaultPosition;
                MessageBox.SystemTip(m_SystemNotify, 3f, null, resque);  //弹出系统弹框 3秒后消失
                m_SystemNotify = "";
            }
        }


        /// <summary>
        /// 当切换账号时候清理本地数据
        /// </summary>
        void ClearLocalData()
        { //***这里不用处理 因为场景加载完已经清理 
            foreach (var item in AllPopMsgBoxCache)
                item.Value.Clear();

            TotalBoxWaitingCot = 0;
            CurShowingMsgBox = null;
            IsShowingMsgBox = false;
            m_SystemNotify = "";
            ////AllPopMsgBoxCache.Clear();  //不能清理否则再次进入报空
        }

        #endregion


        public class MsgBox
        {
            public MessageBoxResquest request;
            public MessageBoxCallback callBack;
            public int BoxID;
            public BaseMessageBox BaseMessageBoxObj;
            public MsgBox(MessageBoxResquest req, MessageBoxCallback _call)
            {
                request = req;
                callBack = _call;
                BoxID = BoxIDRecord;
                BaseMessageBoxObj = null;
            }
            private MsgBox() { }

        }

    }




}
