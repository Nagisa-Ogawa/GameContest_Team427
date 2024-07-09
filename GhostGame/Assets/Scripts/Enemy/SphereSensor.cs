using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SphereSensor : MonoBehaviour
{
    //プレイヤーが感知範囲外にいる時間
    //一定時間以上感知範囲外にいた場合、追跡状態をやめる
    private float outSensorTime  = 0;

    [SerializeField]
    private float chaseEndTime = 5.0f;

    //感知範囲内に入っているか
    private bool isEnter;

    // Start is called before the first frame update
    void Start()
    {
        isEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnter)
        {
            outSensorTime += Time.deltaTime;
            if (outSensorTime >= chaseEndTime)
            {
                isEnter = false;
            }
        }
    }

    public void ResetOutSensorTime()
    {
        outSensorTime = 0.0f;
    }

    private void OnTriggerStay(Collider target)
    {
        isEnter = true;
    }

    public bool GetIsEnter()
    {
        return isEnter;
    }
    
}
