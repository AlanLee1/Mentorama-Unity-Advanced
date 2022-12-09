using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossEnterCutscene : MonoBehaviour
{
    [SerializeField] PlayableDirector timeline;
    [SerializeField] TimelineAsset timelineAsset;

    public void Start()
    {
        StartCoroutine(EDelayInitialization());
    }

    IEnumerator EDelayInitialization()
    {
        yield return new WaitForEndOfFrame();

        SetReferences();
    }

    void SetReferences()
    {
        var cinemachineTrack = timelineAsset.GetOutputTrack(3);
        var activationTrack_Cam = timelineAsset.GetOutputTrack(1);
        var activationTrack_Boss = timelineAsset.GetOutputTrack(5);

        GameObject cam = Camera.main.gameObject;

        timeline.SetGenericBinding(cinemachineTrack, cam.GetComponent<CinemachineBrain>());
        timeline.SetGenericBinding(activationTrack_Cam, cam);
        timeline.SetGenericBinding(activationTrack_Boss, GameManager.instance.currentLevelManager.GetBossGameObject());
    }
}
