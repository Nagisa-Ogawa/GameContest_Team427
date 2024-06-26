using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Title : MonoBehaviour
{
    [SerializeField] private Button StartButton;
    [SerializeField] private Button OptionButton;
    [SerializeField] private Button EndButton;

    [SerializeField] private GameObject TitlePanel;
    [SerializeField] private GameObject OptionPanel;

    [SerializeField] private Button OptionBackButton;



    void Start()
    {
        StartButton.Select();
        StartButton.onClick.AddListener(GameStart);
        OptionButton.onClick.AddListener(Option);
        EndButton.onClick.AddListener(End);

    }

    private void Update()
    {

    }


    private void GameStart()
    {
        //ゲームシーンへ遷移
        SceneManager.LoadScene("PlayerTestScene");
    }
    private void Option()
    {
        OptionBackButton.Select();
        OptionPanel.SetActive(true);
    }

    private void End()
    {
        //終了
        Application.Quit();
    }
}
