using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Result : MonoBehaviour
{
    [SerializeField] private Button TitleButton;



    void Start()
    {
        TitleButton.Select();
        TitleButton.onClick.AddListener(End);

    }

    private void Update()
    {

    }

    private void End()
    {
        //タイトルシーンへ遷移
        SceneManager.LoadScene("TitleScene");
    }
}
