using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WindowSizeChange : Selectable
{
    [SerializeField] Text windowSizeChangeText;

    [SerializeField] PlayerInput playerInput;
    [SerializeField] EventSystem eventSystem;


    public enum Resolution
    {
        None,
        x1920x1080,
        x1280x720,
        x960x540,

        num,
    }

    Resolution size;

    //���E�擾
    Vector2 moveInput = Vector2.zero;
    Vector2 moveInputPrev = Vector2.zero;


    //�t���X�N���[��:true �E�B���h�E:false
    [SerializeField] Toggle fullScreenToggle;

    void OnEnable()
    {
        //���݃t���X�N���[����Ԃ��擾
        fullScreenToggle.isOn = Screen.fullScreen;

        //���݂̃T�C�Y�ɉ����ăT�C�Y�A�e�L�X�g�\����ύX
        switch (Screen.width)
        {
            case 1920:
                windowSizeChangeText.text = "1920x1080";
                size = Resolution.x1920x1080;
                break;

            case 1280:
                windowSizeChangeText.text = "1280x720";
                size = Resolution.x1280x720;
                break;

            case 960:
                windowSizeChangeText.text = "960x540";
                size = Resolution.x960x540;
                break;

            default:
                windowSizeChangeText.text = "test";
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //�ȑO�̓��͂��擾
        moveInputPrev = moveInput;

        //input���獶�E���擾���ăT�C�Y�ύX������
        //move����擾����@�����Ɨǂ��������肻���@
        moveInput = playerInput.currentActionMap["Move"].ReadValue<Vector2>();

        //���ݑI�����Ă�Q�[���I�u�W�F�N�g���擾�@�����Ɨǂ��������肻��
        if (this.gameObject == eventSystem.currentSelectedGameObject.gameObject)
        {
            // �𑜓x�ύX�̂��߂̍��E���͂����o
            //�E
            if (moveInput.x > 0.1 && moveInputPrev.x <= 0.1)
            {
                if (size > Resolution.None + 1)
                    size--;
            }
            //��
            else if (moveInput.x < -0.1 && moveInputPrev.x >= -0.1)
            {
                
                if (size < Resolution.num - 1)
                    size++;
            }
            else
            {
                return;
            }

            //�T�C�Y�ύX
            ScreenSizeChange();

        }
    }
      

    //�T�C�Y�ύX
    public void ScreenSizeChange()
    {
        switch (size)
        {
            case Resolution.x1920x1080:
                windowSizeChangeText.text = "1920x1080";
                if (fullScreenToggle.isOn)
                {
                    Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 60);
                }
                else 
                {
                    Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, 60);
                }

                break;

            case Resolution.x1280x720:
                windowSizeChangeText.text = "1280x720";
                if (fullScreenToggle.isOn)
                {
                    Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow, 60);
                }
                else
                {
                    Screen.SetResolution(1280, 720, FullScreenMode.Windowed, 60);
                }
                break;

            case Resolution.x960x540:
                windowSizeChangeText.text = "960x540";
                if (fullScreenToggle.isOn)
                {
                    Screen.SetResolution(960, 540, FullScreenMode.FullScreenWindow, 60);
                }
                else
                {
                    Screen.SetResolution(960, 540, FullScreenMode.Windowed, 60);
                }
                break;

            default:
                windowSizeChangeText.text = "test";
                break;
        }
    }

}
