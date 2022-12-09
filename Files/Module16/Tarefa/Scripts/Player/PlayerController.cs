using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    NONE,
    START_ATTACK,
    ATTACKED,
    START_COMBO_ATTACK,
    COMBO_ATTACKED,
    CHARGING,
    CHARGED,
    CHARGE_ATTACKED
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerState playerState = PlayerState.NONE;
    [SerializeField] Animator animator = null;
    Player player;
    Rigidbody rb = null;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 30f;
    [SerializeField] float dashForce = 5f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] GameObject dashTrail;
    Vector3 moveDirection = Vector3.zero;
    bool isAttacking = false;
    bool isHit = false;
    bool isDead = false;
    Coroutine canRunCoroutine = null;
    float dashReady = 0f;

    [Header("Attack")]
    [SerializeField] float attackCooldown = .15f;
    [SerializeField] float comboMinTime = .2f;
    [SerializeField] float comboMaxTime = .6f;
    float attackReady = 0f;
    float comboMinReady = 0f;
    float comboMaxReady = 0f;

    [Header("Charge Attack")]
    [SerializeField] float chargeStartTime = .2f;
    [SerializeField] float chargeHoldTime = 1f;
    [SerializeField] float chargeAttackCooldown = .5f;
    Coroutine _ChargeCoroutine = null;

    [Header("Ranged Attack")]
    [SerializeField] float rangedAttackCooldown = .4f;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject fireProjectile;
    [SerializeField] LayerMask mouseLayer;

    [Header("SFX")]
    [SerializeField] AudioSource dashChannel;

    //Animation Parameters
    int anim_normalAttack = Animator.StringToHash("attack");
    int anim_comboAttack = Animator.StringToHash("comboAttack");
    int anim_chargeAttack = Animator.StringToHash("chargeAttack");
    int anim_rangedAttack = Animator.StringToHash("rangedAttack");
    int anim_hit = Animator.StringToHash("hit");
    int anim_isRunning = Animator.StringToHash("isRunning");
    int anim_isCharging = Animator.StringToHash("isCharging");
    int anim_isDead = Animator.StringToHash("isDead");


    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    #region PlayerInputLogic

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead) return;

        moveDirection =  Vector3.right * context.ReadValue<Vector2>().x + Vector3.forward * context.ReadValue<Vector2>().y;
        //moveDirection = Quaternion.Euler(0f, -30, 0f) * moveDirection;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (isDead) return;

        if (Time.time > dashReady)
        {
            dashReady = Time.time + dashCooldown;

            dashTrail.SetActive(true);
            AudioManager.instance.PlaySFX(dashChannel, dashChannel.clip, true);
            StartCoroutine(DisableDashTrail(.6f));

            if (moveDirection.magnitude > 0f)
            {           
                rb.AddForce(moveDirection * dashForce * 1000f, ForceMode.Impulse);

                Quaternion rotateDirection = Quaternion.LookRotation(moveDirection);
                transform.rotation = rotateDirection;
            }
            else
            {
                rb.AddForce(transform.forward * dashForce * 1000f, ForceMode.Impulse);
            }

        }
    }

    public IEnumerator DisableDashTrail(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        dashTrail.SetActive(false);
    }

    public void OnRangedAttack()
    {
        if (isDead) return;

        if (Time.time > attackReady && player.currentRangedCharges > 0)
        {
            attackReady = Time.time + rangedAttackCooldown;
            player.UseRangedCharge();

            Ray mouseRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if(Physics.Raycast(mouseRay, out RaycastHit hit, 500f, mouseLayer))
            {
                Vector3 mouseDirection = hit.point - transform.position;
                mouseDirection.y = 0f;

                animator.SetTrigger(anim_rangedAttack);
                transform.rotation = Quaternion.LookRotation(mouseDirection, Vector3.up);

                if (canRunCoroutine != null)
                {
                    StopCoroutine(canRunCoroutine);
                }

                canRunCoroutine = StartCoroutine(EWaitRangedAttackAnimationToRun(rangedAttackCooldown));
            }
        }
    }

    private IEnumerator EWaitRangedAttackAnimationToRun(float waitTime)
    {
        isAttacking = true;
        yield return new WaitForSeconds(.1f);

        Instantiate(fireProjectile, firePoint.position, firePoint.rotation);
        
        yield return new WaitForSeconds(waitTime);
        isAttacking = false;

        yield break;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (isDead) return;
        
        if (context.started)
        {
           if (Time.time > attackReady && !(Time.time >= comboMinReady && Time.time <= comboMaxReady))
            {
                playerState = PlayerState.START_ATTACK;
                _ChargeCoroutine = StartCoroutine(ECheckCharging());
            }
            else if(Time.time >= comboMinReady && Time.time <= comboMaxReady && playerState == PlayerState.ATTACKED)
            {
                playerState = PlayerState.START_COMBO_ATTACK;
                _ChargeCoroutine = StartCoroutine(ECheckCharging());
            }
        }


        if(playerState == PlayerState.START_ATTACK || playerState == PlayerState.START_COMBO_ATTACK || playerState == PlayerState.CHARGING || playerState == PlayerState.CHARGED)
        {
            if (context.canceled)
            {
                StopCoroutine(_ChargeCoroutine);
                animator.SetBool(anim_isCharging, false);
                animator.SetLayerWeight(1, 0);

                if (context.duration >= chargeHoldTime)
                {
                    Debug.Log("Triggered a Charged Attack! " + context.duration);

                    playerState = PlayerState.CHARGE_ATTACKED;
                    animator.SetTrigger(anim_chargeAttack);

                    if(canRunCoroutine != null)
                    {
                        StopCoroutine(canRunCoroutine);
                    }
                    canRunCoroutine = StartCoroutine(EWaitAttackAnimationToRun(attackCooldown * 3f));

                    attackReady = Time.time + chargeAttackCooldown;
                }
                else
                {
                    if(playerState == PlayerState.START_ATTACK || playerState == PlayerState.CHARGING)
                    {
                        Debug.Log("Triggered a Normal Attack! " + context.duration);

                        playerState = PlayerState.ATTACKED;
                        animator.SetTrigger(anim_normalAttack);

                        if (canRunCoroutine != null)
                        {
                            StopCoroutine(canRunCoroutine);
                        }
                        canRunCoroutine = StartCoroutine(EWaitAttackAnimationToRun(attackCooldown));

                        attackReady = Time.time + attackCooldown;
                        comboMinReady = Time.time + comboMinTime;
                        comboMaxReady = Time.time + comboMaxTime;
                    }
                    else if(playerState == PlayerState.START_COMBO_ATTACK)
                    {
                        Debug.Log("Triggered a Combo Attack! " + context.duration);

                        playerState = PlayerState.COMBO_ATTACKED;
                        animator.SetTrigger(anim_comboAttack);

                        if (canRunCoroutine != null)
                        {
                            StopCoroutine(canRunCoroutine);
                        }
                        canRunCoroutine = StartCoroutine(EWaitAttackAnimationToRun(attackCooldown * 2f));

                        attackReady = Time.time + attackCooldown * 1.5f;
                    }

                }
            }
        }

    }

    private IEnumerator ECheckCharging()
    {
        WaitForSeconds waitTime = new WaitForSeconds(chargeStartTime);
        yield return waitTime;

        if (playerState == PlayerState.START_ATTACK || playerState == PlayerState.START_COMBO_ATTACK)
        {
            Debug.Log("Started Charging");
            playerState = PlayerState.CHARGING;
            animator.SetBool(anim_isCharging, true);
            animator.SetLayerWeight(1, 1);

        }


        waitTime = new WaitForSeconds(chargeHoldTime - chargeStartTime);
        yield return waitTime;

        if (playerState == PlayerState.CHARGING)
        {
            Debug.Log("Charge Complete - Can now Trigger a Charge Attack");
            playerState = PlayerState.CHARGED;
        }

        yield return null;
    }

    private IEnumerator EWaitAttackAnimationToRun(float waitTime)
    {
        isAttacking = true;
        yield return new WaitForSeconds(waitTime);
        isAttacking = false;

        yield break;
    }

    #endregion

    private void FixedUpdate()
    {
        if (isDead) return;

        rb.velocity = Vector3.zero;

        if (!isAttacking && !isHit)
        {
            rb.velocity = moveDirection * moveSpeed; // * Time.deltaTime;

            if (moveDirection.magnitude > 0f)
            {
                animator.SetBool(anim_isRunning, true);

                //Rotates only if when moving
                Quaternion rotateDirection = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateDirection, rotateSpeed);
            }
            else
            {
                animator.SetBool(anim_isRunning, false);
            }
        }       
    }

    public void SetHitStunState()
    {
        if(!isAttacking)
        {
            animator.SetTrigger(anim_hit);
            isHit = true;
        }
    }

    public void ReturnFromHitStun()
    {
        isHit = false;
    }

    public void SetDeadState()
    {
        isDead = true;
        animator.SetBool(anim_isDead, true);
    }



}
