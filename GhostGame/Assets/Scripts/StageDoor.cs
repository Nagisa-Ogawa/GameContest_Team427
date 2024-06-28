using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StageDoor : MonoBehaviour
{
    
    private StageManager sm;
    //次のシーンに移動できる位置にプレイヤーがいるかどうか
    bool isEnter = false;

    [SerializeField]
    private GameObject LeftDoor;

    [SerializeField]
    private GameObject RightDoor;

    PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.FindWithTag("StageManager").GetComponent<StageManager>();
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sm.GetIsOpen())
        {
            //ドア回転させて、見た目を開ける
            LeftDoor.transform.rotation = Quaternion.Slerp(LeftDoor.transform.rotation, transform.rotation * Quaternion.AngleAxis(-90, Vector3.up), Time.deltaTime);

            RightDoor.transform.rotation = Quaternion.Slerp(RightDoor.transform.rotation, transform.rotation *  Quaternion.AngleAxis(90, Vector3.up), Time.deltaTime);

        }

        if (isEnter && sm.GetIsOpen())
        {
            //一瞬で移動しちゃうからプレイヤーが歩いていく動作をつけたい
            sm.NextStage();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isEnter = false;
        }
    }
}
