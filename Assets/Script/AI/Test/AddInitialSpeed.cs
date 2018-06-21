using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFramework.AI;

/// <summary>
/// 添加一个初始速度  
/// </summary>
public class AddInitialSpeed : MonoBehaviour
{
    private Vehicle m_Vehicle;
    public float m_Speed = 5;

    // Use this for initialization
    void Start()
    {
        m_Vehicle = GetComponent<Vehicle>();
        //给一个随机的速度 Test
        m_Vehicle.m_Velocity = new Vector3(Random.value, Random.value, Random.value) * m_Speed;
    }


}
