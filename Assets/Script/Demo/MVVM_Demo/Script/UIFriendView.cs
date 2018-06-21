using MFramework.UI.Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    public class UIFriendView : VrViewBaseEx<FriendCfg>
    {
        private LayoutScrollView m_LayoutScrollViewScript;

        #region  Frame
        protected override void InitialUIView()
        {
            if (m_LayoutScrollViewScript == null)
                m_LayoutScrollViewScript = GetComponent<LayoutScrollView>();

            if (m_LayoutScrollViewScript == null)
            {
                Debug.LogError("InitialUIView Fail,Miss Component LayoutScrollView");
                return;
            }
            m_LayoutScrollViewScript.OnLayoutInitial_AfterItemUIButtonCreateAct += OnFriendPageItemInitial;  //注册列表项点击事件
            m_LayoutScrollViewScript.InitialLayout(); //初始化布局列表项

        }
        //设置语言
        protected override void SetTextViewOfLauguage()
        {
            base.SetTextViewOfLauguage();
            //    HeadText.text = Manager.Luaguage.GetText("100010");  //好友
        }

        public override void Open()
        {
            if (m_IsOpen) return;
            bindContex = UIFriendDataModel.GetInstance();
            base.Open();
            OpenWithOutIEnumerator();
        }
        protected override void OpenWithOutIEnumerator()
        {
            //base.OpenWithOutIEnumerator();
            m_LayoutScrollViewScript.ReBuildView(bindContex.CurViewBindDataBase.Count, true, true);
            //     FireMsg(UIFriendController.UIFriendEvent.GetAllFriendRoomStateReq);  //获取所有好友的详细房间信息
        }
        protected override IEnumerator OpenEffect()
        {
            //  if (viewPanelUIEffect != null)
            {
                //   viewPanelUIEffect.Show(0f);   //最外层的面板特效
                yield return new WaitForSeconds(ConstDefine.UIOpenTweenTime);
            }
            OpenWithOutIEnumerator();
        }

        public override void Close(bool isCloseDirect = false)
        {
            if (m_IsOpen == false) return;
            bindContex = null;
            base.Close(isCloseDirect);
        }
        protected override void CloseWithOutIEnumerator()
        {
            //base.CloseWithOutIEnumerator();
            gameObject.SetActive(false);
        }
        protected override IEnumerator CloseEffect(bool isCloseDirect)
        {
            // if (viewPanelUIEffect != null && isCloseDirect == false)
            {
                //     viewPanelUIEffect.Hide(0f);
                yield return new WaitForSeconds(ConstDefine.UICloseTweenTime);
            }  //最外层的面板特效
            CloseWithOutIEnumerator();
        }
        protected override void Dispose()
        {
            if (m_LayoutScrollViewScript != null)
            {
                m_LayoutScrollViewScript.OnLayoutInitial_AfterItemUIButtonCreateAct -= OnFriendPageItemInitial;  //注册列表项点击事件
            }
            base.Dispose();
        }
        protected override void OnViewButtonClick(GameObject paramater)
        {
            if (IsPanelInteractive == false) return;
            switch (paramater.name)
            {
                case "CloseButton":
                    //          UIManagerModel.GetInstance().CloseUI<UIFriendView>(false, true);
                    break;
            }
        }

        #endregion
        //布局类初始化
        void OnFriendPageItemInitial(BaseLayoutButton itemBtn)
        {
            UIFriendItemBtn friendItemBtn = itemBtn as UIFriendItemBtn;
            if (friendItemBtn == null)
            {
                Debug.LogError("OnFriendPageItemInitial Fail,Not Right Type " + itemBtn.GetType());
                return;
            }
            //***注册按钮的回调事件
            //friendItemBtn.m_Delete.PointerUpEvent += OnViewPageItemClick;
            //friendItemBtn.m_Invite.PointerUpEvent += OnViewPageItemClick;
            //friendItemBtn.m_Transfer.PointerUpEvent += OnViewPageItemClick;

        }

        void OnViewPageItemClick(GameObject parameter)
        {
            if (IsPanelInteractive == false) return;
            UIFriendItemBtn button = parameter.transform.parent.GetComponent<UIFriendItemBtn>();
            if (button == null) return;
            switch (parameter.name)
            {
                case "Invite":

                    break;
                case "Transfer":

                    break;
                case "Delete":

                    break;
            }
        }

        #region MVVM
        protected override void OnBindingContextChanged(IDataModel<FriendCfg> oldModel, IDataModel<FriendCfg> newModel)
        {
            if (oldModel != null)
            {
                oldModel.CurViewBindDataBase.OnValueChangedEvent -= OnDataBaseChange_Signal;
                oldModel.CurViewBindDataBase.OnValueChangedEvent2 -= OnDataBaseChange_Multiple;
            }

            if (newModel != null)
            {
                //Debug.Log("注册事件");
                newModel.CurViewBindDataBase.OnValueChangedEvent -= OnDataBaseChange_Signal;
                newModel.CurViewBindDataBase.OnValueChangedEvent2 -= OnDataBaseChange_Multiple;

                newModel.CurViewBindDataBase.OnValueChangedEvent += OnDataBaseChange_Signal;
                newModel.CurViewBindDataBase.OnValueChangedEvent2 += OnDataBaseChange_Multiple;
            }
        }
        void OnDataBaseChange_Signal(FriendCfg oldData, FriendCfg newData, int _dex, CollectionEvent action)
        {
            switch (action)
            {
                case CollectionEvent.AddSignal:
                    m_LayoutScrollViewScript.DataModelChangeSignal(action, newData, _dex, oldData, bindContex.CurViewBindDataBase);
                    break;
                case CollectionEvent.DeleteSignal:
                    m_LayoutScrollViewScript.DataModelChangeSignal(action, newData, _dex, oldData, bindContex.CurViewBindDataBase);
                    break;
                case CollectionEvent.Update:
                    m_LayoutScrollViewScript.DataModelChangeSignal(action, newData, _dex, oldData, bindContex.CurViewBindDataBase);
                    break;
                default:
                    Debug.LogError("Miss  EnumType " + action);
                    break;
            }
        }
        void OnDataBaseChange_Multiple(IList<FriendCfg> oldData, IList<FriendCfg> newData, CollectionEvent action)
        {
            switch (action)
            {
                case CollectionEvent.AddRang:
                    m_LayoutScrollViewScript.DataModelChange(action, newData, 0, bindContex.CurViewBindDataBase);
                    break;
                case CollectionEvent.DeleteRangle:
                    m_LayoutScrollViewScript.DataModelChange(action, newData, 0, bindContex.CurViewBindDataBase);
                    break;
                case CollectionEvent.Clear:
                    m_LayoutScrollViewScript.DataModelChange(action, newData, 0, bindContex.CurViewBindDataBase);
                    break;
                case CollectionEvent.Flush:
                    Debug.Log("Flush View");
                    m_LayoutScrollViewScript.DataModelChange(action, newData, 0, bindContex.CurViewBindDataBase);
                    break;
                default:
                    Debug.LogError("Miss  EnumType " + action);
                    break;
            }
        }


        #endregion

    }
}