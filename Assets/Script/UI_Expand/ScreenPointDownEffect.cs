using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 点击屏幕时候生成一个特效
    /// </summary>
    public class ScreenPointDownEffect : MonoBehaviour
    {
        [System.Serializable]
        public class EffectRecord
        {
            public GameObject m_EffectObj;
            public float m_RemainTime;
            public EffectRecord(GameObject go, float time)
            {
                m_EffectObj = go;
                m_RemainTime = time;
            }
        }

        Queue<GameObject> m_EffectObjs = new Queue<GameObject>();
        List<EffectRecord> m_AllShowingEffectObj = new List<EffectRecord>();
        public float m_MaxAliveTime = 0.5f;  //每个特效存在时间

        private Vector3 m_MouseCanvasPostion; //点击时候鼠标的位置 转换到中心点再屏幕正中心的坐标


        void Update()
        {
            UpdateEffectObjsState();

            if (Input.GetMouseButtonDown(0))
            {
                m_MouseCanvasPostion = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
                m_MouseCanvasPostion = Camera.main.ScreenToWorldPoint(m_MouseCanvasPostion);

                //****方法2  GetRectTransformPos脚本中的方式获取某个Canvs 上鼠标的屏幕坐标



                GameObject go = GetEffectObject();
                go.transform.localPosition = Camera.main.transform.InverseTransformPoint(m_MouseCanvasPostion); ;
            }
        }

        void OnDisable()
        {
            m_EffectObjs.Clear();
            m_AllShowingEffectObj.Clear();
        }


        private GameObject GetEffectObject()
        {
            if (m_EffectObjs.Count != 0)
            {
                GameObject obj = m_EffectObjs.Dequeue();
                if (obj != null)
                {
                    obj.SetActive(true);

                    EffectRecord record2 = new EffectRecord(obj, m_MaxAliveTime);
                    m_AllShowingEffectObj.Add(record2);
                    return obj;
                }
            } //获取之前生成的Effect

            //      GameObject go = ResourceMgr.instance.Instantiate(EffectResPath.ScreenEffectItemPath, Camera.main.transform);  //2017/11/10注释掉 这里用来加载特效并放在相机下
            GameObject go = new GameObject("Effect");
            go.transform.localPosition = Camera.main.transform.InverseTransformPoint(m_MouseCanvasPostion); ;
            go.transform.localScale = Vector3.one;

            EffectRecord record = new EffectRecord(go, m_MaxAliveTime);
            m_AllShowingEffectObj.Add(record);
            return go;
        }

        /// <summary>
        /// 更新所有正在显示的特效的状态
        /// </summary>
        void UpdateEffectObjsState()
        {
            if (m_AllShowingEffectObj.Count > 0)
            {
                for (int dex = 0; dex < m_AllShowingEffectObj.Count; ++dex)
                {
                    m_AllShowingEffectObj[dex].m_RemainTime -= Time.deltaTime;
                }
                if (m_AllShowingEffectObj[0].m_RemainTime <= 0)
                {
                    m_AllShowingEffectObj[0].m_EffectObj.SetActive(false);
                    m_EffectObjs.Enqueue(m_AllShowingEffectObj[0].m_EffectObj);
                    m_AllShowingEffectObj.RemoveAt(0);
                }
            }
        }




    }

}