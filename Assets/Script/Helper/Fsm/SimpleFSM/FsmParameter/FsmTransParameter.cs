using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.FSM
{
    public class FsmTransParameter : IFSMTransParameter
    {
        public class  Parameter
        {
            public string ParameterName;
            public object Value;

            public Parameter(string _name,object _value)
            {
                ParameterName = _name;
                Value = _value;
            }

        }

        public IFSMController FsmController { private set; get; }
        public string FsmTransParameterName { private set; get; }

        private Dictionary<string, object> m_AllTransParameterDic = new Dictionary<string, object>();

        public FsmTransParameter(IFSMController _controller,string _name)
        {
            FsmTransParameterName = _name;
            if (_controller == null)
            {
                Debug.LogError("FSMTransParameter " + _name + " IFSMController Can't Be Null");
                return;
            }
            FsmController = _controller;

        }

        public bool AddParameter(string _name, object _value)
        {
            if (m_AllTransParameterDic.ContainsKey(_name))
            {
                Debug.LogError("Repead Parameter  of Name: " + _name);
                return false;
            }
            m_AllTransParameterDic.Add(_name, _value);
            return true;
        }

        public object GetParameterVale(string _name)
        {
            if (m_AllTransParameterDic.ContainsKey(_name) == false)
            {
                Debug.LogError("Not Contiant  Parameter  of Name: " + _name);
                return null;
            }

            return m_AllTransParameterDic[_name];
        }

        public bool UpdateParameter(string _name, object _value)
        {
            if (m_AllTransParameterDic.ContainsKey(_name) == false)
            {
                Debug.LogError("Not Contiant  Parameter  of Name: " + _name);
                return false;
            }
            if (m_AllTransParameterDic[_name].Equals(_value))
                return false;

            Debug.Log("UpdateParameter " + _name + " " + _value);

            m_AllTransParameterDic[_name] = _value;
            ((FsmLayerStateMachine)(FsmController.CurStateMachine)).TryTransLateAny();  //×´Ì¬»úÔË×ª
            return true;

        }

        public void SetParameters( params object[] obj)
        {
            if (obj == null || obj.Length == 0) return;
            Parameter _Parameter = null;
            for (int dex = 0; dex < obj.Length; ++dex)
            {
                _Parameter = obj[dex] as Parameter;
                if (m_AllTransParameterDic.ContainsKey(_Parameter.ParameterName) == false)
                {
                    Debug.LogError("Not Contiant  Parameter  of Name: " + _Parameter.ParameterName);
                    continue;
                }
                if (m_AllTransParameterDic[_Parameter.ParameterName].Equals(_Parameter.Value) == false)
                    m_AllTransParameterDic[_Parameter.ParameterName] = _Parameter.Value;
            }
        }




    }
}
