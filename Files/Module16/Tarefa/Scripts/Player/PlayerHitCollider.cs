using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCollider : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject hitParticle;
    [SerializeField] [Range(0.5f, 3f)] float damageMultiplier = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(Mathf.FloorToInt(playerData.attack * damageMultiplier), hitParticle);
        }
    }
}
