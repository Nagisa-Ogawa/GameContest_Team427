using System.Collections;
using UnityEngine;

public class TigerStanAttack : MonoBehaviour
{
    private TigerEnenmy tiger;
    private bool isAttacking = false;

   
    void Start()
    {
        tiger = transform.parent.GetComponent<TigerEnenmy>();
        if (tiger == null)
        {
            Debug.LogError("TigerEnenmy component not found on parent object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine(StanAttackRoutine(other));
        }
    }

    private IEnumerator StanAttackRoutine(Collider player)
    {
        isAttacking = true;

        // 攻击释放时间
        yield return new WaitForSeconds(0.2f);

        // 攻击判定
        if (player.CompareTag("Player"))
        {
            tiger.player.TakeStanDamage(5);
            Debug.Log("スタンダメージ10");
        }

        // 攻击总时间
        yield return new WaitForSeconds(1.3f); // 总时间1.5秒减去之前等待的0.2秒

        isAttacking = false;
    }

    // 调试用Gizmos，显示攻击范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
