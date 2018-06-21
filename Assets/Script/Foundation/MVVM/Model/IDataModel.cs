using UnityEngine;
using System.Collections;

namespace MFramework
{
    public interface IDataModel<T>
    {
        BindListProperty<T> CurViewBindDataBase { get; }

        T GetDataByIndex(int dex);

    }
}
