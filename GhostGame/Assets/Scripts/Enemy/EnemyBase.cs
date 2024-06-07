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
    public int maxHp;
    public int hp;
    [SerializeField]
    int maxStanPoint;
    int stanPoint;

    bool isStan;
    public float stanTime;

    int damage;
    int stanDamage;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    int attackInterval;

    public PlayerController player;

    protected EnemyState state;
    protected Transform targetTransform;
    protected EnemyGage enemyGage;



    // Start is called before the first frame update
    protected virtual void Start()
    {
        hp = maxHp;
        stanPoint = maxStanPoint;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
            Debug.Log("ai");

        //Object‚ðŽæ“¾  Ž©•ª‚ÌŽq‹Ÿ‚ÌEnemyGage‚Ì‚Ý‚ðŽæ“¾
        enemyGage = transform.Find("EnemyHPUI").transform.Find("EnemyGage").GetComponent<EnemyGage>();
        enemyGage.SetEnemy(this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        enemyGage.GageReduction(damage);

        hp -= damage;
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeStanDamage(int stanDamage)
    {
        stanPoint -= stanDamage;
        if(stanPoint <= 0)
        {
            SetState(EnemyState.Freeze);

        }
    }

    public virtual void Attack() { }

    public virtual void StanAttack() { }

    public virtual void Freeze() { }

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
