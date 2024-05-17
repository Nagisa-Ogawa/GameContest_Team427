using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    //敵のMaxHP
    [SerializeField]
    private int maxhp = 10;
    //敵のHP
    [SerializeField]
    private int hp;

    //表示用UI
    [SerializeField]
    private GameObject ChoiceUI;


    // Start is called before the first frame update
    void Start()
    {
        hp = maxhp;

    }

    public void SetHP(int hp)
    {
        this.hp = hp;

        if(hp > 0 )
        {
            HideStatusUI();
           
        }
       else if (hp <= 0)
        {
            Debug.Log("test");
            //UIを表示する
            StatusUI();
        }
    }

    public void HideStatusUI()//生きてたらUIを非表示にする
    {
        ChoiceUI.SetActive(false);
    }

    public void StatusUI() //死んだらUIを表示する
    {
        ChoiceUI.SetActive(true);
    }

   

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            SetHP(0);
        }
    }
}
