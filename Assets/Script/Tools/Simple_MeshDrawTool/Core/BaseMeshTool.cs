using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    public enum DrawMeshState
    {
        None,
        Begin,
        //  Drawing,
        End
    }
    public abstract class BaseMeshTool : MonoBehaviour
    {
        public Material m_DefaultMateial;
        protected int m_SubsectionOfTwoKeyPoint = 5; //两个点被分割的段数  采用贝塞尔曲线
        protected DrawMeshState m_DrawMeshState = DrawMeshState.None;
        protected int CurrentMeshID = 0;

        public Dictionary<int, List<BaseMeshPoint>> m_AllKeyPathpointDics = new Dictionary<int, List<BaseMeshPoint>>(); //关键路径点
        public Dictionary<int, List<Vector3>> m_AllMeshPointsDics = new Dictionary<int, List<Vector3>>(); //Mesh中包含的点 
        public Dictionary<int, List<int>> m_AllTranglesDics = new Dictionary<int, List<int>>();//所有的三角形索引

        protected Dictionary<int, BaseSubMesh> m_AllSubMeshDics = new Dictionary<int, BaseSubMesh>();


#if UNITY_EDITOR
        [SerializeField]
        protected float m_RandomRange = 10;
#endif

        protected virtual void CreateSubMesh<T>() where T : BaseSubMesh
        {
            //    if (m_DrawMeshState == DrawMeshState.Begin) return;
            m_DrawMeshState = DrawMeshState.Begin;
            ++CurrentMeshID;

            #region Initial DataBase
            //KeyPath
            if (m_AllKeyPathpointDics.ContainsKey(CurrentMeshID) == false)
                m_AllKeyPathpointDics.Add(CurrentMeshID, new List<BaseMeshPoint>());
            else
                m_AllKeyPathpointDics[CurrentMeshID].Clear();

            //MeshPoint
            if (m_AllMeshPointsDics.ContainsKey(CurrentMeshID) == false)
                m_AllMeshPointsDics.Add(CurrentMeshID, new List<Vector3>());
            else
                m_AllMeshPointsDics[CurrentMeshID].Clear();

            //Trangles Index
            if (m_AllTranglesDics.ContainsKey(CurrentMeshID) == false)
                m_AllTranglesDics.Add(CurrentMeshID, new List<int>());
            else
                m_AllTranglesDics[CurrentMeshID].Clear();
            #endregion

            #region CreateSubMesh
            GameObject obj = new GameObject("SubMesh_Point_" + CurrentMeshID);
            obj.transform.SetParent(transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localPosition = Vector3.zero;
            BaseSubMesh _subMeshScript = obj.AddComponent<T>();
            _subMeshScript.InitialSubMesh(this, m_DefaultMateial, CurrentMeshID);

            if (m_AllSubMeshDics.ContainsKey(CurrentMeshID))
                m_AllSubMeshDics[CurrentMeshID] = _subMeshScript as T;
            else
                m_AllSubMeshDics.Add(CurrentMeshID, _subMeshScript as T);
            #endregion


        }

        public virtual void AddPoint(Vector3 point)
        {
            if (m_AllSubMeshDics.ContainsKey(CurrentMeshID) == false)
            {
                Debug.LogError("AddPoint Fail ,MeshID Not Exit " + CurrentMeshID);
                return;
            }
            m_AllSubMeshDics[CurrentMeshID].AddPoint(point);
            m_AllSubMeshDics[CurrentMeshID].FlushMesh();

        }


        //public virtual void RecordPoint(int meshID,Vector3 position)
        //{
        //    if (m_AllMeshPointsDics.ContainsKey(meshID) == false)
        //    {
        //        Debug.LogError("RecordPoint UnIdentify subMeshID " + meshID);
        //        return;
        //    }
        //    m_AllMeshPointsDics[meshID].Add(position);
        //}
        //public virtual void RecordTrangles(int meshID, int[] tranglesIndex)
        //{
        //    if (m_AllTranglesDics.ContainsKey(meshID) == false)
        //    {
        //        Debug.LogError("RecordTrangles UnIdentify subMeshID " + meshID);
        //        return;
        //    }
        //    m_AllTranglesDics[meshID].AddRange(tranglesIndex);
        //}


    }


    [System.Serializable]
    public class BaseMeshPoint
    {
        public Vector3 Point_Center;  //Key Center Position
        public float LineWidth = 1f;  // 当前Mesh 宽度

        public BaseMeshPoint(Vector3 point)
        {
            Point_Center = point;
        }

    }
}