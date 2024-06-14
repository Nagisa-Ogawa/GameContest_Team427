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
    protected float maxFreezeTime = 10.0f;
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

    //�U���G�t�F�N�g
    [SerializeField]
    protected GameObject hitEffectObj = null;

    //EnemyHP�Q�[�W
    protected EnemyGage enemyGage;


    protected virtual void Awake()
    {
        hp = maxHp;
        stanPoint = maxStanPoint;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Object���擾  �����̎q����EnemyGage�݂̂��擾
        enemyGage = transform.Find("EnemyHPUI").transform.Find("EnemyGage").GetComponent<EnemyGage>();
        enemyGage.SetEnemy(this);

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
                //GameObject model = transform.Find("Mouse/default").gameObject;
                Material mat = transform.GetComponentInChildren<MeshRenderer>().material;
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
        enemyGage.GageReduction(damage);

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
            //GameObject model = transform.GetComponentInChildren<MeshRenderer>().material;
            Material mat = transform.GetComponentInChildren<MeshRenderer>().material;
            // ���̐F���o���Ă���
            normalColor =mat.color;
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

    public Coroutine GetWorkingAttackCoroutine()
    {
        return workingAttackCoroutine;
    }

    public void PlayAttackEffect(GameObject target)
    {
        // �U���G�t�F�N�g���쐬
        GameObject hitEffectObj = GameObject.Instantiate(player.HitEffectObj, target.transform.position, Quaternion.identity);
        Camera camera = Camera.main;
        hitEffectObj.transform.position = Vector3.Lerp(target.transform.position, camera.transform.position, 0.1f);
        ParticleSystem hitEffect = hitEffectObj.GetComponent<ParticleSystem>();
        hitEffect.Play();

    }

}
