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

    //���߈˂��Ă���G�l�~�[
    private GameObject possessionEnemy = null;
    //�߈ˑΏۂɂȂ��Ă���G�l�~�[
    public GameObject possessionTargetEnemy = null;
    // �v���C���[���߈ˉ\�ȋ���
    public float possessionDistance = 10.0f;


    // �ړ����x
    [SerializeField]
    private float speed = 3.0f; 
    public float Speed
    {
                get { return speed; }
        private set {speed = value; }
    }

    // �U���q�b�g���̃G�t�F�N�g
    [SerializeField]
    private GameObject hitEffectObj = null;
    public GameObject HitEffectObj
    {
        get { return hitEffectObj; }
        private set { hitEffectObj = value; }
    }

    // �X�^���U���q�b�g���̃G�t�F�N�g
    [SerializeField]
    private GameObject stanHitEffectObj = null;
    public GameObject StanHitEffectObj
    {
        get { return stanHitEffectObj; }
        private set { stanHitEffectObj = value; }
    }

    private StanAllowUIManager stanAllowUIManager = null;

    // �v���C���[�C���v�b�g
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
        // �߈ˑΏەύX����
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
        Debug.Log("��e:�c��HP" + hp);
        if (hp < 0)
        {
            // ���S���̏���
            gameObject.SetActive(false);
            hp = 0;
        }
    }

    /// <summary>
    /// �X�^���U�����󂯂��ۂ̏���
    /// </summary>
    /// <param name="stanDamage"></param>
    public void TakeStanDamage(int stanDamage)
    {
        stanPoint -= stanDamage;
        if(stanPoint < 0)
        {
            // �X�^�����̏���
            stanPoint = 0;
        }
    }

    //���߈˂��Ă���G�l�~�[���擾
    public GameObject GetPossessionEnemy()
    {
        return possessionEnemy;
    }

    //�߈ˉ������Ɏg�p
    //�߈˒��G�l�~�[�ϐ���null�ɂ���
    public void ResetPossessionEnemy()
    {
        possessionEnemy = null;
    }
}
