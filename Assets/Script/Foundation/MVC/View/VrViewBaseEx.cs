using UnityEngine;
using System.Collections;


namespace MFramework
{
    public abstract class VrViewBaseEx <T>: ViewBaseFU_EX, IMvvmView<T>
    {

        protected readonly BindProperty<IDataModel<T>> DataModelProperty = new BindProperty<IDataModel<T>>();
        public IDataModel<T> bindContex
        {
            get { return DataModelProperty.Value; }
            set { DataModelProperty.Value = value; }
        }


        //****�ر�ע�� ��Ҫ���������ȵ���    base.Awake(); 
        //***Ȼ���ٰ󶨵����ݲ� ��������� û�а��� OnValueChangedEvent2 ��OnValueChangedEvent �����

        protected override void Awake()
        {
            this.DataModelProperty.OnValueChangedEvent += OnBindingContextChanged;
            base.Awake();
            //Debug.Log("Add DataModel Property" + gameObject);
        }



        protected override void Dispose()
        {
            OnBindingContextChanged(bindContex, null);   //********** ����¼���  �������View������ ���¼����ɱ�ע����
            base.Dispose();
        }


        //when Bind DataModel Change
        protected abstract void OnBindingContextChanged(IDataModel<T> oldModel, IDataModel<T> newModel);


    }
}
