using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MFramework.MultiLanguage
{
    /// <summary>
    /// Text组件本地化脚本
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    public class UITextLocalization : MonoBehaviour
    {
        /// <summary>
        /// 文本配置
        /// </summary>
        [System.Serializable]
        public class TextConfig
        {
            [Header("语言名称")]
            public string languageName;
            [Header("语言类型")]
            public LocalizationManager.LanguageType languageType;
            [Header("文本字体属性")]
            public TextFontProperty fontProperty;
        }


        /// <summary>
        /// Text字体属性
        /// </summary>
        [System.Serializable]
        public class TextFontProperty
        {
            public TMPro.TMP_FontAsset font;
            public Material fontSharedMaterial;
            public TMPro.FontStyles fontStyle;
            public Color color;
            public TMPro.VertexGradient colorGradient;
            public float fontSize;
            public TMPro.TextAlignmentOptions alignment;
            public bool enableWordWrapping;
            public TMPro.TextOverflowModes overflowMode;
        }

        #region 变量属性
        [Header("文本翻译的关键字")]
        public string key;
        [Header("文本翻译的配置文件名称")]
        public string fileName;
        [Header("当前语言类型")]
        public LocalizationManager.LanguageType currentLanguageType;
        [Header("是否启用字体属性修改")]
        public bool enableFontProperty;
        [Header("文本多语言配置")]
        public List<TextConfig> textConfigs = new List<TextConfig>();

        private LocalizationManager.LanguageType m_lastLanguageType;
        #endregion

        #region 编辑器操作
#if UNITY_EDITOR
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            GenConfigKey();
            currentLanguageType = LocalizationManager.LanguageType.cn;
            m_lastLanguageType = currentLanguageType;

            textConfigs.Clear();
            foreach (var obj in System.Enum.GetValues(typeof(LocalizationManager.LanguageType)))
            {
                LocalizationManager.LanguageType languageType = (LocalizationManager.LanguageType)obj;
                TextConfig textConfig = new TextConfig();
                textConfig.languageName = languageType.ToString();
                textConfig.languageType = languageType;
                textConfigs.Add(textConfig);
            }
        }

        private void OnValidate()
        {
            if (m_lastLanguageType == currentLanguageType)
                return;
            InitView(currentLanguageType);
            m_lastLanguageType = currentLanguageType;
        }

        public void ChangeLanguageType(LocalizationManager.LanguageType languageType)
        {
            currentLanguageType = languageType;
            OnValidate();
        }

        /// <summary>
        /// 生成配置文件关键字
        /// </summary>
        private void GenConfigKey()
        {
            UITextLocalizationManager textLocalizationMgr = gameObject.GetComponentInParent<UITextLocalizationManager>();
            Transform tfRoot = textLocalizationMgr == null ? transform.root : textLocalizationMgr.transform;
            //fileName = tfRoot.name;

            key = transform.name;
            Transform tfParent = transform.parent;
            while (tfParent != null && tfParent != tfRoot.parent)
            {
                key = tfParent.name + "/" + key;
                tfParent = tfParent.parent;
            }
        }

        /// <summary>
        /// 保存视图
        /// </summary>
        private void SaveView(LocalizationManager.LanguageType languageType)
        {
            TextConfig textConfig = textConfigs.FirstOrDefault(x => x.languageType == languageType);
            if (textConfig == null)
                return;

            TextFontProperty fontProperty = textConfig.fontProperty;
            if (enableFontProperty)
            {
                TMPro.TextMeshProUGUI text = GetComponent<TMPro.TextMeshProUGUI>();
                fontProperty.font = text.font;
                fontProperty.fontSharedMaterial = text.fontSharedMaterial;
                fontProperty.fontStyle = text.fontStyle;
                fontProperty.color = text.color;
                fontProperty.colorGradient = text.colorGradient;
                fontProperty.fontSize = text.fontSize;
                fontProperty.alignment = text.alignment;
                fontProperty.enableWordWrapping = text.enableWordWrapping;
                fontProperty.overflowMode = text.overflowMode;
            }
        }

        [MenuItem("CONTEXT/UITextLocalization/保存当前视图")]
        /// <summary>
        /// 保存当前视图
        /// </summary>
        private static void SaveCurrentView(UnityEditor.MenuCommand cmd)
        {
            UITextLocalization current = cmd.context as UITextLocalization;
            if (current != null)
            {
                current.SaveView(current.currentLanguageType);
            }
        }
#endif
        #endregion

        #region 视图操作
        private void Awake()
        {
            InitView(LocalizationManager.GetInstance().CurrentLanguage);
        }

        /// <summary>
        /// 初始化视图
        /// </summary>
        private void InitView(LocalizationManager.LanguageType languageType)
        {
            TMPro.TextMeshProUGUI text = GetComponent<TMPro.TextMeshProUGUI>();
           text.text = LocalizationManager.GetInstance().GetLocalTextString("UIStatic/" + fileName, key, languageType);

            TextConfig textConfig = textConfigs.FirstOrDefault(x => x.languageType == languageType);
            if (textConfig == null)
                return;

            TextFontProperty fontProperty = textConfig.fontProperty;
            if (enableFontProperty)
            {
                text.font = fontProperty.font;
                text.fontSharedMaterial = fontProperty.fontSharedMaterial;
                text.fontStyle = fontProperty.fontStyle;
                text.color = fontProperty.color;
                text.colorGradient = fontProperty.colorGradient;
                text.fontSize = fontProperty.fontSize;
                text.alignment = fontProperty.alignment;
                text.enableWordWrapping = fontProperty.enableWordWrapping;
                text.overflowMode = fontProperty.overflowMode;
            }
        }
        #endregion
    }
}