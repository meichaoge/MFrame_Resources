using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UpdateUIScale : MonoBehaviour
{
    [SerializeField]
    float planeDistance = 1;
    [SerializeField]
    private float m_AndroidplaneDistance = 1.5f;
    [SerializeField]
    private float distanceScele = 1;

    private Vector2 canvasSize = new Vector2(2560, 1440);

    void Awake()
    {
        SetUIScaleRatio();
    }


    /// <summary>
    /// / 根据设定的画布分辨率调整画布缩放已达到最佳的视觉效果
    /// </summary>
    /// <param name="isOnlyPositionZ">是否只设置Z轴距离</param>
    public void SetUIScaleRatio(bool isOnlyPositionZ = false)
    {
        float canvasHeight, halfFovVerticalRadian;
        canvasHeight = canvasSize.y;
#if UNITY_STANDALONE || UNITY_EDITOR
        if (isOnlyPositionZ == false)
            transform.localPosition = new Vector3(0, 0, planeDistance);
        else
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, planeDistance);

        halfFovVerticalRadian = Mathf.Deg2Rad * Camera.main.fieldOfView / 2;
#elif UNITY_ANDROID
           if (isOnlyPositionZ == false)
            transform.localPosition = new Vector3(0, 0, m_AndroidplaneDistance);
        else
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, m_AndroidplaneDistance);
        halfFovVerticalRadian = 106.1888f; //安卓Fov是固定的
#endif

        float uiScaleRatio = (Mathf.Tan(halfFovVerticalRadian) * planeDistance * 2) * distanceScele / canvasHeight;
        transform.localScale = new Vector3(Mathf.Abs(uiScaleRatio), Mathf.Abs(uiScaleRatio), Mathf.Abs(uiScaleRatio));
    }
}



#region 
//public enum ScreenResolution
//{
//    K_Normal,//1920*1080
//    K_1,//2560*1440
//    K_2,//3200*1800
//    k_3,//3840*2160
//    K_4,//5120*2880
//    K_5,//7680*4320
//}

//[RequireComponent(typeof(Canvas))]
//[ExecuteInEditMode]
//public class UpdateUIScale : MonoBehaviour
//{
//    [MenuItem("CONTEXT/UpdateUIScale/设置UI")]
//     void SetPanelViewOfResolution()
//    {
//        GetAllCanvasItem(transform);
//        Initial(m_CurrentResolution, m_defaultResouition);
//    }

//    public float planeDistance = 100;

//    static RectTransform uiRectTransform;
//    static float canvasWidth, canvasHeight, halfFovVerticalRadian;


//    //	public delegate void OnScreenUIResolutionChangeHandle(ScreenResolution _newRosolution,ScreenResolution _previousResolution);
//    //	public event OnScreenUIResolutionChangeHandle OnScreenUIResolutionChangeEvent;
//    [SerializeField]
//    private ScreenResolution m_defaultResouition = ScreenResolution.K_1;
//    [SerializeField]
//    private ScreenResolution m_CurrentResolution = ScreenResolution.K_1;
//    //	private ScreenResolution m_ScreenResolutionType;
//    //	public ScreenResolution ScreenResolutionType{
//    //		 set{ 
//    //			if (m_ScreenResolutionType != value) {
//    //				//Debug.Log ("屏幕分辨率修改");
//    //				if (OnScreenUIResolutionChangeEvent != null)
//    //					OnScreenUIResolutionChangeEvent (value,m_ScreenResolutionType);
//    //			};
//    //			m_ScreenResolutionType = value;
//    //		}
//    //		 get{	return m_ScreenResolutionType;	}
//    //	}

//    static List<LocalTransInfor> allCanvasItem = new List<LocalTransInfor>();

//    //	void Awake()
//    //	{
//    //		GetAllCanvasItem (transform);
//    //	//	OnScreenUIResolutionChangeEvent += Initial;
//    //		Initial (m_defaultResouition,m_defaultResouition);
//    //	}

