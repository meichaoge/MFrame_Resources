using UnityEngine;
using System.Collections;

namespace MFramework
{
    public interface IMvvmView<T>
    {
        IDataModel<T> bindContex { get; set; }
    }
}
