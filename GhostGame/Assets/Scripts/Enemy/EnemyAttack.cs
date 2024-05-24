using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyBase enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.parent.GetComponent<EnemyBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        //�߈ˏ�Ԃ���Ȃ����
        if (enemy.GetState() != EnemyBase.EnemyState.Possession)
        {
            //�v���C���[�ɓ������Ă���_���[�W
            if (other.CompareTag("Player"))
            {
                enemy.player.TakeDamage(enemy.damage);
                Debug.Log("�_���[�W8");

            }
        }
        //�߈ˏ�Ԃ�������
        else
        {
            //�G�ɓ������Ă���_���[�W
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyBase>().TakeDamage(enemy.damage);
                Debug.Log("�_���[�W8");

            }
        }
    }

}
