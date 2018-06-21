using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.FSM
{
    [System.Serializable]
    public class FsmState : IFSMState
    {
#if UNITY_EDITOR 
        public string _ShowName;
        public List<string> ShowAllTrans = new List<string>();

#endif

        public FsmLayerStateMachine FsmLayerStateMachineContainer { protected set; get; }
        public string StateName { protected set; get; }
        public Action EnterStateAct { protected set; get; }
        public Action ExitStateAct { protected set; get; }
        public Dictionary<string, FsmTranslation> AllStateTranslations { protected set; get; }

        /// <summary>
        /// 状态机的状态 
        /// </summary>
        /// <param name="_stateMachine">当前状态所属的状态机层管理器</param>
        /// <param name="_name"></param>
        /// <param name="_enterAct"></param>
        /// <param name="_exitAct"></param>
        public FsmState(IFSMLayerStateMachine _stateMachine ,string _name, Action _enterAct = null, Action _exitAct = null)
        {
            if (_stateMachine == null)
            {
                Debug.LogError("FSMState " + _name + " IFSMStateMachine  Can't Be Null");
                return;
            }
            FsmLayerStateMachineContainer = (FsmLayerStateMachine) _stateMachine;
            StateName = _name;
            EnterStateAct = _enterAct;
            ExitStateAct = _exitAct;
            AllStateTranslations = new Dictionary<string, FsmTranslation>();


#if UNITY_EDITOR
            _ShowName = _name;
            ShowAllTrans.Clear();
#endif

        }

        public void AddStateTranslation(IFSMState _to, StateTransHandler _handler)
        {
            if (AllStateTranslations.ContainsKey(_to.StateName))
            {
                Debug.LogError("ReRegister..");
                return;
            }
            if (_to == this)
            {
                Debug.LogError("ReRegister   Self.."); 
                return;
            }
            AllStateTranslations.Add(_to.StateName, new FsmTranslation(this, _to, _handler));

#if UNITY_EDITOR
            ShowAllTrans.Add(_to.StateName);
#endif

        }

        public void OnEnterState()
        {
            if (EnterStateAct != null)
                EnterStateAct();
        }

        public void OnExitState()
        {
            if (ExitStateAct != null)
                ExitStateAct();
        }

        public IFSMState TryTranslateToSpecial(string _toState)
        {
            if (AllStateTranslations.ContainsKey(_toState) == false) return null;
            if (AllStateTranslations[_toState].Handler != null && AllStateTranslations[_toState].Handler() == false)
                return null;

            IFSMState TransToState = AllStateTranslations[_toState].StateTo;
            return TransToState;
        }

        public IFSMState TryTranslateToAny()
        {
            IFSMState TransToState = null;
            foreach (var state in AllStateTranslations)
            {
                if (state.Value.StateTo.StateName == this.StateName)
                {
                    Debug.LogError("TryTranslateToAny    Self..");
                    continue;
                }

                TransToState = TryTranslateToSpecial(state.Value.StateTo.StateName);
                if (TransToState!=null)
                    return TransToState;
            }
            return null;
        }


    }
}
