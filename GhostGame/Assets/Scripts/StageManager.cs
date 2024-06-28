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


    // Start is called before the first frame update
    void Start()
    {
        // ゲーム開始時にステージにいる敵の数を取得する
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesRemaining = enemies.Length;
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

        // 敵が全滅したら扉を開く
        if (enemiesRemaining <= 0)
        {
            Debug.Log("door open");
            isOpen = true;
        }
    }

    // 扉を開く関数
    void OpenDoor()
    {
        
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
