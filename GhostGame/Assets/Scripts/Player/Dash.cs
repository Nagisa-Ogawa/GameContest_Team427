using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    PlayerController controller;

    // ダッシュスピードとダッシュ時間の変数宣言
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // KかコントローラーのAが押された瞬間（ダッシュ時）
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            StartCoroutine(DashCoroutine());
        }
    }

    // ダッシュのコルーチン
    private IEnumerator DashCoroutine()
    {
        Vector3 dashVelocity = controller.transform.forward * dashSpeed;
        controller.Rb.velocity = new Vector3(dashVelocity.x, controller.Rb.velocity.y, dashVelocity.z); // ダッシュ方向に速度を設定
        yield return new WaitForSeconds(dashDuration);
        controller.Rb.velocity = Vector3.zero; // ダッシュ後に速度をリセット
    }
}
