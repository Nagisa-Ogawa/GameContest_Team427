using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SphereSensor : MonoBehaviour
{
    private EnemyBase enemy;

    //プレイヤーが感知範囲外にいる時間
    //一定時間以上感知範囲外にいた場合、追跡状態をやめる
    private float outSensorTime  = 0;

    [SerializeField]
    private float chaseEndTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.parent.GetComponent<EnemyBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.GetState() == EnemyBase.EnemyState.Chase)
        {
            outSensorTime += Time.deltaTime;
            if (outSensorTime >= chaseEndTime)
            {
                enemy.SetState(EnemyBase.EnemyState.Idle);
            }
        }
    }

    public void ResetOutSensorTime()
    {
        outSensorTime = 0.0f;
    }

    private void OnTriggerStay(Collider target)
    {
        if(target.gameObject.tag == "Player")
        {
            //追跡状態じゃなければ追跡状態に変更
            if (enemy.GetState() == EnemyBase.EnemyState.Idle)
            {
                enemy.SetState(EnemyBase.EnemyState.Chase, target.transform);
            }

            ResetOutSensorTime();
        }
    }

    

//#if UNITY_EDITOR
//    //　サーチする角度表示
//    private void OnDrawGizmos()
//    {
//        Handles.color = Color.red;
//        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
//    }
//#endif

}
