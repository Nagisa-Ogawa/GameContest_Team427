using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject followPlayer;
    [SerializeField]
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followPlayer.GetComponent<PlayerController>().GetPossessionEnemy() == null)
        {
            transform.position = followPlayer.transform.position + offset;
        }
        else
        {
            transform.position = followPlayer.GetComponent<PlayerController>().GetPossessionEnemy().transform.position + offset;
        }
        
    }
}
