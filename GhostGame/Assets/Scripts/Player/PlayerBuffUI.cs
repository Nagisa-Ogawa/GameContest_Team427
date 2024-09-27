using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffUI : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    [SerializeField]
    private GameObject BuffUI = null;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.NowBuffStack > 0 && BuffUI.activeSelf == false)
        {
            BuffUI.SetActive(true);
        }
        else if (playerController.NowBuffStack == 0 && BuffUI.activeSelf == true)
        {
            BuffUI.SetActive(false);
        }
    }
}
