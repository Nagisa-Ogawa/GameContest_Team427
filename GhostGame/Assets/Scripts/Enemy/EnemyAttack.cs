using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyBase enemy;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.parent.GetComponent<EnemyBase>();
        player = enemy.player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
      
        //�G�l�~�[���߈ˏ�Ԃ���Ȃ����
        if (enemy.GetState() != EnemyBase.EnemyState.Possession)
        {
            //�v���C���[���߈˂��Ă���G�����Ȃ�������
            if(player.GetPossessionEnemy() == null)
            {
                //�v���C���[�ɓ������Ă���_���[�W
                if (other.CompareTag("Player"))
                {
                    player.TakeDamage(enemy.damage);
                    Debug.Log("�_���[�W8");

                }
            }
            else
            {
                //�G�l�~�[�ɓ������Ă�
                if (other.CompareTag("Enemy"))
                {
                    //���̃G�l�~�[���߈ˏ�Ԃ�������
                    if(other.gameObject.GetComponentInParent<EnemyBase>().GetState() == EnemyBase.EnemyState.Possession)
                    {
                        Debug.Log("�G�l�~�[���G�l�~�[�Ƀ_���[�W");
                        player.GetPossessionEnemy().GetComponent<EnemyBase>().TakeDamage(enemy.damage);
                    }
                }
            }
            
        }
        //�߈ˏ�Ԃ�������
        else
        {
            //�G�ɓ������Ă���_���[�W
            if (other.CompareTag("Enemy"))
            {
                other.GetComponentInParent<EnemyBase>().TakeDamage(enemy.damage);
                Debug.Log("�_���[�W8");
            }
        }
    }

}
