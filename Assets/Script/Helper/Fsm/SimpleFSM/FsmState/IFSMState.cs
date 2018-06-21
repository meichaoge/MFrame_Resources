using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MFramework.FSM
{
    public interface IFSMState
    {
        /// <summary>
        /// 当前状态所属的状态机
        /// </summary>
        FsmLayerStateMachine FsmLayerStateMachineContainer { get; }
        string StateName { get; }
        /// <summary>
        /// 当前状态的所有跳转可能
        /// </summary>
        Dictionary<string, FsmTranslation> AllStateTranslations { get; }



        Action EnterStateAct { get; }
        Action ExitStateAct { get; }

        void OnEnterState();

        void OnExitState();
        /// <summary>
        /// Add Translation to FSMState _TO
        /// </summary>
        /// <param name="_to"></param>
        void AddStateTranslation(IFSMState _to, StateTransHandler _handler);

        /// <summary>
        /// Try Translate to someState
        /// </summary>
        /// <param name="_toState">to Some State</param>
        /// <returns></returns>
        IFSMState TryTranslateToSpecial(string _toState);

        /// <summary>
        /// Try Translate To Any eligible State
        /// </summary>
        /// <returns></returns>
        IFSMState TryTranslateToAny();
    }
}
