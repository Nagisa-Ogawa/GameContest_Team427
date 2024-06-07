using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class SpiderEnemy : EnemyBase
{
    [SerializeField]
    BoxCollider attackCollider;
    [SerializeField]
    BoxCollider stanAttackCollider;

    [SerializeField]
    float attackRange = 0.8f;
    [SerializeField]
    GameObject bullet;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        SetState(EnemyState.Idle);

        //StartCoroutine("AttackTest");


    }

    // Update is called once per frame
    protected override void Update()
    {
        if (state == EnemyState.Chase)
        {
            if (targetTransform == null)
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
        else if (state == EnemyState.Attack)
        {
            if (workingAttackCoroutine == null)
            {
                Attack();
            }

        }


    }

    public override void Attack()
    {
        base.Attack();

        StartCoroutine("AttackCoroutine");
    }

    public override void PossessionAttack()
    {
        base.PossessionAttack();

        if (workingAttackCoroutine == null)
        {
            workingAttackCoroutine = StartCoroutine("PossessionAttackCoroutine");
        }
    }

    public override void PossessionStanAttack()
    {
        base.PossessionStanAttack();

        if (workingAttackCoroutine == null)
        {
            workingAttackCoroutine = StartCoroutine("PossessionStanAttackCoroutine");
        }
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.6f);

        EnableAttackCollider();

        PlayAttackEffect(attackCollider.gameObject);

        yield return new WaitForSeconds(1.2f);

        DisableAttackCollider();

        yield return new WaitForSeconds(1.0f);

        workingAttackCoroutine = null;

        SetState(EnemyState.Chase, targetTransform);
    }

    private IEnumerator PossessionAttackCoroutine()
    {
        yield return new WaitForSeconds(0.6f);

        EnableAttackCollider();

        PlayAttackEffect(attackCollider.gameObject);

        yield return new WaitForSeconds(1.2f);

        DisableAttackCollider();

        yield return new WaitForSeconds(1.0f);

        workingAttackCoroutine = null;
    }



    private IEnumerator PossessionStanAttackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 pos = transform.position + transform.forward*2.0f;
        GameObject go;
       go = Instantiate(bullet,pos,Quaternion.identity);
        Vector3 dir = targetTransform.position - transform.position;
        dir = dir.normalized;
        go.GetComponent<SpiderBullet>().SetDirection(dir);
        //yield return new WaitForSeconds(1.5f);

        //DisableStanAttackCollider();

        yield return new WaitForSeconds(1.0f);

        workingAttackCoroutine = null;
    }


    public void EnableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }
    }

    public void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    public void EnableStanAttackCollider()
    {
        if (stanAttackCollider != null)
        {
            stanAttackCollider.enabled = true;
        }
    }

    public void DisableStanAttackCollider()
    {
        if (stanAttackCollider != null)
        {
            stanAttackCollider.enabled = false;
        }
    }
}
