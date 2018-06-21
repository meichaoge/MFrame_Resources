using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

namespace MFramework
{
    public enum UIPanelLevel
    {
        FixedDirectionUI,  //固定方向(如主UI)
        FollowTargetUI,    //跟随UI(如键盘)
        SceneUI, //Only Show At Special Scene
    }

    /// <summary>
    /// UI 面板保存的信息
    /// </summary>
    public class PanelInfor
    {
        public GameObject m_Panel;
        public ViewBaseFU_EX m_ViewControlScript;  //控制脚本
        public PanelInfor(GameObject obj, ViewBaseFU_EX viewScript)
        {
            m_Panel = obj;
            m_ViewControlScript = viewScript;
        }
    }


    /// <summary>
    /// 管理UI面板之剑的交互
    /// </summary>
    public class UIManagerModel : Singleton_Static<UIManagerModel>
    {
        Dictionary<UIPanelLevel, Transform> m_AllUIRootBaseOnLevelDic = new Dictionary<UIPanelLevel, Transform>(); //存储所有的层级UI主节点
        Dictionary<string, UIPanelConfgInfor> m_AllUIPanelConf = new Dictionary<string, UIPanelConfgInfor>(); //UIPanel配置文件
        HashSet<UICanvasController> m_AllCanvas = new HashSet<UICanvasController>();
        Dictionary<Type, PanelInfor> m_AllInitialedPanel = new Dictionary<Type, PanelInfor>(); //所有生成的UI


        #region State
        public bool IsUIOpen { private set; get; } //主UI是否是打开状态
        public bool IsBaseUIInitialed { private set; get; } //是否完成了初始化

        public Action LaugangeChanEvent;  //多语言切换事件
        public PanelInfor m_PreviousSelectView { get; private set; } //之前选择的UI
        private static int ForbiddenUICount = 0;  //调用禁用UI次数
        private bool m_IsNeedFollowCamera = true; //标示是否需要跟随相机 在场景切换时候不需要跟随


        private Transform m_FixedDirUIRoot = null;
        private Transform FixedDirUIRoot
        {
            get
            {
                if (m_FixedDirUIRoot == null)
                {
                    if (m_AllUIRootBaseOnLevelDic.ContainsKey(UIPanelLevel.FixedDirectionUI))
                        m_FixedDirUIRoot = m_AllUIRootBaseOnLevelDic[UIPanelLevel.FixedDirectionUI];
                }
                return m_FixedDirUIRoot;
            }
            set
            {
                m_FixedDirUIRoot = value;
            }
        }//主界面UI根节点
        #endregion



        #region Initial
        protected override void InitialSingleton()
        {
            IsBaseUIInitialed = false;
            //     GlobalEntity.GetInstance().AddListener(FrameWorkEvent.QuickMoveOk, ResetUIView); //监听传送重置
            //   GlobalEntity.GetInstance().AddListener(SceneStateController.mEvent.loadingOverNotify, OnSceneLoadOver);
            //   GlobalEntity.GetInstance().AddListener<string>(SceneStateController.mEvent.loadingSwitchNotify, OnSceneSwitchNotify);
            //      GlobalEntity.GetInstance().AddListener(BeanVRUIEvent.switchAccount, ClearLocalData);
        }
        #endregion

        #region 重置UI
        public void ResetUIView()
        {
            if (FixedDirUIRoot != null)
                FixedDirUIRoot.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
        }
        #endregion

        #region UI跟随
        Vector3 recordPosition;
        void FollowCamera()
        {
            if (m_IsNeedFollowCamera == false)
            {
                //#if UNITY_EDITOR
                //                Debug.Log("FollowCamera>>>>>>>>>>>>>>>>>>>>> ");
                //#endif
                return; //此时在场景切换不需要跟随
            }

            float distance = Vector3.Distance(recordPosition, Camera.main.transform.position);
            if (distance <= 0.5f) return;
            //     Debug.Log("distance " + distance);
            if (FixedDirUIRoot != null)
                FixedDirUIRoot.DOMove(Camera.main.transform.position, 0.1f);
            recordPosition = Camera.main.transform.position;
        }

        void FollowCamera_Immediately()
        {
            recordPosition = FixedDirUIRoot.position = Camera.main.transform.position;
        }

        #endregion


