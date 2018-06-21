using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{

    public class Simple_Quad_MeshLineTool_Parent : BaseMeshTool
    {
        protected override void CreateSubMesh<Simple_Quad_SubMesh>()
        {
            if (m_DrawMeshState == DrawMeshState.Begin) return;
            base.CreateSubMesh<Simple_Quad_SubMesh>();

        }


#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                float[] RandomFloats = RandomTool.GetRandomFloat(-1 * m_RandomRange, m_RandomRange, 3);
                Vector3 point = new Vector3(RandomFloats[0], RandomFloats[1], RandomFloats[2]);
                Debug.Log("AA " + point);
                CreateSubMesh<Simple_Quad_SubMesh>();
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