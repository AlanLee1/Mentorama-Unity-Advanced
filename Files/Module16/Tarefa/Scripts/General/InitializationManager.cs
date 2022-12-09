using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class InitializationManager : MonoBehaviour
{
    [Header("Vignette Video")]
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] float playVideoWaitTime = 3f;

    [Header("Title Scene")]
    [SerializeField] int titleSceneID = 0;
    [SerializeField] float loadTitleSceneWaitTime = 3f;

    private void Start()
    {
        StartCoroutine(EWaitToPlayVideo());
    }

    IEnumerator EWaitToPlayVideo()
    {
        yield return new WaitForSeconds(playVideoWaitTime);
        
        if(videoPlayer != null)
        {
            videoPlayer.Play();
        }

        yield return new WaitForSeconds(loadTitleSceneWaitTime);

        SceneManager.LoadScene(titleSceneID);

    }


}
