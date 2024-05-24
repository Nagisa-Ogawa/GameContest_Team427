using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : IState
{
    //�߈˃X�e�[�g�X�N���v�g


    private PlayerController player;
    private Camera camera;

    Rigidbody rb;

    public Vector3 velocity;


    public Possession(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        rb = player.Rb;
        camera = Camera.main;

    }

    public void Update()
    {

        if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
        {
            velocity = new Vector3(0, 0, 0);
            // �J�������猩�����E�ƑO��̓��͒l���󂯎��
            velocity += Input.GetAxis("Horizontal") * new Vector3(camera.transform.right.x, 0.0f, camera.transform.right.z).normalized;
            velocity += Input.GetAxis("Vertical") * new Vector3(camera.transform.forward.x, 0.0f, camera.transform.forward.z).normalized;
            velocity = velocity.normalized * player.Speed;
            if (velocity != Vector3.zero)
            {
                // �v���C���[�̌������ړ������֌�������
                //player.GetEnemy().transform.forward = velocity.normalized;
            }

            //player.Rb.velocity = velocity;

            player.GetPossessionEnemy().GetComponent<Rigidbody>().velocity = velocity;

            Quaternion setRotation = Quaternion.LookRotation(velocity);
            //�Z�o���������̊p�x�ɉ�]
            player.GetPossessionEnemy().transform.rotation = Quaternion.Slerp(player.GetPossessionEnemy().transform.rotation, setRotation, 5.0f * Time.deltaTime);

        }

        player.transform.position = player.GetPossessionEnemy().transform.position - player.GetPossessionEnemy().transform.forward * 1.5f + player.transform.up * 1.0f;

        if (Input.GetKeyDown("j"))
        {
            player.GetPossessionEnemy().GetComponent<EnemyBase>().SetState(EnemyBase.EnemyState.Idle);
            player.ResetPossessionEnemy();
            player.GetComponent<CapsuleCollider>().isTrigger = false;
            player.Change(player.idle);
            

        }

        if (Input.GetKey("i"))
        {
            player.GetPossessionEnemy().GetComponent<EnemyBase>().Attack();
        }
        if (Input.GetKey("o"))
        {
            player.GetPossessionEnemy().GetComponent<EnemyBase>().StanAttack();
        }

    }

    public void Exit()
    {

    }

}
