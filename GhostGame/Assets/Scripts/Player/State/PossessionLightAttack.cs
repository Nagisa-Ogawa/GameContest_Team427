using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class PossessionLightAttack : IState
{
    private PlayerController player;
    private Rigidbody rb;
    GameObject target = null;
    float lightAttackRadius = 4.0f; // 弱攻撃の敵を捕捉する球体の半径
    float lightAttackOffset = 3.0f; // 接近して弱攻撃する際の敵との距離オフセット
    float moveSpeed = 5.0f; // 敵に接近する際の速度
    float lastAttackTime = 0.0f;
    GameObject possEnemy = null;

    // コンボ用
    int maxComboCount = 3;
    int nowComboCount = 0;
    float comboDuration = 1.5f;     // コンボの継続時間
    float comboCD = 0.15f;

    public PossessionLightAttack(PlayerController player)
    {
        this.player = player;
        rb=player.GetComponent<Rigidbody>();
        //リキャストリセット
        lastAttackTime = Time.time - player.NowLightAttackCD;
    }

    public void Enter()
    {
        possEnemy = player.possessionEnemy;
        // 周囲に攻撃が届く敵がいるかチェック
        if (!SearchEnemy())
        {
            // いないなら攻撃をやめる
            player.Change(player.possession);
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
        if (deltaTime <= player.NowLightAttackCD)
        {
            // いないなら攻撃をやめる
            player.Change(player.possession);
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
        //プレイヤーの位置を憑依した敵の後ろにくっつける
        player.transform.position = UpdatePossessionPlayerPosition();
    }

    public void Exit()
    {
        target = null;
    }

    bool SearchEnemy()
    {
        // 範囲内にいる敵を全て取得
        var enemies = Physics.OverlapSphere(possEnemy.transform.position, lightAttackRadius).Where(coll => coll.tag == "Enemy");
        if (enemies.Count() <= 1)
        {
            target = null;
            return false;
        }
        float minDistance = 999.0f;
        Transform enemyChild = null;
        for(int i=0;i<possEnemy.transform.childCount;i++)
        {
            var child=possEnemy.transform.GetChild(i);
            if (child.tag == "Enemy")
            {
                enemyChild = child; ;
            }
        }
        // 一番近い敵をターゲットに
        foreach (var enemy in enemies)
        {
            if(enemy.gameObject==enemyChild.gameObject)
            {
                continue;
            }
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
        // 攻撃をする敵へ接近
        Coroutine coroutine = player.StartCoroutine(MoveToEnemy());
        yield return coroutine;
        //coroutine = player.StartCoroutine(MoveArm());
        // 斬撃エフェクトを作成
        // GameObject slashEffctObj = GameObject.Instantiate(player.SlashEffectObj,possEnemy.transform);
        GameObject slashEffectObj = GameObject.FindWithTag("SlashEffect").gameObject;
        slashEffectObj.transform.position=possEnemy.transform.position;
        Slash slash=slashEffectObj.GetComponent<Slash>();
        // コンボ数に応じて斬撃エフェクトの角度を変更
        slashEffectObj.transform.GetChild(0).transform.eulerAngles = possEnemy.transform.eulerAngles + slash.comboSlashRot[nowComboCount];
        VisualEffect slashEffect = slashEffectObj.GetComponentInChildren<VisualEffect>();
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
        target.GetComponentInParent<EnemyBase>().TakeDamage(player.LightAttackDamage);
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
        player.Change(player.possession);
        yield return null;
    }

    IEnumerator MoveToEnemy()
    {
        Vector3 dir = target.transform.position - possEnemy.transform.position;
        dir.y = 0.0f;
        possEnemy.transform.forward = dir.normalized;
        while (true)
        {
            possEnemy.transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            float distance = (target.transform.position - possEnemy.transform.position).magnitude;
            if (distance <= lightAttackOffset)
            {
                yield break;
            }
            yield return null;
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

