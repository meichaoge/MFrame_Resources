using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.FSM
{
    /// <summary>
    /// 状态机控制器 每个对象只能有个一个
    /// </summary>
    public interface IFSMController
    {
        /// <summary>
        /// 当前状态所在的状态机
        /// </summary>
        FsmLayerStateMachine CurStateMachine { get; }
        /// <summary>
        /// 当前状态
        /// </summary>
        FsmState CurState { get; }
        /// <summary>
        /// 顶层状态机 有且只能有一个 控制器的入口
        /// </summary>
        FsmLayerStateMachine TopStateMachine { get; }
        /// <summary>
        /// 当前状态机控制器的所有参数设置
        /// </summary>
        FsmTransParameter Contro_TransParamater { get; }


        /// <summary>
        /// 设置顶层状态机 IFSMStateMachine.IsTopStateMachine=true;
        /// </summary>
        /// <param name="_stateMachine"></param>
        void SetTopStateMachine(IFSMLayerStateMachine _stateMachine);

        /// <summary>
        /// 更新状态控制器的整体状态信息
        /// </summary>
        /// <param name="_stateMachine"></param>
        /// <param name="_state"></param>
        void UpdateFSMInfor(IFSMLayerStateMachine _stateMachine, IFSMState _state);

        /// <summary>
        /// 启动状态机控制器
        /// </summary>
        void StartUpFsmController();

    }
}