        #region UI System Initial
        public void OnApplicationInitialed(Transform ui)
        {
          Debug.Log("OnApplicationInitialed ... ");
       //     GlobalEntity.GetInstance().RemoveListener<Transform>(EventDef.gEvent.gHardInitOK, OnApplicationInitialed);
            EventCenter.GetInstance().DelayDoEnumerator(3F, () =>
            {
                FixedDirUIRoot = ui;

#if UNITY_ANDROID && !UNITY_EDITOR
            EventCenter.GetInstance().AddUpdateEvent(FollowCamera, EventCenter.UpdateRate.DelayOneFrame);
            EventCenter.GetInstance().DelayDoEnumerator(10f, () =>
            {
                EventCenter.GetInstance().RemoveUpdateEvent(FollowCamera, EventCenter.UpdateRate.DelayOneFrame);
            });  //手机端由于不会有定位，因此只需要进行一端事件完成初始化 跟随即可
#else
                EventCenter.GetInstance().AddUpdateEvent(FollowCamera, EventCenter.UpdateRate.NormalFrame);   //**FollowCamera
#endif

                CreateUIRootOnLevel(); //创建层级根节点
                LoadUIPanelConfg();
            });

        }
        void CreateUIRootOnLevel()
        {
            #region  FixedDirectionUI UIRoot
            FixedDirUIRoot.gameObject.name = "FixedDirectionUI";
            m_AllUIRootBaseOnLevelDic.Add(UIPanelLevel.FixedDirectionUI, FixedDirUIRoot);
            #endregion

            #region  FollowTargetUI UIRoot
            GameObject FollowTargetUIObj = GenerationObjUtility.CreateObjectByName("FollowTargetUI",null,true);
            GameObject.DontDestroyOnLoad(FollowTargetUIObj);
            m_AllUIRootBaseOnLevelDic.Add(UIPanelLevel.FollowTargetUI, FollowTargetUIObj.transform);
            #endregion

            #region  SceneUI UIRoot
            GameObject SceneUIObj = GenerationObjUtility.CreateObjectByName("SceneUI", null, true);// CommonOperate.CreateObjWhithName("SceneUI");
            GameObject.DontDestroyOnLoad(SceneUIObj);
            SceneUIObj.AddComponent<UIManager_SceneUILevel>();
            m_AllUIRootBaseOnLevelDic.Add(UIPanelLevel.SceneUI, SceneUIObj.transform);
            #endregion

        }

        /// <summary>
        /// Load UI Panel Configure
        /// </summary>
        void LoadUIPanelConfg()
        {
          Debug.Log("LoadUIPanelConfg......");
            TextAsset asset = Resources.Load<TextAsset>(ConstDefine.ConfigurePath + ConstDefine.UIPanelConfigure);              //("Confg/UIPanelConfg");
            List<UIPanelConfgInfor> allConfigure = JsonParser.Deserialize<List<UIPanelConfgInfor>>(asset.text);
            foreach (var item in allConfigure)
            {
                if (m_AllUIPanelConf.ContainsKey(item.PanelName))
                    Debug.LogError("LoadUIPanelConfg Fail...Repead Configure; " + item.PanelName);
                else
                    m_AllUIPanelConf.Add(item.PanelName, item);
            }
        }

        public void InitialVrKeyBoard()
        {
            if (IsBaseUIInitialed) return;
            IsBaseUIInitialed = true;

            GameObject keyPrefab = Resources.Load<GameObject>(ConstDefine.UIPanelResourcePath + "Keyboard");
            //     Manager.ins.ResourceMgr.LoadAsset(ConstDefine.UIPanelResourcePath + "Keyboard", (UnityEngine.Object obj) =>
            //  {
            if (keyPrefab == null)
            {
                Debug.LogError("keyboard iS Null");
                return;
            }//if
            Debug.Log("keyboard iS Initialed");
            GameObject keyboard = GameObject.Instantiate(keyPrefab) as GameObject;
            keyboard.name = "Keyboard";
            keyboard.transform.SetParent(m_AllUIRootBaseOnLevelDic[UIPanelLevel.FollowTargetUI], true);
            KeyBoard.Instance.InitialVrKeyBoard();
            //   });
        }


        #endregion

