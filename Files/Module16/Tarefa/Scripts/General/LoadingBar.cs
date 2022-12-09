using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animation))]
public class LoadingBar : MonoBehaviour
{
    [SerializeField] Slider loadingBar;

    Animation anim;
    readonly string anim_fadeIn = "anim_fade_in";
    readonly string anim_fadeOut = "anim_fade_out";

    public void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void UpdateLoadingBar(float progress)
    {
        loadingBar.value = progress;
    }

    public void PlayFadeIn()
    {
        anim.Stop();
        anim.Play(anim_fadeIn);
    }

    public void PlayFadeOut()
    {
        anim.Stop();
        anim.Play(anim_fadeOut);
    }
}
