using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBullet : MonoBehaviour
{
    public Vector3 direction;

    [SerializeField]
    private int stanDamage = 5;

    [SerializeField]
    private float speed = 10.0f;

    private float activeTime;

    // Start is called before the first frame update
    void Start()
    {
        activeTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        activeTime += Time.deltaTime;
        if(activeTime >= speed)
        {
            Destroy(gameObject);
        }
    }

   public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if(other.GetComponentInParent<EnemyBase>().GetState() != EnemyBase.EnemyState.Possession)
            {
                other.GetComponentInParent<EnemyBase>().TakeStanDamage(stanDamage);
                Destroy(gameObject);
            }
        }
    }
}
