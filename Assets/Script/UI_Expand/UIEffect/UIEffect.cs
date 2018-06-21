using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public delegate void PointerEnterDelegate(GameObject gameObject);
public delegate void PointerExitDelegate(GameObject gameObject);
public delegate void PointerDownDelegate(GameObject gameObject);
public delegate void PointerUpDelegate(GameObject gameObject);
public delegate void PointerClickDelegate(GameObject gameObject);
public delegate void PointerDragDelegate(GameObject gameObject);

[RequireComponent(typeof(AudioSource))]
public class UIEffect : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler
{
    /// <summary>
    /// 申明指针进入事件
    /// </summary>
    public event PointerEnterDelegate PointerEnterEvent;

    /// <summary>
    /// 申明指针离开事件
    /// </summary>
    public event PointerExitDelegate PointerExitEvent;

    /// <summary>
    /// 申明指针按下事件
    /// </summary>
    public event PointerDownDelegate PointerDownEvent;

    /// <summary>
    /// 申明指针弹起事件
    /// </summary>
    public event PointerUpDelegate PointerUpEvent;

    /// <summary>
    /// 申明指针弹起事件
    /// </summary>
    public event PointerClickDelegate PointerClickEvent;

    /// <summary>
    /// 申明指针拖拽事件
    /// </summary>
    public event PointerDragDelegate PointerDragEvent;

    /// <summary>
    /// 枚举UI效果的状态
    /// </summary>
    public enum State
    {
        Show,
        Hide,
        Normal,
        Normal_Over,
        Over_Normal,
        Over_Press,
        Press_Over,
        Selected,
        Enable,
        Disable,
        Other
    }

    /// <summary>
    /// 枚举UI效果的方式
    /// </summary>
    public enum Type
    {
        Position,
        LocalPosition,
        EulerAngle,
        LocalEulerAngle,
        Scale,
        Color,
        Alpha,
        LocalPositionX,
        LocalPositionY,
        LocalPositionZ,
        None
    }

    public enum Loop
    {
        OffsetPosistion,
        OffsetAngle,
        Position,
        LocalPosition,
        EulerAngle,
        LocalEulerAngle,
        Scale,
        Color,
        Alpha,
        None
    }

    /// <summary>
    /// 配置UI效果，成员变量序列化成属性供编辑器使用
    /// </summary>
    [System.Serializable]
    public class Config
    {
        public Transform target;
        public State state;
        public Type type;

        public Ease ease;
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        public float delay;
        public float duration;

        public Vector3 vector3FromValue;
        public Color colorFromValue;
        public float floatFromValue;

        public Vector3 vector3ToValue;
        public Color colorToValue;
        public float floatToValue;

        public AudioClip source;


        public Config()
        {
            this.target = null;
            this.state = State.Normal;
            this.type = Type.None;
            this.ease = Ease.InQuad;

            this.duration = 0.3f;

            this.vector3FromValue = Vector3.one;
            this.colorFromValue = Color.white;
            this.floatFromValue = 0;

            this.vector3ToValue = Vector3.one;
            this.colorToValue = Color.white;
            this.floatToValue = 0;
        }

        public Config(State state, Type type)
        {
            this.target = null;
            this.state = state;
            this.type = type;
            this.ease = Ease.InQuad;

            this.duration = 0.3f;

            this.vector3FromValue = Vector3.one;
            this.colorFromValue = Color.white;
            this.floatFromValue = 0;

            this.vector3ToValue = Vector3.one;
            this.colorToValue = Color.white;
            this.floatToValue = 0;
        }
    }

    public bool useSelectedEffect = false;

    //使用点击反转功能
    public bool useClickRestore = false;

    [HideInInspector]
    public bool isSelected = false;

    private static GameObject previousSelected = null;

    private Tweener tweener = null;

    /// <summary>
    /// 使用这个标示控制UIeffect状态，使用组件的enabled 会直接打断状态
    /// </summary>
    public bool isEffectEnable = true;

    //默认类型：Show、Hide、Normal、Normal_Over、Over_Normal、Over_Press、Press_Over、Selected、Disable
    public Config[] show = new Config[1]
    {
        new Config(State.Show, Type.None)
    };

    public Config[] hide = new Config[1]
    {
        new Config(State.Hide, Type.None)
    };

    public Config[] normal = new Config[1]
    {
        new Config(State.Normal, Type.None)
    };

    public Config[] normalOver = new Config[1]
    {
        new Config(State.Normal_Over, Type.None)
    };

    public Config[] overNormal = new Config[1]
    {
        new Config(State.Over_Normal, Type.None)
    };

    public Config[] overPress = new Config[1]
    {
        new Config(State.Over_Press, Type.None)
    };

    public Config[] pressOver = new Config[1]
    {
        new Config(State.Press_Over, Type.None)
    };

    public Config[] selected = new Config[1]
    {
        new Config(State.Selected, Type.None)
    };

    public Config[] enable = new Config[1]
    {
        new Config(State.Enable, Type.None)
    };

    public Config[] disable = new Config[1]
    {
        new Config(State.Disable, Type.None)
    };

