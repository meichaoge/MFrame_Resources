using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FriendCfg : BaseJsonObject<FriendCfg>
{
    public long m_ID;
    public string m_Name = "";   //名字
    public string m_ImageName = ""; //头像图标
    public int m_RoomID;  //所在房间
    public string m_Description = "";  //描述
    public bool m_Sex;  //角色性别

    //6_23 新增  version=2.0.5
    public object NetOperate = null;  //网络消息属性 处理邮件时需要关注


}