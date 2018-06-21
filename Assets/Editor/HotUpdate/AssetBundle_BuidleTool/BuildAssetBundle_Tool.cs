using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MFramework.Re
{
    /// <summary>
    /// 打包Resource 目录下BuildAssetBundlePath 目录中的资源(如果不存在需要创建一个目录)
    /// </summary>
    public class BuildAssetBundle_Tool : Editor
    {
      private  static BuildTarget CurrentBuildTarget = BuildTarget.StandaloneWindows64;  //打包的资源平台

        private static string AssetBundleOutPath { get { return Application.streamingAssetsPath + "/" + GetPlatformPath(CurrentBuildTarget); } }  //AssetBundle 输出目录
        private static Dictionary<string, string> AllFile2AssetBunleRecord = new Dictionary<string, string>(); //Key :文件相对resource路径，value所属的AssetBundleName
        private static HotAssetRecordInfor S_HotAssetRecordInfor = new HotAssetRecordInfor();  //需要记录的资源AB信息

        private static BuildAssetBundleWindow S_BuildAssetBundleWindow;  //打包的自定义窗口


        public static void BegingPackAssetBundle(BuildTarget target, BuildAssetBundleWindow editorWin)
        {
            CurrentBuildTarget = target;
            S_BuildAssetBundleWindow = editorWin;
            PackAssetBundle();
        }

        /// <summary>
        /// 开始打包AssetBundle
        /// </summary>
      private  static void PackAssetBundle()
        {
            ClearAllPreviousAssetBundleName();
            S_HotAssetRecordInfor.m_AllAssetRecordsDic.Clear();  //清理本地记录的数据

            GetAndSetNeedPackAssetName(Application.dataPath + @"/Resources\");
            BuildAssetBundle();  //生成AssetBundle

            CreateAssetBundleDepends(); //创建依赖关系字典
            SaveAllDepdenceToLocalFile();

            AssetDatabase.Refresh();
            if (S_BuildAssetBundleWindow != null)
                S_BuildAssetBundleWindow.Close();
        }

        /// <summary>
        /// 根据打包平台获取不同的资源路径
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static string GetPlatformPath(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Window";
                case BuildTarget.iOS:
                    return "IOS";
                default:
                    Debug.LogError("UnIdentify Platform " + target);
                    return "UnKnow";
            }
        }

        /// <summary>
        /// 清理所有资源的AssetBundle 名,避免打包不必要的资源
        /// </summary>
        private static void ClearAllPreviousAssetBundleName()
        {
            string[] previousAssetName = AssetDatabase.GetAllAssetBundleNames();  //获得当前所有设置AssetBundle 名
            if (previousAssetName == null || previousAssetName.Length == 0) return;
            // Debug.Log("Before .. " + previousAssetName.Length);
            for (int dex = 0; dex < previousAssetName.Length; ++dex)
                AssetDatabase.RemoveAssetBundleName(previousAssetName[dex], true);

            //Debug.Log("End  .. " + AssetDatabase.GetAllAssetBundleNames().Length);
        }

        #region  获取需要打包的文件和目录
        /// <summary>
        /// 获取Resources 下所有的文件夹和文件 并且检测是否需要打包
        /// </summary>
        /// <param name="resourcePath"></param>
        private static void GetAndSetNeedPackAssetName(string resourcePath)
        {
            string[] directorys = System.IO.Directory.GetDirectories(resourcePath, "*", SearchOption.TopDirectoryOnly);
            foreach (var directory in directorys)
            {
                if (S_BuildAssetBundleWindow.CheckIfNeedPacked(directory, false))
                    SearchAllSubDirectorys(directory);
            }

            string[] files = System.IO.Directory.GetFiles(resourcePath, "*", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                if (S_BuildAssetBundleWindow.CheckIfNeedPacked(file, true))
                    SetAssetBundleNameByPath(file);
            }
        }

        /// <summary>
        /// 获得所有的文件并设置AssetBundle 名
        /// </summary>
        private static void SearchAllSubDirectorys(string resourcePath)
        {
            Debug.Log("GetAllFileAndSetAssetBundleName Path:  " + resourcePath);
            string[] _SubDire = Directory.GetDirectories(resourcePath); //获得所有的子文件夹
            foreach (var item in _SubDire)
                SearchAllSubDirectorys(item);

            string[] _ContainFiles = Directory.GetFiles(resourcePath);  //获得所有的文件
            foreach (var file in _ContainFiles)
            {
                SetAssetBundleNameByPath(file);
            }

        }

        #endregion





        /// <summary>
        /// 根据文件路径设置AssetBundle Name
        /// </summary>
        /// <param name="filePath"></param>
        private static void SetAssetBundleNameByPath(string filePath)
        {
            if (Path.GetExtension(filePath) == ".meta")
            {
                // Debug.Log(".meta 文件跳过 ");
                return;
            }

            string fileAssetPath = filePath.Substring(filePath.IndexOf("Assets/")); //相对于Assets目录 方便AssetImporter 使用
            string filePathRelativeResource = fileAssetPath.Substring((@"Assets/Resources\").Length);//相对于Resource的路径
            Debug.Log("fileAssetPath=" + fileAssetPath);
            Debug.Log("filePathRelativeResource=" + filePathRelativeResource);

            //相对于Resource路径下不带扩展名的文件名
            string filePathRelativeResourceWithNoExtension = filePathRelativeResource.Substring(0, filePathRelativeResource.IndexOf(Path.GetExtension(filePathRelativeResource)));
            Debug.Log("filePathRelativeResourceWithNoExtension=" + filePathRelativeResourceWithNoExtension);
            string assetName = "";
            string extensionName = Path.GetExtension(filePath);
            if (extensionName == ".prefab" || extensionName == ".unity" || extensionName == ".asset")
            { //Prefab 和 Scene 文件单独打包成AssetBundle
                assetName = filePathRelativeResourceWithNoExtension.Replace(@"\", "/") + ConstDefine.ABundleObjExtensitonName;
            }
            else
            {  //其他文件按照目录设置文件名 打包到一个AssetBundle
                string fileDirec = Path.GetDirectoryName(filePathRelativeResourceWithNoExtension);  //当前文件的目录
                string _fileName = Path.GetFileName(fileDirec); //获得当前目录的名字
                assetName = (fileDirec + "/" + _fileName).Replace(@"\", "/") + ConstDefine.ABundleObjExtensitonName;  //以目录+"/"+最后一个目录 为AssetBundleName
            }
            Debug.Log("当前文件夹路径是 " + filePath + " 当前文件AssetName= " + assetName);

            //AssetImporter.GetAtPath()  获取 Assets目录下资源必须带后缀名
            AssetImporter _impoter = AssetImporter.GetAtPath(fileAssetPath);   //**** Assets/Resources/AssetBundle_Path/Obj/obj1.prefab
            _impoter.assetBundleName = assetName.ToLower();

            string fileAssetRecordPath = filePathRelativeResource.Replace(@"\", "/");
            string fileAssetBundleRecordName = assetName.ToLower();
            if (AllFile2AssetBunleRecord.ContainsKey(fileAssetRecordPath))
            {
                Debug.LogError("重复的资源路径" + filePathRelativeResource);
                return;
            }
            AllFile2AssetBunleRecord.Add(fileAssetRecordPath, fileAssetBundleRecordName); //记录资源路径到AssetBundle路径
        }
        /// <summary>
        /// 生成AssetBundle
        /// </summary>
        private static void BuildAssetBundle()
        {
            CleanOutPutPath();
            BuildPipeline.BuildAssetBundles(AssetBundleOutPath, BuildAssetBundleOptions.DeterministicAssetBundle, CurrentBuildTarget);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 清理输出目录
        /// </summary>
        private static void CleanOutPutPath()
        {
            if (Directory.Exists(AssetBundleOutPath))
                Directory.Delete(AssetBundleOutPath, true);

            Directory.CreateDirectory(AssetBundleOutPath);
        }

        /// <summary>
        /// 创建依赖关系
        /// </summary>
        private static void CreateAssetBundleDepends()
        {
            AssetBundle mainAssetBundle = AssetBundle.LoadFromFile(AssetBundleOutPath + "/" + GetPlatformPath(CurrentBuildTarget));
            AssetBundleManifest mainFest = mainAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest"); //获得主AssetBundleManifest

            RecordPackAbundleMainifestInfor(GetPlatformPath(CurrentBuildTarget));  //不同平台的打包主AssetBundle
            RecordPackAbundleMainifestInfor(GetPlatformPath(CurrentBuildTarget) + ".manifest");  // AssetBundleManifest

            #region 遍历记录所有的AssetBundle
            string[] allAssets = mainFest.GetAllAssetBundles();//获得所有的AssetBundleName
            foreach (var item in allAssets)
            {
                string[] depences = mainFest.GetAllDependencies(item); //获得当前资源的所有依赖关系
                string path = Application.streamingAssetsPath + "/" + GetPlatformPath(CurrentBuildTarget) + "/" + item;  //获取打包后的资源AssetBundle 绝对路径

                #region 记录当前AssetBundle 信息
                HotAssetInfor_AssetBundle _infor = new HotAssetInfor_AssetBundle();
                _infor.m_MD5Code = Sercurity.MD5Helper.GetFileMD5(path);
                System.IO.FileInfo fileInfor = new System.IO.FileInfo(path);
                _infor.m_ByteSize = (int)fileInfor.Length;
                _infor.m_Dependece.AddRange(depences);
                if (S_HotAssetRecordInfor.m_AllAssetRecordsDic.ContainsKey(item))
                {
                    Debug.LogError("重复的AssetBundleName=" + item);
                    break;
                }
                S_HotAssetRecordInfor.m_AllAssetRecordsDic.Add(item, _infor);  //记录当前的 AssetBundle 资源
                #endregion

                #region 记录当前文件 .meta信息
                HotAssetInfor_AssetBundle _metaInfor = new HotAssetInfor_AssetBundle();
                _metaInfor.m_MD5Code = Sercurity.MD5Helper.GetFileMD5(path + ".meta");
                FileInfo mataFileInfor = new System.IO.FileInfo(path + ".meta");
                _metaInfor.m_ByteSize = (int)mataFileInfor.Length;
                if (S_HotAssetRecordInfor.m_AllAssetRecordsDic.ContainsKey(item + ".meta"))
                {
                    Debug.LogError("重复的AssetBundleName=" + item + ".meta");
                    break;
                }
                S_HotAssetRecordInfor.m_AllAssetRecordsDic.Add(item + ".meta", _metaInfor);  //记录当前的 AssetBundle 资源
                #endregion

            }
            #endregion

            mainAssetBundle.Unload(true);//卸载所有的 AssetBundle 资源
        }

        /// <summary>
        /// 记录不同平台打包下生成的主 mainAssetBundle 和 mainFest 信息
        /// </summary>
        /// <param name="fileName"></param>
        private static void RecordPackAbundleMainifestInfor(string fileName)
        {
            string PlatformABundlePath = AssetBundleOutPath + "/" + fileName;

            //当AssetBundle
            HotAssetInfor_AssetBundle _infor = new HotAssetInfor_AssetBundle();
            System.IO.FileInfo fileInfor = new System.IO.FileInfo(PlatformABundlePath);
            _infor.m_ByteSize = (int)fileInfor.Length;
            _infor.m_MD5Code = Sercurity.MD5Helper.GetFileMD5(PlatformABundlePath);
            S_HotAssetRecordInfor.m_AllAssetRecordsDic.Add(fileName, _infor);


            //.meta
            HotAssetInfor_AssetBundle _metaInfor = new HotAssetInfor_AssetBundle();
            System.IO.FileInfo metaFileInfor = new System.IO.FileInfo(PlatformABundlePath + ".meta");
            _metaInfor.m_ByteSize = (int)metaFileInfor.Length;
            _metaInfor.m_MD5Code = Sercurity.MD5Helper.GetFileMD5(PlatformABundlePath + ".meta");
            S_HotAssetRecordInfor.m_AllAssetRecordsDic.Add(fileName + ".meta", _metaInfor);
        }


        /// <summary>
        /// 保存到本地
        /// </summary>
        private static void SaveAllDepdenceToLocalFile()
        {
            string msg = LitJson.JsonMapper.ToJson(S_HotAssetRecordInfor);
            string configRecordPath = Application.streamingAssetsPath + "/" + GetPlatformPath(CurrentBuildTarget) + ConstDefine.ABundleConfigFileName;
            if (System.IO.File.Exists(configRecordPath))
            {
                System.IO.File.Delete(configRecordPath);
            }
            System.IO.File.WriteAllText(configRecordPath, msg);

        }





    }
}