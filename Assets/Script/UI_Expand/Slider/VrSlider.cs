using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MFramework.UI;
using MFramework.UI.Layout;

namespace MFramework
{
    public class VrSlider : MonoBehaviour, IScrollHandler
    {
        [Range(0.01f, 0.2f)]
        public float MoveSpeedPercent = 0.01f;
        public Image Silder_Bar;
        public Image Silder_FillArea;

        public float SilderValue = 0;

        [SerializeField]
        Vector2 m_FillRect = Vector2.zero; //填充区域
        [SerializeField]
        Vector2 m_SliderDis = Vector2.zero;  //Slider 滑动的区域
        Vector3 intialPosition;

        public System.Action<float> OnSilderValueChange;

        void Awake()
        {
            RectTransform _recTrans = Silder_FillArea.transform.parent.GetComponent<RectTransform>();
            if (_recTrans == null)
            {
                Debug.LogError("Miss Parent");
                return;
            }
            m_FillRect = new Vector2(_recTrans.rect.width - Silder_Bar.rectTransform.rect.width, _recTrans.rect.height - Silder_Bar.rectTransform.rect.height);  //计算填充区域
            Silder_FillArea.rectTransform.sizeDelta = Vector2.zero;

            RectTransform _silderBarRec = Silder_Bar.transform.parent.GetComponent<RectTransform>();
            m_SliderDis = _silderBarRec.sizeDelta - Silder_Bar.rectTransform.sizeDelta;  //Get The Rect Of Silde Can do
            Silder_Bar.rectTransform.localPosition = new Vector3(-1 * _silderBarRec.rect.width / 2f, 0, 0);

            intialPosition = Silder_Bar.rectTransform.localPosition;
            SilderValue = 0;
            SetSilderFillRectByPersent();
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (eventData == null) return;
            //  if (IsHorizontialSilder)
            {
                if (eventData.scrollDelta.x < 0)
                {//left
                    Scroll(ScrollPageDirection.LEFT);
                }
                else if (eventData.scrollDelta.x > 0)
                {//right
                    Scroll(ScrollPageDirection.RIGHT);
                }
            }//
            //else
            //{
            //    if (eventData.scrollDelta.y < 0)
            //    {
            //        //下
            //        Scroll(ScrollPageDirection.DOWN);
            //    }
            //    else if (eventData.scrollDelta.y > 0)
            //    {
            //        Scroll(ScrollPageDirection.UP);
            //    }
            //}//else
        }

        void Scroll(ScrollPageDirection _direct)
        {
            switch (_direct)
            {
                case ScrollPageDirection.LEFT:
                    if (Mathf.Approximately(SilderValue, 0))
                    {
                        SilderValue = 0;
                        return;
                    }
                    SilderValue = Mathf.Max(SilderValue - MoveSpeedPercent, 0);
                    SetSilderFillRectByPersent();

                    break;
                case ScrollPageDirection.RIGHT:
                    if (Mathf.Approximately(SilderValue, 1))
                    {
                        SilderValue = 1;
                        return;
                    }
                    SilderValue = Mathf.Min(SilderValue + MoveSpeedPercent, 1);
                    SetSilderFillRectByPersent();
                    break;
            }

            if (OnSilderValueChange != null)
                OnSilderValueChange(SilderValue);
        }

        void SetSilderFillRectByPersent()
        {
            Silder_Bar.transform.localPosition = intialPosition + new Vector3(SilderValue * m_SliderDis.x, 0, 0);
            Silder_FillArea.rectTransform.sizeDelta = new Vector2(SilderValue * m_SliderDis.x + Silder_Bar.rectTransform.rect.width / 2f, Silder_FillArea.rectTransform.sizeDelta.y);
        }

#if UNITY_EDITOR

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                //if (IsHorizontialSilder)
                //    Scroll(ScrollPageDirection.RIGHT);
                //else
                Scroll(ScrollPageDirection.DOWN);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                //if (IsHorizontialSilder)
                //    Scroll(ScrollPageDirection.LEFT);
                //else
                Scroll(ScrollPageDirection.UP);
            }
        }

#endif

    }
}
