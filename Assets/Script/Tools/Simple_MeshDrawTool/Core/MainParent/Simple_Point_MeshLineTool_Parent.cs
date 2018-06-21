using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    public class Simple_Point_MeshLineTool_Parent : BaseMeshTool
    {

        protected override void CreateSubMesh<Simple_Point_SubMesh>()
        {
            if (m_DrawMeshState == DrawMeshState.Begin) return;
            base.CreateSubMesh<Simple_Point_SubMesh>();
            //GameObject obj = new GameObject("SubMesh_Point_" + CurrentMeshID);
            //obj.transform.SetParent(transform);
            //obj.transform.localScale = Vector3.one;
            //obj.transform.localRotation = Quaternion.identity;
            //obj.transform.localPosition = Vector3.zero;
            //BaseSubMesh _subMeshScript = obj.AddComponent<Simple_Point_SubMesh>();
            //_subMeshScript.InitialSubMesh(this,m_DefaultMateial, CurrentMeshID);

            //if (m_AllSubMeshDics.ContainsKey(CurrentMeshID))
            //    m_AllSubMeshDics[CurrentMeshID] = _subMeshScript as Simple_Point_SubMesh;
            //else
            //    m_AllSubMeshDics.Add(CurrentMeshID, _subMeshScript as Simple_Point_SubMesh);
        }





#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                float[] RandomFloats = RandomTool.GetRandomFloat(-1 * m_RandomRange, m_RandomRange, 3);
                Vector3 point = new Vector3(RandomFloats[0], RandomFloats[1], RandomFloats[2]);
                Debug.Log("AA " + point);
                CreateSubMesh<Simple_Point_SubMesh>();
                AddPoint(point);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                m_DrawMeshState = DrawMeshState.End;
            }
        }


#endif


    }
}
