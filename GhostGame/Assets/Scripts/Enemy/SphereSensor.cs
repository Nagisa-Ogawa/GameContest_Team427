using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SphereSensor : MonoBehaviour
{
    private EnemyBase enemy;

    //�v���C���[�����m�͈͊O�ɂ��鎞��
    //��莞�Ԉȏ㊴�m�͈͊O�ɂ����ꍇ�A�ǐՏ�Ԃ���߂�
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
            //�ǐՏ�Ԃ���Ȃ���ΒǐՏ�ԂɕύX
            if (enemy.GetState() == EnemyBase.EnemyState.Idle)
            {
                enemy.SetState(EnemyBase.EnemyState.Chase, target.transform);
            }

            ResetOutSensorTime();
        }
    }

    

//#if UNITY_EDITOR
//    //�@�T�[�`����p�x�\��
//    private void OnDrawGizmos()
//    {
//        Handles.color = Color.red;
//        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
//    }
//#endif

}
