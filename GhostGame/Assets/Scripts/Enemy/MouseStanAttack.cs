using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStanAttack : MonoBehaviour
{

    private MouseEnemy mouse;

    // Start is called before the first frame update
    void Start()
    {
        mouse = transform.parent.GetComponent<MouseEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mouse.player.TakeStanDamage(5);
            Debug.Log("スタンダメージ５");

        }
    }
}
