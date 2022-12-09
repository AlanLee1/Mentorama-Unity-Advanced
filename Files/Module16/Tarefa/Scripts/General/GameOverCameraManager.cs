using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameOverCameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera gameOverVirtualCamera;
    [SerializeField] LayerMask gameOverMask;

    public void CallGameOverCamera()
    {
        //Camera cam = GetComponent<Camera>();
        //cam.cullingMask = gameOverMask;
        //cam.backgroundColor = Color.black;
        //cam.clearFlags = CameraClearFlags.Color;

        gameOverVirtualCamera.Priority = 20;
    }
}
