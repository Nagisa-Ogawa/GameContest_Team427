// GorillaAttack.cs
using System.Collections;
using UnityEngine;

public class GorillaAttack : MonoBehaviour
{
    private GorillaEnemy gorilla;
    private bool playerInTrigger = false;

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
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = true;
            StartCoroutine(PerformAttacks(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    private IEnumerator PerformAttacks(Collider player)
    {
        yield return PerformSingleAttack(player, 6, 0.5f);
        yield return new WaitForSeconds(0.2f);

        yield return PerformSingleAttack(player, 6, 0.5f);
        yield return new WaitForSeconds(0.2f);

        yield return PerformSingleAttack(player, 8, 0.8f);
        yield return new WaitForSeconds(0.4f);
    }

    private IEnumerator PerformSingleAttack(Collider player, int damage, float duration)
    {
        if (playerInTrigger)
        {
            if (gorilla != null && gorilla.player != null)
            {
                gorilla.player.TakeDamage(damage);
                Debug.Log("É_ÉÅÅ[ÉW" + damage);
            }
            else
            {
                Debug.LogError("Gorilla or Player reference is null.");
            }
        }

        yield return new WaitForSeconds(duration);
    }
}
