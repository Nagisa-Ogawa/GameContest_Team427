using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StanAttack : IState
{
    private PlayerController player;
    float stanAttackCD = 3.0f;     // �X�^���U���̃N�[���_�E��
    // float stanAttackRadius = 3.0f; // �X�^���U���̓G��ߑ����鋅�̂̔��a
    // float stanAttackOffset = 3.0f; // �ڋ߂��ăX�^���U������ۂ̓G�Ƃ̋����I�t�Z�b�g
    // float moveSpeed = 7.0f; // �G�ɐڋ߂���ۂ̑��x
    float lastAttackTime = 0.0f;
    // GameObject target = null;
    float setupTime = 1.0f; // �͈͍U���̏�������
    float hitTime = 0.1f;   // �͈͍U���̓����蔻������鎞��

    GameObject areaObj = null;
    GameObject areaModelObj = null;
    float addScale = 0.0f;
    float areaScale = 6.0f;
    float setupAlpha = 0.3f;
    float hitAlpha = 0.8f;

    //Vector3 startAngle = Vector3.zero;
    //float rotatePower = 1.0f;
    //float totalRotate = 0;
    public StanAttack(PlayerController player)
    {
        this.player = player;
        areaObj = GameObject.FindWithTag("PlayerStanArea");
        areaModelObj = areaObj.transform.GetChild(0).gameObject;
    }

    public void Enter()
    {
        // ���ݎ�������N���[�_�E�����������Ă��邩�`�F�b�N
        if ((Time.time - lastAttackTime) <= stanAttackCD)
        {
            // ���Ȃ��Ȃ�U������߂�
            player.Change(player.idle);
            return;
        }
        // �U���R���[�`���J�n
        player.StartCoroutine(StanAttackCoroutine());
    }

    public void Update()
    {

    }

    public void Exit()
    {
        // target = null;
    }

    //bool SearchEnemy()
    //{
    //    // �͈͓��ɂ���G��S�Ď擾
    //    var enemies = Physics.OverlapSphere(player.transform.position, stanAttackRadius).Where(coll => coll.tag == "Enemy");
    //    if (enemies.Count() == 0)
    //    {
    //        target = null;
    //        return false;
    //    }
    //    Debug.Log("enemy : " + enemies.Count());
    //    float minDistance = 999.0f;
    //    // ��ԋ߂��G���^�[�Q�b�g��
    //    foreach (var enemy in enemies)
    //    {
    //        float distance = (enemy.transform.position - player.transform.position).magnitude;
    //        if (minDistance >= distance)
    //        {
    //            minDistance = distance;
    //            target = enemy.gameObject;
    //        }
    //    }
    //    return true;
    //}

    IEnumerator StanAttackCoroutine()
    {
        // �U�������i1�b�ҋ@�j
        Coroutine coroutine = player.StartCoroutine(AreaGrowingAnimation());
        yield return coroutine;
        // �͈͍U���p���f���̐F��ύX
        Material mat = areaModelObj.GetComponent<MeshRenderer>().material;
        Color color = mat.color;
        color.a = hitAlpha;
        mat.color = color;
        // �͈̓X�^���U���p�R���W������\��
        areaObj.GetComponent<SphereCollider>().enabled = true;
        // �����ҋ@
        yield return new WaitForSeconds(hitTime);
        // �͈̓X�^���U���p�R���W�������\��
        areaObj.GetComponent<SphereCollider>().enabled = false;
        areaModelObj.SetActive(false);
        // ���ݎ������擾
        lastAttackTime = Time.time;
        // �U�����I��
        player.Change(player.idle);
    }


    /// <summary>
    /// �f�o�b�N�p�I�I
    /// �͈͍U���̃A�j���[�V�����p�R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator AreaGrowingAnimation()
    {
        // �������Ԃ���1�t���[�����Ƃɑ傫������l���Z�o
        addScale = areaScale / setupTime;
        areaModelObj.SetActive(true);
        areaModelObj.transform.localScale = Vector3.zero;
        Material mat = areaModelObj.GetComponent<MeshRenderer>().material;
        Color color=mat.color;
        color.a = setupAlpha;
        mat.color = color;
        while (true)
        {
            float addScalePerFrame = addScale * Time.deltaTime;
            areaModelObj.transform.localScale += new Vector3(addScalePerFrame, addScalePerFrame, addScalePerFrame);
            if (areaModelObj.transform.localScale.x >= areaScale)
            {
                areaModelObj.transform.localScale=new Vector3(areaScale,areaScale,areaScale);
                yield break;
            }
            yield return null;
        }
    }

    //IEnumerator Attack()
    //{
    //    // �U��������G�֐ڋ�
    //    Coroutine coroutine = player.StartCoroutine(MoveToEnemy());
    //    yield return coroutine;
    //    coroutine = player.StartCoroutine(MoveArm());
    //    yield return coroutine;
    //    // �U���G�t�F�N�g���쐬
    //    GameObject hitEffectObj = GameObject.Instantiate(player.StanHitEffectObj, target.transform.position, Quaternion.identity);
    //    Camera camera = Camera.main;
    //    hitEffectObj.transform.position = Vector3.Lerp(target.transform.position, camera.transform.position, 0.1f);
    //    ParticleSystem hitEffect = hitEffectObj.GetComponent<ParticleSystem>();
    //    hitEffect.Play();
    //    // �_���[�W��^����
    //    target.GetComponentInParent<EnemyBase>().TakeStanDamage(5);
    //    // ���ݎ������擾
    //    lastAttackTime = Time.time;
    //    // �U�����I��
    //    player.Change(player.idle);
    //    yield return null;
    //}


    //IEnumerator MoveToEnemy()
    //{
    //    Vector3 dir = target.transform.position - player.transform.position;
    //    dir.y = 0.0f;
    //    player.Rb.velocity = dir.normalized * moveSpeed;
    //    player.transform.forward = dir.normalized;
    //    while (true)
    //    {
    //        float distance = (target.transform.position - player.transform.position).magnitude;
    //        if (distance <= stanAttackOffset)
    //        {
    //            player.Rb.velocity = Vector3.zero;
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}

    //IEnumerator MoveArm()
    //{
    //    startAngle = player.PlayerArmObj.transform.localEulerAngles;
    //    while (true)
    //    {
    //        Vector3 angle = player.PlayerArmObj.transform.localEulerAngles;
    //        angle.x += rotatePower;
    //        angle.y -= rotatePower;
    //        player.PlayerArmObj.transform.localEulerAngles = angle;
    //        totalRotate += rotatePower;
    //        if (totalRotate > 120.0f)
    //        {
    //            player.PlayerArmObj.transform.localEulerAngles = startAngle;
    //            totalRotate = 0.0f;
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}
}



