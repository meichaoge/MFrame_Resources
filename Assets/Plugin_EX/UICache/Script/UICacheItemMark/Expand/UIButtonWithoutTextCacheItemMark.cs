using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button属性设置
/// </summary>
[System.Serializable]
public class ButtonSetting
{
    //是否启用ButtonEffect 脚本
    public bool m_IsBtnEffectEnable = false;
    //图片格式
    public Selectable.Transition m_Transition = Selectable.Transition.None;
    //置灰图片路径，用于SpriteSwap模式
    public string m_DisabledSpritePath;
#if UNITY_EDITOR
    //置灰图片预制体
    public GameObject m_DisabledSpritePrefab;
#endif
}

/// <summary>
/// 挂载在需要一个无Text 的Button 上
/// </summary>
public class UIButtonWithoutTextCacheItemMark : UIImageCacheItemMark
{
    public ButtonSetting m_ButtonSetting;
    protected ButtonEffect m_ButtonEffect;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("CONTEXT/UIButtonWithoutTextCacheItemMark/选择图片")]
    static void SelectDisabledSprite(UnityEditor.MenuCommand cmd)
    {
        if (Application.isPlaying) return;
        UIButtonWithoutTextCacheItemMark current = cmd.context as UIButtonWithoutTextCacheItemMark;
        if (current != null)
        {
            string filePath = UnityEditor.EditorUtility.OpenFilePanel("选择图片", Application.dataPath + "/Resources/Sprite", "prefab");
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            string assetPath = filePath.Substring(filePath.IndexOf("Assets"));
            current.m_ButtonSetting.m_DisabledSpritePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            current.UpdateUICacheRefence();
        }
    }

    protected override void Reset()
    {
        base.Reset();
        //m_ImageSetting.m_RaycastTarget = true;
        m_ComponentNames = new string[1]
        {
            "Button",
        };
    }

    public override void UpdateUICacheRefence()
    {
        base.UpdateUICacheRefence();
        m_ButtonSetting.m_DisabledSpritePath = UICachTool.GetPrefabPath(m_ButtonSetting.m_DisabledSpritePrefab.gameObject);
    }
#endif

    public override void LoadCacheUIItem(UICacheMarkOperate operateEnum, string parameter)
    {
        base.LoadCacheUIItem(operateEnum, parameter);
        if (m_UICachePrefabScript == null)
            return;

        //设置Button的相关属性
        Button button = GetAttachComponent<Button>();
        if (button == null)
            return;

        //    Debug.Log("UIButtonWithoutTextCacheItemMark");
        m_ButtonEffect = m_UICachePrefabScript.transform.GetAddComponent<ButtonEffect>();
        m_ButtonEffect.enabled = m_ButtonSetting.m_IsBtnEffectEnable;

        button.transition = m_ButtonSetting.m_Transition;
        if (button.transition == Selectable.Transition.SpriteSwap)
        {
            SpriteState spriteState = button.spriteState;
            if (m_ButtonSetting.m_DisabledSpritePath != string.Empty)
            {  //2017/11/27注释掉
            //    spriteState.disabledSprite = ResourceMgr.instance.LoadSprite(m_ButtonSetting.m_DisabledSpritePath);
                button.spriteState = spriteState;
            }
        }
    }

    public override T GetAttachComponent<T>()
    {
        System.Type t = typeof(T);
        if (t == typeof(Image))
        {
            return GetMainAttachUI_ImageComponent() as T;
        }
        else if (t == typeof(Button))
        {
            return GetMainAttachUIComponent<Button>() as T;
        }
        return base.GetAttachComponent<T>();
    }



}
