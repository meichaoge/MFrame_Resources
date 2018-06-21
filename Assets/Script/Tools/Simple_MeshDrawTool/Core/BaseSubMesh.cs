using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{

    public abstract class BaseSubMesh : MonoBehaviour
    {

        protected int m_CurrentSubMeshID = -1;  //
        protected BaseMeshTool m_BaseParentMeshTool;

        #region Mesh Infor
        protected MeshFilter m_MeshFilter;
        protected Mesh m_Mesh;
        protected MeshRenderer m_MeshRender;
        #endregion

        #region DrawMeshData
        [System.NonSerialized]
        public List<BaseMeshPoint> m_AllSubKeyPathpoints; //关键路径点
                                                          //  [SerializeField]
        public List<Vector3> m_AllSubMeshPoints; //Mesh中包含的点 
        [SerializeField]
        protected List<int> m_AllSubTrangles;  //所有的三角形索引
        #endregion

        protected virtual void Start() { }

        public virtual void InitialSubMesh(BaseMeshTool meshTool, Material meshMaterial, int meshID)
        {
            m_MeshFilter = transform.GetComponent<MeshFilter>();
            if (m_MeshFilter == null)
            {
                m_Mesh = new Mesh();
                m_MeshFilter = gameObject.AddComponent<MeshFilter>();
                m_MeshFilter.mesh = m_Mesh;
            }

            m_MeshRender = transform.GetComponent<MeshRenderer>();
            if (m_MeshRender == null)
                m_MeshRender = gameObject.AddComponent<MeshRenderer>();
            m_MeshRender.material = meshMaterial;

            m_BaseParentMeshTool = meshTool;
            m_CurrentSubMeshID = meshID;

            m_AllSubKeyPathpoints = m_BaseParentMeshTool.m_AllKeyPathpointDics[m_CurrentSubMeshID];
            m_AllSubMeshPoints = m_BaseParentMeshTool.m_AllMeshPointsDics[m_CurrentSubMeshID];
            m_AllSubTrangles = m_BaseParentMeshTool.m_AllTranglesDics[m_CurrentSubMeshID];

            Debug.Log("Finish InitialSubMesh " + m_CurrentSubMeshID);
        }

        public virtual void AddPoint(Vector3 point)    {     }

        protected virtual void DrawSubMesh()
        {
            if (m_AllSubMeshPoints.Count <= 2) return;
            m_Mesh.vertices = m_AllSubMeshPoints.ToArray();
            m_Mesh.triangles = m_AllSubTrangles.ToArray();
            m_Mesh.RecalculateBounds();
            m_Mesh.RecalculateNormals();
        }

        public virtual void FlushMesh()
        {
            DrawSubMesh();
        }

        //Create First Key LinePoint
        protected abstract BaseMeshPoint CreateFirstMeshPoint(Vector3 point);

        //Create SubPoint Between Two KeyLinePoint
        protected abstract BaseMeshPoint CreateSubKeyPoint(BaseMeshPoint previous, Vector3 currentPoint, float widthRatio = 1);


    }
}
