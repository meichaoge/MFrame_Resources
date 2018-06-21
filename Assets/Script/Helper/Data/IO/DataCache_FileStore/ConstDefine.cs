using UnityEngine;
using System.Collections;

namespace MFramework
{

    public class ConstDefine
    {
        public const string UIPanelResourcePath = "Prefabs/UI/Panel/"; //UIPrefab �����ResourceĿ¼
        public const string UIMsgBoxResourcePath = "Prefabs/UI/MessageBox/"; //MessageBox �����ResourceĿ¼
                                                                             // public const string PictureStorePath = "/storage/emulated/0/DCIM/";  //�����ļ������
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
        } //Android �ֻ�Sd����Ŀ¼  �������Ŀ¼m_AndroidSDRootPath+"DCIM/"

        public static string TextureCachePath
        {
            get
            {
                return Application.persistentDataPath + "/beanvr/Cache/";
            }
        }//Ӧ�õ�ͼƬ�����ַ ��Ӧ�ý���ʱ���

        #region  AssetBundle 
        #region ���AssetBundle ����
        public static string ABundleObjExtensitonName = ".unity3d"; //AB����չ��

        #endregion

        public static string ABundleTopPath
        {
            get
            {
                return Application.persistentDataPath + "/ABundle/";
            }
        }//���й���AssetBundle ��Դ��·�� �������ص���Bundle �Լ�����

        public const string ABundleConfigFileName = "_bundleInfor.txt";  //ABundle ������Ϣ���ļ��� ǰ�滹��Ҫƽ̨��
        public const string ABundleConfigFileNameWithOutExtension = "_bundleInfor";  //ABundle ������Ϣ���ļ��� ǰ�滹��Ҫƽ̨��
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
        }  //��ͬƽ̨�¶�ӦABundle ������ԴĿ¼��
        public static string AssetBundleConfigurePath
        {
            get
            {
                return ABundleTopFileNameOfPlatformat + ABundleConfigFileNameWithOutExtension;
            }
        }// ��������Դ�����ļ���

        #endregion

        #region Xlua
        public static string LuaAssetPackPath = "/LuaAsset"; //��Ҫ��¼�����lua Asset Path   (�����Application.dataPath)
        public static string LuaConfigureFileName = "XLuaAssetConfigure"; //lua�����ļ��� �����ɵ�Editor �ű��п��ܱ����ĸ�
        public static string XluaAssetTopPath
        {
            get
            {
                return Application.persistentDataPath + "/LuaAsset/";
            }
        }//���й��� Lua �ı� ��Դ��·�� �������ص��� Lua �Լ�����

        public static string LuaConfigResourcePath = "Configure/Lua/";  //���ɵ�lua �����ļ�Resource �����Ŀ¼

        #endregion

        #region Editor UI
#if UNITY_EDITOR
        public const string UIEdirtorViewTemplatePath = "Editor/Template/UIView.tpl.txt";  //�༭��View ģ���ļ�
#endif
        #endregion


        #region Data
        public const float UIOpenTweenTime = 0.2f; //The time for tween Open
        public const float UICloseTweenTime = 0.1f; //The time For Tween Close
        public const float UICloseTweenTimeDelate = 0.01f; //The time For Tween Close More

        #endregion

        #region LocalStore
        public const string LocalLanguageConfg = "LocalLanguageConfg";  //������������
        #endregion


        #region Local Account
        public const string LocalAccountStorePath = "/beanvr/cfg_data/";
        public const string LocalAccountStoreFileName = "localuserInfor.txt";
        #endregion


        #region Configure FileName

        public const string ConfigurePath = "Confg/";  //�����ļ���Ŀ¼

        public const string SceneDynamicConfigure = "SceneDynamicGenerationConfg"; //�����ж�̬������Ϣ�ļ���
        public const string UIPanelConfigure = "UIPanelConfg";  //UI��������ļ�
        public const string SceneFuctionConfigure = "SceneFunCfg"; //�����й������õ��ļ�
        #endregion



    }
}
