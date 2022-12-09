using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] float speed = 5f;
    [SerializeField] float lifetime = 5f;
    [SerializeField] Transform tipPoint;

    [Header("Hit Collider")]
    [SerializeField] EnemyData enemyData;
    [SerializeField] GameObject hitParticle;
    [SerializeField] [Range(0.5f, 3f)] float damageMultiplier = 1f;


    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(Mathf.FloorToInt(enemyData.attack * damageMultiplier), hitParticle);
        }
        else
        {
            Instantiate(hitParticle, tipPoint.position, Quaternion.identity);
        }

        Destroy(this.gameObject, .1f);
    }
}
