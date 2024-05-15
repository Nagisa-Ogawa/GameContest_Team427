using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Move : IState
{
    private PlayerController player;
    private Camera camera;
    public Vector3 velocity;

    public Move(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        camera= Camera.main;
    }

    public void Update()
    {
        bool ISmove = false;
        velocity = new Vector3(0, 0, 0);
        // �J�������猩�����E�ƑO��̓��͒l���󂯎��
        velocity += Input.GetAxis("Horizontal") * new Vector3(camera.transform.right.x, 0.0f, camera.transform.right.z).normalized;
        velocity += Input.GetAxis("Vertical") * new Vector3(camera.transform.forward.x, 0.0f, camera.transform.forward.z).normalized;
        velocity = velocity.normalized * player.Speed;
        if(velocity!=Vector3.zero)
        {
            // �v���C���[�̌������ړ������֌�������
            player.transform.forward = velocity.normalized;
            // �ړ��t���O��ON
            ISmove = true;
        }
            
        player.Rb.velocity = velocity;

        if (Input.GetKeyDown("joystick button 2"))
        {
            player.Change(player.lightAttack);
        }
        if (Input.GetKeyDown("joystick button 4"))
        {
            player.Change(player.stanAttack);
        }
        // ���̃t���[���ňړ����͂��Ȃ������Ȃ�ړ��X�e�[�g���甲����
        if (ISmove == false)
        {
            player.Change(player.idle);
        }
    }

    public void Exit()
    {
        velocity = new Vector3(0, 0, 0);
        player.Rb.velocity = new Vector3(0, 0, 0);
    }

}
