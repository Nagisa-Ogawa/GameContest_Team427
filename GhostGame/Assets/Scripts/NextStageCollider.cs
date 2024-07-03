using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageCollider : MonoBehaviour
{
    //次のシーンに移動できる位置にプレイヤーがいるかどうか
    bool isEnter = false;

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

    public bool GetIsEnter()
    {
        return isEnter;
    }
}
