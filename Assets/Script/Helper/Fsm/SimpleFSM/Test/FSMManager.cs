//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//namespace MFramework.FSM
//{
//    public class FSMManager 
//    {

//        public  Dictionary<string, FSMState> AllFsmStates = new Dictionary<string, FSMState>();
//        public IFSMState CurrentState { protected set; get; }


//        #region Add FsmState
//        public void AddFsmState(string _statename, Action _enter, Action _Exit)
//        {
//            if (AllFsmStates.ContainsKey(_statename) == false)
//            {
//                FSMState _state = new FSMState(_statename, _enter, _Exit);
//                AllFsmStates.Add(_statename, _state);
//            }
//            Debug.Log("Repead Regist State " + _statename);
//            return;
//        }
//        public void AddFsmState(FSMState _state)
//        {
//            if (AllFsmStates.ContainsKey(_state.StateName) == false)
//            {
//                AllFsmStates.Add(_state.StateName, _state);
//                return;
//            }
//            Debug.Log("Repead Regist State " + _state.StateName);
//            return;
//        }
//        #endregion

//        #region Add FsmTranslation
//        public void AddFsmTranslation(FSMTranslation  _trans)
//        {
//            if (AllFsmStates.ContainsKey(_trans.StateFrom.StateName) == false)
//            {
//                Debug.LogError("Miss  State From ");
//                return;
//            }
//            if (AllFsmStates.ContainsKey(_trans.StateTo.StateName) == false)
//            {
//                Debug.LogError("Miss State To ");
//                return;
//            }

//            Debug.Log("AddTrans " + _trans.StateFrom.StateName + "  " + _trans.StateTo.StateName);
//            FSMState _StateFrom = AllFsmStates[_trans.StateFrom.StateName];
//            FSMState _StateTo = AllFsmStates[_trans.StateTo.StateName];
//            _StateFrom.AddStateTranslation(_StateTo);
//        }
//        #endregion

//        /// <summary>
//        /// Start Up The Top StateMachine
//        /// </summary>
//        /// <param name="_state"></param>
//        public void StartUpFsmManager(string _state)
//        {
//            if (AllFsmStates.ContainsKey(_state) == false)
//            {
//                Debug.LogError("Don't   Containt This State " + _state);
//                return;
//            }
//            CurrentState = AllFsmStates[_state];
//            CurrentState.OnEnterState();
//        }


//        #region Fsm TransLation

//        ///// <summary>
//        ///// Try Any  First Possible FsmState  From This State 
//        ///// </summary>
//        //public void TryTransLateAny()
//        //{
//        //    if (CurrentState != null)
//        //    {
//        //        if (CurrentState.TryTranslateToAny())
//        //        {
//        //            CurrentState = FSMState.TransToState;
//        //            Debug.Log("TryTransLateAny Success:To ..." +CurrentState.StateName);
//        //        }
//        //    }
//        //}
//        ///// <summary>
//        ///// Try To Translate To _toState
//        ///// </summary>
//        ///// <param name="_toState"></param>
//        //public void TryTransLateOne(string _toState)
//        //{
//        //    if (CurrentState != null)
//        //    {
//        //        if (CurrentState.TryTranslateToSpecial(_toState))
//        //        {
//        //            CurrentState = FSMState.TransToState;
//        //            Debug.Log("TryTransLateOne  Success");
//        //        }
//        //    }
//        //}

//        #endregion

//    }
//}
