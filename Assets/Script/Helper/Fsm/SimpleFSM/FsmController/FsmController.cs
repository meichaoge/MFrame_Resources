using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.FSM
{
    /// <summary>
    /// ״̬�������� ����״̬���������Լ����а�����StateMachine
    /// </summary>
    public class FsmController : MonoBehaviour, IFSMController
    {
        public FsmLayerStateMachine CurStateMachine { private set; get; }
        public FsmState CurState { private set; get; }
        public FsmLayerStateMachine TopStateMachine { private set; get; }
        public FsmTransParameter Contro_TransParamater { private set; get; }


#if UNITY_EDITOR
        #region ��ʾ״̬
        public FsmState _CurrentState;
        protected void Update()
        {
            _CurrentState = CurState;

            AllCurrentRecordState.Clear();
            foreach (var item in CurStateMachine.AllFsmStates)
            {
                AllCurrentRecordState.Add(item.Value);
            }
        }

        public List<FsmState> AllCurrentRecordState = new List<FsmState>();
        #endregion
#endif


        protected virtual void Awake()
        {
            Contro_TransParamater = new FsmTransParameter(this, gameObject.name + "_Fsm_Parameter");
        }


        public void SetTopStateMachine(IFSMLayerStateMachine _stateMachine)
        {
            if (TopStateMachine != null)
            {
                Debug.LogError("There Cant Be More Than One TopStateMachine");
                return;
            }
            TopStateMachine = (FsmLayerStateMachine)_stateMachine ;
        }
        public void UpdateFSMInfor(IFSMLayerStateMachine _stateMachine, IFSMState _state)
        {
            CurStateMachine = (FsmLayerStateMachine)_stateMachine;
            CurState = (FsmState)_state;
        }

        public void StartUpFsmController()
        {
            if (TopStateMachine == null)
            {
                Debug.LogError("FSMController Cant Start ,TopStateMachine is Null");
                return;
            }
            if (TopStateMachine.DefaultState == null)
            {
                Debug.LogError("FSMController Cant Start ,TopStateMachine  DefaultState is Null");
                return;
            }

            CurStateMachine = TopStateMachine;
            CurState = TopStateMachine.DefaultState;

            TopStateMachine.OnEnterStateMachine();

        }



    }
}