using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    //�\���pUI
    [SerializeField]
    private GameObject ChoiceUI;


    public void HideStatusUI()//�����Ă���UI���\���ɂ���
    {
        ChoiceUI.SetActive(false);
    }

    public void StatusUI() //���񂾂�UI��\������
    {
        ChoiceUI.SetActive(true);
    }

  
}
