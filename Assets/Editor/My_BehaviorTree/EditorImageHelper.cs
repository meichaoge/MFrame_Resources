using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
public class EditorImageHelper
{

    private static Dictionary<string, Texture> m_AllNeedImags = new Dictionary<string, Texture>();
    private static Texture m_DefaultImage;
    protected static Texture DefaultImage
    {
        get
        {
            if (m_DefaultImage == null)
                m_DefaultImage = GetImageByPath("Assets/Editor/EditorResources/ActionNodeItembg.png");
            return m_DefaultImage;
        }
    } //默认图片



    /// <summary>
    /// 根据路径加载图片
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Texture GetImageByPath(string path)
    {
        Texture texture = null;
        if(m_AllNeedImags.TryGetValue(path,out texture))
        {
            if (texture != null) return texture;
        }

        texture= AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture;
        if(texture==null)
        {
            Debug.Log("GetImageByPath Fail,Image Not Exit" + path);
            return DefaultImage;
        }
        return texture;
    }

}
#endif