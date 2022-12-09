using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float masterVolume = 1f;
    public float bgmVolume = .6f;

    [SerializeField] AudioSource bgmChannel;
    [SerializeField] AudioSource globalChannel;
    [SerializeField] float crossfadeTime = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    #region BGM

    public bool IsAlreadyPlayingBGM(AudioClip clip)
    {
        return bgmChannel.clip == clip;
    }

    public void SetBGMSong(AudioClip clip)
    {
        StartCoroutine(EDoCrossfade(clip));
    }

    IEnumerator EDoCrossfade(AudioClip clip)
    {
        float currentVolume = bgmVolume;

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(crossfadeTime / 10f);

            currentVolume -= bgmVolume / 10f;

            bgmChannel.volume = Mathf.Clamp(currentVolume, 0f, 1f) * masterVolume; 
        }

        bgmChannel.Stop();
        bgmChannel.clip = clip;
        bgmChannel.Play();

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(crossfadeTime / 10f);

            currentVolume += bgmVolume / 10f;

            bgmChannel.volume = Mathf.Clamp(currentVolume, 0f, 1f) * masterVolume;
        }

        bgmChannel.volume = bgmVolume * masterVolume;

    }

    public void StopBGMSong()
    {
        if(bgmChannel.isPlaying)
        {
            bgmChannel.Stop();
        }
    }

#endregion

    public void PlaySFX(AudioSource output, AudioClip clip, bool changePitch = false)
    {
        output.volume *= masterVolume;

        if(output.isPlaying)
        {
            output.Stop();
        }

        if(changePitch)
        {
            output.pitch = Random.Range(.8f, 1.2f);
        }
        else
        {
            output.pitch = 1;
        }

        output.clip = clip;
        output.Play();
    }


    public void PlaySFX(AudioSource output, float volume, AudioClip clip, bool changePitch = false)
    {
        output.volume = volume;

        PlaySFX(output, clip, changePitch);
    }

    public void PlayGlobalSFX(AudioClip clip, bool changePitch = false)
    {
        PlaySFX(globalChannel, 1f, clip, changePitch);
    }

    public void PlayGlobalSFX(float volume, AudioClip clip, bool changePitch = false)
    {
        PlaySFX(globalChannel, volume, clip, changePitch);
    }

}
