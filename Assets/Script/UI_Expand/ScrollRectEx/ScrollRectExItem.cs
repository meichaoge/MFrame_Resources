using UnityEngine;

[DisallowMultipleComponent]
public class ScrollRectExItem : MonoBehaviour {
    private RectTransform rtf;
	private int index;
	public ScrollRectEx scrollRectEx;

	void OnDestroy(){
        if (scrollRectEx.onDestroyItem != null && index >= 0)
        {
            scrollRectEx.onDestroyItem(gameObject, index);
        }
        scrollRectEx = null;
	}

	public int Index {
		set{
			index = value;
			gameObject.name = (index<10)?("0"+index):(""+index);
			if (scrollRectEx.onInitializeItem != null && index>=0) {
				scrollRectEx.onInitializeItem (gameObject,index);
			}
		}
		get{ 
			return index;
		}
	}

    public void SetItem()
    {
        if (rtf == null)
        {
            rtf = this.GetComponent<RectTransform>();
        }
        transform.localPosition = scrollRectEx.GetLocalPositionByIndex(index);
        if (index >= 0)
        {
            rtf.sizeDelta = scrollRectEx.GetSizeDeltaByIndex(index);
            scrollRectEx.SetUpdateContentSize();
        }

    }
}
