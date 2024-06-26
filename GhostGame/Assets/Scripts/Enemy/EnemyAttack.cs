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
      
        //エネミーが憑依状態じゃなければ
        if (enemy.GetState() != EnemyBase.EnemyState.Possession)
        {
            //プレイヤーが憑依している敵がいなかったら
            if(player.GetPossessionEnemy() == null)
            {
                //プレイヤーに当たってたらダメージ
                if (other.CompareTag("Player"))
                {
                    player.TakeDamage(enemy.damage);
                    Debug.Log("ダメージ8");

                }
            }
            else
            {
                //エネミーに当たってて
                if (other.CompareTag("Enemy"))
                {
                    //そのエネミーが憑依状態だったら
                    if(other.gameObject.GetComponentInParent<EnemyBase>().GetState() == EnemyBase.EnemyState.Possession)
                    {
                        Debug.Log("エネミーがエネミーにダメージ");
                        player.GetPossessionEnemy().GetComponent<EnemyBase>().TakeDamage(enemy.damage);
                    }
                }
            }
            
        }
        //憑依状態だったら
        else
        {
            //敵に当たってたらダメージ
            if (other.CompareTag("Enemy"))
            {
                other.GetComponentInParent<EnemyBase>().TakeDamage(enemy.damage);
                Debug.Log("ダメージ8");
            }
        }
    }

}
