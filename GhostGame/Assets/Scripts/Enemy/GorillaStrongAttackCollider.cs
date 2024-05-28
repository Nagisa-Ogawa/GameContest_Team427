using System.Collections;
using UnityEngine;

public class GorillaStrongAttackCollider : MonoBehaviour
{
    private GorillaEnemy gorilla;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        gorilla = transform.parent.GetComponent<GorillaEnemy>();
        if (gorilla == null)
        {
            Debug.LogError("GorillaEnemy component not found on parent object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine(PerformStrongAttack(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // You may want to handle cases where the player exits the trigger area mid-attack.
        }
    }

    private IEnumerator PerformStrongAttack(Collider player)
    {
        isAttacking = true;

        // 攻击释放时间
        yield return new WaitForSeconds(0.6f);

        // 攻击判定
        if (player.CompareTag("Player"))
        {
            if (gorilla != null && gorilla.player != null)
            {
                gorilla.player.TakeDamage(20);
                Debug.Log("ダメージ20");
            }
            else
            {
                Debug.LogError("Gorilla or Player reference is null.");
            }
        }

        // 攻击总时间
        yield return new WaitForSeconds(0.9f); // 总时间1.5秒减去之前等待的0.6秒

        isAttacking = false;
    }

    // 调试用Gizmos，显示攻击范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
