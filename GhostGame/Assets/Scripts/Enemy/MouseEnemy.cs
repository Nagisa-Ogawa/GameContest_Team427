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
    float attackRange = 3.0f;

    //�U���s�������ǂ���
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
        base.Update();

        if(state == EnemyState.Chase)
        {
            if(targetTransform == null)
            {
                SetState(EnemyState.Idle);
            }
            else
            {
                //�v���C���[�ւ̃x�N�g��
                Vector3 vec = (targetTransform.transform.position - transform.position);

                //�����擾
                float distance = vec.magnitude;

                //���K���x�N�g��
                Vector3 dir = vec.normalized;
                dir.y = 0;

                //�U���͈͋�������������
                if (distance <= attackRange)
                {
                    SetState(EnemyState.Attack, targetTransform);
                }
                else
                {
                    //�G�l�~�[�ړ�
                    transform.position += dir * moveSpeed * Time.deltaTime;

                }

                Quaternion setRotation = Quaternion.LookRotation(dir);
                //�Z�o���������̊p�x�ɉ�]
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
                }
                else
                {
                    StanAttack();
                }

                isAttack = true;
            }
            
        }
        else if(state == EnemyState.Possession)
        {

        }
        

    }

    public override void Attack()
    {
        base.Attack();

        workingAttackCoroutine = StartCoroutine("AttackCoroutine");
    }

    public override void StanAttack()
    {
        base.StanAttack();

        workingAttackCoroutine = StartCoroutine("StanAttackCoroutine");        
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.6f);

        EnableAttackCollider();

        yield return new WaitForSeconds(0.2f);

        DisableAttackCollider();

        isAttack = false;

        SetState(EnemyState.Idle);
    }

    private IEnumerator StanAttackCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        EnableStanAttackCollider();

        yield return new WaitForSeconds(0.2f);

        DisableStanAttackCollider();

        isAttack = false;

        SetState(EnemyState.Idle);

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
}
