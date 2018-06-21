using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace MFramework.EditorExpand
{
    /// <summary>
    /// 监听Unity 打包前后的事件
    /// </summary>
    public class ListenerBuildProject
    {
        private static bool isBeforeBuildFlag = false;
        public static System.Action OnBeforeBuildPlayerEvent = null;
        public static System.Action<BuildTarget, string> OnPostBuildPlayerEvent = null;


        [PostProcessScene]
        private static void OnProcessScene()
        {
            if (!isBeforeBuildFlag && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.LogInfor("Unity标准Build 前处理函数  .. OnProcessScene");

                isBeforeBuildFlag = true;

                if (OnBeforeBuildPlayerEvent != null)
                    OnBeforeBuildPlayerEvent();
            }
        }

        /// <summary>
        /// Unity标准Build后处理函数
        /// </summary>
        [PostProcessBuild()]
        private static void OnPostBuildPlayer(BuildTarget target, string pathToBuiltProject)
        {
            Debug.LogInfor("Unity标准Build后处理函数  .. OnPostBuildPlayer");
            if (OnPostBuildPlayerEvent != null)
            {
                OnPostBuildPlayerEvent(target, pathToBuiltProject);
            }

            UnityEngine.Debug.Log(string.Format("Success build ({0}) : {1}", target, pathToBuiltProject));
        }

    }
}