using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    public IState currentState { get; private set; }

[SerializeField]
        
    public Idle idle;
    public Move move;
    public LightAttack lightAttack;
    public StanAttack stanAttack;
    protected PlayerGage playerGage;

    [SerializeField]
    public int maxHp;
    public int hp;
    [SerializeField]
    int maxStanPoint;
    int stanPoint;

    bool isStan;

    [SerializeField]
    private Rigidbody rb = null;
    public Rigidbody Rb
    {
                get { return rb; }
        private set { rb = value; }

    }

    [SerializeField]
    private float speed = 3.0f; 
    public float Speed
    {
                get { return speed; }
        private set {speed = value; }
    }

    [SerializeField]
    private GameObject hitEffectObj = null;
    public GameObject HitEffectObj
    {
        get { return hitEffectObj; }
        private set { hitEffectObj = value; }
    }

    [SerializeField]
    private GameObject stanHitEffectObj = null;
    public GameObject StanHitEffectObj
    {
        get { return stanHitEffectObj; }
        private set { stanHitEffectObj = value; }
    }


    [SerializeField]
    private GameObject playerArmObj = null;
    public GameObject PlayerArmObj
    {
        get { return playerArmObj; }
        private set { playerArmObj = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        idle = new Idle(this);
        move = new Move(this);
        lightAttack = new LightAttack(this);
        stanAttack= new StanAttack(this);

        Change(idle);

        hp = maxHp;

        playerGage = GameObject.FindObjectOfType<PlayerGage>();
        playerGage.SetPlayer(this);

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
    }

    public void TakeDamage(int damage)
    {
        playerGage.GageReduction(damage);

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
