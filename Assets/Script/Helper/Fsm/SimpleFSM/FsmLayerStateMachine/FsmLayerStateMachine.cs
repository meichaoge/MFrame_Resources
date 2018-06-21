using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.FSM
{
    /// <summary>
    /// ״̬���Ļ��� �����ж��״̬�� ����ֻ����һ������״̬��
    /// </summary>
    public class FsmLayerStateMachine : IFSMLayerStateMachine
    {
        public IFSMController FsmController { protected get; set; } //��ǰ״̬�����ĸ�����������
        public FsmState DefaultState { get; set; }
        public FsmState CurrentState { protected set; get; }  //��ǰ״̬����״̬ 
        public string StateMachineName { get; set; }
        public bool IsTopStateMachine { get; set; }

        public Dictionary<string, FsmState> AllFsmStates = new Dictionary<string, FsmState>();  //��ǰ״̬��������״̬
        public Dictionary<string, IFSMLayerStateMachine> AllGetOutStateMachine = new Dictionary<string, IFSMLayerStateMachine>();  //���дӵ�ǰ״̬���ܵ������һ��״̬��

#if UNITY_EDITOR
        public string _ShowName;
        public bool _IsTopStateMachine;
        public List<string> ShowAllTrans = new List<string>();

#endif

        public FsmLayerStateMachine(IFSMController _controller, string _name, bool isTopStateMachine)
        {
            if (_controller == null)
            {
                Debug.LogError("FsmStateMachine " + _name + " IFSMController Can't Be Null");
                return;
            }
            FsmController = _controller;
            StateMachineName = _name + "_StateMachine";


            DefaultState = new FsmState(this, StateMachineName+ "_defaultState ", null, null);
            IsTopStateMachine = isTopStateMachine;

            if (IsTopStateMachine)
                FsmController.SetTopStateMachine(this);

#if UNITY_EDITOR
            _ShowName = _name;
            ShowAllTrans.Clear();
            _IsTopStateMachine = IsTopStateMachine;
#endif

        }

        public void SetDefaultState(FsmState _defaultState)
        {
            if (_defaultState == null)
            {
                Debug.LogError("FsmStateMachine " + StateMachineName + "Set DefaultState  Can't Be Null");
                return;
            }
            DefaultState = _defaultState;
        }

        public virtual void OnEnterStateMachine()
        {
            FsmController.UpdateFSMInfor(this, DefaultState);  //����״̬��������״̬

            Debug.Log("OnEnterStateMachine  name=" + StateMachineName + "  DefaultState=" + DefaultState);

            CurrentState = DefaultState;
            if (CurrentState != null)
                CurrentState.OnEnterState();

        }

        public virtual void OnExitStateMachine()
        {
            CurrentState = null;
        }

        public virtual bool TryEnterStateMachine()
        {
            return true;
        }

        #region AddFsmStateMachine
        /// <summary>
        /// ���ת��״̬��
        /// </summary>
        /// <param name="_stateMachine"></param>
        public virtual void AddFSMStateMachine(IFSMLayerStateMachine _stateMachine)
        {
            if (_stateMachine == null)
            {
                Debug.LogError("AddFSMStateMachine Fail, Add StateMachine is Null");
                return;
            }

            if (_stateMachine == this)
            {
                Debug.LogError("AddFSMStateMachine Fail, Can't Add Self StateMachine");
                return;
            }

            if (AllGetOutStateMachine.ContainsKey(_stateMachine.StateMachineName))
            {
                Debug.LogError("Repead AddFSMStateMachine " + _stateMachine.StateMachineName);
                return;
            }
            AllGetOutStateMachine.Add(_stateMachine.StateMachineName,_stateMachine);
            Debug.Log("AddFSMStateMachine Success , " + _stateMachine.StateMachineName + "  Count =" + AllGetOutStateMachine.Count);
        }
        #endregion

        #region Add FsmState
        public void AddFsmState(string _statename, Action _enter, Action _Exit)
        {
            if (AllFsmStates.ContainsKey(_statename) == false)
            {
                FsmState _state = new FsmState(this, _statename, _enter, _Exit);
                AllFsmStates.Add(_statename, _state);
#if UNITY_EDITOR
                ShowAllTrans.Add(_statename);
#endif
            }
            Debug.Log("Repead Regist State " + _statename);

            return;
        }

        public void AddFsmState(FsmState _state)
        {
            if (AllFsmStates.ContainsKey(_state.StateName) == false)
            {
                AllFsmStates.Add(_state.StateName, _state);
#if UNITY_EDITOR
                ShowAllTrans.Add(_state.StateName);
#endif
                return;
            }
            Debug.Log("Repead Regist State " + _state.StateName);
            return;
        }
        #endregion

        #region Add FsmTranslation
        public void AddFsmTranslation(FsmTranslation _trans)
        {
            if (AllFsmStates.ContainsKey(_trans.StateFrom.StateName) == false)
            {
                Debug.LogError("Miss  State From ");
                return;
            }
            if (AllFsmStates.ContainsKey(_trans.StateTo.StateName) == false)
            {
                Debug.LogError("Miss State To ");
                return;
            }

            Debug.Log("AddTrans " + _trans.StateFrom.StateName + "  " + _trans.StateTo.StateName);
            FsmState _StateFrom = AllFsmStates[_trans.StateFrom.StateName];
            FsmState _StateTo = AllFsmStates[_trans.StateTo.StateName];
            _StateFrom.AddStateTranslation(_StateTo, _trans.Handler);
        }

        public void AddFsmTranslation(FsmState _from, FsmState _to, StateTransHandler _handler)
        {
            FsmTranslation _translation = new FsmTranslation(_from, _to, _handler);
            AddFsmTranslation(_translation);
        }

        public void AddFsmTranslation(string _from, string _to, StateTransHandler _handler)
        {
            if (AllFsmStates.ContainsKey(_from) == false)
            {
                Debug.LogError("This StateMachine Don't Contain State " + _from);
                return;
            }
            if (AllFsmStates.ContainsKey(_to) == false)
            {
                Debug.LogError("This StateMachine Don't Contain State " + _to);
                return;
            }

            AddFsmTranslation(AllFsmStates[_from], AllFsmStates[_to], _handler);
        }


        #endregion

        #region Fsm TransLation

        /// <summary>
        /// Try Any  First Possible FsmState  From This State 
        /// </summary>
        public void TryTransLateAny()
        {
            if (CurrentState == null)
            {
                Debug.LogError("CurrentState is Null,StateMachine : " + StateMachineName);
                return;
            }

            IFSMState _TransToState = null;
            //different State of This Statemachine  Switch Check
            foreach (var state in AllFsmStates)
            { //��ǰ״̬����״̬�л�
                _TransToState = state.Value.TryTranslateToAny();
                if (_TransToState != null)
                {
                    Debug.Log("StateMachine: " + StateMachineName + " TryTransLateAny Success  :    CurrentState=" + CurrentState.StateName + "  " + _TransToState.StateName);
                    CurrentState.OnExitState();
                    CurrentState = (FsmState)_TransToState;   //����״̬
                    _TransToState.OnEnterState();
                    FsmController.UpdateFSMInfor(this, CurrentState);  //����״̬��������״̬
                   // Debug.Log("StateMachine: " + StateMachineName + "  TryTransLateAny Success  :  StateName=" + CurrentState.StateName);
                    return;
                }
            }

            if (AllGetOutStateMachine == null || AllGetOutStateMachine.Count == 0)
            {
                Debug.Log("StateMachine: " + StateMachineName + "  TryTransLateAny Fai   :  No GetOut StateMachine");
                return;
            }
            //Different StateMchine Switch Check
            foreach (var stateMachine in AllGetOutStateMachine.Values)
            {
                if (stateMachine.TryEnterStateMachine())
                {
                    Debug.Log("StateMachine: " + StateMachineName + "  TryTransLateAny Success  :  StateMachineName=" + stateMachine.StateMachineName);
                    CurrentState.OnExitState();
                    OnExitStateMachine(); //�뿪��ǰ״̬i
                    stateMachine.OnEnterStateMachine();  //������һ��״̬��

                    return;
                }
            }

        }
        /// <summary>
        /// Try To Translate To _toState
        /// </summary>
        /// <param name="_toState"></param>
        public void TryTransLateOne(string _toState)
        {
            if (CurrentState == null)
            {
                Debug.LogError("CurrentState is Null,StateMachine : " + StateMachineName);
                return;
            }

            IFSMState _TransToState = CurrentState.TryTranslateToSpecial(_toState);
            if (_TransToState == null)
            {
                Debug.Log("StateMachine: " + StateMachineName + "  TryTransLateOne  Fai   :  Can't Trans " + _toState);
                return;
            }
            Debug.Log("StateMachine: " + StateMachineName + "CurrentState=" + CurrentState.StateName + "  " + _TransToState.StateName);
            CurrentState.OnExitState();
            CurrentState = (FsmState)_TransToState;   //����״̬
            _TransToState.OnEnterState();
            FsmController.UpdateFSMInfor(this, CurrentState);  //����״̬��������״̬
            Debug.Log("StateMachine: " + StateMachineName + "  TryTransLateOne Success  :  StateName=" + CurrentState.StateName);
            return;
        }

        #endregion


    }
}