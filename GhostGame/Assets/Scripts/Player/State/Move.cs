using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Move : IState
{
    private Rigidbody rb;
    private PlayerController player;
    private Camera camera;
    public Vector3 velocity;

    Vector2 moveInput = Vector2.zero;
    bool isLightAttack = false;
    bool isStanAttack = false;

    public Move(PlayerController player)
    {
        this.player = player;
        rb=player.GetComponent<Rigidbody>();
    }

    public void Enter()
    {
        camera= Camera.main;
    }

    public void Update()
    {
        bool ISmove = false;
        velocity = new Vector3(0, 0, 0);
        moveInput = player.PlayerInput.currentActionMap["Move"].ReadValue<Vector2>();
        // カメラから見た左右と前後の入力値を受け取る
        velocity += moveInput.x * new Vector3(camera.transform.right.x, 0.0f, camera.transform.right.z).normalized;
        velocity += moveInput.y * new Vector3(camera.transform.forward.x, 0.0f, camera.transform.forward.z).normalized;
        velocity = velocity.normalized * player.Speed;
        if(velocity!=Vector3.zero)
        {
            // プレイヤーの向きを移動方向へ向かせる
            player.transform.forward = velocity.normalized;
            // 移動フラグをON
            ISmove = true;
        }
        rb.velocity = velocity;

        isLightAttack = player.PlayerInput.currentActionMap["LightAttack"].WasPressedThisFrame();
        if (isLightAttack)
        {
            player.Change(player.lightAttack);
        }
        isStanAttack = player.PlayerInput.currentActionMap["StanAttack"].WasPressedThisFrame();
        if (isStanAttack)
        {
            player.Change(player.stanAttack);
        }
        // このフレームで移動入力がなかったなら移動ステートから抜ける
        if (ISmove == false)
        {
            player.Change(player.idle);
        }
    }

    public void Exit()
    {
        velocity = new Vector3(0, 0, 0);
        rb.velocity = new Vector3(0, 0, 0);
    }

}
