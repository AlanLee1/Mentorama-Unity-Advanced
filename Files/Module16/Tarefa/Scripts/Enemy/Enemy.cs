using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] EnemyController enemyController;

    [Header("Status")]
    public int currentHealth;
    Slider healthHubBar;

    [Header("Damage")]
    [SerializeField] float hitCooldown = .2f;
    [SerializeField] float hitStunTime = .2f;
    [SerializeField] Transform hitPosition;
    float hitReady = 0f;

    [Header("Animation")]
    [SerializeField] Animation hitAnim;

    [Header("SFX")]
    [SerializeField] AudioSource hitChannel;

    WaitForSeconds hitWaitTime;
    Coroutine _HitStunCoroutine;

    public void Awake()
    {
        hitWaitTime = new WaitForSeconds(hitStunTime);
    }

    public void Start()
    {
        currentHealth = enemyData.health;
        if (this.gameObject.name == "Enemy_Dark_Ethan(Clone)")
        {
            healthHubBar = GameObject.Find("health_boss").GetComponent<Slider>();
        }
    }

    public void TakeDamage(int damage, GameObject hitParticle)
    {
        if (Time.time > hitReady && currentHealth > 0)
        {
            hitReady = Time.time + hitCooldown;

            currentHealth -= damage;

            if (this.gameObject.name == "Enemy_Dark_Ethan(Clone)")
            {
                Debug.Log("entrou");
                healthHubBar.value = (float)(currentHealth * 1) / (float)enemyData.health;
            }

            if (_HitStunCoroutine != null)
            {
                StopCoroutine(_HitStunCoroutine);
            }

            hitAnim.Stop();
            hitAnim.Play();
            Instantiate(hitParticle, hitPosition.position, Quaternion.identity, hitPosition);
            AudioManager.instance.PlaySFX(hitChannel, hitChannel.clip, true);

            if (currentHealth <= 0)
            {
                enemyController.SetDeadState();
            } else
            {
                enemyController.SetHitStunState();

                _HitStunCoroutine = StartCoroutine(EWaitHitStun());
            }

        }
    }

    IEnumerator EWaitHitStun()
    {
        yield return hitWaitTime;
        enemyController.ReturnFromHitStun();
    }

}
