using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/// <summary>
/// ScrollRect扩展组件，支持子项复用
/// </summary>
[DisallowMultipleComponent]
public class ScrollRectEx : MonoBehaviour {

	public delegate void OnInitializeItem(GameObject go,int dataIndex);
	public OnInitializeItem onInitializeItem;
    public delegate void OnDestroyItem(GameObject go, int dataIndex);
    public OnDestroyItem onDestroyItem;

    public enum Arrangement
	{
		Horizontal,
		Vertical,
	}

	/// <summary>
	/// Type of arrangement -- vertical or horizontal.
	/// </summary>

	public Arrangement arrangement = Arrangement.Horizontal;

	/// <summary>
	/// Maximum children per line.
	/// If the arrangement is horizontal, this denotes the number of columns.
	/// If the arrangement is vertical, this stands for the number of rows.
	/// </summary>
	[Range(1,50)]
	private int maxPerLine = 1;

	public float cellLength
    {
        get
        {
            Vector2 sizeDelta = goItemPrefab.GetComponent<RectTransform>().sizeDelta;
            return arrangement == Arrangement.Vertical ? sizeDelta.x : sizeDelta.y;
        }
    }

	[Range(0,30)]
	public int viewCount = 5;

	public ScrollRect scrollRect;

	public RectTransform content;

	public GameObject goItemPrefab;

	private int dataCount = 0;

	private int curScrollPerLineIndex = -1;

	private List<ScrollRectExItem> listItem;

	private Queue<ScrollRectExItem> unUseItem;

    private List<ScrollRectExDataItem> listItemData;

    [Serializable]
    public struct Padding
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }
    public Padding padding;
    public float spacing = 0f;


#if UNITY_EDITOR
    private void Reset()
    {
        transform.tag = "ScrollRectEx";
    }
