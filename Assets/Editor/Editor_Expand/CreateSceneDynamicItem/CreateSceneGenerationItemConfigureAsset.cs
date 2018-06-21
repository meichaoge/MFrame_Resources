using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BeanVR;
using System.IO;
using Newtonsoft.Json;

namespace MFramework.EditorExpand
{

    public class CreateSceneGenerationItemConfigureAsset
    {
        [MenuItem("Tools/UI/Create SceneGeneration/ SceneGeneration Asset")]
        public static void CreateSceneItemConfg()
        {
            ScriptableObjectUtility.CreateUnityAsset<SceneItemGenarationInfor_Total>();
        }

        [MenuItem("Tools/UI/Create SceneGeneration/SceneGeneration JsonText")]
        public static void CreateSceneItemConfg_Json()
        {
            string path = EditorDialogUtility.OpenFileDialog("Get AllSceneGeneration Items Confg Asset", "", "");
            Debug.Log("SourcePath  ... " + path);
            path = path.Substring(path.IndexOf("Assets"));
            Debug.Log("RelativePath   ... " + path.Substring(path.IndexOf("Assets")));

            SceneItemGenarationInfor_Total bullet = AssetDatabase.LoadAssetAtPath<SceneItemGenarationInfor_Total>(path);
            Debug.Log(bullet);
            string msg = JsonParser.Serialize(bullet.TotalConfgForAllScene);
            Debug.Log(msg);

            //string savePath = EditorDialogUtility.SaveFileDialog("±£´æJsonÎÄ±¾", "", "UIPanelConfg", "txt");
            //Debug.Log("CreatePanelConfgText >>savePath= " + savePath);
            //if (File.Exists(savePath) == false)
            //    File.Create(savePath);
            //File.WriteAllText(savePath, msg);
            File.WriteAllText(Application.dataPath + "/"+ConstDefine.SceneDynamicConfigure+".txt", msg);

            AssetDatabase.Refresh();
        }



    }


    public class SceneItemGenarationInfor_Total : ScriptableObject
    {
        public List<SceneItemGenarationInfor> TotalConfgForAllScene = new List<SceneItemGenarationInfor>();
    }
}
