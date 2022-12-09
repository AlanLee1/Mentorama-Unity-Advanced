using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance = null;

    public Player player;

    [Header("Scene Loading")]
    public int sceneToLoad;
    public int previousSceneID;
    [SerializeField] LoadingBar loadingBar;
    AsyncOperation asyncLoad = null;

    [Header("Current Level")]
    public LevelManager currentLevelManager;

    [Header("Game Over")]
    [SerializeField] GameOverCameraManager gameOverCamera;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] int gameSceneID = 2;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        player.gameObject.SetActive(false);
    }

    private void Start()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        if (!loadingBar.gameObject.activeInHierarchy)
        {
            loadingBar.gameObject.SetActive(true);
        }

        if (sceneToLoad != 0)
        {
            StartCoroutine(ELoadSceneAsync());
        }
    }

    private IEnumerator ELoadSceneAsync()
    {
        //WaitFadeOut
        yield return new WaitForSeconds(1f);

        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();

        asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        loadingBar.UpdateLoadingBar(0);

        if (asyncLoad != null)
        {
            while(!asyncLoad.isDone)
            {
                Debug.Log("progress: " + asyncLoad.progress);
                loadingBar.UpdateLoadingBar(asyncLoad.progress);
                yield return waitFrame;
            }

            loadingBar.UpdateLoadingBar(asyncLoad.progress);
            yield return waitFrame;

            loadingBar.PlayFadeOut();

            yield return new WaitForSeconds(.5f);
            loadingBar.gameObject.SetActive(false);

        }
    }

    public void LoadNextScene(int nextSceneId)
    {
        loadingBar.gameObject.SetActive(true);
        loadingBar.PlayFadeIn();

        player.gameObject.SetActive(false);

        previousSceneID = sceneToLoad;
        sceneToLoad = nextSceneId;

        LoadScene();
        UnloadPreviousScene();
    }

    public void UnloadPreviousScene()
    {
        if(previousSceneID != 0)
        {
            SceneManager.UnloadSceneAsync(previousSceneID);
        }
    }

    public void GameOver()
    {
        gameOverCamera.CallGameOverCamera();
        gameOverCanvas.SetActive(true);
    }

    public void ReloadGameScene()
    {
        SceneManager.LoadScene(gameSceneID, LoadSceneMode.Single);
    }
}
