using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int playerHP;
    public IState playerState;

    public GameObject possessionEnemyPrefab;
    public int enemyHP;
    public int enemyStun;
    public EnemyBase.EnemyState enemyState;

    //引き継ぐデータがあるかどうか
    //2つ目以降のステージでtrueになる
    public bool isMemory = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
