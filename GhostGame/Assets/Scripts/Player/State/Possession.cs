using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : IState
{
    //憑依ステートスクリプト


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
        // 剛体のパラメータを変更
        possEnemy.GetComponent<Rigidbody>().drag = 0;
        possEnemy.GetComponent<Rigidbody>().angularDrag = 0.5f;
        // 憑依した敵の色を戻す
        // 色を戻す
        GameObject model = possEnemy.transform.Find("Mouse/default").gameObject;
        Material mat = model.GetComponent<MeshRenderer>().material;
        mat.color = possEnemy.GetComponent<EnemyBase>().NormalColor;
        StanAllowUIManager stanAllowUIManager = GameObject.FindWithTag("StanAllowUIManager").GetComponent<StanAllowUIManager>();
        stanAllowUIManager.DeleteEnemyList(possEnemy);

    }

    public void Update()
    {
        //憑依しているエネミーが非アクティブだったら(倒されたら)
        if(possEnemy.activeInHierarchy == false)
        {
            player.GetComponent<PlayerController>().Change(player.idle);
            player.GetComponent<PlayerController>().ResetPossessionEnemy();
            return;
        }

        velocity = new Vector3(0, 0, 0);

        //憑依しているエネミーが攻撃中じゃなければ入力を受け取る
        if(possEnemy.GetComponent<EnemyBase>().GetWorkingAttackCoroutine() == null)
        {
            moveInput = player.PlayerInput.currentActionMap["Move"].ReadValue<Vector2>();
        }
        
        // カメラから見た左右と前後の入力値を受け取る
        velocity += moveInput.x * new Vector3(camera.transform.right.x, 0.0f, camera.transform.right.z).normalized;
        velocity += moveInput.y * new Vector3(camera.transform.forward.x, 0.0f, camera.transform.forward.z).normalized;
        velocity = velocity.normalized * player.Speed;
        if (velocity != Vector3.zero)
        {
            // プレイヤーの向きを移動方向へ向かせる
            //player.GetEnemy().transform.forward = velocity.normalized;

            Quaternion setRotation = Quaternion.LookRotation(velocity);
            //算出した方向の角度に回転
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
        // 憑依から解放する
        possEnemy.GetComponent<EnemyBase>().SetState(EnemyBase.EnemyState.Idle);
    }

}
