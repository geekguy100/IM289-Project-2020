/*****************************************************************************
// File Name : MenuScreenBehaviour
// Author : Kyle Grenier
// Creation Date : May 03, 2020
//
// Brief Description : Behaviour for backing out of screens (ESC button).
*****************************************************************************/

using UnityEngine;

public class MenuScreenBehaviour : MonoBehaviour
{
    public GameObject previousScreen;

    //Pressing SPACE or RETURN loads a level if the cursor is on the appropriate screen.
    [SerializeField]
    private bool keyboardLoadsLevel = false;
    [SerializeField]
    private bool returnGoesBack = true;
    [SerializeField]
    private string levelToLoad = "";


    private void Update()
    {
        if (Input.GetKeyUp("escape"))
        {
            previousScreen.SetActive(true);
            gameObject.SetActive(false);
        }

        if ((Input.GetKeyUp("space") || Input.GetKeyUp("return")))
        {
            if (keyboardLoadsLevel)
            {
                if (levelToLoad == "")
                {
                    Debug.LogWarning("There is no inputted level to load!: " + gameObject.name);
                    return;
                }

                MenuBehavior.LoadLevelKeyboard(levelToLoad);
            }
            else if (returnGoesBack)
            {
                previousScreen.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
