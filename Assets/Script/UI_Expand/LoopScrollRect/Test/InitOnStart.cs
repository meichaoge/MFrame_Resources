using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SG
{
    [RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
    [DisallowMultipleComponent]
    public class InitOnStart : MonoBehaviour
    {
        public LoopScrollRect m_LoopScrollRect;
        public int m_DataCount = 50;
        public int offset = 0;

        private void Awake()
        {
              m_LoopScrollRect = GetComponent<LoopScrollRect>();
        }
        void Start()
        {
           // m_LoopScrollRect.RefillCells(m_DataCount);
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                m_LoopScrollRect.RefillCells(m_DataCount, offset);
            }
        }




    }
}