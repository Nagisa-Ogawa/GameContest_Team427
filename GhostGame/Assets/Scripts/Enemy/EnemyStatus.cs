using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    //�G��MaxHP
    [SerializeField]
    private int maxhp = 10;
    //�G��HP
    [SerializeField]
    private int hp;

    //�\���pUI
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
            //UI��\������
            StatusUI();
        }
    }

    public void HideStatusUI()//�����Ă���UI���\���ɂ���
    {
        ChoiceUI.SetActive(false);
    }

    public void StatusUI() //���񂾂�UI��\������
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
