using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class LightAttack : IState
{
    private PlayerController player;
    private Rigidbody rb;
    GameObject target = null;
    int lightAttackDamage = 5;      // 弱攻撃のダメージ数
    float lightAttackCD = 2.0f;     // 弱攻撃のクールダウン
    float lightAttackRadius = 3.0f; // 弱攻撃の敵を捕捉する球体の半径
    float lightAttackOffset = 3.0f; // 接近して弱攻撃する際の敵との距離オフセット
    float moveSpeed = 7.0f; // 敵に接近する際の速度
    float lastAttackTime = 0.0f;

    // コンボ用
    int maxComboCount = 3;
    int nowComboCount = 0;
    float comboDuration = 1.5f;     // コンボの継続時間
    float comboCD = 0.15f;

    // デバック用腕アニメーション
    GameObject armObj = null;
    Vector3 startAngle = Vector3.zero;
    float rotatePower = 1.0f;
    float totalRotate = 0;
    public LightAttack(PlayerController player)
    {
        this.player = player;
        rb=player.GetComponent<Rigidbody>();
        armObj = GameObject.FindWithTag("PlayerArm");
    }

    public void Enter()
    {
        // 周囲に攻撃が届く敵がいるかチェック
        if (!SearchEnemy())
        {
            // いないなら攻撃をやめる
            player.Change(player.idle);
            return;
        }
        float deltaTime = Time.time - lastAttackTime;
        // 現在コンボの途中かチェック
        if (nowComboCount > 0)
        {
            // クールダウンが解消しているかチェック
            if (deltaTime>=comboCD)
            {
                // コンボが継続出来るかチェック
                if (deltaTime <= comboDuration)
                {
                    // コンボ攻撃継続
                    player.StartCoroutine(Attack());
                    return;
                }
                else
                {
                    nowComboCount = 0;
                }

            }

        }
        // クルーダウンが解消しているかチェック
        if (deltaTime <= lightAttackCD)
        {
            // いないなら攻撃をやめる
            player.Change(player.idle);
            return;
        }
        else
        {
            // 一段目から始める
            player.StartCoroutine(Attack());
            return;
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
        // 範囲内にいる敵を全て取得
        var enemies = Physics.OverlapSphere(player.transform.position, lightAttackRadius).Where(coll => coll.tag == "Enemy");
        if (enemies.Count() == 0)
        {
            target = null;
            return false;
        }
        // Debug.Log("enemy : "+enemies.Count());
        float minDistance = 999.0f;
        // 一番近い敵をターゲットに
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
        Debug.Log("攻撃" + nowComboCount + "番目");
        // 攻撃をする敵へ接近
        Coroutine coroutine = player.StartCoroutine(MoveToEnemy());
        yield return coroutine;
        //coroutine = player.StartCoroutine(MoveArm());
        // 斬撃エフェクトを作成
        GameObject slashEffctObj = GameObject.Instantiate(player.SlashEffectObj,player.transform);
        Slash slash=slashEffctObj.GetComponent<Slash>();
        // コンボ数に応じて斬撃エフェクトの角度を変更
        slashEffctObj.transform.eulerAngles = player.transform.eulerAngles + slash.comboSlashRot[nowComboCount];
        VisualEffect slashEffect = slashEffctObj.GetComponentInChildren<VisualEffect>();
        slashEffect.Play();
        // yield return coroutine;
        if (target == null)
            yield break;
        // 攻撃エフェクトを作成
        GameObject hitEffectObj = GameObject.Instantiate(player.HitEffectObj,target.transform.position,Quaternion.identity);
        Camera camera=Camera.main;
        hitEffectObj.transform.position = Vector3.Lerp(target.transform.position, camera.transform.position, 0.1f);
        ParticleSystem hitEffect=hitEffectObj.GetComponent<ParticleSystem>();
        hitEffect.Play();
        // ダメージを与える
        target.GetComponentInParent<EnemyBase>().TakeDamage(lightAttackDamage);
        // 現在時刻を取得
        lastAttackTime = Time.time;
        // コンボ数を更新
        nowComboCount++;
        // コンボ数が３段目以上なら１段目に戻す
        if (nowComboCount>2)
        {
            nowComboCount = 0;
        }
        // 攻撃を終了
        player.Change(player.idle);
        yield return null;
    }

    IEnumerator MoveToEnemy()
    {
        Vector3 dir = target.transform.position - player.transform.position;
        dir.y = 0.0f;
        rb.velocity = dir.normalized * moveSpeed;
        player.transform.forward = dir.normalized;
        while (true)
        {
            float distance = (target.transform.position - player.transform.position).magnitude;
            if (distance <= lightAttackOffset)
            {
                rb.velocity = Vector3.zero;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator MoveArm()
    {
        startAngle = armObj.transform.localEulerAngles;
        while (true)
        {
            Vector3 angle = armObj.transform.localEulerAngles;
            angle.x += rotatePower;
            angle.y -= rotatePower;
            armObj.transform.localEulerAngles = angle;
            totalRotate += rotatePower;
            if (totalRotate > 120.0f)
            {
                armObj.transform.localEulerAngles = startAngle;
                totalRotate = 0.0f;
                yield break;
            }
            yield return null;
        }
    }
}

