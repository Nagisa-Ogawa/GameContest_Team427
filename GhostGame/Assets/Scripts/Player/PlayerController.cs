using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public IState currentState { get; private set; }

[SerializeField]
        
    public Idle idle;
    public Move move;
    public LightAttack lightAttack;
    public StanAttack stanAttack;
    public Possession possession;


    [SerializeField]
    int maxHp;
    int hp;
    [SerializeField]
    int maxStanPoint;
    int stanPoint;

    bool isStan;

    //今憑依しているエネミー
    private GameObject possessionEnemy = null;
    //憑依対象になっているエネミー
    public GameObject possessionTargetEnemy = null;
    // プレイヤーが憑依可能な距離
    public float possessionDistance = 10.0f;


    // 移動速度
    [SerializeField]
    private float speed = 3.0f; 
    public float Speed
    {
                get { return speed; }
        private set {speed = value; }
    }

    // 攻撃ヒット時のエフェクト
    [SerializeField]
    private GameObject hitEffectObj = null;
    public GameObject HitEffectObj
    {
        get { return hitEffectObj; }
        private set { hitEffectObj = value; }
    }

    // スタン攻撃ヒット時のエフェクト
    [SerializeField]
    private GameObject stanHitEffectObj = null;
    public GameObject StanHitEffectObj
    {
        get { return stanHitEffectObj; }
        private set { stanHitEffectObj = value; }
    }

    private StanAllowUIManager stanAllowUIManager = null;

    // プレイヤーインプット
    PlayerInput playerInput = null;
    public PlayerInput PlayerInput
    {
        get { return playerInput; }
        private set { playerInput = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        idle = new Idle(this);
        move = new Move(this);
        lightAttack = new LightAttack(this);
        stanAttack= new StanAttack(this);
        possession = new Possession(this);

        playerInput=GetComponent<PlayerInput>();
        stanAllowUIManager=GameObject.FindWithTag("StanAllowUIManager").GetComponent<StanAllowUIManager>();

        Change(idle);

        hp = maxHp;
    }

    public void Change(IState nextState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        if (nextState != null)
        {
            currentState = nextState;
            nextState.Enter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != null)
        {
            currentState.Update();
        }
        // 憑依対象変更入力
        if (playerInput.currentActionMap["NextTarget"].WasPressedThisFrame())
        {
            stanAllowUIManager.NextListEnemy();
        }
        if (playerInput.currentActionMap["PrevTarget"].WasPressedThisFrame())
        {
            stanAllowUIManager.PrevListEnemy();
        }

        if (PlayerInput.currentActionMap["Possession"].IsPressed())
        {
            if(possessionTargetEnemy != null)
            {
                possessionEnemy = possessionTargetEnemy;
                possessionEnemy.GetComponent<EnemyBase>().SetState(EnemyBase.EnemyState.Possession);
                GetComponent<CapsuleCollider>().isTrigger = true;
                Change(possession);
            }
            
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("被弾:残りHP" + hp);
        if (hp < 0)
        {
            // 死亡時の処理
            gameObject.SetActive(false);
            hp = 0;
        }
    }

    /// <summary>
    /// スタン攻撃を受けた際の処理
    /// </summary>
    /// <param name="stanDamage"></param>
    public void TakeStanDamage(int stanDamage)
    {
        stanPoint -= stanDamage;
        if(stanPoint < 0)
        {
            // スタン時の処理
            stanPoint = 0;
        }
    }

    //今憑依しているエネミーを取得
    public GameObject GetPossessionEnemy()
    {
        return possessionEnemy;
    }

    //憑依解除時に使用
    //憑依中エネミー変数をnullにする
    public void ResetPossessionEnemy()
    {
        possessionEnemy = null;
    }
}
