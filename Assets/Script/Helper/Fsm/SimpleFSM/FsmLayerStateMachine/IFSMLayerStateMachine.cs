using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.FSM
{
    /// <summary>
    /// ״̬��������� �����ӿ� ������չ����״̬��
    /// </summary>
    public interface IFSMLayerStateMachine
    {
        IFSMController FsmController {  set; }

        /// <summary>
        /// ����״̬��Ĭ��״̬
        /// </summary>
        FsmState DefaultState { get; set; }
        /// <summary>
        /// ״̬����
        /// </summary>
        string StateMachineName { get; set; }

        bool IsTopStateMachine { get; set; }


        /// <summary>
        /// ����״̬������
        /// </summary>
        void OnEnterStateMachine();
        /// <summary>
        /// �뿪״̬������
        /// </summary>
        void OnExitStateMachine();
        /// <summary>
        /// ����������״̬���е���ǰ״̬��
        /// </summary>
        /// <returns></returns>
        bool TryEnterStateMachine();


        void TryTransLateAny();

        void TryTransLateOne(string _toState);





    }

}
