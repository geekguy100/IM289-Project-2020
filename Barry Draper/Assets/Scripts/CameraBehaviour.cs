/*****************************************************************************
// File Name : CameraBehaviour
// Author : Kyle Grenier
// Creation Date : March 02, 2020
//
// Brief Description : Code to snap the camera back to player's position when he is idle and controls the free camera mode.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private CinemachineFramingTransposer ft;

    private KylePlayerController player;
    private Rigidbody2D playerRb;

    private float initialLookaheadSmoothing;
    private float initialLookaheadTime;
    private float initialOrthoSize;

    [Header("Free Mode Attributes")]
    private bool freeMode = false;
    public bool clampInFreeMode = true;
    public float freeModeOrthoSize = 15;
    public float freeCamSpeed = 2f;
    public Vector2 minPosClamp;
    public Vector2 maxPosClamp;

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        ft = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        player = vcam.Follow.GetComponent<KylePlayerController>();
        playerRb = vcam.Follow.GetComponent<Rigidbody2D>();

        initialLookaheadSmoothing = ft.m_LookaheadSmoothing;
        initialLookaheadTime = ft.m_LookaheadTime;
        initialOrthoSize = vcam.m_Lens.OrthographicSize;
    }


    void Update()
    {
        //Snapping camera back to player when he is idle (not moving).
        if ((player.newPos == Vector2.zero) && (ft.m_LookaheadSmoothing > 0f) && (playerRb.velocity == Vector2.zero))
        {
            ft.m_LookaheadSmoothing = 0;
        }
        else if ((player.newPos != Vector2.zero) && (ft.m_LookaheadSmoothing == 0))
        {
            ft.m_LookaheadSmoothing = initialLookaheadSmoothing;
        }

        //If the 'U' key is pressed, handle free cam mode.
        if (Input.GetButtonDown("Free Cam Mode"))
        {
            HandleFreeCamMode();
        }

        if (freeMode)
        {
            transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), freeCamSpeed * Time.deltaTime);

            //Clamp the camera's position if it should be clamped.
            if (clampInFreeMode)
            {
                Vector3 pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, minPosClamp.x, maxPosClamp.x);
                pos.y = Mathf.Clamp(pos.y, minPosClamp.y, maxPosClamp.y);

                transform.position = pos;
            }
        }
    }

    void HandleFreeCamMode()
    {
        freeMode = !freeMode;

        if (freeMode)
        {
            vcam.m_Lens.OrthographicSize = freeModeOrthoSize;
            vcam.Follow = null;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            vcam.m_Lens.OrthographicSize = initialOrthoSize;

            vcam.Follow = player.transform;
        }
    }
}