        #region 获取配置信息
        public UIPanelConfgInfor GetUIPanelConfigureByName(string _name)
        {
            if (m_AllUIPanelConf.ContainsKey(_name))
                return m_AllUIPanelConf[_name];

            return null;
        }
        #endregion

        #region 创建和获取UI面板
        public PanelInfor GetUIPanelByType<T>(string panelName, bool createWhenNotExit) where T : ViewBaseFU_EX
        {
            if (string.IsNullOrEmpty(panelName))
            {
              Debug.Log("GetUIPanelByType Fail , " + panelName);
                return null;
            }
            return LoadUIPanelByName(panelName, createWhenNotExit);
        }

        /// <summary>
        /// 获得控制脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panelName"></param>
        /// <param name="createWhenNotExit"></param>
        /// <returns></returns>
        public T GetUIPanelControlByType<T>(string panelName, bool createWhenNotExit) where T : ViewBaseFU_EX
        {
            if (string.IsNullOrEmpty(panelName))
            {
              Debug.Log("GetUIPanelByType Fail , " + panelName);
                return null;
            }
            PanelInfor infor = LoadUIPanelByName(panelName, createWhenNotExit);
            if (infor == null)
            {
                Debug.LogError("GetUIPanelByType Fail. Not Exit  " + typeof(T));
                return null;
            }
            return infor.m_ViewControlScript as T;
        }

        /// <summary>
        ///Get UI Panel By Name
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        PanelInfor LoadUIPanelByName(string panelName, bool createWhenNotExit)
        {
            if (string.IsNullOrEmpty(panelName))
            {
              Debug.Log("LoadUIPanelByName Fail , " + panelName);
                return null;
            }

            UIPanelConfgInfor _confg = null;
            if (m_AllUIPanelConf.TryGetValue(panelName, out _confg) == false)
            {
                Debug.LogError("Loading Panel Not Exit" + panelName);
                return null;
            }
            foreach (var item in m_AllInitialedPanel.Values)
            {
                if (item != null && item.m_Panel != null && item.m_Panel.name == panelName)
                    return item;
            }

          Debug.Log("LoadUIPanelByName ..This Panel Not Initial Yet.. " + panelName);
            PanelInfor infor = CreatePanelByConfg(_confg, createWhenNotExit);
            return infor;
        }

        PanelInfor CreatePanelByConfg(UIPanelConfgInfor confg, bool createWhenNotExit)
        {
            if (confg == null)
            {
                Debug.LogError("SetPanelAtRightPanel Fail, Parameter is null");
                return null;
            }
          Debug.Log("CreatePanelByConfg ....  " + confg.PanelName);

            #region Create Object BaseOn  confg

         //   GameObject uiAsset = null;
            GameObject obj = Resources.Load<GameObject>(ConstDefine.UIPanelResourcePath + confg.PanelPath);
         //   Manager.ins.ResourceMgr.LoadAsset(ConstDefine.UIPanelResourcePath + confg.PanelPath, (obj =>
        //    {
        //        uiAsset = obj as GameObject;
       //     }));
            //if (uiAsset == null)
            //{
            //    Debug.LogError("SetPanelAtRightPanel l Fail >>Panel Not Exit " + confg.PanelPath);
            //    return null;
            //}

            GameObject _panelObj = GameObject.Instantiate(obj);
            _panelObj.name = confg.PanelName;

            #endregion

            #region GetComponent Of This Obj ,Make Sure Inherit From ViewBaseFU_EX,Then Set Local psotion And Rotation
            Transform root = m_AllUIRootBaseOnLevelDic[confg.m_UIPanelLevel];
            if (root == null)
            {
                Debug.LogError("SetPanelAtRightPanel Fail,Root is Null");
                return null;
            }
            if (confg.m_UIPanelLevel != UIPanelLevel.FixedDirectionUI || confg.IsParentRoot)
            {
                _panelObj.transform.SetParent(root);
                ///,Log4Helper.Info(">>>>>>> Set to Root");
            } //主UI 和 根节点UI设置
            else
            {
                PanelInfor _infor = LoadUIPanelByName("UIMainCanvasView", true);
                if (_infor == null)
                {
                    Debug.LogError("CreatePanelByConfg Fail....  UIMainCanvasView Not Exit");
                    _panelObj.transform.SetParent(root);
                }
                else
                {
                    _panelObj.transform.SetParent(_infor.m_Panel.transform);
                    _panelObj.transform.localScale = Vector3.one; //主要的UI

                    if (_panelObj.name != "UIMainView")
                        _panelObj.transform.SetAsFirstSibling();

                  Debug.Log(" CreatePanelByConfg. MainMenu.." + _panelObj.gameObject.name);
                }
            } //根据UI类型设置不同的父节点


            ViewBaseFU_EX _view = _panelObj.transform.GetComponent<ViewBaseFU_EX>();
            if (_view == null)
            {
                Debug.LogError("CreatePanelByConfg Fail..Miss ViewBaseFU_EX :" + _panelObj.name);
                return null;
            }
            //Set Panel At Confg Position And Rotation
            _panelObj.transform.localPosition = _view.m_InitialPosition;
            _panelObj.transform.localEulerAngles = _view.m_InitialAngle;

            #endregion

            #region Check And Record This Panel At m_AllInitialedPanel

            Type type = _view.GetType();
            PanelInfor infor = new PanelInfor(_panelObj, _view);
            if (m_AllInitialedPanel.ContainsKey(type) == false)
            {
                m_AllInitialedPanel.Add(type, infor);  //Record
            }
            else if (m_AllInitialedPanel[type] == null || m_AllInitialedPanel[type].m_Panel == null)
            {
              Debug.Log("CreatePanelByConfg , Object Destroyed ,Record Now " + confg.PanelName);
                m_AllInitialedPanel.Remove(type);
                m_AllInitialedPanel.Add(type, infor);  //Record
            }
            else
            {
                m_AllInitialedPanel[type] = infor;  //Record
            }

            #endregion

            _panelObj.gameObject.SetActive(false);
            return infor;
        }

