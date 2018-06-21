using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Editor_ApplicationPause : MonoBehaviour
{
    public bool IsOn = true;

#if UNITY_EDITOR

    public bool DoTest = true;
    public static Editor_ApplicationPause Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && IsOn)
        {
            EditorApplication.isPaused = true;
        }

        //if (Input.GetKeyDown(KeyCode.R) && IsOn)
        //{
        //    if (EditorApplication.isPaused)
        //        EditorApplication.isPaused = false;

        //    EditorApplication.isPlaying = true;

        //}
    }
#endif
}

