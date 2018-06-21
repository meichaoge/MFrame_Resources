using UnityEngine;
using System.Collections;

namespace MFramework.UI.Layout
{
    public interface ILayoutEvent
    {
        void AddLayoutViewTransChangeListener(LayoutViewTransChangeHandle handle);
        void RemoveLayoutViewTransChangeListener(LayoutViewTransChangeHandle handle);
    }
}
