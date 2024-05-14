using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Freeze,
        Sleep
    }


    [SerializeField]
    int maxHp;
    int hp;
    [SerializeField]
    int maxStanPoint;
    int stanPoint;

    bool isStan;

    int damage;
    int stanDamage;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    int attackInterval;

    public PlayerTest player;

    protected EnemyState state;
    protected Transform targetTransform;



    // Start is called before the first frame update
    protected virtual void Start()
    {
        hp = maxHp;
        stanPoint = maxStanPoint;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerTest>();
        if (player != null)
            Debug.Log("ai");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {

        }
    }

    public void TakeStanDamage(int stanDamage)
    {
        stanPoint -= stanDamage;
        if(stanPoint <= 0)
        {

        }
    }

    public virtual void Attack() { }

    public virtual void StanAttack() { }


    public void SetState(EnemyState tempstate, Transform targetObject = null)
    {
        state = tempstate;
        targetTransform = targetObject;

    }

    public EnemyState GetState()
    {
        return state;
    }

}
