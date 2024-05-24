using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanAttackArea : MonoBehaviour
{
    [SerializeField]
    int stanDamage = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            Debug.Log("Hit");
            // ƒXƒ^ƒ“‚Ìˆ—
            EnemyBase enemy=other.gameObject.GetComponentInParent<EnemyBase>();
            enemy.TakeStanDamage(stanDamage);
        }
    }
}
