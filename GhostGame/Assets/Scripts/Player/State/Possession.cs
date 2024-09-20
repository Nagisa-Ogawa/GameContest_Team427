using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : IState
{
    //憑依ステートスクリプト


    private PlayerController player;
    private Camera camera;
    Vector2 moveInput = Vector2.zero;
    bool isAttack = false;

    Rigidbody rb;

    public Vector3 velocity;
    GameObject possEnemy = null;

    private StageManager sm;

    public Possession(PlayerController p)
    {
        //this.player = player;
        //player =  GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //rb =player.GetComponent<Rigidbody>();
        //sm = GameObject.FindWithTag("StageManager").GetComponent<StageManager>();

        Debug.Log("player設定している");

    }

    public void Enter()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody>();
        sm = GameObject.FindWithTag("StageManager").GetComponent<StageManager>();

        camera = Camera.main;
        possEnemy = player.GetPossessionEnemy();
        if(possEnemy == null)
        {
            Debug.Log("エネミーがnull");
        }
        // 剛体のパラメータを変更
        possEnemy.GetComponent<Rigidbody>().drag = 0;
        possEnemy.GetComponent<Rigidbody>().angularDrag = 0.5f;
        // 憑依した敵の色を戻す
        // 色を戻す
        //GameObject model = possEnemy.transform.Find("Mouse/default").gameObject;
        Material mat = possEnemy.transform.GetComponentInChildren<MeshRenderer>().material;
        mat.color = possEnemy.GetComponent<EnemyBase>().NormalColor;
        StanAllowUIManager stanAllowUIManager = GameObject.FindWithTag("StanAllowUIManager").GetComponent<StanAllowUIManager>();
        stanAllowUIManager.DeleteEnemyList(possEnemy);
        player.possessionTargetEnemy = null;
        //憑依したら敵の数カウントを1減らす
        sm.EnemyPossession();
    }

    public void Update()
    {
        //憑依しているエネミーが非アクティブだったら(倒されたら)
        if(possEnemy.activeInHierarchy == false)
        {
            player.Change(player.idle);
            player.ResetPossessionEnemy();
            player.GetComponent<CapsuleCollider>().isTrigger = false;
            return;
        }

        velocity = new Vector3(0, 0, 0);

        //憑依しているエネミーが攻撃中じゃなければ入力を受け取る
        if(possEnemy.GetComponent<EnemyBase>().GetWorkingAttackCoroutine() == null)
        {
            moveInput = player.PlayerInput.currentActionMap["Move"].ReadValue<Vector2>();

            // カメラから見た左右と前後の入力値を受け取る
            velocity += moveInput.x * new Vector3(camera.transform.right.x, 0.0f, camera.transform.right.z).normalized;
            velocity += moveInput.y * new Vector3(camera.transform.forward.x, 0.0f, camera.transform.forward.z).normalized;
            velocity = velocity.normalized * player.Speed;
        }
        

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
        //possEnemy.GetComponent<Rigidbody>().velocity = velocity;
        possEnemy.transform.position += velocity * Time.deltaTime;

        //プレイヤーの位置を憑依した敵の後ろにくっつける
        player.transform.position = UpdatePossessionPlayerPosition();

        if (player.PlayerInput.currentActionMap["PossessionCancel"].IsPressed())
        {
            possEnemy.GetComponent<EnemyBase>().SetState(EnemyBase.EnemyState.Idle);
            player.ResetPossessionEnemy();
            player.GetComponent<CapsuleCollider>().isTrigger = false;
            player.Change(player.idle);
        }

        if (player.PlayerInput.currentActionMap["PossessionAttack"].WasPressedThisFrame())
        {
            possEnemy.GetComponent<EnemyBase>().PossessionAttack();
        }
        isAttack = false;
        if (player.PlayerInput.currentActionMap["LightAttack"].WasPressedThisFrame())
        {
            isAttack = true;
            player.Change(player.lightAttack);
        }
        if (player.PlayerInput.currentActionMap["StanAttack"].WasPressedThisFrame())
        {
            possEnemy.GetComponent<EnemyBase>().PossessionStanAttack();
        }

    }

    public void Exit()
    {
        if(isAttack)
        {

        }
        else
        {
            possEnemy.GetComponent<Rigidbody>().drag = 100;
            possEnemy.GetComponent<Rigidbody>().angularDrag = 100;
            // 憑依から解放する
            possEnemy.GetComponent<EnemyBase>().SetState(EnemyBase.EnemyState.Idle);

            //憑依から解放したら敵の数カウントを1増やす
            sm.EnemyPossessionCancel();
            Debug.Log("possessioncancel");
        }
    }

    Vector3 UpdatePossessionPlayerPosition()
    {
        Vector3 position;

        //憑依している敵の位置
        position = player.GetPossessionEnemy().transform.position;
        //敵の後ろ方向に指定距離離す
        position -= player.GetPossessionEnemy().transform.forward * possEnemy.GetComponent<EnemyBase>().GetPossessionPlayerDistance();
        //地面にめり込むため少し上方向に離す
        position += player.GetPossessionEnemy().transform.up * 1.0f;

        return position;
    }
}

