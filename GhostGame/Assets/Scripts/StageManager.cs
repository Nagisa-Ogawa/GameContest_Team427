using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class StageManager : MonoBehaviour
{
    // 敵の数を管理する変数
    private int enemiesRemaining;

    [SerializeField]
    private string nextSceneName;

    //ドアが開いているかどうか
    bool isOpen = false;

    //何ウェーブ目か
    int waveNum;

    [System.Serializable]
    public struct WaveEnemy
    {
        public GameObject enemyPrefab;
        public Vector3 spawnPosition;
    }

    [System.Serializable]
    public struct SpawnWave
    {
        public WaveEnemy[] waveEnemy;
    }

    [SerializeField]
    private SpawnWave[] waveList;

    // Start is called before the first frame update
    void Start()
    {
        waveNum = 0;

        for (int i = 0; i < waveList[waveNum].waveEnemy.Length; i++)
        {
            WaveEnemy spawnEnemy = waveList[waveNum].waveEnemy[i];
            Instantiate(spawnEnemy.enemyPrefab, spawnEnemy.spawnPosition, Quaternion.identity);

            enemiesRemaining++;
        }

        // ゲーム開始時にステージにいる敵の数を取得する
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //enemiesRemaining = enemies.Length;
        Debug.Log("エネミーの数" + enemiesRemaining);
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDefeated()
    {
        enemiesRemaining--;

        CheckWave();
    }

    //ウェーブが進むかどうか確認する関数
    void CheckWave()
    {
        // 敵が全滅したら
        if (enemiesRemaining <= 0)
        {
            //現在のウェーブが最終ウェーブだったら扉を開く
            if (waveNum == waveList.Length - 1)
            {
                isOpen = true;
            }
            else
            {
                //違ったら次のウェーブをスポーンさせる
                waveNum++;

                for (int i = 0; i < waveList[waveNum].waveEnemy.Length; i++)
                {
                    WaveEnemy spawnEnemy = waveList[waveNum].waveEnemy[i];
                    Instantiate(spawnEnemy.enemyPrefab, spawnEnemy.spawnPosition, Quaternion.identity);

                    enemiesRemaining++;
                }
            }

        }
    }

    public void EnemyPossession()
    {
        enemiesRemaining--;

        CheckWave();
    }

    public void EnemyPossessionCancel()
    {
        enemiesRemaining++;

        CheckWave();
    }

    // 次のステージへ進む関数
    public void NextStage()
    {
        //ドアに近づきボタンを押したらこの関数を呼ぶ

        SceneManager.LoadScene(nextSceneName);
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }
}
