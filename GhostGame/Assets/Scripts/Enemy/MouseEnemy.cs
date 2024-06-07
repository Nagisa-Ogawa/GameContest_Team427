using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class MouseEnemy : EnemyBase
{

    private NavMeshAgent navMeshAgent;
    private Vector3 destination;

    [SerializeField]
    BoxCollider attackCollider;
    [SerializeField]
    BoxCollider stanAttackCollider;

    [SerializeField]
    float attackRange = 1.0f;

    //攻撃行動中かどうか
    bool isAttack = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        navMeshAgent = GetComponent<NavMeshAgent>();

        SetState(EnemyState.Idle);

        //StartCoroutine("AttackTest");


    }

    // Update is called once per frame
    protected override void Update()
    {
        if(state == EnemyState.Chase)
        {
            if(targetTransform == null)
            {
                SetState(EnemyState.Idle);
            }
            else
            {
                //プレイヤーへのベクトル
                Vector3 vec = (targetTransform.transform.position - transform.position);

                //距離取得
                float distance = vec.magnitude;

                //正規化ベクトル
                Vector3 dir = vec.normalized;
                dir.y = 0;

                //攻撃範囲距離内だったら
                if (distance <= attackRange)
                {
                    SetState(EnemyState.Attack, targetTransform);
                }
                else
                {
                    //エネミー移動
                    transform.position += dir * moveSpeed * Time.deltaTime;

                }

                Quaternion setRotation = Quaternion.LookRotation(dir);
                //算出した方向の角度に回転
                transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, 5.0f * Time.deltaTime);
            }

        }
        else if(state == EnemyState.Attack)
        {
            if(!isAttack)
            {
                int temp = Random.Range(0, 2);
                if (temp == 0)
                {
                    Attack();
                    Debug.Log("attack");
                }
                else
                {
                    StanAttack();
                    Debug.Log("stanattack");
                }

                isAttack = true;
            }
            
        }
        else if(state == EnemyState.Freeze)
        {
            Freeze();
            Debug.Log("freeze");
        }

    }

    public override void Attack()
    {
        base.Attack();

        StartCoroutine("AttackCoroutine");
    }

    public override void StanAttack()
    {
        base.StanAttack();

        StartCoroutine("StanAttackCoroutine");        
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.6f);

        EnableAttackCollider();

        yield return new WaitForSeconds(1.2f);

        DisableAttackCollider();

        yield return new WaitForSeconds(1.0f);

        SetState(EnemyState.Chase, targetTransform);

        isAttack = false;
    }

    private IEnumerator StanAttackCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        EnableStanAttackCollider();

        yield return new WaitForSeconds(1.5f);

        DisableStanAttackCollider();

        yield return new WaitForSeconds(1.0f);

        SetState(EnemyState.Chase, targetTransform);

        isAttack = false;

    }

    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    public Vector3 GetDestination()
    {
        return destination;
    }


    public void EnableAttackCollider()
    {
        if(attackCollider != null)
        {
            attackCollider.enabled = true;
           // Debug.Log("attack enable");

        }
    }

    public void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
           // Debug.Log("attack disable");
        }
    }

    public void EnableStanAttackCollider()
    {
        if (stanAttackCollider != null)
        {
            stanAttackCollider.enabled = true;
            //Debug.Log("stanattack enable");

        }
    }

    public void DisableStanAttackCollider()
    {
        if (stanAttackCollider != null)
        {
            stanAttackCollider.enabled = false;
           // Debug.Log("stanattack disable");
        }
    }

    public override void Freeze()
    {
        base.Freeze();
        stanTime += Time.deltaTime;

        if (stanTime >= 5.0f)
        {
            SetState(EnemyState.Idle);
        }
    }

}
