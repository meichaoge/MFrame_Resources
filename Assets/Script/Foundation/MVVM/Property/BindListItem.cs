using UnityEngine;
using System.Collections;
using System;

namespace MFramework
{

    public class BindListItem
    {
        public BindProperty<int> m_ItemIndex = new BindProperty<int>();
        public delegate void SubItemChange(object oldValu, int _index);

        public SubItemChange m_SubItemChangeHandle;
        protected readonly BindListItem OldValue;

        public BindListItem()
        {
            m_ItemIndex.Value = -1;
            OldValue = this;
        }

        //Call When this Object is BindListPrpperty's  SubItem then When object change Should Call BindList Containner
        protected virtual void OnItemValueChange()
        {
            if (m_SubItemChangeHandle != null)
            {
                m_SubItemChangeHandle(OldValue, m_ItemIndex.Value);
            }
        }


    }

}
