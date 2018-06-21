using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MFramework
{
    public class ViewBaseFU_EX : UIViewBase
    {
        protected HashSet<int> m_RegistUIEffect = new HashSet<int>(); //存储注册事件防止重复注册点击事件

        public Vector3 m_InitialPosition = new Vector3(0, 0, 1);
        public Vector3 m_InitialAngle = Vector3.zero;

        [System.NonSerialized]
        public bool IsPanelInteractive = true; //标示是否可以交互

        public bool m_IsOpen { private set; get; } //标示当前对象是Open 还是Close 状态 避免重复调用

    //    protected UIPanelConfgInfor m_PanelConfg = null;


        //*****使用规则****************
        //1重写 InitialUIView 和 SetTextViewOfLauguage ====>Awake
        //2 重写  DefaultView                       =====>主动在 Open 调用
        //3 重写 ShowUIEffect ==> Open 和 Close 主动调用,
        //4 重写Open 和Close ，使用m_IsOpen 标示 避免Open 和Close 被无意义的调用


        #region MonoFrame
        protected override void Awake()
        {
            base.Awake();
            m_IsOpen = false;
            InitialUIView();
        }


        protected override void OnDisable()
        {
            StopAllCoroutines();
            base.OnDisable();
        }
        protected override void Dispose()
        {
            m_RegistUIEffect.Clear();  //Clear Data
            base.Dispose();
        }
        #endregion

        #region   UIViewBase 
        protected override void RegistListener()
        {
  //          GlobalEntity.GetInstance().AddListener<string>(SceneStateController.mEvent.loadingSwitchNotify, OnSceneChangeNotify);
            base.RegistListener();
        }

        protected override void UnRegistListener()
        {
     //       GlobalEntity.GetInstance().RemoveListener<string>(SceneStateController.mEvent.loadingSwitchNotify, OnSceneChangeNotify);
            base.UnRegistListener();
        }


        protected void OnSceneChangeNotify(string StrName)
        {
            if (gameObject.activeSelf)
                Close(true);
        }
        #endregion

        #region My UI Frame

        #region View Seetting
        //the view default show Show Be Call Every Time When Open
        protected virtual void DefaultView() { }
        //初始化视图 
        protected virtual void InitialUIView() { }
        //Show Text Of Selcet Laugange
        protected virtual void SetTextViewOfLauguage() { }
        #endregion

        #region Open
        public virtual void Open()
        {
            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);

            SetTextViewOfLauguage();
       //     UIManagerModel.GetInstance().LaugangeChanEvent += SetTextViewOfLauguage;

            IsPanelInteractive = true;
            m_IsOpen = true;
        }
        /// <summary>
        /// 打开视图时候不使用协程 以节省性能
        /// </summary>
        protected virtual void OpenWithOutIEnumerator() { }

        /// <summary>
        /// 当视图包含UIeffect时候执行效果动画 否则执行OpenWithOutIEnumerator
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator OpenEffect() { yield return null; }
        protected virtual IEnumerator OpenEffect(System.Action action) { yield return null; }

        #endregion

        #region Close


        public virtual void Close(bool isCloseDirect = false)
        {
            IsPanelInteractive = false;
            m_IsOpen = false;
            //UIManagerModel.GetInstance().LaugangeChanEvent -= SetTextViewOfLauguage;
        }

        /// <summary>
        /// 关闭视图使用协程
        /// </summary>
        protected virtual void CloseWithOutIEnumerator() { }

        protected virtual IEnumerator CloseEffect(bool isCloseDirect)
        {
            if (isCloseDirect == false)
                yield return null;
        }

        protected virtual IEnumerator CloseEffect(System.Action action) { yield return null; }
        #endregion



        protected virtual void OnViewButtonClick(GameObject paramater) { }
        #endregion


        #region UIEffect Event

        //**********PointUpEvent 
        /// <summary>
        /// Addlistener to uieffect PointerUpEvent event
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="handle"></param>
        protected void AddUIEffectListener(UIEffect effect, PointerUpDelegate handle)
        {
            if (effect == null)
                return;

            if (m_RegistUIEffect.Contains(effect.GetInstanceID()))
                return;

            m_RegistUIEffect.Add(effect.GetInstanceID());
            effect.PointerUpEvent += handle;  //注册点击的抬起事件
        }

        /// <summary>
        /// remove listenner of  PointerUpEvent
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="handle"></param>
        protected void RemoveUIEffectListener(UIEffect effect, PointerUpDelegate handle)
        {
            if (effect == null)
                return;

            if (m_RegistUIEffect.Contains(effect.GetInstanceID()) == false)
                return;

            m_RegistUIEffect.Remove(effect.GetInstanceID());
            effect.PointerUpEvent -= handle;  //注销点击的抬起事件
        }

        #endregion



    }
}
