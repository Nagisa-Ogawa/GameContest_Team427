using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StanAttack : IState
{
    private PlayerController player;
    float stanAttackCD = 3.0f;     // スタン攻撃のクールダウン
    // float stanAttackRadius = 3.0f; // スタン攻撃の敵を捕捉する球体の半径
    // float stanAttackOffset = 3.0f; // 接近してスタン攻撃する際の敵との距離オフセット
    // float moveSpeed = 7.0f; // 敵に接近する際の速度
    float lastAttackTime = 0.0f;
    // GameObject target = null;
    float setupTime = 1.0f; // 範囲攻撃の準備時間
    float hitTime = 0.1f;   // 範囲攻撃の当たり判定をする時間

    GameObject areaObj = null;
    GameObject areaModelObj = null;
    float addScale = 0.0f;
    float areaScale = 6.0f;
    float setupAlpha = 0.3f;
    float hitAlpha = 0.8f;

    //Vector3 startAngle = Vector3.zero;
    //float rotatePower = 1.0f;
    //float totalRotate = 0;
    public StanAttack(PlayerController player)
    {
        this.player = player;
        areaObj = GameObject.FindWithTag("PlayerStanArea");
        areaModelObj = areaObj.transform.GetChild(0).gameObject;
    }

    public void Enter()
    {
        // 現在時刻からクルーダウンが解消しているかチェック
        if ((Time.time - lastAttackTime) <= stanAttackCD)
        {
            // いないなら攻撃をやめる
            player.Change(player.idle);
            return;
        }
        // 攻撃コルーチン開始
        player.StartCoroutine(StanAttackCoroutine());
    }

    public void Update()
    {

    }

    public void Exit()
    {
        // target = null;
    }

    //bool SearchEnemy()
    //{
    //    // 範囲内にいる敵を全て取得
    //    var enemies = Physics.OverlapSphere(player.transform.position, stanAttackRadius).Where(coll => coll.tag == "Enemy");
    //    if (enemies.Count() == 0)
    //    {
    //        target = null;
    //        return false;
    //    }
    //    Debug.Log("enemy : " + enemies.Count());
    //    float minDistance = 999.0f;
    //    // 一番近い敵をターゲットに
    //    foreach (var enemy in enemies)
    //    {
    //        float distance = (enemy.transform.position - player.transform.position).magnitude;
    //        if (minDistance >= distance)
    //        {
    //            minDistance = distance;
    //            target = enemy.gameObject;
    //        }
    //    }
    //    return true;
    //}

    IEnumerator StanAttackCoroutine()
    {
        // 攻撃準備（1秒待機）
        Coroutine coroutine = player.StartCoroutine(AreaGrowingAnimation());
        yield return coroutine;
        // 範囲攻撃用モデルの色を変更
        Material mat = areaModelObj.GetComponent<MeshRenderer>().material;
        Color color = mat.color;
        color.a = hitAlpha;
        mat.color = color;
        // 範囲スタン攻撃用コリジョンを表示
        areaObj.GetComponent<SphereCollider>().enabled = true;
        // 少し待機
        yield return new WaitForSeconds(hitTime);
        // 範囲スタン攻撃用コリジョンを非表示
        areaObj.GetComponent<SphereCollider>().enabled = false;
        areaModelObj.SetActive(false);
        // 現在時刻を取得
        lastAttackTime = Time.time;
        // 攻撃を終了
        player.Change(player.idle);
    }


    /// <summary>
    /// デバック用！！
    /// 範囲攻撃のアニメーション用コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator AreaGrowingAnimation()
    {
        // 準備時間から1フレームごとに大きくする値を算出
        addScale = areaScale / setupTime;
        areaModelObj.SetActive(true);
        areaModelObj.transform.localScale = Vector3.zero;
        Material mat = areaModelObj.GetComponent<MeshRenderer>().material;
        Color color=mat.color;
        color.a = setupAlpha;
        mat.color = color;
        while (true)
        {
            float addScalePerFrame = addScale * Time.deltaTime;
            areaModelObj.transform.localScale += new Vector3(addScalePerFrame, addScalePerFrame, addScalePerFrame);
            if (areaModelObj.transform.localScale.x >= areaScale)
            {
                areaModelObj.transform.localScale=new Vector3(areaScale,areaScale,areaScale);
                yield break;
            }
            yield return null;
        }
    }

    //IEnumerator Attack()
    //{
    //    // 攻撃をする敵へ接近
    //    Coroutine coroutine = player.StartCoroutine(MoveToEnemy());
    //    yield return coroutine;
    //    coroutine = player.StartCoroutine(MoveArm());
    //    yield return coroutine;
    //    // 攻撃エフェクトを作成
    //    GameObject hitEffectObj = GameObject.Instantiate(player.StanHitEffectObj, target.transform.position, Quaternion.identity);
    //    Camera camera = Camera.main;
    //    hitEffectObj.transform.position = Vector3.Lerp(target.transform.position, camera.transform.position, 0.1f);
    //    ParticleSystem hitEffect = hitEffectObj.GetComponent<ParticleSystem>();
    //    hitEffect.Play();
    //    // ダメージを与える
    //    target.GetComponentInParent<EnemyBase>().TakeStanDamage(5);
    //    // 現在時刻を取得
    //    lastAttackTime = Time.time;
    //    // 攻撃を終了
    //    player.Change(player.idle);
    //    yield return null;
    //}


    //IEnumerator MoveToEnemy()
    //{
    //    Vector3 dir = target.transform.position - player.transform.position;
    //    dir.y = 0.0f;
    //    player.Rb.velocity = dir.normalized * moveSpeed;
    //    player.transform.forward = dir.normalized;
    //    while (true)
    //    {
    //        float distance = (target.transform.position - player.transform.position).magnitude;
    //        if (distance <= stanAttackOffset)
    //        {
    //            player.Rb.velocity = Vector3.zero;
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}

    //IEnumerator MoveArm()
    //{
    //    startAngle = player.PlayerArmObj.transform.localEulerAngles;
    //    while (true)
    //    {
    //        Vector3 angle = player.PlayerArmObj.transform.localEulerAngles;
    //        angle.x += rotatePower;
    //        angle.y -= rotatePower;
    //        player.PlayerArmObj.transform.localEulerAngles = angle;
    //        totalRotate += rotatePower;
    //        if (totalRotate > 120.0f)
    //        {
    //            player.PlayerArmObj.transform.localEulerAngles = startAngle;
    //            totalRotate = 0.0f;
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}
}



