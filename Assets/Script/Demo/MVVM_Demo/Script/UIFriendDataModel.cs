using MFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIFriendDataModel : Singleton_Static<UIFriendDataModel>, IDataModel<FriendCfg>
{
    public BindListProperty<FriendCfg> CurViewBindDataBase { private set; get; } //视图层绑定的数据
 


    protected override void InitialSingleton()
    {
        CurViewBindDataBase = new BindListProperty<FriendCfg>();
    }



    public FriendCfg GetDataByIndex(int dex)
    {
        if (dex < 0 || dex >= CurViewBindDataBase.Count) return null;
        return CurViewBindDataBase[dex];
    }

    public FriendCfg GetFriendCfgByID(long id)
    {
        for (int _dex = 0; _dex < CurViewBindDataBase.Count; ++_dex)
        {
            if (CurViewBindDataBase[_dex].m_ID == id)
                return CurViewBindDataBase[_dex];
        }
        return null;
    }

}
