using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerHUDManager playerHUD;


    [Header("Status")]
    public int currentHealth;
    public int currentRangedCharges;
    public float rangedChargeRefresh = 5f;

    [Header("Damage")]
    [SerializeField] float hitCooldown = .2f;
    [SerializeField] float hitStunTime = .2f;
    [SerializeField] Transform hitPosition;
    float hitReady = 0f;

    [Header("Animation")]
    [SerializeField] Animation hitAnim;

    [Header("SFX")]
    [SerializeField] AudioSource hitChannel;
    [SerializeField] AudioClip sfx_hit;
    [SerializeField] AudioClip sfx_dead;

    WaitForSeconds hitWaitTime;
    WaitForSeconds refreshWaitTime;
    Coroutine _HitStunCoroutine;


    public void Awake()
    {
        hitWaitTime = new WaitForSeconds(hitStunTime);
        refreshWaitTime = new WaitForSeconds(rangedChargeRefresh);
        currentHealth = playerData.health;
    }

    public void OnEnable()
    {
        currentRangedCharges = playerData.rangedCharges;

        playerHUD.UpdateHealthHUD(currentHealth, playerData.health);
        playerHUD.UpdateChargeHUD(currentRangedCharges, playerData.rangedCharges);
    }

    public void UseRangedCharge()
    {
        currentRangedCharges--;
        playerHUD.UpdateChargeHUD(currentRangedCharges, playerData.rangedCharges);

        if(currentRangedCharges < playerData.rangedCharges)
        {
            StartCoroutine(ERefreshRangedCharge());
        }
    }

    IEnumerator ERefreshRangedCharge()
    {
        yield return refreshWaitTime;

        if (currentRangedCharges < playerData.rangedCharges)
        {
            currentRangedCharges++;
            playerHUD.UpdateChargeHUD(currentRangedCharges, playerData.rangedCharges);
        }
    }

    public void TakeDamage(int damage, GameObject hitParticle)
    {
        if (Time.time > hitReady && currentHealth > 0)
        {
            hitReady = Time.time + hitCooldown;

            currentHealth -= damage;
            playerHUD.UpdateHealthHUD(currentHealth, playerData.health);

            if (_HitStunCoroutine != null)
            {
                StopCoroutine(_HitStunCoroutine);
            }

            hitAnim.Stop();
            hitAnim.Play();
            Instantiate(hitParticle, hitPosition.position, Quaternion.identity, hitPosition);
            AudioManager.instance.PlaySFX(hitChannel, sfx_hit, true);

            if (currentHealth <= 0)
            {
                AudioManager.instance.PlayGlobalSFX(sfx_dead, false);

                playerController.SetDeadState();
                playerHUD.DisableHUD();
                GameManager.instance.GameOver();
            }
            else
            {
                playerController.SetHitStunState();
                _HitStunCoroutine = StartCoroutine(EWaitHitStun());
            }

        }
    }

    IEnumerator EWaitHitStun()
    {
        yield return hitWaitTime;
        playerController.ReturnFromHitStun();
    }

    public void RestoreFullHealth()
    {
        currentHealth = playerData.health;
        playerHUD.UpdateHealthHUD(currentHealth, playerData.health);

    }

}
