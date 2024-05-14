using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAttack : MonoBehaviour
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
        
        if (other.gameObject.tag == "Player")
        {
            mouse.player.TakeDamage(8);
            Debug.Log("É_ÉÅÅ[ÉW8");
        }
    }

}
