/*****************************************************************************
// File Name : BackgroundParallaxBehaviour
// Author : Connor Dunn
// Creation Date : April 28th, 2020
//
// Brief Description : Makes background objects have parallax movement.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallaxBehaviour : MonoBehaviour
{
    public float VertiSpeedDampening = 3;
    public float HorizSpeedDampening = 3;

    public GameObject player;
    public GameObject tile;

    private Transform ct;

    //Vector2 Mov;

    private float playerXMov;
    private float playerYMov;
    private float bgXMov;
    private float bgYMov;
    
    private Transform tr;
    

    // Start is called before the first frame update
    void Start()
    {
        ct = player.GetComponent<Transform>();
        tr = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        playerXMov = ct.transform.position.x;
        playerYMov = ct.transform.position.y;

        /*if (Input.GetKeyDown(KeyCode.J))
        {
            print(playerXMov + ", " + playerYMov);
        }*/
        
        //playerYMov = rb.velocity.y;
        //playerXMov = rb.velocity.x;
        MoveParallax();
        //TileImages();
    }

    void MoveParallax()
    {
        //Mov.x = playerXMov;
        //Mov.y = playerYMov;
        //gameObject.transform.position = Mov;
        //print(Mov.x + ", " + Mov.y);
        bgXMov = playerXMov / HorizSpeedDampening;
        bgYMov = playerYMov / VertiSpeedDampening;
        tr.position = new Vector2(bgXMov, bgYMov);
        transform.position = tr.position;

    }


    //public float positiveXBounds = 40f;
    //public float negativeXBouunds = 40f;
    //public float positiveYBounds = 1f;
    //public float negativeYBounds = 1f;
    //void TileImages()
    //{
    //    Vector3 pos = transform.position;

    //    if (tr.position.y >= (ct.transform.position.y + positiveYBounds))
    //    {
    //        pos.y = (ct.transform.position.y - negativeYBounds);
    //    }

    //    else if (tr.position.y <= (ct.transform.position.y - negativeYBounds))
    //    {
    //        pos.y = (ct.transform.position.y + positiveYBounds);
    //    }

    //    else if (tr.position.x <= (ct.transform.position.x - negativeXBouunds))
    //    {
    //        pos.x = (ct.transform.position.x + positiveXBounds);
    //    }

    //    else if (tr.position.x >= (ct.transform.position.x + positiveXBounds))
    //    {
    //        pos.x = (ct.transform.position.x - negativeXBouunds);
    //    }

    //    else
    //    {
    //        pos = tr.position;
    //    }
    //    tr.position = pos;

    //}

}
