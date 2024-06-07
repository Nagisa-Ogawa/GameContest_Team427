using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    //表示用UI
    [SerializeField]
    private GameObject ChoiceUI;


    public void HideStatusUI()//生きてたらUIを非表示にする
    {
        ChoiceUI.SetActive(false);
    }

    public void StatusUI() //死んだらUIを表示する
    {
        ChoiceUI.SetActive(true);
    }

  
}
