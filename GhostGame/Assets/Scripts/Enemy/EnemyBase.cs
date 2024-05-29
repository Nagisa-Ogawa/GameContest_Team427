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
    int maxHp;
    int hp;
    [SerializeField]
    int maxStanPoint;
    int stanPoint;

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

    //攻撃後などの硬直時間
    protected float maxFreezeTime = 10.0f;
    protected float freezeTime = 0.0f;

    //元の色
    private Color normalColor;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        hp = maxHp;
        stanPoint = maxStanPoint;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
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
                GameObject model = transform.Find("Mouse/default").gameObject;
                Material mat = model.GetComponent<MeshRenderer>().material;
                mat.color = normalColor;
                StanAllowUIManager stanAllowUIManager = GameObject.FindWithTag("StanAllowUIManager").GetComponent<StanAllowUIManager>();
                stanAllowUIManager.DeleteEnemyList(gameObject);
                stanPoint = maxStanPoint;

                //idleに戻す
                SetState(EnemyState.Idle);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeStanDamage(int stanDamage)
    {
        stanPoint -= stanDamage;
        if(stanPoint <= 0)
        {
            
            // スタン状態へ
            state = EnemyState.Freeze;
            // 色を青くする
            GameObject model = transform.Find("Mouse/default").gameObject;
            Material mat = model.GetComponent<MeshRenderer>().material;
            // 元の色を覚えておく
            normalColor=mat.color;
            Color color = mat.color;
            color.b = 1.0f;
            mat.color = color;
            freezeTime = maxFreezeTime;
            // スタンしたことを伝える
            StanAllowUIManager stanAllowUIManager = GameObject.FindWithTag("StanAllowUIManager").GetComponent<StanAllowUIManager>();
            stanAllowUIManager.AddEnemyList(gameObject);
        }
    }

    public virtual void Attack() { }

    public virtual void StanAttack() { }


    public void SetState(EnemyState tempstate, Transform targetObject = null)
    {
        state = tempstate;
        targetTransform = targetObject;

    }

    public EnemyState GetState()
    {
        return state;
    }

}
