using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MFramework
{
    /// <summary>
    /// 处理摄像机层的帮助类
    /// Helper Class To Set Camera Layer
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraLayerHelper : MonoBehaviour
    {
        Dictionary<string, bool> allSettingLayer = new Dictionary<string, bool>();

        public void SettingCullingMask(Camera operateCamera, string _operateLayerStr, bool _isHide = true)
        {
            int _currentOperateLayer = 0;
            int _layerCout = 0;
            #region  处理三个特殊层 Nothing=0   Everything=-1  Default=1
            if (_operateLayerStr == "Nothing")  //" Everything "的值是0 
            { //Nothing 层其他的层都是不可见的
                operateCamera.cullingMask = 0;
                allSettingLayer.Clear();
            }//if
            else if (_operateLayerStr == "Everything")  //" Everything "的值是-1
            {//Everything 所有层可用
                allSettingLayer.Clear();
                if (_isHide)
                    operateCamera.cullingMask = 0; //隐藏所有层
                else
                    operateCamera.cullingMask = -1;  //显示所有层
                return;
            }
            else
            {
                bool state = false;
                if (allSettingLayer.TryGetValue(_operateLayerStr, out state))
                {
                    if (state == _isHide) return;  //已经设置过
                }
                allSettingLayer[_operateLayerStr] = _isHide;
                if (_operateLayerStr == "Default")  //" Default "的值是1 
                    _layerCout = 1;
                else
                {
                    _currentOperateLayer = LayerMask.NameToLayer(_operateLayerStr);
                    if (_currentOperateLayer == -1)
                        return; //-1值表示当前的层没有找到
                    _layerCout = Mathf.RoundToInt(Mathf.Pow(2, _currentOperateLayer));
                }
                if (_isHide)
                    operateCamera.cullingMask -= _layerCout;//隐藏 default 层 cullingMask -1
                else
                    operateCamera.cullingMask += _layerCout;
            }
            #endregion
        }

        private void OnDestroy()
        {
            allSettingLayer.Clear();
        }

    }
}
