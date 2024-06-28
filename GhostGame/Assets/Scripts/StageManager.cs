using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    // �G�̐����Ǘ�����ϐ�
    private int enemiesRemaining;

    [SerializeField]
    private string nextSceneName;

    //�h�A���J���Ă��邩�ǂ���
    bool isOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        // �Q�[���J�n���ɃX�e�[�W�ɂ���G�̐����擾����
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesRemaining = enemies.Length;
        Debug.Log("�G�l�~�[�̐�" + enemiesRemaining);
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDefeated()
    {
        enemiesRemaining--;

        // �G���S�ł���������J��
        if (enemiesRemaining <= 0)
        {
            Debug.Log("door open");
            isOpen = true;
        }
    }

    // �����J���֐�
    void OpenDoor()
    {
        
    }

    // ���̃X�e�[�W�֐i�ފ֐�
    public void NextStage()
    {
        //�h�A�ɋ߂Â��{�^�����������炱�̊֐����Ă�

        SceneManager.LoadScene(nextSceneName);
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }
}
