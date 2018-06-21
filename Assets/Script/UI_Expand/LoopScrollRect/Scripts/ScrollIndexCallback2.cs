﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollIndexCallback2 : LoopScrollRectItemBase
{
    public Text text;
    public LayoutElement element;
    private static float[] randomWidths = new float[3] { 100, 150, 50 };

    public override void ScrollCellIndex(int idx)
    {
        base.ScrollCellIndex(idx);
        if (text != null)
        {
            text.text = name;
        }
        element.preferredWidth = randomWidths[Mathf.Abs(idx) % 3];
    }
}
