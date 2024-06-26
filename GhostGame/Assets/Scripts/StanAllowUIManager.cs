using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanAllowUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerObj=null;
    List<StanAllowUI> stanAllowUIList = new List<StanAllowUI>();
    int activeUIIndex = -1;     // 現在矢印UIを表示しているエネミーのリストの添え字
    StanAllowUI activeUI = null;
    public StanAllowUI ActiveUI
    {
        get { return activeUI; }
        set
        {
            activeUI = value;
            if(value != null)
            {
                // UIに何かセットされた時プレイヤーの憑依対象も変更
                GameObject enemy = value.transform.parent.gameObject;
                playerObj.GetComponent<PlayerController>().possessionTargetEnemy = enemy;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        // 現在矢印を表示しているか
        if(activeUIIndex != -1)
        {
            // 常に現在矢印を表示している敵が憑依可能かチェック
            if (!ActiveUI.CanShowUI)
            {
                // 憑依不可能なら矢印を表示しない
                HideAllowUI(ActiveUI);
                // 他に憑依可能な敵がいるかチェック
                CheckCanShowAllowUI();
            }
        }
        else
        {
            // 矢印を表示していななら憑依可能な敵がいないかチェック
            CheckCanShowAllowUI();
        }
    }

    void ShowAllowUI(StanAllowUI ui,int index)
    {
        ActiveUI = ui;
        activeUIIndex = index;
        ui.ShowAllowUI();
    }

    void HideAllowUI(StanAllowUI ui) 
    {
        ActiveUI = null;
        activeUIIndex = -1;
        ui.HideAllowUI();
    }

    /// <summary>
    /// スタンした敵をリストに追加する処理
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemyList(GameObject enemy)
    {
        StanAllowUI stanAllowUI = enemy.GetComponentInChildren<StanAllowUI>();
        // リストが空の場合
        if (stanAllowUIList.Count == 0)
        {
            // 憑依可能範囲内ならすぐに矢印を表示
            if(stanAllowUI.CanShowUI)
            {
                ShowAllowUI(stanAllowUI,0);
            }
            stanAllowUIList.Add(stanAllowUI);
        }
        else
        {
            stanAllowUIList.Add(stanAllowUI);
            // リストにエネミーを追加した後にX座標を基準にソート
            stanAllowUIList.Sort((a,b)=>a.transform.parent.transform.position.x.CompareTo(b.transform.parent.transform.position.x));
            // 既に矢印を表示していたのなら順番が変わっている可能性があるので再度セット
            for(int i = 0; i < stanAllowUIList.Count; i++)
            {
                if(activeUI == stanAllowUIList[i])
                {
                    activeUIIndex = i;
                    break;
                }
            }
        }
    }


    public void DeleteEnemyList(GameObject enemy)
    {
        StanAllowUI stanAllowUI = enemy.GetComponentInChildren<StanAllowUI>();
        // 削除する矢印UIが現在アクティブなら
        if (stanAllowUI == ActiveUI)
        {
            // 違う矢印UIをアクティブにする
            CheckCanShowAllowUI();
        }
        HideAllowUI(stanAllowUI);
        stanAllowUIList.Remove(stanAllowUI);
    }

    public void NextListEnemy()
    {
        // リストに二つ以上ないなら変えない
        if (stanAllowUIList.Count < 2)
        {
            return;
        }
        // リストを順にたどって憑依可能なUIを見つけたならそのUIに変更
        for(int i=activeUIIndex+1;i<stanAllowUIList.Count;i++)
        {
            if (stanAllowUIList[i].CanShowUI)
            {
                HideAllowUI(ActiveUI);
                ShowAllowUI(stanAllowUIList[i], i);
                return;
            }
        }
        for(int i = 0; i < activeUIIndex; i++)
        {
            if (stanAllowUIList[i].CanShowUI)
            {
                HideAllowUI(ActiveUI);
                ShowAllowUI(stanAllowUIList[i], i);
                return;
            }
        }
    }

    public void PrevListEnemy()
    {
        // リストに二つ以上ないなら変えない
        if (stanAllowUIList.Count < 2)
        {
            return;
        }
        // リストを逆順にたどって憑依可能なUIを見つけたならそのUIに変更
        for(int i=activeUIIndex-1;i>=0;i--)
        {
            if (stanAllowUIList[i].CanShowUI)
            {
                HideAllowUI(ActiveUI);
                ShowAllowUI(stanAllowUIList[i], i);
                return;
            }
        }
        for(int i=stanAllowUIList.Count-1;i>activeUIIndex;i--)
        {
            if (stanAllowUIList[i].CanShowUI)
            {
                HideAllowUI(ActiveUI);
                ShowAllowUI(stanAllowUIList[i], i);
                return;
            }
        }
    }

    void CheckCanShowAllowUI()
    {
        for (int i = 0; i < stanAllowUIList.Count; i++)
        {
            if (stanAllowUIList[i].CanShowUI)
            {
                ShowAllowUI(stanAllowUIList[i], i);
                return;
            }
        }
    }
}