    /// <summary>
    /// 可供初始显示效果使用。
    /// </summary>
    /// <param name="delay"></param>
    public void Show(float delay)
    {
        DOTween.ClearCachedTweens();
        if (isEffectEnable)
        {
            int num = show.Length;
            for (int i = 0; i < num; ++i)
            {
                show[i].delay = delay;
                TweenPlay(show[i]);
            }
        }
    }

    /// <summary>
    /// 可供隐藏显示效果使用。
    /// </summary>
    /// <param name="delay"></param>
    public void Hide(float delay)
    {
        //	Debug.Log ("hide"+gameObject.name);
        DOTween.CompleteAll();
        if (isEffectEnable)
        {
            int num = hide.Length;
            for (int i = 0; i < num; ++i)
            {
                hide[i].delay = delay;
                TweenPlay(hide[i]);
            }
        }
    }

    /// <summary>
    /// 设置按键可用
    /// </summary>
    public void SetEnable()
    {
        //	Debug.Log ("setEnable"+gameObject.name);
        if (isEffectEnable)
        {
            int num = enable.Length;
            for (int i = 0; i < num; ++i)
                TweenPlay(enable[i]);
        }
    }

    /// <summary>
    /// 设置按键不可用
    /// </summary>
    public void SetDisable()
    {
        ///	Debug.Log ("setDisable "+gameObject.name);
        if (isEffectEnable)
        {
            int num = disable.Length;
            for (int i = 0; i < num; ++i)
                TweenPlay(disable[i]);
        }
    }

    /// <summary>
    /// 设置选中状态或者点击去掉选中
    /// </summary>
	public void SetSelected()
    {
        DOTween.CompleteAll();
        if (isEffectEnable)
        {
            if (useClickRestore)
            {
                if (!isSelected)
                { //选中
                  //			Debug.Log ("setselected "+gameObject.name);
                    int num = selected.Length;
                    for (int i = 0; i < num; ++i)
                        TweenPlay(selected[i]);
                }
                else
                {//取消选中
                 //			Debug.Log ("set反选 "+gameObject.name);

                    int num = pressOver.Length;
                    for (int i = 0; i < num; ++i)
                        TweenPlay(pressOver[i]);
                }
                isSelected = !isSelected;
            }
            else
            {
                if (isSelected)
                    return;
                //	Debug.Log ("setSelwect "+gameObject.name);

                isSelected = true;
                int num = selected.Length;
                for (int i = 0; i < num; ++i)
                    TweenPlay(selected[i]);
            }
        }
    }

    /// <summary>
    /// 被取消选中
    /// </summary>
    public void UnSelected()
    {
        if (isEffectEnable)
        {
            //      tweener.Kill();
            isSelected = false;
            int num = normal.Length;
            for (int i = 0; i < num; ++i)
                TweenPlay(normal[i]);
        }
    }

