using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    NONE = 0,
    PATROL,
    TARGET_FOUND,
    TARGET_IN_RANGE,
    ATTACKING,
    HIT_STUN,
    DEAD
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected EnemyData enemyData;
    [SerializeField] EnemyHitCollider enemyHitCollider = null;

    public EnemyState enemyState = EnemyState.NONE;

    [Header("Patrol")]
    public List<Transform> patrolWaypoints;
    [SerializeField] float patrolCooldown = 3f;
    float patrolReady = 0f;
    int currentWaypoint = 0;

    [Header("Target")]
    protected Transform playerTransform;

    [Header("Attack")]
    [SerializeField] protected float attackDuration = 1f;
    protected float attackReady = 0f;
    protected WaitForSeconds attackWaitTime;

    [Header("Animation")]
    [SerializeField] Animator animator;
    readonly int anim_walk = Animator.StringToHash("isRunning");
    readonly int anim_attack = Animator.StringToHash("attack");
    readonly int anim_hit = Animator.StringToHash("hit");
    readonly int anim_die = Animator.StringToHash("isDead");

    //Internal
    protected NavMeshAgent agent;


    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyData.speed;
        attackWaitTime = new WaitForSeconds(attackDuration);
    }

    public void Start()
    {
        playerTransform = GameManager.instance.player.transform;
    }

    public void EnemySetup(List<Transform> waypoints)
    {
        patrolWaypoints = waypoints;
        currentWaypoint = Random.Range(0, patrolWaypoints.Count);

        enemyState = EnemyState.PATROL;
    }

    public void Update()
    {
        if (enemyState == EnemyState.HIT_STUN || enemyState == EnemyState.DEAD || enemyState == EnemyState.NONE)
        {
            return;
        }

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            animator.SetBool(anim_walk, true);
        } else
        {
            animator.SetBool(anim_walk, false);
        }

        if (enemyState == EnemyState.PATROL)
        {
            Patrol();
            return;
        }

        if (enemyState == EnemyState.TARGET_FOUND)
        {
            CheckTargetRange();
        }

        if (enemyState == EnemyState.TARGET_IN_RANGE)
        {
            PrepareToAttack();
        }
    }

    #region UpdateStates

    void Patrol()
    {
        int length = patrolWaypoints.Count;

        if (length > 0)
        {
            if ((transform.position - patrolWaypoints[currentWaypoint].position).sqrMagnitude > 1f)
            {
                agent.SetDestination(patrolWaypoints[currentWaypoint].position);
            } else
            {
                if (Time.time > patrolReady)
                {
                    currentWaypoint = Random.Range(0, patrolWaypoints.Count);

                    patrolReady = Time.time + patrolCooldown;

                    agent.SetDestination(patrolWaypoints[currentWaypoint].position);
                }
            }
        }


    }

    protected virtual void CheckTargetRange()
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        if ((transform.position - playerTransform.position).magnitude > enemyData.attackRange)
        {
            agent.SetDestination(playerTransform.position);
        } else
        {
            agent.isStopped = true;
            enemyState = EnemyState.TARGET_IN_RANGE;
        }
    }

    protected virtual void PrepareToAttack()
    {
        Vector3 playerDirection = playerTransform.position - transform.position;
        playerDirection.y = 0;

        Quaternion desiredRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, 720f * Time.deltaTime);

        if (transform.rotation != desiredRotation)
        {
            return;
        }

        if (Time.time > attackReady)
        {
            attackReady = Time.time + enemyData.attackCooldown;
            AttackTarget();
        } else
        {
            enemyState = EnemyState.TARGET_FOUND;
        }
    }

    protected virtual void AttackTarget()
    {
        if (GameManager.instance.player.currentHealth <= 0) return;

        enemyState = EnemyState.ATTACKING;
        animator.SetTrigger(anim_attack);

        Debug.Log("Enemy Attacked!");

        StartCoroutine(EWaitAttackDuration());
    }

    protected virtual IEnumerator EWaitAttackDuration()
    {
        yield return attackWaitTime;

        if (enemyState == EnemyState.ATTACKING)
        {
            CheckTargetRange();
            enemyState = EnemyState.TARGET_FOUND;
        }
    }

    #endregion

    public void SetTargetFound()
    {
        enemyState = EnemyState.TARGET_FOUND;
    }

    public void SetHitStunState()
    {
        agent.speed = 0f;
        if (enemyState != EnemyState.ATTACKING)
        {
            animator.SetTrigger(anim_hit);
            enemyState = EnemyState.HIT_STUN;
        }
    }

    public void ReturnFromHitStun()
    {
        agent.speed = enemyData.speed;
        if (enemyState != EnemyState.ATTACKING)
        {
            enemyState = EnemyState.TARGET_FOUND;
        }
    }

    public void SetDeadState()
    {
        enemyState = EnemyState.DEAD;
        animator.SetBool(anim_die, true);

        agent.enabled = false;
        GetComponent<Collider>().enabled = false;

        if (enemyHitCollider != null)
        {
            enemyHitCollider.gameObject.SetActive(false);
        }

        GameManager.instance.currentLevelManager.EnemyDead();
    }

}
