using UnityEngine;

public class CollectibleHeart : MonoBehaviour
{
    [SerializeField] GameObject visualGroup;
    [SerializeField] GameObject vfxCollect;
    [SerializeField] AudioClip sfxCollect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().RestoreFullHealth();
            visualGroup.SetActive(false);
            vfxCollect.SetActive(true);

            AudioManager.instance.PlayGlobalSFX(sfxCollect, true);

            this.gameObject.SetActive(false);

        }
    }
}
