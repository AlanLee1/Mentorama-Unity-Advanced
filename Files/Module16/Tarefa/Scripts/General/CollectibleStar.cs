using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleStar : MonoBehaviour
{
    [SerializeField] AudioClip sfxCollect;
    [SerializeField] Animator animatorStar;
    readonly int anim_win = Animator.StringToHash("Win");

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PanelFinal());
        }
    }

    IEnumerator PanelFinal()
    {
        AudioManager.instance.PlayGlobalSFX(sfxCollect, true);
        animatorStar.SetTrigger(anim_win);
        yield return new WaitForSecondsRealtime(6f);
        SceneManager.LoadScene("scene_Final", LoadSceneMode.Single);
    }
}
