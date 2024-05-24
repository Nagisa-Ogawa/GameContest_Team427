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
        Sleep,
        Possession
    }


    [SerializeField]
    int maxHp;
    int hp;
    [SerializeField]
    int maxStanPoint;
    int stanPoint;

    bool isStan;

    public int damage;
    public int stanDamage;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    int attackInterval;

    public PlayerController player;

    protected EnemyState state;
    protected Transform targetTransform;

    //�U����Ȃǂ̍d������
    protected float freezeTime;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        hp = maxHp;
        stanPoint = maxStanPoint;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //�S�G�l�~�[���ʁH
        if(state == EnemyState.Freeze)
        {
            //�d�����Ԍ���
            freezeTime -= Time.deltaTime;

            //�d�����Ԃ��I��������
            if(freezeTime <= 0.0f)
            {
                //idle�ɖ߂�
                freezeTime = 0.0f;
                SetState(EnemyState.Idle);
            }
        }
    }

    public void TakeDamage(int damage)
    {
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
