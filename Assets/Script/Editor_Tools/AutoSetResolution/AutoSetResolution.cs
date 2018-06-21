using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 自动根据屏幕分辨率设置适配的分辨率(测试貌似没用)
    /// </summary>
    public class AutoSetResolution : MonoBehaviour
    {
        private int scaleWidth = 0;
        private int scaleHeight = 0;
        public void setDesignContentScale()
        {
#if UNITY_ANDROID
		if(scaleWidth ==0 && scaleHeight ==0)
		{
			int width = Screen.currentResolution.width;
			int height = Screen.currentResolution.height;
			int designWidth = 960;
			int designHeight = 640;
			float s1 = (float)designWidth / (float)designHeight;
			float s2 = (float)width / (float)height;
			if(s1 < s2) {
				designWidth = (int)Mathf.FloorToInt(designHeight * s2);
			} else if(s1 > s2) {
				designHeight = (int)Mathf.FloorToInt(designWidth / s2);
			}
			float contentScale = (float)designWidth/(float)width;
			if(contentScale < 1.0f) { 
				scaleWidth = designWidth;
				scaleHeight = designHeight;
			}
		}
		if(scaleWidth >0 && scaleHeight >0)
		{
			if(scaleWidth % 2 == 0) {
				scaleWidth += 1;
			} else {
				scaleWidth -= 1;					
			}
			Screen.SetResolution(scaleWidth,scaleHeight,true);
		}
#endif
        }

        void OnApplicationPause(bool paused)
        {
            if (paused == false)
            {
                setDesignContentScale();
            }
        }

    }
}