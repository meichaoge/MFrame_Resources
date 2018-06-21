using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.FSM
{
    public interface IFSMTransParameter
    {
        IFSMController FsmController { get; }
        string FsmTransParameterName { get; }
    }
}
