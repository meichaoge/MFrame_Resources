using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace MFramework
{

    public class TriggerTool : MonoBehaviour
    {

        public Action<GameObject> EnterTriggerEvent;
        public Action<GameObject> ExitTriggerEvent;
       
        public Collider CurrentCollider
        {
            get
            {
                Collider _collider = gameObject.GetComponent<Collider>();
                if (_collider == null)
                    Debug.LogError(gameObject.name + "  Miss Component of  Collider");

                return _collider;
            }
        }

        [Range(0.1f, 20f)]
        public float CheckDistance;
        [Range(0, 360)]
        public int CheckAngle;


        private GameObject m_CurrentEnterObj;

        private void Awake()  {  }

        private void Start()
        {
            //    InvokeRepeating("CheckCameraPositionForExitState", 0.01f, 0.02f);
        }

        private void OnDestroy()
        {
            //    CancelInvoke("CheckCameraPositionForExitState");
        }

        void OnTriggerEnter(Collider other)
        {
            m_CurrentEnterObj = other.gameObject;
            if (EnterTriggerEvent != null)
                EnterTriggerEvent(other.gameObject);
        }


        void OnTriggerExit(Collider other)
        {
            m_CurrentEnterObj = null;
            if (ExitTriggerEvent != null)
                ExitTriggerEvent(other.gameObject);
        }

        [SerializeField]
        float currentDistance = 0f;
        [SerializeField]
        float currentAngle = 0f;
        void CheckCameraPositionForExitState()
        {
            currentDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            currentAngle = Vector3.Angle(transform.forward, Camera.main.transform.position - transform.position);
            if (currentDistance > CheckDistance || currentAngle > CheckAngle)
            {
                if (m_CurrentEnterObj != null && ExitTriggerEvent != null)
                    ExitTriggerEvent(m_CurrentEnterObj);
            }//if

        }



    }
}
