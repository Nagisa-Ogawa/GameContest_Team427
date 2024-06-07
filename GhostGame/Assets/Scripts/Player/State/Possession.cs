using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : IState
{
    //�߈˃X�e�[�g�X�N���v�g


    private PlayerController player;
    private Camera camera;
    Vector2 moveInput = Vector2.zero;

    Rigidbody rb;

    public Vector3 velocity;
    GameObject possEnemy = null;


    public Possession(PlayerController player)
    {
        this.player = player;
        rb=player.GetComponent<Rigidbody>();
    }

    public void Enter()
    {
        camera = Camera.main;
        possEnemy = player.GetPossessionEnemy();
        // ���̂̃p�����[�^��ύX
        possEnemy.GetComponent<Rigidbody>().drag = 0;
        possEnemy.GetComponent<Rigidbody>().angularDrag = 0.5f;
        // �߈˂����G�̐F��߂�
        // �F��߂�
        GameObject model = possEnemy.transform.Find("Mouse/default").gameObject;
        Material mat = model.GetComponent<MeshRenderer>().material;
        mat.color = possEnemy.GetComponent<EnemyBase>().NormalColor;
        StanAllowUIManager stanAllowUIManager = GameObject.FindWithTag("StanAllowUIManager").GetComponent<StanAllowUIManager>();
        stanAllowUIManager.DeleteEnemyList(possEnemy);

    }

    public void Update()
    {
        //�߈˂��Ă���G�l�~�[����A�N�e�B�u��������(�|���ꂽ��)
        if(possEnemy.activeInHierarchy == false)
        {
            player.GetComponent<PlayerController>().Change(player.idle);
            player.GetComponent<PlayerController>().ResetPossessionEnemy();
            return;
        }

        velocity = new Vector3(0, 0, 0);

        //�߈˂��Ă���G�l�~�[���U��������Ȃ���Γ��͂��󂯎��
        if(possEnemy.GetComponent<EnemyBase>().GetWorkingAttackCoroutine() == null)
        {
            moveInput = player.PlayerInput.currentActionMap["Move"].ReadValue<Vector2>();
        }
        
        // �J�������猩�����E�ƑO��̓��͒l���󂯎��
        velocity += moveInput.x * new Vector3(camera.transform.right.x, 0.0f, camera.transform.right.z).normalized;
        velocity += moveInput.y * new Vector3(camera.transform.forward.x, 0.0f, camera.transform.forward.z).normalized;
        velocity = velocity.normalized * player.Speed;
        if (velocity != Vector3.zero)
        {
            // �v���C���[�̌������ړ������֌�������
            //player.GetEnemy().transform.forward = velocity.normalized;

            Quaternion setRotation = Quaternion.LookRotation(velocity);
            //�Z�o���������̊p�x�ɉ�]
            possEnemy.transform.rotation = Quaternion.Slerp(player.GetPossessionEnemy().transform.rotation, setRotation, 10.0f * Time.deltaTime);
            player.transform.rotation = Quaternion.Slerp(player.GetPossessionEnemy().transform.rotation, setRotation, 10.0f * Time.deltaTime);

        }

        //player.Rb.velocity = velocity;
        possEnemy.GetComponent<Rigidbody>().velocity = velocity;



        player.transform.position = player.GetPossessionEnemy().transform.position - player.GetPossessionEnemy().transform.forward * 1.5f + player.transform.up * 1.0f;

        if (player.PlayerInput.currentActionMap["PossessionCancel"].IsPressed())
        {
            possEnemy.GetComponent<EnemyBase>().SetState(EnemyBase.EnemyState.Idle);
            player.ResetPossessionEnemy();
            player.GetComponent<CapsuleCollider>().isTrigger = false;
            player.Change(player.idle);
            

        }

        if (player.PlayerInput.currentActionMap["LightAttack"].WasPressedThisFrame())
        {
            possEnemy.GetComponent<EnemyBase>().PossessionAttack();
        }

        if (player.PlayerInput.currentActionMap["StanAttack"].WasPressedThisFrame())
        {
            possEnemy.GetComponent<EnemyBase>().PossessionStanAttack();
        }

    }

    public void Exit()
    {
        possEnemy.GetComponent<Rigidbody>().drag = 100;
        possEnemy.GetComponent<Rigidbody>().angularDrag = 100;
        // �߈˂���������
        possEnemy.GetComponent<EnemyBase>().SetState(EnemyBase.EnemyState.Idle);
    }

}
