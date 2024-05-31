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

    //�X�^���Ȃǂ̍d������
    protected float maxFreezeTime = 2.0f;
    protected float freezeTime = 0.0f;

    //���̐F
    private Color normalColor;
    public Color NormalColor
    {
        get { return normalColor; }
        private set { normalColor = value; }
    }

    //�����삵�Ă���U���R���[�`��
    protected Coroutine workingAttackCoroutine;


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
        //�S�G�l�~�[���ʁH
        if(state == EnemyState.Freeze)
        {
            //�d�����Ԍ���
            freezeTime -= Time.deltaTime;

            //�d�����Ԃ��I��������
            if(freezeTime <= 0.0f)
            {
                freezeTime = maxFreezeTime;
                // �F��߂�
                GameObject model = transform.Find("Mouse/default").gameObject;
                Material mat = model.GetComponent<MeshRenderer>().material;
                mat.color = normalColor;
                StanAllowUIManager stanAllowUIManager = GameObject.FindWithTag("StanAllowUIManager").GetComponent<StanAllowUIManager>();
                stanAllowUIManager.DeleteEnemyList(gameObject);
                stanPoint = maxStanPoint;

                //idle�ɖ߂�
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
            // �X�^����Ԃ�
            state = EnemyState.Freeze;
            // �����Ă���R���[�`��������Ȃ��~
            StopWorkingCoroutine();
            // �X�^���l�����Z�b�g
            stanPoint = maxStanPoint;
            // �F�������
            GameObject model = transform.Find("Mouse/default").gameObject;
            Material mat = model.GetComponent<MeshRenderer>().material;
            // ���̐F���o���Ă���
            normalColor=mat.color;
            Color color = mat.color;
            color.b = 1.0f;
            mat.color = color;
            freezeTime = maxFreezeTime;
            // �X�^���������Ƃ�`����
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

    //�������Ă���U���R���[�`�����I��
    public void StopWorkingCoroutine()
    {
        //�������Ă���R���[�`��������Ȃ�
        if(workingAttackCoroutine != null)
        {
            //�R���[�`���I��
            StopCoroutine(workingAttackCoroutine);
            //�ϐ���null�ɖ߂�
            workingAttackCoroutine = null;
        }
    }


}
