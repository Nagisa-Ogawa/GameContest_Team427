using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanAllowUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerObj=null;
    List<StanAllowUI> stanAllowUIList = new List<StanAllowUI>();
    int activeUIIndex = -1;     // ���ݖ��UI��\�����Ă���G�l�~�[�̃��X�g�̓Y����
    StanAllowUI activeUI = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        // ���ݖ���\�����Ă��邩
        if(activeUIIndex != -1)
        {
            // ��Ɍ��ݖ���\�����Ă���G���߈ˉ\���`�F�b�N
            if (!activeUI.CanShowUI)
            {
                // �߈˕s�\�Ȃ����\�����Ȃ�
                HideAllowUI(activeUI);
                // ���ɜ߈ˉ\�ȓG�����邩�`�F�b�N
                CheckCanShowAllowUI();
            }
        }
        else
        {
            // ����\�����Ă��ȂȂ�߈ˉ\�ȓG�����Ȃ����`�F�b�N
            CheckCanShowAllowUI();
        }
    }

    void ShowAllowUI(StanAllowUI ui,int index)
    {
        activeUI = ui;
        activeUIIndex = index;
        ui.ShowAllowUI();
    }

    void HideAllowUI(StanAllowUI ui) 
    {
        activeUI = null;
        activeUIIndex = -1;
        ui.HideAllowUI();
    }

    /// <summary>
    /// �X�^�������G�����X�g�ɒǉ����鏈��
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemyList(GameObject enemy)
    {
        StanAllowUI stanAllowUI = enemy.GetComponentInChildren<StanAllowUI>();
        // ���X�g����̏ꍇ
        if (stanAllowUIList.Count == 0)
        {
            // �߈ˉ\�͈͓��Ȃ炷���ɖ���\��
            if(stanAllowUI.CanShowUI)
            {
                ShowAllowUI(stanAllowUI,0);
            }
            stanAllowUIList.Add(stanAllowUI);
        }
        else
        {
            stanAllowUIList.Add(stanAllowUI);
            // ���X�g�ɃG�l�~�[��ǉ��������X���W����Ƀ\�[�g
            stanAllowUIList.Sort((a,b)=>a.transform.parent.transform.position.x.CompareTo(b.transform.parent.transform.position.x));
            // ���ɖ���\�����Ă����̂Ȃ珇�Ԃ��ς���Ă���\��������̂ōēx�Z�b�g
            for(int i = 0; i < stanAllowUIList.Count; i++)
            {
                if(activeUI == stanAllowUIList[i])
                {
                    activeUIIndex = i;
                    break;
                }
            }
        }
    }


    public void DeleteEnemyList(GameObject enemy)
    {
        // �폜������UI�����݃A�N�e�B�u�Ȃ�
            // �Ⴄ���UI���A�N�e�B�u�ɂ���
        StanAllowUI stanAllowUI = enemy.GetComponentInChildren<StanAllowUI>();
        stanAllowUIList.Remove(stanAllowUI);
    }

    public void NextListEnemy()
    {
        // ���X�g�ɓ�ȏ�Ȃ��Ȃ�ς��Ȃ�
        if (stanAllowUIList.Count < 2)
        {
            return;
        }
        // ���X�g�����ɂ��ǂ��Ĝ߈ˉ\��UI���������Ȃ炻��UI�ɕύX
        for(int i=activeUIIndex+1;i<stanAllowUIList.Count;i++)
        {
            if (stanAllowUIList[i].CanShowUI)
            {
                HideAllowUI(activeUI);
                ShowAllowUI(stanAllowUIList[i], i);
                return;
            }
        }
        for(int i = 0; i < activeUIIndex; i++)
        {
            if (stanAllowUIList[i].CanShowUI)
            {
                HideAllowUI(activeUI);
                ShowAllowUI(stanAllowUIList[i], i);
                return;
            }
        }
    }

    public void PrevListEnemy()
    {
        // ���X�g�ɓ�ȏ�Ȃ��Ȃ�ς��Ȃ�
        if (stanAllowUIList.Count < 2)
        {
            return;
        }
        // ���X�g���t���ɂ��ǂ��Ĝ߈ˉ\��UI���������Ȃ炻��UI�ɕύX
        for(int i=activeUIIndex-1;i>=0;i--)
        {
            if (stanAllowUIList[i].CanShowUI)
            {
                HideAllowUI(activeUI);
                ShowAllowUI(stanAllowUIList[i], i);
                return;
            }
        }
        for(int i=stanAllowUIList.Count-1;i>activeUIIndex;i--)
        {
            if (stanAllowUIList[i].CanShowUI)
            {
                HideAllowUI(activeUI);
                ShowAllowUI(stanAllowUIList[i], i);
                return;
            }
        }
    }

    void CheckCanShowAllowUI()
    {
        for (int i = 0; i < stanAllowUIList.Count; i++)
        {
            if (stanAllowUIList[i].CanShowUI)
            {
                ShowAllowUI(stanAllowUIList[i], i);
                return;
            }
        }
    }
}
