using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class BirdEnemy : EnemyBase
{
    [SerializeField]
    BoxCollider attackCollider;
    [SerializeField]
    BoxCollider stanAttackCollider;

    [SerializeField]
    float attackRange = 1.0f;

    private bool isAttack;

    //�ːi�U���̌o�ߎ���
    private float attackElapsedTime;
    //�ːi�U���̃X�s�[�h
    [SerializeField]
    private float chargeSpeed = 5.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        SetState(EnemyState.Idle);
        isAttack = false;
        attackElapsedTime = 0.0f;
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
        else if (state == EnemyState.Attack)
        {
            if (workingAttackCoroutine == null)
            {
                Attack();
            }
            else
            {
                //�ːi����
                transform.position += transform.forward * chargeSpeed * Time.deltaTime;
                attackElapsedTime += Time.deltaTime;

                //���莞�ԓːi������
                if(attackElapsedTime >= 0.75f)
                {
                    isAttack = false;
                    workingAttackCoroutine = null;
                    SetState(EnemyState.Idle);
                }
            }

        }
    }

    public override void Attack()
    {
        base.Attack();

        if (workingAttackCoroutine == null)
        {
            workingAttackCoroutine = StartCoroutine("AttackCoroutine");
        }

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

        isAttack = true;

        attackElapsedTime = 0.0f;
    }

    private IEnumerator PossessionAttackCoroutine()
    {
        yield return new WaitForSeconds(0.6f);

        EnableAttackCollider();

        PlayAttackEffect(attackCollider.gameObject);

        yield return new WaitForSeconds(0.2f);

        DisableAttackCollider();

        yield return new WaitForSeconds(1.0f);

        workingAttackCoroutine = null;
    }



    private IEnumerator PossessionStanAttackCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        EnableStanAttackCollider();

        PlayAttackEffect(stanAttackCollider.gameObject);

        yield return new WaitForSeconds(1.5f);

        DisableStanAttackCollider();

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

    public bool GetIsAttack()
    {
        return isAttack;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != EnemyState.Possession)
        {
            if (isAttack)
            {
                if (collision.transform.tag == "Player")
                {
                    player.TakeDamage(damage);
                }
            }
        }
        else
        {
            if (isAttack)
            {
                if (collision.transform.tag == "Enemy")
                {
                    collision.transform.GetComponent<EnemyBase>().TakeDamage(damage);
                }
            }
        }
        
    }
}
