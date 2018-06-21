using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace MFramework.EditorExpand
{
    /// <summary>
    /// 批量修改预制体后替换原有的资源
    /// </summary>
    public class ModifyTextFontInfor : EditorWindow
    {

        #region  Data 
        private static ModifyTextFontInfor S_ModifyTextFontInfor = null;
        private string m_TopAssetPath = "";  //显示需要配置的目录
        private string m_RelativeAssetPath = "";  //显示需要配置的目录

        private string m_NewFontAssetName = ""; //需要替换的资源名
        private string m_NewFontMaterialName = "";// 需要替换的字体资源材质球

        private float m_Process = 0;
        private List<string> m_AllPrefabsInfor = new List<string>();

        #endregion

        [MenuItem("Tools/批量修改预制体/修改TextMeshPro 字体")]
        private static void ChangeTextMeshProFont()
        {
            S_ModifyTextFontInfor = EditorWindow.GetWindow<ModifyTextFontInfor>("修改TextMeshPro上字体信息");
            S_ModifyTextFontInfor.minSize = new Vector2(300, 100);
            S_ModifyTextFontInfor.Show();
        }

        private void OnEnable()
        {
            m_TopAssetPath = Application.dataPath + "/";
            m_NewFontAssetName = "Fonts & Materials/FZLTHJW SDF";
            m_NewFontMaterialName = "Fonts & Materials/";
        }


        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("顶层目录:"), GUILayout.Width(100));
            m_TopAssetPath = EditorTextField_Expand.TextField(m_TopAssetPath, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("相对目录:"), GUILayout.Width(100));
            m_RelativeAssetPath = EditorTextField_Expand.TextField(m_RelativeAssetPath, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("字体资源名:"), GUILayout.Width(100));
            m_NewFontAssetName = EditorTextField_Expand.TextField(m_NewFontAssetName, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();

            GUILayout.Label(new GUIContent("如果没有设置字体材质球名则保持原有的材质球或者使用默认的材质球"));
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("字体资源材质球:"), GUILayout.Width(100));
            m_NewFontMaterialName = EditorTextField_Expand.TextField(m_NewFontMaterialName, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("批量修改"), GUILayout.Width(150), GUILayout.Height(50)))
            {
                m_Process = 0;
                if (GetAllPrefabsPathInfor() == false)
                    return;
                CreateAndShowPrefab();
                S_ModifyTextFontInfor.Close();
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 获取所有的prefab
        /// </summary>
        private bool  GetAllPrefabsPathInfor()
        {
            m_AllPrefabsInfor.Clear();

            if(System.IO.Directory.Exists(m_TopAssetPath + m_RelativeAssetPath)==false)
            {
                Debug.LogError("当前配置的目录不存在  " + m_TopAssetPath + m_RelativeAssetPath);
                return false;
            }


            string[] allPrefabs = System.IO.Directory.GetFiles(m_TopAssetPath + m_RelativeAssetPath, "*.prefab", System.IO.SearchOption.AllDirectories);
            string assetPath = @"Assets";
            for (int dex = 0; dex < allPrefabs.Length; dex++)
            {
                int index = allPrefabs[dex].IndexOf(assetPath);
                m_AllPrefabsInfor.Add(allPrefabs[dex].Substring(index).Replace(@"\", "/"));
            }

            //for (int dex = 0; dex < m_AllPrefabsInfor.Count; ++dex)
            //{
            //    Debug.Log(m_AllPrefabsInfor[dex]);
            //}
            return true;
        }
        /// <summary>
        /// 创建预制体
        /// </summary>
        private void CreateAndShowPrefab()
        {
            if (m_AllPrefabsInfor.Count == 0) return;
            GameObject TopGo = new GameObject("Asset");
            //   TopGo.hideFlags = HideFlags.HideInHierarchy;
            TMP_FontAsset newFont = GetFontInfor(m_NewFontAssetName);  //需要加载的字体
            Material material = GetFontMaterial(m_NewFontMaterialName);
            #region 修改TextMeshPro  字体以及材质球

            for (int dex = 0; dex < m_AllPrefabsInfor.Count; ++dex)
            {
                EditorUtility.DisplayProgressBar("替换字体资源", m_AllPrefabsInfor[dex], m_Process / m_AllPrefabsInfor.Count);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(m_AllPrefabsInfor[dex]);
                if (prefab != null)
                {
                    GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    go.name = prefab.name;
                    go.transform.SetParent(TopGo.transform, false);


                    TextMeshProUGUI[] textComponents = go.GetComponentsInChildren<TextMeshProUGUI>(true);
                    bool IsNeedReplace = false;
                    for (int index = 0; index < textComponents.Length; ++index)
                    {
                        //textComponents[index].fontSize = 36;
                        if (textComponents[index].font != newFont)
                        {
                            IsNeedReplace = true;
                            textComponents[index].font = newFont;
                        } //修改字体

                        if (material != null && textComponents[index].fontSharedMaterial != material)
                        {
                            IsNeedReplace = true;
                            textComponents[index].fontSharedMaterial = material;
                        }//修改材质球
                        //textComponents[index].color = Color.red;
                    }
                    if (IsNeedReplace)
                    {
                        Debug.LogInfor("替换预制体 " + m_AllPrefabsInfor[dex]);
                        PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.ConnectToPrefab);
                    }
                }
                ++m_Process;
            }
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("字体资源", "字体资源完成", "确定");
            GameObject.DestroyImmediate(TopGo);
            #endregion  
        }

        /// <summary>
        /// 加载字体资源
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        private TMP_FontAsset GetFontInfor(string fontName)
        {
            return Resources.Load<TMP_FontAsset>(fontName);
        }

        /// <summary>
        /// 加载字体材质球
        /// </summary>
        /// <param name="materialName"></param>
        /// <returns></returns>
        private Material GetFontMaterial(string materialName)
        {
            if (string.IsNullOrEmpty(materialName))
                return null;
            return Resources.Load<Material>(materialName);
        }


    }
}