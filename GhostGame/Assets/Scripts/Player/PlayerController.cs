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


    [SerializeField]
    int maxHp;
    int hp;
    [SerializeField]
    int maxStanPoint;
    int stanPoint;

    bool isStan;

    public float possessionDistance = 10.0f;

    [SerializeField]
    private StanAllowUIManager stanAllowUIManager = null;

    [SerializeField]
    private Rigidbody rb = null;
    public Rigidbody Rb
    {
                get { return rb; }
        private set { rb = value; }

    }

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

    // プレイヤーの仮攻撃アニメーション用オブジェクト
    [SerializeField]
    private GameObject playerArmObj = null;
    public GameObject PlayerArmObj
    {
        get { return playerArmObj; }
        private set { playerArmObj = value; }
    }

    // スタン攻撃時の範囲用オブジェクト
    [SerializeField]
    private GameObject stanAttackAreaObj = null;
    public GameObject StanAttackAreaObj
    {
        get { return stanAttackAreaObj; }
        private set { stanAttackAreaObj = value; }
    }

    // スタン攻撃時の範囲用オブジェクト用モデル
    [SerializeField]
    private GameObject stanAttackAreaModelObj = null;
    public GameObject StanAttackAreaModelObj
    {
        get { return stanAttackAreaModelObj; }
        private set { stanAttackAreaModelObj = value; }
    }


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

        playerInput=GetComponent<PlayerInput>();

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
        // 憑依先選択の入力処理
        if (PlayerInput.currentActionMap["NextTarget"].WasPressedThisFrame())
        {
            stanAllowUIManager.NextListEnemy();
        }
        if(PlayerInput.currentActionMap["PrevTarget"].WasPressedThisFrame())
        {
            stanAllowUIManager.PrevListEnemy();
        }
        if (currentState != null)
        {
            currentState.Update();
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
}
