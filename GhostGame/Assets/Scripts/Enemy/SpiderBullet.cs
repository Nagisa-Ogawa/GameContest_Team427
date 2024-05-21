using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBullet : MonoBehaviour
{
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * 10.0f*Time.deltaTime;
    }
   public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
    private void OnTriggerEnter(Collider player)
    {
        if (player.transform.tag == "Player" )
        {
            player.GetComponent<PlayerController>().TakeStanDamage(5);
            Destroy(gameObject);
        }
    }
}
