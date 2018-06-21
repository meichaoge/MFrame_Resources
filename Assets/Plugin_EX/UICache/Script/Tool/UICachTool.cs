using UnityEngine;

public class UICachTool
{
#if UNITY_EDITOR
    /// <summary>
    /// 获取预制体路径
    /// </summary>
    /// <returns></returns>
    public static string GetPrefabPath(GameObject gameObject)
    {
        if (gameObject == null)
            return "";

        string path = UnityEditor.AssetDatabase.GetAssetPath(gameObject);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("请关联Project 中 Resource 路径下的缓存UI预制体  ::" + gameObject.name);
            return "";
        }
        return GetPrefabRelativePath(path, "Resources/");
    }
#endif

    /// <summary>
    /// 获取预制体相对路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parentPath">相对于哪一个目录</param>
    /// <returns></returns>
    public static string GetPrefabRelativePath(string path, string parentPath = "Resources/")
    {
        int index = path.IndexOf(parentPath);
        if (index != -1)
            path = path.Substring(index + parentPath.Length);
        index = path.IndexOf(".prefab");
        if (index != -1)
        {
            path = path.Substring(0, path.Length - ".prefab".Length);
        }
        else
        {
            Debug.LogError("GetPrefabRelativePath 当前资源不是预制体");
        }
        return path;
    }
}
