using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Freeze,
        Sleep,
        Possession
    }


    [SerializeField]
    public int maxHp;
    public int hp;
    [SerializeField]
    public int maxStanPoint;
    public int stanPoint;

    public GameObject enemyPrefab;

    bool isStan;

    public int damage;
    public int stanDamage;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    int attackInterval;

    public PlayerController player;

    [SerializeField]
    protected EnemyState state;
    protected Transform targetTransform;

    //スタンなどの硬直時間
    protected float maxFreezeTime = 10.0f;
    protected float freezeTime = 0.0f;

    //元の色
    private Color normalColor;
    public Color NormalColor
    {
        get { return normalColor; }
        private set { normalColor = value; }
    }

    //今動作している攻撃コルーチン
    protected Coroutine workingAttackCoroutine;

    //攻撃エフェクト
    [SerializeField]
    protected GameObject hitEffectObj = null;

    //EnemyHPゲージ
    protected EnemyGage enemyGage;

    //憑依したとき敵の後ろにプレイヤーがくっつく
    //その敵とプレイヤーの距離
    [SerializeField]
    private float possessionPlayerDistance;

    //EnemyStanゲージ
    protected EnemyStanGage enemyStanGage;

    //EnemeyUIOnOff
    protected EnemyUIOnOff enemyUIOnOff;

    //ステージ管理変数
    //次のステージに進める状態かどうかなど
    private StageManager sm;

    private GameManager gm;


    //プレイヤーを追いかける範囲を感知するコライダー
    [SerializeField]
    private SphereSensor sSensor;

    public GameObject damageUI;

    AudioSource audioSource;
    public AudioClip damageSE;



    protected virtual void Awake()
    {
        hp = maxHp;
        stanPoint = maxStanPoint;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Objectを取得  自分の子供のEnemyGageのみを取得
        enemyGage = transform.Find("EnemyHP_Stan").transform.Find("EnemyHPUI").transform.Find("EnemyGage").GetComponent<EnemyGage>();
        enemyGage.SetEnemy(this);


        //StanUI用
        enemyStanGage = transform.Find("EnemyHP_Stan").transform.Find("EnemyHPUI").transform.Find("EnemyGage").transform.Find("EnemyStanGage").GetComponent<EnemyStanGage>();
        enemyStanGage.SetStanEnemy(this);

        //UIOnOff用
        enemyUIOnOff = transform.Find("EnemyHP_Stan").GetComponent<EnemyUIOnOff>();
        enemyUIOnOff.SetUIEnemy(this);

        sm = GameObject.FindWithTag("StageManager").GetComponent<StageManager>();
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (!gm.isMemory)
        {
            SetState(EnemyState.Idle);
        }

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (state != EnemyState.Possession)
        {
            if (sSensor.GetIsEnter())
            {
                //プレイヤーが追跡範囲内に入っていたら
                if (player.gameObject.tag == "Player")
                {
                    //攻撃状態または硬直状態じゃなければ更新
                    if (state != EnemyBase.EnemyState.Freeze && state != EnemyBase.EnemyState.Attack && state != EnemyBase.EnemyState.Possession)
                    {
                        //プレイヤーが憑依しているエネミーがいるなら
                        if (player.GetComponent<PlayerController>().GetPossessionEnemy() != null)
                        {
                            //PossessionEnemyのTransformをターゲットに入れる
                            SetState(EnemyBase.EnemyState.Chase, player.GetComponent<PlayerController>().GetPossessionEnemy().transform);
                        }
                        else
                        {
                            //いないならPlayerのTransformをターゲットに入れる
                            SetState(EnemyBase.EnemyState.Chase, player.transform);
                        }
                    }

                    //範囲外に行った際の追跡終了　カウントをリセット
                    sSensor.ResetOutSensorTime();
                }
            }
            else
            {
                SetState(EnemyState.Idle);
            }
        }
        //全エネミー共通？
        if(state == EnemyState.Freeze)
        {
            //硬直時間減少
            freezeTime -= Time.deltaTime;

            //硬直時間が終了したら
            if(freezeTime <= 0.0f)
            {
                freezeTime = maxFreezeTime;
                // 色を戻す
                MeshRenderer[] meshs = transform.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mesh in meshs)
                {
                    if (mesh.gameObject.tag != "MeshColor")
                    {
                        continue;
                    }
                    Material mat = mesh.material;
                    mat.color = normalColor;
                }
                StanAllowUIManager stanAllowUIManager = GameObject.FindWithTag("StanAllowUIManager").GetComponent<StanAllowUIManager>();
                stanAllowUIManager.DeleteEnemyList(gameObject);
                stanPoint = maxStanPoint;

                //idleに戻す
                SetState(EnemyState.Idle);
                enemyStanGage.GageGain();
            }
        }

    }

    public void TakeDamage(int damage)
    {
        enemyGage.GageReduction(damage);

        hp -= damage;

        GameObject damagetext = Instantiate(damageUI, transform.position - Camera.main.transform.forward * 0.2f, Quaternion.identity);
        damagetext.GetComponent<DamageUI>().Init();
        damagetext.GetComponent<DamageUI>().TextChange(damage.ToString());

        audioSource.PlayOneShot(damageSE);


        if (hp <= 0)
        {
            gameObject.SetActive(false);
            sm.EnemyDefeated();
        }
    }

    public void TakeStanDamage(int stanDamage)
    {
        enemyStanGage.GageReduction(stanDamage);

        stanPoint -= stanDamage;

        audioSource.PlayOneShot(damageSE);

        if (stanPoint <= 0)
        {
            // スタン状態へ
            state = EnemyState.Freeze;
            // 動いているコルーチンがあるなら停止
            StopWorkingCoroutine();
            // スタン値をリセット
            //stanPoint = maxStanPoint;
            // 色を青くする
            MeshRenderer[] meshs = transform.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer mesh in meshs)
            {
                if (mesh.gameObject.tag != "MeshColor")
                {
                    continue;
                }
                Material mat = mesh.material;
                // 元の色を覚えておく
                normalColor = mat.color;
                Color color = Color.black;
                color.b = 1.0f;
                mat.color = color;
            }
            freezeTime = maxFreezeTime;
            // スタンしたことを伝える
            StanAllowUIManager stanAllowUIManager = GameObject.FindWithTag("StanAllowUIManager").GetComponent<StanAllowUIManager>();
            stanAllowUIManager.AddEnemyList(gameObject);
        }
    }

    public virtual void Attack() { }

    public virtual void PossessionAttack() { }

    public virtual void PossessionStanAttack() { }



    public void SetState(EnemyState tempstate, Transform targetObject = null)
    {
        state = tempstate;
        targetTransform = targetObject;
    }

    public EnemyState GetState()
    {
        return state;
    }

    //今動いている攻撃コルーチンを終了
    public void StopWorkingCoroutine()
    {
        //今動いているコルーチンがあるなら
        if(workingAttackCoroutine != null)
        {
            //コルーチン終了
            StopCoroutine(workingAttackCoroutine);
            //変数をnullに戻す
            workingAttackCoroutine = null;
        }
    }

    public Coroutine GetWorkingAttackCoroutine()
    {
        return workingAttackCoroutine;
    }

    public void PlayAttackEffect(GameObject target)
    {
        // 攻撃エフェクトを作成
        GameObject hitEffectObj = GameObject.Instantiate(player.HitEffectObj, target.transform.position, Quaternion.identity);
        Camera camera = Camera.main;
        hitEffectObj.transform.position = Vector3.Lerp(target.transform.position, camera.transform.position, 0.1f);
        ParticleSystem hitEffect = hitEffectObj.GetComponent<ParticleSystem>();
        hitEffect.Play();

    }

    public float GetPossessionPlayerDistance()
    {
        return possessionPlayerDistance;
    }

}
