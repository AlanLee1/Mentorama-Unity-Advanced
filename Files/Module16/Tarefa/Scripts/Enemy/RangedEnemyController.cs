using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    [Header("Ranged Attack")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject smokeEffect;
    [SerializeField] GameObject fireProjectile;
    [SerializeField] LayerMask lineOfSightLayer;

    protected override void CheckTargetRange()
    {

        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        Vector3 playerDirection = playerTransform.position - transform.position;

        float playerDistance = playerDirection.magnitude;

        if (playerDistance > enemyData.attackRange)
        {
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            if (IsPlayerOnSight(playerDirection))
            {
                agent.isStopped = true;
                enemyState = EnemyState.TARGET_IN_RANGE;
            }
            else
            {
                agent.SetDestination(playerTransform.position);
            }
            
        }
    }

    private bool IsPlayerOnSight(Vector3 playerDirection)
    {
        Ray lineOfSightRay = new Ray(transform.position + Vector3.up, playerDirection);
        if (Physics.Raycast(lineOfSightRay, out RaycastHit hit, 500f, lineOfSightLayer))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player not on sight! Layer Hit: " + hit.collider.gameObject.layer);
                return false;
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    protected override void PrepareToAttack()
    {
        if (GameManager.instance.player.currentHealth <= 0) return;

        Vector3 playerDirection = playerTransform.position - transform.position;
        playerDirection.y = 0;

        Quaternion desiredRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, 540f * Time.deltaTime);

        if (Time.time > attackReady)
        {
            agent.updateRotation = false;
            transform.rotation = desiredRotation;

            attackReady = Time.time + enemyData.attackCooldown;
            AttackTarget();
            
        }
        else
        {
            enemyState = EnemyState.TARGET_FOUND;
        }
    }

    protected override void AttackTarget()
    {
        base.AttackTarget();

        Instantiate(fireProjectile, firePoint.position, firePoint.rotation);

        if(!smokeEffect.activeInHierarchy)
        {
            smokeEffect.SetActive(true);
        }

    }

    protected override IEnumerator EWaitAttackDuration()
    {
        yield return attackWaitTime;

        if (enemyState == EnemyState.ATTACKING)
        {
            CheckTargetRange();
            enemyState = EnemyState.TARGET_FOUND;
        }

        agent.updateRotation = true;
    }
}
