using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitCollider : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] GameObject hitParticle;
    [SerializeField] [Range(0.5f, 3f)] float damageMultiplier = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(Mathf.FloorToInt(enemyData.attack * damageMultiplier), hitParticle);
        }
    }
}
