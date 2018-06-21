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


        //****特别注意 需要在子类中先调用    base.Awake(); 
        //***然后再绑定到数据层 ，否则出现 没有绑定上 OnValueChangedEvent2 、OnValueChangedEvent 的情况

        protected override void Awake()
        {
            this.DataModelProperty.OnValueChangedEvent += OnBindingContextChanged;
            base.Awake();
            //Debug.Log("Add DataModel Property" + gameObject);
        }



        protected override void Dispose()
        {
            OnBindingContextChanged(bindContex, null);   //********** 解除事件绑定  否则如果View被销毁 则事件依旧被注册了
            base.Dispose();
        }


        //when Bind DataModel Change
        protected abstract void OnBindingContextChanged(IDataModel<T> oldModel, IDataModel<T> newModel);


    }
}
