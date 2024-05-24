using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : IState
{
    //憑依ステートスクリプト


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
            // カメラから見た左右と前後の入力値を受け取る
            velocity += Input.GetAxis("Horizontal") * new Vector3(camera.transform.right.x, 0.0f, camera.transform.right.z).normalized;
            velocity += Input.GetAxis("Vertical") * new Vector3(camera.transform.forward.x, 0.0f, camera.transform.forward.z).normalized;
            velocity = velocity.normalized * player.Speed;
            if (velocity != Vector3.zero)
            {
                // プレイヤーの向きを移動方向へ向かせる
                //player.GetEnemy().transform.forward = velocity.normalized;
            }

            //player.Rb.velocity = velocity;

            player.GetPossessionEnemy().GetComponent<Rigidbody>().velocity = velocity;

            Quaternion setRotation = Quaternion.LookRotation(velocity);
            //算出した方向の角度に回転
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
