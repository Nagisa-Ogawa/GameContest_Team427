using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIOnOff : MonoBehaviour
{
    public GameObject obj;
    private EnemyBase enemy;



    // Start is called before the first frame update
    void Start()
    {
        obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.hp < enemy.maxHp)
        {
            obj.SetActive(true);

        }
       

        if(enemy.stanPoint < enemy.maxStanPoint)
        {
            obj.SetActive(true);
        }
       
    }

    public void SetUIEnemy(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

}
