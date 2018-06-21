using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BeanVR
{
    public class UIEffectManager
    {

        private List<UIEffect> allSelectedUIEffect = new List<UIEffect>(); //ѡ�е�UIEffect
        private List<UIEffect> allSpecialSelectUIEff = new List<UIEffect>();//�����ѡ��UIEffect ���ᱻ������UIEffect�ر� ����UIMicrophpne

        private static UIEffectManager instance;
        public static UIEffectManager GetInstance()
        {
            if (instance == null)
                instance = new UIEffectManager();
            return instance;
        }
        private UIEffectManager()
        {
          //  GlobalEntity.GetInstance().AddListener(BeanVRUIEvent.switchAccount, ClearLocalData);   //�����л��˺�
        //    GlobalEntity.GetInstance().AddListener<string>(SceneStateController.mEvent.loadingSwitchNotify, OnSceneChange); //Listen SceneChange

        }



        #region ����ѡ����


        /// <summary>
        /// ����ѡ�е�UIEffect
        /// </summary>
        /// <param name="_operateUIEffect">Operate user interface effect.</param>
        /// <param name="_UnSelectPrevious">�Ƿ������һ�β��������ѡ��Ч��</param>
        public void StoreSelectState(UIEffect _operateUIEffect, bool _UnSelectPrevious)
        {
            if (_operateUIEffect == null) return;
            if (_operateUIEffect.isSelected)
            {
                //Debug.Log ("�Ѿ�ѡ��Ч�� " + _operateUIEffect.gameObject.name);
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
                allSelectedUIEffect.Add(_operateUIEffect); //��֤��ǰ������UIEffect�����һ��
            }

            if (_UnSelectPrevious)
            {
                if (allSelectedUIEffect.Count > 1)
                {
                    UIEffect previousEffect = allSelectedUIEffect[allSelectedUIEffect.Count - 2];
                    if (previousEffect != null)
                    {
                        _operateUIEffect.isEffectEnable = true;
                        previousEffect.UnSelected(); //ȡ��֮ǰ�������ѡ��
                        allSelectedUIEffect.Remove(previousEffect);
                    }//if
                }//if
            }//if

        }
        /// <summary>
        /// ״̬�Զ��л���UIEffect
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
        /// ���������UIEffect ��ѡ��Ч��ֻ�յ�������� �Լ������л�����
        /// </summary>
        /// <param name="_operateUIEffect">Operate user interface effect.</param>
        /// <param name="_autoFlipState">�Ƿ����Զ���ת״̬</param>
        public void StoreSpecialSelect(UIEffect _operateUIEffect, bool _autoFlipState)
        {
            if (_operateUIEffect.isSelected)
            {
                if (_autoFlipState)
                {
                    _operateUIEffect.isEffectEnable = true;
                    _operateUIEffect.UnSelected(); //��ת״̬
                                                   // _operateUIEffect.isEffectEnable = false;
                }
                else
                {
                    Debug.Log("�Ѿ�ѡ��״̬");
                }//else
            }//if
            else
            {
                if (_autoFlipState)
                {
                    _operateUIEffect.isEffectEnable = true;
                    _operateUIEffect.SetSelected(); //��ת״̬
                }
            }//else

            if (allSpecialSelectUIEff.Contains(_operateUIEffect))
            {
                if (_operateUIEffect.isSelected == false)
                    allSpecialSelectUIEff.Remove(_operateUIEffect); //�Ƴ��Ѿ���ѡ�е�UIEffect
            }
            else
            {
                if (_operateUIEffect.isSelected)
                    allSpecialSelectUIEff.Add(_operateUIEffect); //�����Ѿ���ѡ�е�UIEffect
            }
        }

        /// <summary>
        /// ǿ��ѡ�������UIEffect
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
        /// ǿ�Ʒ�ѡ����UIEffect
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
        /// �Զ���ת״̬
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
            { //�����
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
            ClearAllSeclet();  //ȡ�����е�ѡ��״̬
        }



        void ClearLocalData()
        {
            allSelectedUIEffect.Clear();
            allSpecialSelectUIEff.Clear();
        }








    }
}
