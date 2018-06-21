using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.FSM
{
    [System.Serializable]
    public class FsmTranslation : IFSMTranslation
    {
#if UNITY_EDITOR
        public IFSMState _Show_StateFrom;
        public IFSMState _Show_StateTo;

#endif


        public IFSMState StateFrom { protected set; get; }

        public IFSMState StateTo { protected set; get; }

        /// <summary>
        /// ÅÐ¶ÏÄÜ·ñ×´Ì¬Ìø×ª
        /// </summary>
        public StateTransHandler Handler { protected set; get; }


        public FsmTranslation(IFSMState _from, IFSMState _to, StateTransHandler _handler = null)
        {
            if (_to.StateName == _from.StateName)
            {
                Debug.LogError("FSMTranslation _from Can't Be The Same With _to");
                return;
            }

            StateFrom = _from;
            StateTo = _to;
            Handler = _handler;

#if UNITY_EDITOR
            _Show_StateFrom = StateFrom;
            _Show_StateTo = StateTo;
#endif

        }


        public void TranslateState(IFSMState _from, IFSMState _to)
        {
            if (_to == _from)
            {
                Debug.LogError("TranslateState   _from Can't Be The Same With _to");
                return;
            }

            if (_from != null)
                _from.OnExitState();

            if (_to != null)
                _to.OnEnterState();
        }
    }
}