//    //	void OnDestroy(){
//    //		OnScreenUIResolutionChangeEvent -= Initial;
//    //	}

//    //	void Update(){
//    //		if (Input.GetMouseButtonDown (0)) {
//    //			int value= Random.Range (0, 5);
//    //			ScreenResolutionType = (ScreenResolution)value;
//    //			Debug.Log ("New   "+GetCanvasFromRosolution(ScreenResolutionType));
//    //		}
//    //	}

//    static void Initial(ScreenResolution _resolution, ScreenResolution _previousResolution)
//    {
//        uiRectTransform = gameObject.GetComponent<RectTransform>();
//        Vector2 canvasOfResolution = GetCanvasFromRosolution(_resolution);

//        canvasWidth = canvasOfResolution.x;
//        canvasHeight = canvasOfResolution.y;
//#if UNITY_STANDALONE || UNITY_EDITOR
//        halfFovVerticalRadian = Mathf.Deg2Rad * Camera.main.fieldOfView / 2;  //PC
//#elif UNITY_ANDROID
//		halfFovVerticalRadian = 106.1888f; //手机上固定值
//#endif
//        uiRectTransform.localPosition = new Vector3(0, 0, planeDistance);
//        uiRectTransform.sizeDelta = canvasOfResolution;

//        SetUIScaleRatio();
//        UpdataCanvasItem(_resolution, _previousResolution);
//    }

//    static void SetUIScaleRatio()
//    {
//        float uiScaleRatio = (Mathf.Tan(halfFovVerticalRadian) * planeDistance * 2) / canvasHeight;
//        //Debug.Log ("halfFovVerticalRadian " + halfFovVerticalRadian + "   aa" + (Mathf.Tan (halfFovVerticalRadian) * planeDistance * 2));
//        uiRectTransform.localScale = new Vector3(Mathf.Abs(uiScaleRatio), Mathf.Abs(uiScaleRatio), Mathf.Abs(uiScaleRatio));
//    }


//    static void UpdataCanvasItem(ScreenResolution _resolution, ScreenResolution _previousResolution)
//    {
//        //		if (_resolution == m_defaultResouition)
//        //			return;
//        float scaleRa = GetCanvasFromRosolution(_resolution).x / (GetCanvasFromRosolution(m_defaultResouition).x);

//        for (int dex = 1; dex < allCanvasItem.Count; dex++)
//        {
//            //canvasItem.SourceTrans.localScale = canvasItem.InitiallocalScale * scaleRa;
//            if (allCanvasItem[dex].isRectrans == false)
//            {
//            }
//            else {
//                (allCanvasItem[dex].SourceTrans as RectTransform).sizeDelta = scaleRa * allCanvasItem[dex].InitialrectSize;
//            }

//            LocalTransInfor _parent = GetParentInfor(allCanvasItem[dex]);
//            if (_parent.SourceTrans as RectTransform)
//            {//父节点大小改变
//                float rectScalex = 1, rectScaley = 1;
//                if (_parent.InitialrectSize.x == 0 || _parent.InitialrectSize.y == 0)
//                {
//                    //	Debug.LogError ("AAAAAAAAA");

//                }
//                else {
//                    if (_parent.InitialrectSize.x != 0)
//                        rectScalex = (_parent.SourceTrans as RectTransform).sizeDelta.x / _parent.InitialrectSize.x;
//                    else
//                        rectScaley = (_parent.SourceTrans as RectTransform).sizeDelta.y / _parent.InitialrectSize.y;
//                }
//                //	Debug.Log ("父节点缩放"+rectScalex+" :: "+rectScaley+"   ::   "  +allCanvasItem [dex].SourceTrans.gameObject.name);
//                allCanvasItem[dex].SourceTrans.localPosition = new Vector3(allCanvasItem[dex].InitiallocalPosiiton.x * rectScalex, allCanvasItem[dex].InitiallocalPosiiton.y * rectScaley,
//                    allCanvasItem[dex].SourceTrans.localPosition.z); //根据父节点的缩放获取新的局部坐标
//            }
//            Text fontText = allCanvasItem[dex].SourceTrans.GetComponent<Text>();
//            if (fontText != null)
//            {
//                fontText.fontSize = Mathf.RoundToInt(allCanvasItem[dex].fontSize * scaleRa); //调整字体大小
//            }

