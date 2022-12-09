using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] GraphicRaycaster raycaster;

    [Header("Fade Out Animation")]
    [SerializeField] GameObject fadeOutObject;
    [SerializeField] float fadeOutTime = 2f;

    [Header("Game Scene Load")]
    [SerializeField] int gameSceneID = 2;

    [Header("SFX")]
    [SerializeField] AudioSource clickSound;

    public void OnPlayButtonClick()
    {
        raycaster.enabled = false;
        clickSound.Play();
        StartCoroutine(EStartLoadGameScene());
    }

    IEnumerator EStartLoadGameScene()
    {
        fadeOutObject.SetActive(true);

        yield return new WaitForSeconds(fadeOutTime);

        SceneManager.LoadScene(gameSceneID);
    }

    public void OnQuitButtonClick()
    {
        raycaster.enabled = false;
        clickSound.Play();
        StartCoroutine(EStartQuitApplication());
    }

    IEnumerator EStartQuitApplication()
    {
        fadeOutObject.SetActive(true);

        yield return new WaitForSeconds(fadeOutTime);

        Application.Quit();
    }
}
