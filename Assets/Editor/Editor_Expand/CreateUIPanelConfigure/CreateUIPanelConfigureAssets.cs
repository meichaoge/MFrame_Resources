using BeanVR;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;



namespace MFramework.EditorExpand
{

    /// <summary>
    /// 创建UI面板的配置文件
    /// </summary>
    public class CreateUIPanelConfigureAssets
    {

        [MenuItem("Tools/UI/Create AllPanelConfg/UIPanel Asset")]
        public static void CreatePanelConfg()
        {
            ScriptableObjectUtility.CreateUnityAsset<UIPanelConfigure>();
        }


        [MenuItem("Tools/UI/Create AllPanelConfg/UIPanel JsonText")]
        public static void CreatePanelConfgText()
        {
            string path = EditorDialogUtility.OpenFileDialog("GetUIPanelConfigure", "", "");
            Debug.Log("SourcePath  ... " + path);
            path = path.Substring(path.IndexOf("Assets"));
            Debug.Log("RelativePath   ... " + path.Substring(path.IndexOf("Assets")));

            UIPanelConfigure bullet = AssetDatabase.LoadAssetAtPath<UIPanelConfigure>(path);
            string msg = JsonParser.Serialize(bullet.AllUIPanelConfigure);
            Debug.Log(msg);

            //string savePath = EditorDialogUtility.SaveFileDialog("保存Json文本", "", "UIPanelConfg", "txt");
            //Debug.Log("CreatePanelConfgText >>savePath= " + savePath);
            //if (File.Exists(savePath) == false)
            //    File.Create(savePath);
            //File.WriteAllText(savePath, msg);
            File.WriteAllText(Application.dataPath + "/"+ConstDefine.UIPanelConfigure+ ".txt", msg);

            AssetDatabase.Refresh();
        }
    }


    public class UIPanelConfigure : ScriptableObject
    {
        public List<UIPanelConfgInfor> AllUIPanelConfigure = new List<UIPanelConfgInfor>();
    }





}