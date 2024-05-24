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

        //憑依状態じゃなければ
        if (enemy.GetState() != EnemyBase.EnemyState.Possession)
        {
            //プレイヤーに当たってたらダメージ
            if (other.CompareTag("Player"))
            {
                enemy.player.TakeDamage(enemy.damage);
                Debug.Log("ダメージ8");

            }
        }
        //憑依状態だったら
        else
        {
            //敵に当たってたらダメージ
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyBase>().TakeDamage(enemy.damage);
                Debug.Log("ダメージ8");

            }
        }
    }

}
