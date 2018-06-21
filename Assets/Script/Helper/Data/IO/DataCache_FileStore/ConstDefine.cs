using UnityEngine;
using System.Collections;

namespace MFramework
{

    public class ConstDefine
    {
        public const string UIPanelResourcePath = "Prefabs/UI/Panel/"; //UIPrefab 相对于Resource目录
        public const string UIMsgBoxResourcePath = "Prefabs/UI/MessageBox/"; //MessageBox 相对于Resource目录
                                                                             // public const string PictureStorePath = "/storage/emulated/0/DCIM/";  //保存文件到相册
        private static string m_AndroidSDRootPath;
        public static string AndroidSDRootPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_AndroidSDRootPath))
                {
                    using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        AndroidJavaObject curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
                        m_AndroidSDRootPath = curActivity.Call<string>("getStoragePath") + "/";
                    }
                }
                return m_AndroidSDRootPath;
            }
        } //Android 手机Sd卡根目录  例如相册目录m_AndroidSDRootPath+"DCIM/"

        public static string TextureCachePath
        {
            get
            {
                return Application.persistentDataPath + "/beanvr/Cache/";
            }
        }//应用的图片缓存地址 在应用结束时清空

        #region  AssetBundle 
        #region 打包AssetBundle 设置
        public static string ABundleObjExtensitonName = ".unity3d"; //AB包扩展名

        #endregion

        public static string ABundleTopPath
        {
            get
            {
                return Application.persistentDataPath + "/ABundle/";
            }
        }//所有关于AssetBundle 资源的路径 包含下载的新Bundle 以及配置

        public const string ABundleConfigFileName = "_bundleInfor.txt";  //ABundle 配置信息的文件名 前面还需要平台名
        public const string ABundleConfigFileNameWithOutExtension = "_bundleInfor";  //ABundle 配置信息的文件名 前面还需要平台名
        public static string ABundleTopFileNameOfPlatformat
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                        return "Window";
                    case RuntimePlatform.Android:
                        return "Android";
                    case RuntimePlatform.IPhonePlayer:
                        return "IOS";
                    default:
                        Debug.LogError("Unknow PlatFormat " + Application.platform);
                        return "";
                }
            }
        }  //不同平台下对应ABundle 顶层资源目录名
        public static string AssetBundleConfigurePath
        {
            get
            {
                return ABundleTopFileNameOfPlatformat + ABundleConfigFileNameWithOutExtension;
            }
        }// 完整的资源配置文件名

        #endregion

        #region Xlua
        public static string LuaAssetPackPath = "/LuaAsset"; //需要记录打包的lua Asset Path   (相对于Application.dataPath)
        public static string LuaConfigureFileName = "XLuaAssetConfigure"; //lua配置文件名 在生成的Editor 脚本中可能被更改该
        public static string XluaAssetTopPath
        {
            get
            {
                return Application.persistentDataPath + "/LuaAsset/";
            }
        }//所有关于 Lua 文本 资源的路径 包含下载的新 Lua 以及配置

        public static string LuaConfigResourcePath = "Configure/Lua/";  //生成的lua 配置文件Resource 下相对目录

        #endregion

        #region Editor UI
#if UNITY_EDITOR
        public const string UIEdirtorViewTemplatePath = "Editor/Template/UIView.tpl.txt";  //编辑器View 模板文件
#endif
        #endregion


        #region Data
        public const float UIOpenTweenTime = 0.2f; //The time for tween Open
        public const float UICloseTweenTime = 0.1f; //The time For Tween Close
        public const float UICloseTweenTimeDelate = 0.01f; //The time For Tween Close More

        #endregion

        #region LocalStore
        public const string LocalLanguageConfg = "LocalLanguageConfg";  //本地语言配置
        #endregion


        #region Local Account
        public const string LocalAccountStorePath = "/beanvr/cfg_data/";
        public const string LocalAccountStoreFileName = "localuserInfor.txt";
        #endregion


        #region Configure FileName

        public const string ConfigurePath = "Confg/";  //配置文件根目录

        public const string SceneDynamicConfigure = "SceneDynamicGenerationConfg"; //场景中动态配置信息文件名
        public const string UIPanelConfigure = "UIPanelConfg";  //UI面板配置文件
        public const string SceneFuctionConfigure = "SceneFunCfg"; //场景中功能配置的文件
        #endregion



    }
}
