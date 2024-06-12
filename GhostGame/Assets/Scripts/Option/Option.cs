using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private Button BackButton;
    [SerializeField] private Button OptionButton;
    [SerializeField] private GameObject OptionPanel;
    void Start()
    {
        OptionPanel.SetActive(false);
        BackButton.onClick.AddListener(Close);
    }

    private void Close()
    {
        OptionButton.Select();
        OptionPanel.SetActive(false);
    }
}
