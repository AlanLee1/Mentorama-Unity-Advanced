using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] GameObject trailVFX;
    [SerializeField] GameObject chargeAttackVFX;
    [SerializeField] Collider attackCollider;
    [SerializeField] Collider comboAttackCollider;
    [SerializeField] Collider chargeAttackCollider;

    [Header("SFX")]
    [SerializeField] AudioSource footstepsChannel;
    [SerializeField] AudioSource attackChannel;
    [SerializeField] AudioClip sfx_footsteps;
    [SerializeField] AudioClip sfx_swoosh;
    [SerializeField] AudioClip sfx_charged_attack;
    [SerializeField] AudioClip sfx_ranged_attack;


    public void ShowSwordTrail()
    {
        if (!trailVFX.activeInHierarchy)
        {
            trailVFX.SetActive(true);
        }
    }

    public void HideSwordTrail()
    {
        if (trailVFX.gameObject.activeInHierarchy)
        {
            trailVFX.gameObject.SetActive(false);
        }
    }

    public void EnableHitCollider(int num)
    {
        switch(num)
        {
            case 1:
                attackCollider.enabled = true;
                break;

            case 2:
                comboAttackCollider.enabled = true;
                break;

            case 3:
                chargeAttackCollider.enabled = true;
                break;

        }
    }

    public void PlayChargeAttackVFX()
    {
        if (!chargeAttackVFX.activeInHierarchy)
        {
            chargeAttackVFX.SetActive(true);
        }
    }

    public void DisableHitCollider()
    {
        attackCollider.enabled = false;
        comboAttackCollider.enabled = false;
        chargeAttackCollider.enabled = false;
    }

    public void PlayFootstepsSFX()
    {
        AudioManager.instance.PlaySFX(footstepsChannel, sfx_footsteps, true);
    }

    public void PlayAttackSFX()
    {
        AudioManager.instance.PlaySFX(attackChannel, sfx_swoosh, true);
    }

    public void PlayChargeAttackSFX()
    {
        AudioManager.instance.PlaySFX(attackChannel, sfx_charged_attack, true);
    }

    public void PlayRangedAttackSFX()
    {
        AudioManager.instance.PlaySFX(attackChannel, .6f, sfx_ranged_attack);
    }
}
