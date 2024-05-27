using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanAllowUI : MonoBehaviour
{
    [SerializeField]
    GameObject stanAllowUIObj = null;
    private EnemyBase enemy = null;
    // –îˆó‚ð•\Ž¦‚Å‚«‚é‚©‚Ìƒtƒ‰ƒO
    private bool canShowUI=false;
    public bool CanShowUI
    {
        get { return canShowUI; }
        private set { canShowUI = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.parent.GetComponent<EnemyBase>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerToEnemyDistance();
        transform.rotation=Camera.main.transform.rotation;
    }

    public void ShowAllowUI()
    {
        stanAllowUIObj.SetActive(true);
    }

    public void HideAllowUI()
    {
        stanAllowUIObj.SetActive(false);
    }

    void CheckPlayerToEnemyDistance()
    {
        PlayerController player = enemy.player;
        float distance = Vector3.Distance(player.transform.position, transform.parent.transform.position);
        if (distance <= player.possessionDistance)
        {
            canShowUI = true;
        }
        else
        {
            canShowUI = false;
        }
    }

}
