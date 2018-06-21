using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using LitJson;
using MFramework.EditorExpand;
using MFramework.Re;

namespace MFramework.Xlua
{
    public class GenerateXluaConfigure : EditorWindow
    {
      private  static GenerateXluaConfigure windows = null;
        private static HotAssetRecordInfor xluaConfig = new HotAssetRecordInfor();
        private static string xluaConfigureSavePath = "";  //生成的配置文件保存路径

        private static void GetAndShowWindow()
        {
            windows = EditorWindow.GetWindow<GenerateXluaConfigure>("Xlua Asset Pack");
            windows.minSize = new Vector2(500, 200);
            windows.maxSize = new Vector2(500, 200);
            windows.Show();
        }

        #region 编辑窗口工具

        [MenuItem("Tools/热更新/XLua/生成lua 资源记录文件")]
        private static void GenerateCofigure()
        {
            GetAndShowWindow();
            GetAllLuaAsset();
        }
        //[MenuItem("Tools/热更新/XLua/生成空的lua配置文件")]
        //static void GernerateEmptyConfigure()
        //{
        //    GetAndShowWindow();

        //    xluaConfig.m_AllAssetRecordsDic.Clear();
        //}
        #endregion


        private static void GetAllLuaAsset()
        {
            xluaConfig.m_AllAssetRecordsDic.Clear();
            string[] allFiles = Directory.GetFiles(Application.dataPath+"/Resources" + ConstDefine.LuaAssetPackPath, "*.txt", SearchOption.AllDirectories);
            string luaTopDirectory = ConstDefine.LuaAssetPackPath + @"\";
            foreach (var file in allFiles)
            {
                HotAssetInfor_Lua luaInfor = new HotAssetInfor_Lua();

                FileInfo infor = new FileInfo(file);
                luaInfor.m_ByteSize = (int)infor.Length;
               string filePath = file.Substring(file.IndexOf(ConstDefine.LuaAssetPackPath) + luaTopDirectory.Length).Replace('\\','/');  //这里必须把\处理一下 否则无法反序列化
                luaInfor.m_MD5Code= Sercurity.MD5Helper.GetFileMD5(file);
                Debug.Log("path=" + filePath + "    Size=" + luaInfor.m_ByteSize);
                xluaConfig.m_AllAssetRecordsDic.Add(filePath ,luaInfor);
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("lua 版本号:"), GUILayout.Width(100));
            xluaConfig.m_Version = GUILayout.TextField(xluaConfig.m_Version);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("配置文件保存路径:"), GUILayout.Width(100));
            xluaConfigureSavePath = GUILayout.TextField(xluaConfigureSavePath);
            if (GUILayout.Button(new GUIContent("选择保存路径"), GUILayout.Width(100)))
            {
                xluaConfigureSavePath = EditorDialogUtility.SaveFileDialog("保存Lua 配置", Application.dataPath+ "/Resources/Configure/Lua", ConstDefine.LuaConfigureFileName, "txt");
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            #region 生成配置文件按钮
            GUILayout.BeginHorizontal();
          //  if (GUILayout.Button("生成Lua 脚本配置文件",  GUILayout.Height(50),GUILayout.ExpandWidth(true)))
          if(GUI.Button(new Rect(125,100,250,50),new GUIContent("生成Lua 脚本资源信息记录文件")))
            {
                if (string.IsNullOrEmpty(xluaConfigureSavePath))
                {
                    Debug.LogError("选择保存路径之后再生成");
                    return;
                }


                string savePath = System.IO.Path.GetDirectoryName(xluaConfigureSavePath);
                string fileName = System.IO.Path.GetFileName(xluaConfigureSavePath);

                Debug.Log("savePath=" + savePath);
                if (System.IO.Directory.Exists(savePath) == false)
                    System.IO.Directory.CreateDirectory(savePath);

                if (System.IO.File.Exists(xluaConfigureSavePath))
                    System.IO.File.Delete(xluaConfigureSavePath);

                ConstDefine.LuaConfigureFileName = System.IO.Path.GetFileName(xluaConfigureSavePath);

                System.IO.File.WriteAllText(xluaConfigureSavePath, JsonMapper.ToJson(xluaConfig));
                Debug.Log("生成Lua 资源配置文件成功,保存路径 " + xluaConfigureSavePath);
                AssetDatabase.Refresh();
                if(windows!=null)
                    windows.Close();
            }
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.EndVertical();
        }



    }
}