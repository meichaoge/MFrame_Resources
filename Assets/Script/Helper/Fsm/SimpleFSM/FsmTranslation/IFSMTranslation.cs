using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.FSM
{
    public delegate bool StateTransHandler();
    public interface IFSMTranslation
    {
        IFSMState StateFrom { get; }
        IFSMState StateTo { get; }

        StateTransHandler Handler { get; }

        void TranslateState(IFSMState _from, IFSMState _to);

    }
}
