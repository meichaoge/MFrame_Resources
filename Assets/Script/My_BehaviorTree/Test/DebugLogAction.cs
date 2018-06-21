using MFramework;
using MFramework.BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BehaviorActionAttribute("Action/Debug/Log",typeof(DebugLogAction))]
public class DebugLogAction : NodeActionParent
{
    [NodeAttribute("Log",NodeInOutEnum.InOut,true, NodeValueTypeEnum.String,"","Log the String to Console")]
    public string LogString;

    [NodeAttribute("Log_Test", NodeInOutEnum.In, true, NodeValueTypeEnum.Int, 0, "TestShow")]
    public int LogInt;
}
