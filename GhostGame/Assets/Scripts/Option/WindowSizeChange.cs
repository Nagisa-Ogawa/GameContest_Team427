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

    //左右取得
    Vector2 moveInput = Vector2.zero;
    Vector2 moveInputPrev = Vector2.zero;


    //フルスクリーン:true ウィンドウ:false
    [SerializeField] Toggle fullScreenToggle;

    void OnEnable()
    {
        //現在フルスクリーン状態か取得
        fullScreenToggle.isOn = Screen.fullScreen;

        //現在のサイズに応じてサイズ、テキスト表示を変更
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
        //以前の入力を取得
        moveInputPrev = moveInput;

        //inputから左右を取得してサイズ変更させる
        //moveから取得する　もっと良いやり方ありそう　
        moveInput = playerInput.currentActionMap["Move"].ReadValue<Vector2>();

        //現在選択してるゲームオブジェクトを取得　もっと良いやり方ありそう
        if (this.gameObject == eventSystem.currentSelectedGameObject.gameObject)
        {
            // 解像度変更のための左右入力を検出
            //右
            if (moveInput.x > 0.1 && moveInputPrev.x <= 0.1)
            {
                if (size > Resolution.None + 1)
                    size--;
            }
            //左
            else if (moveInput.x < -0.1 && moveInputPrev.x >= -0.1)
            {
                
                if (size < Resolution.num - 1)
                    size++;
            }
            else
            {
                return;
            }

            //サイズ変更
            ScreenSizeChange();

        }
    }
      

    //サイズ変更
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
