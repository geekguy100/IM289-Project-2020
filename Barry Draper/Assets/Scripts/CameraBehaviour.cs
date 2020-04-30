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

    private PlayerController player;
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

    //[Header("Player Movement Offsets")]
    //public float leftOffset = 0.7f;
    //public float rightOffset = 0.3f;
    //public float upOffset = 0.7f;
    //public float downOffset = 0.3f;

    private Vector3 newPos;

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        ft = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        player = vcam.Follow.GetComponent<PlayerController>();
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

        //HandleOffsetChanges();
        
        //If the 'U' key is pressed, handle free cam mode.
        if (Input.GetButtonDown("Free Cam Mode"))
        {
            ToggleFreeCamMode();
        }

        if (freeMode)
        {
            //Get input axes.
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            //Assign the direction the camera should pan in 'newPos.'
            newPos.x = h;
            newPos.y = v;
            newPos.z = 0;

            //Move the camera's current position to the new position with speed 'freeCamSpeed' over Time.deltaTime.
            transform.position += newPos * freeCamSpeed * Time.deltaTime;

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

    //void HandleOffsetChanges()
    //{
    //    if (freeMode)
    //        return;

    //    //If the player is moving to the left, set the cameraX in that direction.
    //    //if (player.newPos.x < 0)
    //    //{
    //    //    ft.m_ScreenX = leftOffset;
    //    //}
    //    //else if (player.newPos.x > 0)
    //    //{
    //    //    ft.m_ScreenX = rightOffset;
    //    //}
    //    //else
    //    //{
    //    //    ft.m_ScreenX = 0.5f;
    //    //}

    //    //if (player.umbrellaUp || playerRb.velocity.y > 0)
    //    //{
    //    //    ft.m_ScreenY = upOffset;
    //    //}
    //    //else if (player.umbrellaDown || playerRb.velocity.y < 0)
    //    //{
    //    //    ft.m_ScreenY = downOffset;
    //    //}
    //    //else
    //    //{
    //    //    ft.m_ScreenY = 0.5f;
    //    //}
    //}

    void ToggleFreeCamMode()
    {
        freeMode = !freeMode;
        GameControllerScript.instance.SetFreeCamMode(freeMode);

        if (freeMode)
        {
            //Initiate the current newPos to the camera's current position.
            newPos = transform.position;

            vcam.m_Lens.OrthographicSize = freeModeOrthoSize;
            vcam.Follow = null;
        }
        else
        {
            vcam.m_Lens.OrthographicSize = initialOrthoSize;

            vcam.Follow = player.transform;
        }
    }
}