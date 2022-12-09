using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class WaitToPlayAnimation : MonoBehaviour
{
    [SerializeField] AnimationClip clip;
    [SerializeField] float waitTime = 2f;
    private void Start()
    {
        StartCoroutine(EWaitToPlayAnimation());
    }

    IEnumerator EWaitToPlayAnimation()
    {
        yield return new WaitForSeconds(waitTime);

        if (clip != null)
        {
            Animation anim = GetComponent<Animation>();
            anim.clip = clip;
            anim.Play();
        }

    }
}
