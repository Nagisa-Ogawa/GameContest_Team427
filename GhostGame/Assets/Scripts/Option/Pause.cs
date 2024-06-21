using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Pause : MonoBehaviour
{
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button OptionButton;
    //[SerializeField] private Button ManualButton;
    [SerializeField] private Button TitleButton;



    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject OptionPanel;

    [SerializeField] private Button OptionBackButton;

    [SerializeField] PlayerInput playerInput;


    void Start()
    {
        PausePanel.SetActive(false);
        ContinueButton.onClick.AddListener(Continue);
        OptionButton.onClick.AddListener(Option);
        //ManualButton.onClick.AddListener(Manual);
        TitleButton.onClick.AddListener(Title);
    }

    private void Update()
    {
        //ポーズキーを押してポーズ画面が開かれてなければ開く
        if (playerInput.currentActionMap["Pause"].WasPressedThisFrame() && !PausePanel.activeSelf)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0.0f;
            ContinueButton.Select();
        }
    }


    private void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    private void Option()
    {
        OptionBackButton.Select();
        OptionPanel.SetActive(true);
    }
    private void Manual()
    {

    }
    private void Title()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TitleScene");
    }

}