    /// <summary>
    /// 指针进入函数。
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter   " + gameObject.name + "     isEffectEnable=" + isEffectEnable + "   useSelectedEffect=" + useSelectedEffect + "  isSelected=" + isSelected);
        if (isEffectEnable)
        {
            if (PointerEnterEvent != null)
                PointerEnterEvent(gameObject);

            if (!isSelected)
            {
                int num = normalOver.Length;
                for (int i = 0; i < num; ++i)
                    TweenPlay(normalOver[i]);
            }
        }
    }

    /// <summary>
    /// 指针离开函数。
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit   " + gameObject.name + "     isEffectEnable=" + isEffectEnable+ "   useSelectedEffect="+ useSelectedEffect + "  isSelected="+ isSelected);
        if (isEffectEnable)
        {
            //  tweener.Kill();
            if (PointerExitEvent != null)
                PointerExitEvent(gameObject);

            if (!useSelectedEffect)
            {
                //	Debug.Log ("OnPointerExit    反选"+gameObject.name);

                int num = overNormal.Length;
                for (int i = 0; i < num; ++i)
                    TweenPlay(overNormal[i]);
            }
            else
            {
                if (!isSelected)
                {
                    //		Debug.Log ("OnPointerExit 2   反选"+gameObject.name);

                    int num = overNormal.Length;
                    for (int i = 0; i < num; ++i)
                        TweenPlay(overNormal[i]);
                }
            }
        }
    }

    /// <summary>
    /// 指针按下函数。
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isEffectEnable)
        {
            if (PointerDownEvent != null)
                PointerDownEvent(gameObject);

            //		Debug.Log ("OnPointerDown  选中"+gameObject.name);

            int num = overPress.Length;
            for (int i = 0; i < num; ++i)
                TweenPlay(overPress[i]);
        }
    }

    /// <summary>
    /// 指针弹起函数。
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isEffectEnable)
        {
            if (PointerUpEvent != null)
                PointerUpEvent(gameObject);

            int num = pressOver.Length;
            for (int i = 0; i < num; ++i)
                TweenPlay(pressOver[i]);
        }
    }

    /// <summary>
    /// 指针点击函数。
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEffectEnable)
        {
            if (PointerClickEvent != null)
                PointerClickEvent(gameObject);
        }
    }

    /// <summary>
    /// 指针拖拽函数。
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (PointerDragEvent != null)
            PointerDragEvent(gameObject);
    }

    //具体播放的效果
    void TweenPlay(Config config)
    {
        Transform t = config.target == null ? transform : config.target;

        AudioSource audioSource = t.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = t.gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 0.3f;

        audioSource.playOnAwake = false;

        if (config.source != null)
        {
            audioSource.clip = config.source;
            audioSource.Play();
        }

        switch (config.type)
        {
            case Type.Position:
                if (tweener == null)
                {
                    t.position = config.vector3FromValue;
                }
                else
                {
                    if (!tweener.IsPlaying())
                        t.position = config.vector3FromValue;
                }

                tweener = t.DOMove(config.vector3ToValue, config.duration);
                break;
            case Type.LocalPosition:
                if (tweener == null)
                {
                    t.localPosition = config.vector3FromValue;
                }
                else
                {
                    if (!tweener.IsPlaying())
                        t.localPosition = config.vector3FromValue;
                }
                tweener = t.DOLocalMove(config.vector3ToValue, config.duration);
                break;

            #region ex meichao 2017_3_1
            case Type.LocalPositionX:
                if (tweener == null)
                {
                    //    t.localPosition = config.vector3FromValue;
                }
                else
                {
                    if (!tweener.IsPlaying())
                        t.localPosition = new Vector3(config.floatFromValue, t.localPosition.y, t.localPosition.z);
                }
                tweener = t.DOLocalMoveX(config.floatToValue, config.duration);
                break;
            case Type.LocalPositionY:
                if (tweener == null)
                {
                    //       t.localPosition = config.vector3FromValue;
                }
                else
                {
                    if (!tweener.IsPlaying())
                        t.localPosition = new Vector3(t.localPosition.x, config.floatFromValue, t.localPosition.z);
                }
                tweener = t.DOLocalMoveY(config.floatToValue, config.duration);
                break;
            case Type.LocalPositionZ:
                if (tweener == null)
                {
                    //     t.localPosition = config.vector3FromValue;
                }
                else
                {
                    if (!tweener.IsPlaying())
                        t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, config.floatFromValue);
                }
                tweener = t.DOLocalMoveZ(config.floatToValue, config.duration);
                break;
            #endregion


            case Type.EulerAngle:
                if (tweener == null)
                {
                    t.eulerAngles = config.vector3FromValue;
                }
                else
                {
                    if (!tweener.IsPlaying())
                        t.eulerAngles = config.vector3FromValue;
                }
                tweener = t.DORotate(config.vector3ToValue, config.duration);
                break;
            case Type.LocalEulerAngle:
                if (tweener == null)
                {
                    t.localEulerAngles = config.vector3FromValue;
                }
                else
                {
                    if (!tweener.IsPlaying())
                        t.localEulerAngles = config.vector3FromValue;
                }
                tweener = t.DOLocalRotate(config.vector3ToValue, config.duration);
                break;
            case Type.Scale:
                if (tweener == null)
                {
                    t.localScale = config.vector3FromValue;
                }
                else
                {
                    if (!tweener.IsPlaying())
                        t.localScale = config.vector3FromValue;
                }
                tweener = t.DOScale(config.vector3ToValue, config.duration);
                break;
            case Type.Color:
                Image image = t.GetComponent<Image>();
                if (image != null)
                {
                    if (tweener == null)
                    {
                        image.color = config.colorFromValue;
                    }
                    else
                    {
                        if (!tweener.IsPlaying())
                            image.color = config.colorFromValue;
                    }
                    tweener = image.DOColor(config.colorToValue, config.duration);
                }

                RawImage rawImage = t.GetComponent<RawImage>();
                if (rawImage != null)
                {
                    if (tweener == null)
                    {
                        rawImage.color = config.colorFromValue;
                    }
                    else
                    {
                        if (!tweener.IsPlaying())
                            rawImage.color = config.colorFromValue;
                    }
                    tweener = rawImage.DOColor(config.colorToValue, config.duration);
                }

                Text text = t.GetComponent<Text>();
                if (text != null)
                {
                    if (tweener == null)
                    {
                        text.color = config.colorFromValue;
                    }
                    else
                    {
                        if (!tweener.IsPlaying())
                            text.color = config.colorFromValue;
                    }
                    tweener = text.DOColor(config.colorToValue, config.duration);
                }
                break;
            case Type.Alpha:
                CanvasGroup canvasGroup = t.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    if (tweener == null)
                    {
                        canvasGroup.alpha = config.floatFromValue;
                    }
                    else
                    {
                        if (!tweener.IsPlaying())
                            canvasGroup.alpha = config.floatFromValue;
                    }
                    tweener = canvasGroup.DOFade(config.floatToValue, config.duration);
                }
                break;
        }
        if (tweener != null)
        {
            tweener.SetDelay(config.delay);

            if (config.ease == Ease.INTERNAL_Custom)
                tweener.SetEase(config.curve);
            else
                tweener.SetEase(config.ease);
        }
        tweener.SetUpdate(true);
        tweener.Play();
    }
}
