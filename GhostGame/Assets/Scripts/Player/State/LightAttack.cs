using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace personal
{
    public class LightAttack : IState
    {
        private PlayerController player;
        int lightAttackDamage = 3;      // ��U���̃_���[�W��
        float lightAttackCD = 1.0f;     // ��U���̃N�[���_�E��
        float lightAttackRadius = 3.0f; // ��U���̓G��ߑ����鋅�̂̔��a
        float lightAttackOffset = 2.0f; // �ڋ߂��Ď�U������ۂ̓G�Ƃ̋����I�t�Z�b�g
        float moveSpeed = 7.0f; // �G�ɐڋ߂���ۂ̑��x
        float lastAttackTime = 0.0f;
        GameObject target = null;

        Vector3 startAngle = Vector3.zero;
        float rotatePower = 1.0f;
        float totalRotate = 0;
        public LightAttack(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            // ���ݎ�������N���[�_�E�����������Ă��邩�`�F�b�N
            if ((Time.time - lastAttackTime) <= lightAttackCD)
            {
                // ���Ȃ��Ȃ�U������߂�
                player.Change(player.idle);
                return;
            }
            // �N�[���_�E���Ȃ�U������߂�
            // ���͂ɍU�����͂��G�����邩�`�F�b�N
            if (!SearchEnemy())
            {
                // ���Ȃ��Ȃ�U������߂�
                player.Change(player.idle);
                return;
            }
            else
            {
                // �U���R���[�`���J�n
                player.StartCoroutine(Attack());
            }
        }

        public void Update()
        {

        }

        public void Exit()
        {
            target = null;
        }

        bool SearchEnemy()
        {
            // �͈͓��ɂ���G��S�Ď擾
            var enemies = Physics.OverlapSphere(player.transform.position, lightAttackRadius).Where(coll => coll.tag == "Enemy");
            if (enemies.Count() == 0)
            {
                target = null;
                return false;
            }
            Debug.Log("enemy : "+enemies.Count());
            float minDistance = 999.0f;
            // ��ԋ߂��G���^�[�Q�b�g��
            foreach (var enemy in enemies)
            {
                float distance = (enemy.transform.position - player.transform.position).magnitude;
                if (minDistance>=distance)
                {
                    minDistance= distance;
                    target = enemy.gameObject;
                }
            }
            return true;
        }

        IEnumerator Attack()
        {
            // �U��������G�֐ڋ�
            Coroutine coroutine = player.StartCoroutine(MoveToEnemy());
            yield return coroutine;
            coroutine = player.StartCoroutine(MoveArm());
            yield return coroutine;
            // �U���G�t�F�N�g���쐬
            GameObject hitEffectObj = GameObject.Instantiate(player.HitEffectObj,target.transform.position,Quaternion.identity);
            Camera camera=Camera.main;
            hitEffectObj.transform.position = Vector3.Lerp(target.transform.position, camera.transform.position, 0.1f);
            ParticleSystem hitEffect=hitEffectObj.GetComponent<ParticleSystem>();
            hitEffect.Play();
            // �_���[�W��^����
            // target.SetActive(false);
            // ���ݎ������擾
            lastAttackTime = Time.time;
            // �U�����I��
            player.Change(player.idle);
            yield return null;
        }

        IEnumerator MoveToEnemy()
        {
            Vector3 dir = target.transform.position - player.transform.position;
            dir.y = 0.0f;
            player.Rb.velocity = dir.normalized * moveSpeed;
            player.transform.forward = dir.normalized;
            while (true)
            {
                float distance = (target.transform.position - player.transform.position).magnitude;
                if (distance <= lightAttackOffset)
                {
                    player.Rb.velocity = Vector3.zero;
                    yield break;
                }
                yield return null;
            }
        }

        IEnumerator MoveArm()
        {
            startAngle = player.PlayerArmObj.transform.localEulerAngles;
            while (true)
            {
                Vector3 angle = player.PlayerArmObj.transform.localEulerAngles;
                angle.x += rotatePower;
                angle.y -= rotatePower;
                player.PlayerArmObj.transform.localEulerAngles = angle;
                totalRotate += rotatePower;
                if (totalRotate > 120.0f)
                {
                    player.PlayerArmObj.transform.localEulerAngles = startAngle;
                    totalRotate = 0.0f;
                    yield break;
                }
                yield return null;
            }
        }
    }
}

