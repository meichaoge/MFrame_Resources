﻿using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : EditorMonoBehaviour
{
    public override void Update()
    {
        //Debug.Log ("每一帧回调一次");
    }

    public override void OnPlaymodeStateChanged(PlayModeState playModeState)
    {
       // Debug.Log("游戏运行模式发生改变， 点击 运行游戏 或者暂停游戏或者 帧运行游戏 按钮时触发: " + playModeState);
    }

    public override void OnGlobalEventHandler(Event e)
    {
        //Debug.Log ("全局事件回调: " + e);
    }

    public override void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        //	Debug.Log (string.Format ("{0} : {1} - {2}", EditorUtility.InstanceIDToObject (instanceID), instanceID, selectionRect));
    }

    public override void OnHierarchyWindowChanged()
    {
        //Debug.Log("层次视图发生变化");
    }

    public override void OnModifierKeysChanged()
    {
        //	Debug.Log ("当触发键盘事件");
    }

    public override void OnProjectWindowChanged()
    {
        //	Debug.Log ("当资源视图发生变化");
    }

    public override void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
    {
        //根据GUID得到资源的准确路径
        //Debug.Log(string.Format("{0} : {1} - {2}", AssetDatabase.GUIDToAssetPath(guid), guid, selectionRect));
    }
}