        #endregion

        #region 销毁和去除标记 /手动标记动态创建的对象
        //public void DestroyUIByType(Type type)
        //{
        //    if (type == null) return;
        //    if (m_AllInitialedPanel.ContainsKey(type))
        //        m_AllInitialedPanel.Remove(type);
        //    //else
        //    //    Debug.LogError("DestroyUIByType Fail ,Not Record Or  Exit " + type);
        //}

        //public void RecordUIPanel(Type type,PanelInfor infor)
        //{
        //}
        #endregion

        #region 获取根节点
        public Transform GetUIRootTrans(UIPanelLevel level)
        {
            return m_AllUIRootBaseOnLevelDic[level].transform;
        }
        #endregion


        #region OpenUI

        public void OpenUI<T>(bool isClosePrevious, string panelName, bool createWhenNotExit, bool needRecordAsCur) where T : ViewBaseFU_EX
        {
            PanelInfor infor = GetUIPanelByType<T>(panelName, createWhenNotExit);
            if (infor == null)
            {
                Debug.LogError("OpenUI Fail ...  Cant Get Panel Of " + typeof(T));
                return;
            }

          Debug.Log("Type " + typeof(T) + "  Name=" + typeof(T).Name);


            //if (typeof(T) == typeof(UIMainView))
            //{
            //    EventCenter.GetInstance().ResetUI();
            //    infor.m_ViewControlScript.Open();
            //    m_PreviousSelectView = null;
            //    IsUIOpen = true;
            //    return;
            //}


            infor.m_ViewControlScript.Open();
            if (isClosePrevious)
            {
                if (m_PreviousSelectView != null && m_PreviousSelectView.m_Panel != null && m_PreviousSelectView.m_Panel.name != infor.m_Panel.name)
                {
                    m_PreviousSelectView.m_ViewControlScript.Close();  //close Previous
                  Debug.Log("OpenUI.....Close Previous View " + m_PreviousSelectView.m_Panel.name);
                }
            }

            if (needRecordAsCur)
                m_PreviousSelectView = infor; //Update Mark 
            return;
        }

        #endregion

        #region  CloseUI

        public void CloseUI<T>(bool isCloseDirect, bool isRecorded) where T : ViewBaseFU_EX
        {

            PanelInfor infor = GetUIPanelByType<T>(typeof(T).Name, false); //不需要创建
            if (infor == null)
            {
                Debug.LogError("CloseUI Fail...Cant Get Panel " + typeof(T));
                return;
            }

            //if (typeof(T) == typeof(UIMainView))
            //{
            //    infor.m_ViewControlScript.Close();   //�ر���UI
            //    IsUIOpen = false;
            //    m_PreviousSelectView = null;
            //    return;
            //}// 主ui关闭 则关闭所有UI

            infor.m_ViewControlScript.Close(isCloseDirect);
          Debug.Log("Close Current UIPanel:: " + infor.m_Panel.name);

            if (isRecorded)
                m_PreviousSelectView = null;
        }

