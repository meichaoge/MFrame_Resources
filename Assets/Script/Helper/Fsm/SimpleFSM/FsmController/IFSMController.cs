using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.FSM
{
    /// <summary>
    /// ״̬�������� ÿ������ֻ���и�һ��
    /// </summary>
    public interface IFSMController
    {
        /// <summary>
        /// ��ǰ״̬���ڵ�״̬��
        /// </summary>
        FsmLayerStateMachine CurStateMachine { get; }
        /// <summary>
        /// ��ǰ״̬
        /// </summary>
        FsmState CurState { get; }
        /// <summary>
        /// ����״̬�� ����ֻ����һ�� �����������
        /// </summary>
        FsmLayerStateMachine TopStateMachine { get; }
        /// <summary>
        /// ��ǰ״̬�������������в�������
        /// </summary>
        FsmTransParameter Contro_TransParamater { get; }


        /// <summary>
        /// ���ö���״̬�� IFSMStateMachine.IsTopStateMachine=true;
        /// </summary>
        /// <param name="_stateMachine"></param>
        void SetTopStateMachine(IFSMLayerStateMachine _stateMachine);

        /// <summary>
        /// ����״̬������������״̬��Ϣ
        /// </summary>
        /// <param name="_stateMachine"></param>
        /// <param name="_state"></param>
        void UpdateFSMInfor(IFSMLayerStateMachine _stateMachine, IFSMState _state);

        /// <summary>
        /// ����״̬��������
        /// </summary>
        void StartUpFsmController();

    }
}
