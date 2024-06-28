using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StageDoor : MonoBehaviour
{
    
    private StageManager sm;
    //���̃V�[���Ɉړ��ł���ʒu�Ƀv���C���[�����邩�ǂ���
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
            //�h�A��]�����āA�����ڂ��J����
            LeftDoor.transform.rotation = Quaternion.Slerp(LeftDoor.transform.rotation, transform.rotation * Quaternion.AngleAxis(-90, Vector3.up), Time.deltaTime);

            RightDoor.transform.rotation = Quaternion.Slerp(RightDoor.transform.rotation, transform.rotation *  Quaternion.AngleAxis(90, Vector3.up), Time.deltaTime);

        }

        if (isEnter && sm.GetIsOpen())
        {
            //��u�ňړ������Ⴄ����v���C���[�������Ă��������������
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
