using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] GameObject trailVFX;
    [SerializeField] Collider attackCollider;

    [Header("SFX")]
    [SerializeField] AudioSource footstepsChannel;
    [SerializeField] AudioSource attackChannel;
    public void EnableHitCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableHitCollider()
    {
        attackCollider.enabled = false;
    }

    public void ShowSwordTrail()
    {
        if (!trailVFX.activeInHierarchy)
        {
            trailVFX.SetActive(true);
        }
    }

    public void ShowAttackTrail()
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

    public void HideAttackTrail()
    {
        if (trailVFX.gameObject.activeInHierarchy)
        {
            trailVFX.gameObject.SetActive(false);
        }
    }

    public void PlayFootstepsSFX()
    {
        AudioManager.instance.PlaySFX(footstepsChannel, footstepsChannel.clip, true);
    }

    public void PlayAttackSFX()
    {
        AudioManager.instance.PlaySFX(attackChannel, attackChannel.clip, true);
    }



}
