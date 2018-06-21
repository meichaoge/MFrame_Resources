using UnityEngine;
using System.Collections;

namespace MFramework
{
    public interface IObjectPool<T> where T : new()
    {
        string SourcePath { get; set; }
        uint MaxCount { get; set; }

        //Recycle The Source ,before setActive(false) do action
        void Recycle(T item, object obj = null, System.Action<T> action = null);

        //Get Resources of the special Name Source,And Do Action Beforem Returen
        T GetInstance(string itemName, object obj = null, Transform parent = null, System.Action<T> action = null);


    }
}
