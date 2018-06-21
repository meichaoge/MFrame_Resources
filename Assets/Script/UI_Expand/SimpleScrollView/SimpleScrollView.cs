using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

namespace MFramework
{

    public class SimpleScrollView : MonoBehaviour, IScrollHandler
    {
        public Vector2 ViewPort;
        public float ScrollSpeed;
        public float TweenTime = 0.2f;
        [Header("UI View")]
        public Text ContentText;
        public RectTransform HorizontialBar;
        public RectTransform HorizontialBar_Bar;

        [SerializeField]
        private float Value = 0;

#if UNITY_EDITOR
        [Header("Test")]
        public string testMsg;
#endif
        #region Data
        //  [SerializeField]
        private Vector3 initialedPosition;
        //  [SerializeField]
        float minHeightPositonY;
        //   [SerializeField]
        float maxHeightPositonY;
        //[SerializeField]
        float endPont;

        float BarScrollDistance;
        float initialPositiony;
        #endregion

        private void Awake()
        {
            initialedPosition = ContentText.rectTransform.localPosition;
            initialPositiony = HorizontialBar_Bar.localPosition.y;
            BarScrollDistance = HorizontialBar.sizeDelta.y - HorizontialBar_Bar.sizeDelta.y;

            if (HorizontialBar.gameObject.activeSelf)
                HorizontialBar.gameObject.SetActive(false);
        }


#if UNITY_EDITOR
        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    ShowMsg(testMsg);
            //}

            if (Input.GetKeyDown(KeyCode.U))
            {
                Scroll(true);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Scroll(false);
            }
        }

#endif

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isHoverTop">是否顶对齐</param>
        public void ShowMsg(string msg, bool isHoverTop)
        {

            ContentText.text = msg;

            ContentText.rectTransform.sizeDelta = new Vector2(ContentText.rectTransform.sizeDelta.x, ContentText.preferredHeight);
            if (isHoverTop)
                ContentText.rectTransform.localPosition = initialedPosition - new Vector3(0, (ContentText.preferredHeight) / 2f, 0);  //加入消息但是任然显示最上面
            else
                ContentText.rectTransform.localPosition = initialedPosition + new Vector3(0, (ContentText.preferredHeight) / 2f, 0);
            ShowScrollBar();
        }



        void ShowScrollBar()
        {
            if (ContentText.rectTransform.sizeDelta.y < ViewPort.y)
            {
                Value = 1;
                if (HorizontialBar.gameObject.activeSelf)
                    HorizontialBar.gameObject.SetActive(false);
            }
            else
            {
                if (HorizontialBar.gameObject.activeSelf == false)
                    HorizontialBar.gameObject.SetActive(true);

                minHeightPositonY = GetMinPositionY();
                maxHeightPositonY = GetMaxPositionY();

                Value = (ContentText.rectTransform.localPosition.y - minHeightPositonY) / (maxHeightPositonY - minHeightPositonY);
                HorizontialBar_Bar.localPosition = new Vector3(0, Value * BarScrollDistance * -1f + initialPositiony, 0);
            }

        }
        float GetMaxPositionY()
        {
            if (ContentText.rectTransform.sizeDelta.y < ViewPort.y)
                return initialedPosition.y;

            return (ContentText.preferredHeight / 2f - ViewPort.y / 2f);
        }
        float GetMinPositionY()
        {
            if (ContentText.rectTransform.sizeDelta.y < ViewPort.y)
                return initialedPosition.y;

            return -1 * (ContentText.preferredHeight / 2f - ViewPort.y / 2f);
        }
        public void Scroll(bool isUp)
        {
            if (isUp)
            {
                endPont = Mathf.Max(GetMinPositionY(), ContentText.rectTransform.localPosition.y - ScrollSpeed);
                ContentText.rectTransform.DOLocalMoveY(endPont, TweenTime);
            }//if
            else
            {
                endPont = Mathf.Min(GetMaxPositionY(), ContentText.rectTransform.localPosition.y + ScrollSpeed);
                ContentText.rectTransform.DOLocalMoveY(endPont, TweenTime);
            }//else
            ShowScrollBar();
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (eventData == null)
                return;

            if (eventData.scrollDelta.y < 0)
            {
                Scroll(false);
            }
            else if (eventData.scrollDelta.y > 0)
            {
                Scroll(true);
            }

        }



    }
}
