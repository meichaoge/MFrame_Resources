using UnityEngine;
using System.Collections;


namespace MFramework
{

    public class BindProperty<T>
    {
        public delegate void ValueChangedHandler(T oldValue, T newValue);
        public event ValueChangedHandler OnValueChangedEvent;

        private T m_Value;
        public T Value
        {
            get { return m_Value; }
            set
            {
                if (!object.Equals(m_Value, value))
                {
                    T old = m_Value;
                    m_Value = value;
                    ValueChanged(old, m_Value);
                }
            }

        }

        private void ValueChanged(T oldValue, T newValue)
        {
            if (OnValueChangedEvent != null)
                OnValueChangedEvent(oldValue, newValue);
        }

        public override string ToString()
        {
            return (Value != null ? Value.ToString() : "");
        }


    }
}
