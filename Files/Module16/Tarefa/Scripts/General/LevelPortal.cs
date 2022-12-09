using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    [SerializeField] int sceneToLoad = 0;
    [SerializeField] GameObject portalGroup;
    [SerializeField] GameObject wallCollider;
    bool isPortalActive = false;

    [Header("SFX")]
    [SerializeField] AudioSource portalChannel;
    [SerializeField] AudioClip sfx_portal_show;
    [SerializeField] AudioClip sfx_portal_enter;

    public void Start()
    {
        portalGroup.SetActive(false);
        wallCollider.SetActive(true);
    }

    public void ShowPortal()
    {
        AudioManager.instance.PlaySFX(portalChannel, sfx_portal_show, true);

        portalGroup.SetActive(true);
        isPortalActive = true;

        wallCollider.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPortalActive && other.CompareTag("Player"))
        {
            AudioManager.instance.PlayGlobalSFX(sfx_portal_enter, true);

            CallLoadScene();
        }
    }

    void CallLoadScene()
    {
        GameManager.instance.LoadNextScene(sceneToLoad);
    }
}
