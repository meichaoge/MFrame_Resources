using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace MFramework.Re
{
    public class BuildAssetRecordEditorInfor
    {
        public string m_AssetPath;
        public bool m_IsSelect = false;  //是否需要被打包
        public bool m_IsFileAsset = true; //是目录还是文件
        public BuildAssetRecordEditorInfor(string path,bool isSelect,bool isFile)
        {
            m_AssetPath = path;
            m_IsSelect = isSelect;
            m_IsFileAsset = isFile;
        }

    }//记录需要打包的AssetBundle 信息


    public class BuildAssetBundleWindow : EditorWindow
    {
        public enum EditorBuildTarget
        {
            Win64,
            Android,
            iOS
        } //目标平台
     


        private EditorBuildTarget m_BuildTarget;
        private bool isSelectAll = true;  //是否选择所有的文件
        private bool isDeSelectAll = false; //全部非选择

        private static BuildAssetBundleWindow S_BuildAssetBundleWindow;
        private static List<BuildAssetRecordEditorInfor> m_AllResourcesTopPath = new List<BuildAssetRecordEditorInfor>(); //Resources 目录下顶层的文件名和目录名
   
        private Vector2 m_ScrollRect = Vector2.zero;

        [MenuItem("Tools/热更新/打包 AssetBundle资源")]
        private static void ShowBuildAssetBundleWin()
        {
            S_BuildAssetBundleWindow = EditorWindow.GetWindow<BuildAssetBundleWindow>("打包AssetBundle");
            S_BuildAssetBundleWindow.minSize = new Vector2(400, 400);
            S_BuildAssetBundleWindow.maxSize = new Vector2(400, 600);
            S_BuildAssetBundleWindow.Show();
            GetAllShowPaths();
        }


        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);

            m_BuildTarget=(EditorBuildTarget) EditorGUILayout.EnumPopup("打包目标平台", m_BuildTarget,GUILayout.Width(350));

            GUILayout.Label("需要打包的资源目录:");
            GUILayout.Space(5);

            GUILayout.Label("LuaAsset目录不需要被打包 ，而是生成LuaAsset目录:");
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            isSelectAll = GUILayout.Toggle(isSelectAll, new GUIContent("全部选中"));
            if (isSelectAll)
                isDeSelectAll = false;


            isDeSelectAll = GUILayout.Toggle(isDeSelectAll, new GUIContent("全部非选中"));
            if (isDeSelectAll)
                isSelectAll = false;

            GUILayout.EndHorizontal();

          if (isSelectAll&& isDeSelectAll==false)
            {
                foreach (var item in m_AllResourcesTopPath)
                {
                    item.m_IsSelect = true;
                }
            }

            if (isSelectAll==false && isDeSelectAll )
            {
                foreach (var item in m_AllResourcesTopPath)
                {
                    item.m_IsSelect = false;
                }
            }


            m_ScrollRect = GUILayout.BeginScrollView(m_ScrollRect,true,true);
            for (int dex=0;dex< m_AllResourcesTopPath.Count;++dex)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.Width(20));
                bool isSelect = GUILayout.Toggle(m_AllResourcesTopPath[dex].m_IsSelect, new GUIContent(GetRelativePath(m_AllResourcesTopPath[dex].m_AssetPath)));
                if (isSelect != m_AllResourcesTopPath[dex].m_IsSelect)
                {
                    m_AllResourcesTopPath[dex].m_IsSelect = isSelect;
                    isSelectAll = isDeSelectAll = false;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.Space(15);

            if (GUILayout.Button("开始生成AssetBundle", GUILayout.Height(50)))
            {
                Debug.Log("开始打包AssetBundle ...............");
                switch (m_BuildTarget)
                {
                    case EditorBuildTarget.Android:
                        BuildAssetBundle_Tool.BegingPackAssetBundle(BuildTarget.Android, S_BuildAssetBundleWindow);
                        break;
                    case EditorBuildTarget.Win64:
                        BuildAssetBundle_Tool.BegingPackAssetBundle(BuildTarget.StandaloneWindows64, S_BuildAssetBundleWindow);
                        break;
                    case EditorBuildTarget.iOS:
                        BuildAssetBundle_Tool.BegingPackAssetBundle(BuildTarget.iOS, S_BuildAssetBundleWindow);
                        break;
                }
            }

            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// 遍历获取所有Resources 目录下的文件和目录
        /// </summary>
        private static void GetAllShowPaths()
        {
            m_AllResourcesTopPath.Clear();

            string[] directorys = System.IO.Directory.GetDirectories(Application.dataPath + "/Resources","*",SearchOption.TopDirectoryOnly);
            string[] files = System.IO.Directory.GetFiles(Application.dataPath + "/Resources", "*", SearchOption.TopDirectoryOnly);

           foreach (var directory in directorys)
           {
              //  Debug.Log("GetAllShowPaths directory=" + directory);

                BuildAssetRecordEditorInfor infor = new BuildAssetRecordEditorInfor(directory,true,false);
                m_AllResourcesTopPath.Add(infor);
           }

            foreach (var file in files)
            {
            //    Debug.Log("GetAllShowPaths file=" + file);
                if(System.IO.Path.GetExtension(file)!=".meta")
                {
                    BuildAssetRecordEditorInfor infor = new BuildAssetRecordEditorInfor(file, true,true);
                    m_AllResourcesTopPath.Add(infor);
                }
            }

        }

        /// <summary>
        /// 获取相对于Resources 下的目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetRelativePath(string path  )
        {
            if (string.IsNullOrEmpty(path)) return "";
            int index = path.IndexOf(@"Resources\");
            if (index!=-1)
            {
                return path.Substring(index );
            }
            return "";
        }


        /// <summary>
        /// 检测当前文件或者目录是否需要被打包
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isFile"></param>
        /// <returns></returns>
        public  bool CheckIfNeedPacked(string path,bool isFile)
        {
            if(System.IO.Path.GetExtension(path)==".meta")
            {
                path = path.Substring(0, path.Length - ".meta".Length);
            }

            for (int dex=0;dex< m_AllResourcesTopPath.Count;++dex)
            {
                if(m_AllResourcesTopPath[dex].m_AssetPath==path)
                {
                    return m_AllResourcesTopPath[dex].m_IsSelect && (m_AllResourcesTopPath[dex].m_IsFileAsset == isFile);
                }
            }

            Debug.LogError("CheckIfNeedPacked Fail,Not Exit Path " + path);
            return false;
        }



    }
}