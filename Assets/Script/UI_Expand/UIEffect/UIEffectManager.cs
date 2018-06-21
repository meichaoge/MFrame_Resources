using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BeanVR
{
    public class UIEffectManager
    {

        private List<UIEffect> allSelectedUIEffect = new List<UIEffect>(); //选中的UIEffect
        private List<UIEffect> allSpecialSelectUIEff = new List<UIEffect>();//特殊的选中UIEffect 不会被其他的UIEffect关闭 例如UIMicrophpne

        private static UIEffectManager instance;
        public static UIEffectManager GetInstance()
        {
            if (instance == null)
                instance = new UIEffectManager();
            return instance;
        }
        private UIEffectManager()
        {
          //  GlobalEntity.GetInstance().AddListener(BeanVRUIEvent.switchAccount, ClearLocalData);   //监听切换账号
        //    GlobalEntity.GetInstance().AddListener<string>(SceneStateController.mEvent.loadingSwitchNotify, OnSceneChange); //Listen SceneChange

        }



        #region 操作选中项


        /// <summary>
        /// 保存选中的UIEffect
        /// </summary>
        /// <param name="_operateUIEffect">Operate user interface effect.</param>
        /// <param name="_UnSelectPrevious">是否清除上一次操作的项的选中效果</param>
        public void StoreSelectState(UIEffect _operateUIEffect, bool _UnSelectPrevious)
        {
            if (_operateUIEffect == null) return;
            if (_operateUIEffect.isSelected)
            {
                //Debug.Log ("已经选中效果 " + _operateUIEffect.gameObject.name);
            }
            else
            {
                _operateUIEffect.isEffectEnable = true;
                _operateUIEffect.SetSelected();
            }
            if (allSelectedUIEffect.Contains(_operateUIEffect) == false)
            {
                allSelectedUIEffect.Add(_operateUIEffect);
            }
            else
            {
                allSelectedUIEffect.Remove(_operateUIEffect);
                allSelectedUIEffect.Add(_operateUIEffect); //保证当前操作的UIEffect在最后一个
            }

            if (_UnSelectPrevious)
            {
                if (allSelectedUIEffect.Count > 1)
                {
                    UIEffect previousEffect = allSelectedUIEffect[allSelectedUIEffect.Count - 2];
                    if (previousEffect != null)
                    {
                        _operateUIEffect.isEffectEnable = true;
                        previousEffect.UnSelected(); //取消之前操作项的选中
                        allSelectedUIEffect.Remove(previousEffect);
                    }//if
                }//if
            }//if

        }
        /// <summary>
        /// 状态自动切换的UIEffect
        /// </summary>
        /// <param name="_operateUIEffect">Operate user interface effect.</param>
        public void StateAutoFlip(UIEffect _operateUIEffect)
        {
            if (_operateUIEffect.isSelected)
            {
                _operateUIEffect.isEffectEnable = true;
                _operateUIEffect.UnSelected();
            }
            else
            {
                _operateUIEffect.isEffectEnable = true;
                _operateUIEffect.SetSelected();
            }

            if (allSelectedUIEffect.Contains(_operateUIEffect))
            {
                if (_operateUIEffect.isSelected == false)
                    allSelectedUIEffect.Remove(_operateUIEffect);
            }
            else
            {
                if (_operateUIEffect.isSelected)
                    allSelectedUIEffect.Add(_operateUIEffect);
            }//else
        }


        /// <summary>
        /// 保存特殊的UIEffect 其选中效果只收到自身控制 以及场景切换控制
        /// </summary>
        /// <param name="_operateUIEffect">Operate user interface effect.</param>
        /// <param name="_autoFlipState">是否点击自动反转状态</param>
        public void StoreSpecialSelect(UIEffect _operateUIEffect, bool _autoFlipState)
        {
            if (_operateUIEffect.isSelected)
            {
                if (_autoFlipState)
                {
                    _operateUIEffect.isEffectEnable = true;
                    _operateUIEffect.UnSelected(); //反转状态
                                                   // _operateUIEffect.isEffectEnable = false;
                }
                else
                {
                    Debug.Log("已经选中状态");
                }//else
            }//if
            else
            {
                if (_autoFlipState)
                {
                    _operateUIEffect.isEffectEnable = true;
                    _operateUIEffect.SetSelected(); //反转状态
                }
            }//else

            if (allSpecialSelectUIEff.Contains(_operateUIEffect))
            {
                if (_operateUIEffect.isSelected == false)
                    allSpecialSelectUIEff.Remove(_operateUIEffect); //移除已经非选中的UIEffect
            }
            else
            {
                if (_operateUIEffect.isSelected)
                    allSpecialSelectUIEff.Add(_operateUIEffect); //保存已经非选中的UIEffect
            }
        }

        /// <summary>
        /// 强制选中特殊的UIEffect
        /// </summary>
        /// <param name="_operateUIEffect">Operate user interface effect.</param>
        public void ForceSelectSpecial(UIEffect _operateUIEffect)
        {
            if (_operateUIEffect == null)
                return;

            if (_operateUIEffect.isSelected == false)
            {
                _operateUIEffect.isEffectEnable = true;
                _operateUIEffect.SetSelected();
            }

            if (allSpecialSelectUIEff.Contains(_operateUIEffect) == false)
                allSpecialSelectUIEff.Add(_operateUIEffect);
        }

        /// <summary>
        /// 强制反选特殊UIEffect
        /// </summary>
        /// <param name="_operateUIEffect">Operate user interface effect.</param>
        public void ForceUnSelectSpecial(UIEffect _operateUIEffect)
        {
            if (_operateUIEffect == null)
                return;

            if (_operateUIEffect.isSelected)
            {
                _operateUIEffect.isEffectEnable = true;
                _operateUIEffect.UnSelected();
                //   _operateUIEffect.isEffectEnable = false;
            }

            if (allSpecialSelectUIEff.Contains(_operateUIEffect))
                allSpecialSelectUIEff.Remove(_operateUIEffect);
        }

        /// <summary>
        /// 自动反转状态
        /// </summary>
        /// <param name="_operateUIEffect">Operate user interface effect.</param>
        public void AutoFlipSpecialUIEffect(UIEffect _operateUIEffect)
        {
            if (_operateUIEffect == null) return;

            if (_operateUIEffect.isSelected)
                ForceUnSelectSpecial(_operateUIEffect);
            else
                ForceSelectSpecial(_operateUIEffect);
        }


        void ClearAllSeclet()
        {
            for (int _index = 0; _index < allSelectedUIEffect.Count; ++_index)
            {
                if (allSelectedUIEffect[_index] != null)
                {
                    allSelectedUIEffect[_index].isEffectEnable = true;
                    allSelectedUIEffect[_index].UnSelected();
                }
            }
            allSelectedUIEffect.Clear();
            for (int _index = 0; _index < allSpecialSelectUIEff.Count; ++_index)
            { //特殊的
                if (allSpecialSelectUIEff[_index] != null)
                {
                    allSpecialSelectUIEff[_index].isEffectEnable = true;
                    allSpecialSelectUIEff[_index].UnSelected();
                }
            }
            allSpecialSelectUIEff.Clear();
        }


        #endregion

        void OnSceneChange(string sceneName)
        {
            ClearAllSeclet();  //取消所有的选中状态
        }



        void ClearLocalData()
        {
            allSelectedUIEffect.Clear();
            allSpecialSelectUIEff.Clear();
        }








    }
}