#endif



    void Awake(){
		listItem = new List<ScrollRectExItem> ();
		unUseItem = new Queue<ScrollRectExItem> ();
        listItemData = new List<ScrollRectExDataItem>();
	}

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {

        if (scrollRect == null || content == null || goItemPrefab == null)
        {
            Debug.LogError("异常:请检测<" + gameObject.name + ">对象上UIWarpContent对应ScrollRect、Content、GoItemPrefab 是否存在值...." + scrollRect + " _" + content + "_" + goItemPrefab);
            return;
        }
        SetDataCount(0);

        scrollRect.onValueChanged.RemoveAllListeners();
        scrollRect.onValueChanged.AddListener(OnValueChanged);

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        unUseItem.Clear();
        listItem.Clear();
        listItemData.Clear();
        SetUpdateRectItem(0);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="count"></param>
    public void Init(int count)
    {
        Init();
        for (int i = 0; i < count; ++i)
        {
            AddItem();
        }
        SetUpdateContentSize();
    }

    private void SetDataCount(int count)
	{
		if (dataCount == count) 
		{
			return;
		}
		dataCount = count;
		SetUpdateContentSize ();
	}

	private void OnValueChanged(Vector2 vt2){
		//switch (arrangement) {
		//case Arrangement.Vertical:
		//	float y = vt2.y;
		//	if (y >= 1.0f || y <= 0.0f) {

  //                  Debug.Log("OnValueChanged: return ");
		//		return;
		//	}
		//	break;
		//case Arrangement.Horizontal:
		//	float x = vt2.x;
		//	if (x <= 0.0f || x >= 1.0f) {
		//		return;
		//	}
		//	break;
		//}
		int _curScrollPerLineIndex = GetCurScrollPerLineIndex ();
        if (_curScrollPerLineIndex == curScrollPerLineIndex){
			return;
		}
		SetUpdateRectItem (_curScrollPerLineIndex);
	}

	/**
	 * @des:设置更新区域内item
	 * 功能:
	 * 1.隐藏区域之外对象
	 * 2.更新区域内数据
	 */
	private void SetUpdateRectItem(int scrollPerLineIndex)
	{
		if (scrollPerLineIndex < 0) 
		{
			return;
		}

		curScrollPerLineIndex = scrollPerLineIndex;
		int startDataIndex = curScrollPerLineIndex * maxPerLine;

		int endDataIndex = (curScrollPerLineIndex + viewCount) * maxPerLine;
		//移除
		for (int i = listItem.Count - 1; i >= 0; i--) 
		{
			ScrollRectExItem item = listItem[i];
			int index = item.Index;
			if (index < startDataIndex || index >= endDataIndex) 
			{
				item.Index = -1;
				listItem.Remove (item);
				unUseItem.Enqueue (item);
			}
		}
		//显示
		for(int dataIndex = startDataIndex;dataIndex<endDataIndex;dataIndex++)
		{
			if (dataIndex >= dataCount) 
			{
				continue;
			}
			if (IsExistDataByDataIndex (dataIndex)) 
			{
				continue;
			}
			CreateItem (dataIndex);
		}
	}



	/**
	 * @des:添加当前数据索引数据
	 */
	public void AddItem(int dataIndex, float length)
	{
        AddWarpContentDataItem(dataIndex, length);
        if (dataIndex<0 ) 
		{
			return;
		}
		//检测是否需添加gameObject
		bool isNeedAdd = false;
		for (int i = listItem.Count-1; i>=0 ; i--) {

			ScrollRectExItem item = listItem [i];
			if (item.Index >= (dataCount - 1)) {
				isNeedAdd = true;
				break;
			}
		}

        if (listItem.Count == 0)
        {
            isNeedAdd = true;
        }
        SetDataCount (dataCount+1);

		if (isNeedAdd) {
			for (int i = 0; i < listItem.Count; i++) {
				ScrollRectExItem item = listItem [i];
				int oldIndex = item.Index;
				if (oldIndex>=dataIndex) {
					item.Index = oldIndex+1;
				}
				item = null;
			}
			SetUpdateRectItem (GetCurScrollPerLineIndex());
		} else {
			//重新刷新数据
			for (int i = 0; i < listItem.Count; i++) {
				ScrollRectExItem item = listItem [i];
				int oldIndex = item.Index;
				if (oldIndex>=dataIndex) {
					item.Index = oldIndex;
				}
				item = null;
			}
		}

	}

    public void AddItem(float length)
    {
        AddItem(listItemData.Count, length);
    }

    public void AddItem()
    {
        Vector2 sizeDelta = goItemPrefab.GetComponent<RectTransform>().sizeDelta;
        float length = arrangement == Arrangement.Vertical ? sizeDelta.y : sizeDelta.x;
        AddItem(length);
    }

    /**
	 * @des:删除当前数据索引下数据
	 */
    public void DelItem(int dataIndex){
		if (dataIndex < 0 || dataIndex >= dataCount) {
			return;
		}
		//删除item逻辑三种情况
		//1.只更新数据，不销毁gameObject,也不移除gameobject
		//2.更新数据，且移除gameObject,不销毁gameObject
		//3.更新数据，销毁gameObject

		bool isNeedDestroyGameObject = (listItem.Count >= dataCount);
		SetDataCount (dataCount-1);

		for (int i = listItem.Count-1; i>=0 ; i--) {
			ScrollRectExItem item = listItem [i];
			int oldIndex = item.Index;
			if (oldIndex == dataIndex) {
				listItem.Remove (item);
				if (isNeedDestroyGameObject) {
					GameObject.Destroy (item.gameObject);
				} else {
					item.Index = -1;
					unUseItem.Enqueue (item);			
				}
			}
			if (oldIndex > dataIndex) {
				item.Index = oldIndex - 1;
			}
		}
		SetUpdateRectItem(GetCurScrollPerLineIndex());
	}

	/**
	 * @des:获取当前index下对应Content下的本地坐标
	 * @param:index
	 * @内部使用
	*/
	public Vector3 GetLocalPositionByIndex(int index){
		float x = 0f;
		float y = 0f;
		float z = 0f;
		switch (arrangement) {
		case Arrangement.Horizontal: //水平方向
            for (int i = 0; i < index;i++)
            {
                x += listItemData[i].Length + spacing;
            }
            x += padding.left;
			y = -(index%maxPerLine) * (cellLength + spacing);
			break;
		case  Arrangement.Vertical://垂直方向
			x =  (index%maxPerLine ) * (cellLength + spacing);
            for (int i = 0; i < index; i++)
            {
                y -= listItemData[i].Length + spacing;
            }
            y -= padding.top;
			break;
		}
		return new Vector3(x,y,z);
	}

    public Vector2 GetSizeDeltaByIndex(int index) {
        float height = 0;
        float width = 0;
        if (arrangement == Arrangement.Horizontal)
        {
            height= cellLength;
            width = listItemData[index].Length;
        }
        else
        {
            width = cellLength;
            height  = listItemData[index].Length;
        }

        return new Vector2(width,height );
    }

    /**
	 * @des:创建元素
	 * @param:dataIndex
	 */
    private void CreateItem(int dataIndex){
		ScrollRectExItem item;
		if (unUseItem.Count > 0) {
			item = unUseItem.Dequeue();
		} else {
			item = AddChild (goItemPrefab, content).AddComponent<ScrollRectExItem>();
		}
		listItem.Add(item);
        item.scrollRectEx = this;
        item.Index = dataIndex;
        item.SetItem();
    }

	/**
	 * @des:当前数据是否存在List中
	 */
	private bool IsExistDataByDataIndex(int dataIndex){
		if (listItem == null || listItem.Count <= 0) {
			return false;
		}
		for (int i = 0; i < listItem.Count; i++) {
			if (listItem [i].Index == dataIndex) {
				return true;
			}
		}
		return false;
	}


	/**
	 * @des:根据Content偏移,计算当前开始显示所在数据列表中的行或列
	 */
	private int GetCurScrollPerLineIndex()
	{
        int curIndex = 0;
        int startIndex = curScrollPerLineIndex;
        int endIndex = curScrollPerLineIndex + viewCount;
        endIndex = endIndex >= listItemData.Count ? listItemData.Count - 1 : endIndex;
        if (endIndex > listItemData.Count)
        {
            curIndex = startIndex;
        }
        else
        {
            switch (arrangement)
            {
                case Arrangement.Horizontal: //水平方向
                    float contentWidth = 0;
                    for (int i = 0; i < endIndex; i++)
                    {
                        if (content.anchoredPosition.x > -contentWidth)
                        {
                            continue;
                        }
                        contentWidth = listItemData[i].Length + spacing + contentWidth;
                        curIndex = i;
                    }
                    break;
                case Arrangement.Vertical://垂直方向
                    float contentHeight = 0;
                    for (int i = 0; i < endIndex; i++)
                    {

                        if (content.anchoredPosition.y < contentHeight)
                        {
                            continue;
                        }
                        contentHeight = listItemData[i].Length + spacing + contentHeight;
                        curIndex = i;
                    }
                    break;
            }
        }

		return curIndex;
	}

	/**
	 * @des:更新Content SizeDelta
	 */
	public void SetUpdateContentSize()
	{
		//int lineCount = Mathf.CeilToInt((float)dataCount/maxPerLine);
        int lineCount = dataCount;
        float height = 0;
        float width = 0;
        switch (arrangement)
		{
		 case Arrangement.Horizontal:
                for (int i = 0; i < listItemData.Count; i++)
                {
                    float itemWidth = listItemData[i].Length + spacing;
                    width += itemWidth;
                }
			content.sizeDelta = new Vector2(width + spacing * (lineCount - 1) + padding.left + padding.right, content.sizeDelta.y);
			break;
		 case Arrangement.Vertical:

            for (int i = 0; i< listItemData.Count; i++)
            {
                float itemHeight = listItemData[i].Length;
                height += itemHeight;
            }
			content.sizeDelta = new Vector2(content.sizeDelta.x, height + spacing * (lineCount - 1) + padding.top + padding.bottom);
			break;
		}
	}

	/**
	 * @des:实例化预设对象 、添加实例化对象到指定的子对象下
	 */
	private GameObject AddChild(GameObject goPrefab,Transform parent)
	{
		if (goPrefab == null || parent == null) {
			Debug.LogError("异常。UIWarpContent.cs addChild(goPrefab = null  || parent = null)");
			return null;
		}
		GameObject goChild = GameObject.Instantiate (goPrefab) as GameObject;
		goChild.layer = parent.gameObject.layer;
		goChild.transform.SetParent (parent,false);

		return goChild;
	}

    public void AddWarpContentDataItem(int index , float length)
    {
        if (index >= listItemData.Count)
            listItemData.Add(new ScrollRectExDataItem(length));
    }

    public void SetWarpContentItemData(int index, float length)
    {
        if (index < listItemData.Count)
        {
            listItemData[index].Length = length;
        }

        for (int i = 0; i < listItem.Count; i++)
        {
            if (listItem[i].Index == index)
            {
                listItem[i].SetItem();
            }
            
        }
    }

    public void SetContentPosition(int index)
    {
        float position = 1;
        if (dataCount != 0)
        {
            position = (float)index / dataCount;
        }
        scrollRect.verticalNormalizedPosition = 1 - position;
        scrollRect.horizontalNormalizedPosition = position;
        OnValueChanged(Vector2.zero);
    }

    public bool IsItemShow(int index)
    {
        bool isItemShow = IsExistDataByDataIndex(index);
        return isItemShow;
    }

    void OnDestroy(){
		
		scrollRect = null;
		content = null;
		goItemPrefab = null;
		onInitializeItem = null;

		listItem.Clear ();
		unUseItem.Clear ();

		listItem = null;
		unUseItem = null;

	}

    protected virtual void OnDisable()
    {
        onInitializeItem = null;
        SetContentPosition(0);
    }
}

public class ScrollRectExDataItem
{
    private string name;
    private float length;

    public ScrollRectExDataItem(float length)
    {
        this.length = length;
    }

    public float Length
    {
        set { length = value; }
        get { return length; }
    }
}
