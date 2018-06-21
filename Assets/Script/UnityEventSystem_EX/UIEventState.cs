using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MFramework.EventEX
{
    /// <summary>
    /// 判断鼠标或者手指点击在UI上
    /// </summary>
    public class UIEventState : MonoBehaviour
    {

        void Update()
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
#if IPHONE || ANDROID
			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
                if (EventSystem.current.IsPointerOverGameObject())
#endif
                    Debug.Log("当前触摸在UI上");

                else
                    Debug.Log("当前没有触摸在UI上");
            }
        }
    }
}