//        }

//    }

//    //获得父节点
//    static LocalTransInfor GetParentInfor(LocalTransInfor _current)
//    {
//        foreach (var item in allCanvasItem)
//        {
//            if (item.SourceTrans == _current.SourceTrans)
//            {
//                return item;
//            }
//        }
//        Debug.LogError("Miss " + _current.SourceTrans.gameObject.name);
//        return null;
//    }

//    //获得多有的子对象
//    static void GetAllCanvasItem(Transform canvas)
//    {
//        if (canvas == null || canvas.childCount == 0)
//            return;

//        allCanvasItem.Add(new LocalTransInfor(canvas));

//        for (int dex = 0; dex < canvas.childCount; dex++)
//        {
//            Transform childTrnas = canvas.GetChild(dex);
//            GetAllCanvasItemSub(childTrnas);
//        }

//    }

//    static void GetAllCanvasItemSub(Transform operateTrans)
//    {
//        if (operateTrans == null)
//            return;

//        if (operateTrans.childCount == 0)
//        {
//            allCanvasItem.Add(new LocalTransInfor(operateTrans));
//            return;
//        }

//        for (int dex = 0; dex < operateTrans.childCount; dex++)
//        {
//            Transform childTrnas = operateTrans.GetChild(dex);
//            GetAllCanvasItemSub(childTrnas);
//        }
//        allCanvasItem.Add(new LocalTransInfor(operateTrans));
//    }


//    //根据分辨率获得新的输入画布尺寸
//    static Vector2 GetCanvasFromRosolution(ScreenResolution resolution)
//    {
//        switch (resolution)
//        {
//            case ScreenResolution.K_Normal:
//                return new Vector2(1920, 1080);
//            case ScreenResolution.K_1:
//                return new Vector2(2560, 1440);
//            case ScreenResolution.K_2:
//                return new Vector2(3200, 1800);
//            case ScreenResolution.k_3:
//                return new Vector2(3840, 2160);
//            case ScreenResolution.K_4:
//                return new Vector2(5120, 2880);
//            case ScreenResolution.K_5:
//                return new Vector2(7680, 4320);
//        }
//        return new Vector2(1920, 1080);
//    }


//}

//[System.Serializable]
//public class LocalTransInfor
//{
//    public Transform SourceTrans;
//    public Vector3 InitiallocalPosiiton;
//    public Vector3 InitiallocalScale;
//    public Vector2 InitialrectSize;
//    public bool isRectrans = false;
//    public float fontSize;

//    public LocalTransInfor(Transform trans)
//    {

//        SourceTrans = trans;
//        InitiallocalPosiiton = trans.localPosition;
//        InitiallocalScale = trans.localScale;
//        if (trans as RectTransform)
//        {
//            RectTransform rectrans = (trans as RectTransform);
//            if (rectrans.anchorMin != Vector2.one / 2f || rectrans.anchorMax != Vector2.one / 2f || rectrans.pivot != Vector2.one / 2f)
//            {
//                Debug.LogError(trans.gameObject.name + "Can Do Set TO" + Vector2.one / 2f);
//                return;
//            }
//            isRectrans = true;
//            InitialrectSize = rectrans.sizeDelta;


//        }
//        else
//            InitialrectSize = Vector2.zero;

//        Text textShow = trans.GetComponent<Text>();
//        if (textShow != null)
//        {
//            fontSize = textShow.fontSize;
//        }
//    }

//}
#endregion


