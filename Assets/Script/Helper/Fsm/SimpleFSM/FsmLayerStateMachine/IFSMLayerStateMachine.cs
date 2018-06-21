using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.FSM
{
    /// <summary>
    /// 状态机层管理器 公共接口 方便扩展出子状态机
    /// </summary>
    public interface IFSMLayerStateMachine
    {
        IFSMController FsmController {  set; }

        /// <summary>
        /// 进入状态的默认状态
        /// </summary>
        FsmState DefaultState { get; set; }
        /// <summary>
        /// 状态机名
        /// </summary>
        string StateMachineName { get; set; }

        bool IsTopStateMachine { get; set; }


        /// <summary>
        /// 进入状态机触发
        /// </summary>
        void OnEnterStateMachine();
        /// <summary>
        /// 离开状态机触发
        /// </summary>
        void OnExitStateMachine();
        /// <summary>
        /// 尝试由其他状态机切到当前状态机
        /// </summary>
        /// <returns></returns>
        bool TryEnterStateMachine();


        void TryTransLateAny();

        void TryTransLateOne(string _toState);





    }

}
