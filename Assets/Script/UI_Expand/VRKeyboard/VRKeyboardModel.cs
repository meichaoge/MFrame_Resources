using UnityEngine;
using System.Collections;
using MFramework;
using System;

namespace BeanVR
{

    public class VRKeyboardModel : Singleton_Static<VRKeyboardModel>
    {
        //   private static VRKeyboardModel instance;
        //   public static VRKeyboardModel GetInstance()
        //   {
        //       if (instance == null)
        //           instance = new VRKeyboardModel();
        //       return instance;
        //   }
        //   private VRKeyboardModel()
        //   {
        //       ///虚拟键盘输入
        //       GlobalEntity.GetInstance().AddListener<bool>(KeyBoard.VRKeyboardEvent.KeyBoardStateNotify, OnKeyBordWork);  //开始或结束输入
        ////       GlobalEntity.GetInstance().AddListener<string>(SceneStateController.mEvent.loadingSwitchNotify, OnSceneChange); //Listen SceneChange
        //   }


        protected override void InitialSingleton()
        {
            ///虚拟键盘输入
            GlobalEntity.GetInstance().AddListener<bool>(KeyBoard.VRKeyboardEvent.KeyBoardStateNotify, OnKeyBordWork);  //开始或结束输入
            //       GlobalEntity.GetInstance().AddListener<string>(SceneStateController.mEvent.loadingSwitchNotify, OnSceneChange); //Listen SceneChange
        }

        /// <summary>
        /// VRInput模块调用虚拟键盘
        /// </summary>
        /// <param name="isStart"></param>
        void OnKeyBordWork(bool isStart)
        {
            //            UIManagerModel.GetInstance().ForbiddenUI(isStart);   //开始输入时禁用UI层
        }

        /// <summary>
        /// 由于键盘会临时关闭UI，所有需要在切换时恢复
        /// </summary>
        /// <param name="_newSecneName"></param>
        void OnSceneChange(string _newSecneName)
        {
            if (KeyBoard.Instance == null) return;
            if (KeyBoard.Instance.gameObject.activeSelf)
                KeyBoard.Instance.gameObject.SetActive(false);  //关闭键盘
        }


    }
}
