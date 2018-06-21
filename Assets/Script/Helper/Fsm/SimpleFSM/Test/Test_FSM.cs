//using MFramework.FSM;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace MFramework
//{

//    public class Test_FSM : MonoBehaviour
//    {
//        [SerializeField]
//        int IntValue = 0;
//        // Use this for initialization
//        void Start()
//        {
//            //**Create And Record FsmState
//            FSMState state1 = new FSMState("_state1");
//            FSMState state2 = new FSMState("_state2");
//            FSMState state3 = new FSMState("_state3");
//            FSMManager.GetInstance().AddFsmState(state1.StateName, () => { Debug.Log("Enter " + state1.StateName); }, () => { Debug.Log("Exit" + state1.StateName); });
//            FSMManager.GetInstance().AddFsmState(state2.StateName, () => { Debug.Log("Enter " + state2.StateName); }, () => { Debug.Log("Exit" + state2.StateName); });
//            FSMManager.GetInstance().AddFsmState(state3.StateName, () => { Debug.Log("Enter " + state3.StateName); }, () => { Debug.Log("Exit" + state3.StateName); });

//            //Create And Record FsmTranslation
//            FSMTranslation trans1 = new FSMTranslation(state1, state2, () =>
//            {
//                if (IntValue % 2 == 0) return true;
//                return false;
//            });
//            FSMTranslation trans2 = new FSMTranslation(state1, state3, () =>
//            {
//                if (IntValue % 3 == 0) return true;
//                return false;
//            });
//            FSMTranslation trans3 = new FSMTranslation(state2, state3, () =>
//            {
//                if (IntValue % 5 == 0) return true;
//                return false;
//            });
//            FSMTranslation trans4 = new FSMTranslation(state3, state1, () =>
//            {
//                if (IntValue % 4 == 0) return true;
//                return false;
//            });
//            FSMManager.GetInstance().AddFsmTranslation(trans1);
//            FSMManager.GetInstance().AddFsmTranslation(trans2);
//            FSMManager.GetInstance().AddFsmTranslation(trans3);
//            FSMManager.GetInstance().AddFsmTranslation(trans4);

//            //StartUp Fsm system
//            FSMManager.GetInstance().StartUpFsmManager(state1.StateName);
//        }

//        int data = 0;
//        void Update()
//        {
//            ++data;
//            if (data % 6 != 0) return;
//            IntValue = Random.Range(0, 6);

//            FSMManager.GetInstance().TryTransLateAny();
//        }


//    }
//}