        public void HideUITemporality<T>(bool isRecover) where T : ViewBaseFU_EX
        {
            //PanelInfor infor = GetUIPanelByType<T>(typeof(T).Name, false);
            //if (infor == null)
            //{
            //    Debug.LogError("HideUITemporality Fail.. Cant Get Panel of " + typeof(T));
            //    return;
            //}

            //IUITemporaOperate _uiTempOp = infor.m_Panel.GetComponent<IUITemporaOperate>();
            //if (_uiTempOp == null)
            //{
            //    Debug.LogError("Cant Get Panel function  IUIOperate of  " + typeof(T));
            //    return;
            //}

            //if (isRecover == false)
            //{
            //    if (typeof(T) == typeof(UIMainView))
            //        _uiTempOp.UITemporality(false);
            //    return;
            //} //Hide

            //ResetUIView();
            //if (typeof(T) == typeof(UIMainView))
            //{
            //    if (infor.m_Panel.gameObject.activeSelf == false)
            //    {
            //        infor.m_Panel.gameObject.SetActive(true);
            //        _uiTempOp.UITemporality(true);
            //    }
            //} //Show
        }
        #endregion


        #region 注册UIcanvas 和禁用UI
        //Get All UICanvas ,when Forbiddle UI this Will Be Use
        public void RegistCanvas(UICanvasController canvas, bool isRegist)
        {
            if (isRegist)
            {
                if (m_AllCanvas.Contains(canvas)) return; //   Debug.Log("�ظ�ע��" + canvas);
                m_AllCanvas.Add(canvas);
                return;
            }//if

            if (m_AllCanvas.Contains(canvas))
                m_AllCanvas.Remove(canvas);
        }


        public void ForbiddenUI(bool isForbidden)
        {
          Debug.Log("ForbiddenUI  " + isForbidden + "::" + ForbiddenUICount);
            if (ForbiddenUICount <= 0) ForbiddenUICount = 0;
            if (isForbidden)
            {
              Debug.Log("ForbiddenUI ... Forbidden Record Count:" + ForbiddenUICount);
                ++ForbiddenUICount;
                foreach (var item in m_AllCanvas)
                    item.m_CanOperate = false;

                return;
            }// ����ui
            --ForbiddenUICount;
            if (ForbiddenUICount == 0)
            {
              Debug.Log("ForbiddenUI ... Recover");
                foreach (var item in m_AllCanvas)
                    item.m_CanOperate = true;

            }//�ָ�UI
        }

        #endregion

        #region 多语言切换

        //public void ChangeLanguageEv(LauguageType type)
        //{
        //    LauguageTool.Ins.ChangeLaugage(type);
        //    if (LaugangeChanEvent != null)
        //        LaugangeChanEvent();
        //}

        #endregion


        #region  场景切换、切换账号

        void OnSceneSwitchNotify(string sceneName)
        {
            FollowCamera_Immediately();
            m_IsNeedFollowCamera = false;
        }
        void OnSceneLoadOver()
        {
            FollowCamera_Immediately();
            m_IsNeedFollowCamera = true;
          Debug.Log("OnSceneLoadOver OnSceneLoadOver UIManagerModel");
            //   RoleManager.Instance.ReSetCameraPose();  //����������
            ResetUIView();
            //EventCenter.GetInstance().DelayDoEnumerator(ConstDefine.UICloseTweenTime, () =>
            //{
            //    ForbiddenUI(false); //恢复UI
            //    if (SceneFucDynamicManager.GetInstance().CheckSceneFuction(RoomModel.CurrentRoomInFormation.RoomType.ToString(), "UIMain"))
            //    {
            //        OpenUI<UIMainView>(false, "UIMainView", true, false);
            //    }
            //});
        }

        void ClearLocalData()
        {
            m_PreviousSelectView = null;
            ForbiddenUICount = 0;
        }
        #endregion




    }
}
