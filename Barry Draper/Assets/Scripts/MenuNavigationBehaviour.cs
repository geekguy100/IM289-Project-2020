/*****************************************************************************
// File Name : MenuNavigationBehaviour
// Author : Kyle Grenier
// Creation Date : May 03, 2020
//
// Brief Description : New script for navigating the menu with arrow keys and WASD.
*****************************************************************************/

using UnityEngine;

public class MenuNavigationBehaviour : MonoBehaviour
{
    //Cursors for the two different screens.
    public CursorBehaviour titleScreenCursor;
    public CursorBehaviour levelSelectionCursor;
    private int titleScreenIndex = 1;
    private int levelSelectionIndex = 1;

    public GameObject titleScreen;
    public GameObject levelSelectionScreen;
    public GameObject creditsScreen;
    public GameObject htpScreen;

    public GameObject lvlOneScreen;
    public GameObject lvlTwoScreen;
    public GameObject lvlThreeScreen;
    public GameObject lvlFourScreen;

    private int currentProgress;


    private void Update()
    {
        //If the title screen is active, move using the up/down and WS keys.
        if (titleScreen.activeInHierarchy)
        {
            //Activate the title screen cursor and deactivate the level select cursor -- ran when the title screen loads.
            if (!titleScreenCursor.IsActive())
                titleScreenCursor.SetActive();
            if (levelSelectionCursor.IsActive())
                levelSelectionCursor.Deactivate();

            //Move to the next or previous cursor.
            if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
            {
                titleScreenCursor = titleScreenCursor.ActivateNext();
                titleScreenIndex = titleScreenCursor.GetIndex();
            }
            else if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
            {
                titleScreenCursor = titleScreenCursor.ActivatePrevious();
                titleScreenIndex = titleScreenCursor.GetIndex();
            }


            //Activating menus
        }
        else if (levelSelectionScreen.activeInHierarchy)
        {
            //Activate the level selection screen cursor and deactivate the level select cursor -- ran when the level selection screen loads.
            if (!levelSelectionCursor.IsActive())
                levelSelectionCursor.SetActive();
            if (titleScreenCursor.IsActive())
                titleScreenCursor.Deactivate();

            //Move to the next or previous cursor.
            if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
            {
                levelSelectionCursor = levelSelectionCursor.ActivateNext();
                levelSelectionIndex = levelSelectionCursor.GetIndex();
            }
            else if (Input.GetKeyDown("left") || Input.GetKeyDown("a"))
            {
                levelSelectionCursor = levelSelectionCursor.ActivatePrevious();
                levelSelectionIndex = levelSelectionCursor.GetIndex();
            }
        }

        HandleSelection();
    }

    //Handling menu item selection.
    private void HandleSelection()
    {
        if (Input.GetKeyUp("space") || Input.GetKeyUp("return"))
        {
            currentProgress = PlayerPrefs.GetInt("Game Progress");

            if (titleScreen.activeInHierarchy)
            {
                switch (titleScreenIndex)
                {
                    //Level selection.
                    case 1:
                        levelSelectionScreen.SetActive(true);
                        titleScreen.SetActive(false);
                        break;
                    //How to play.
                    case 2:
                        htpScreen.SetActive(true);
                        titleScreen.SetActive(false);
                        break;
                    //Credits
                    case 3:
                        creditsScreen.SetActive(true);
                        titleScreen.SetActive(false);
                        break;
                    //Exit game
                    case 4:
                        Application.Quit();
                        break;
                }
            }
            else if (levelSelectionScreen.activeInHierarchy)
            {
                switch (levelSelectionIndex)
                {
                    //Level One.
                    case 1:
                        lvlOneScreen.SetActive(true);
                        levelSelectionScreen.SetActive(false);
                        break;
                    //Level Two.
                    case 2:
                        if(currentProgress > 0)
                        {
                            lvlTwoScreen.SetActive(true);
                            levelSelectionScreen.SetActive(false);
                        }
                        break;
                    //Level Three.
                    case 3:
                        if(currentProgress > 1)
                        {
                            lvlThreeScreen.SetActive(true);
                            levelSelectionScreen.SetActive(false);
                        }
                        break;
                    //Level Four.
                    case 4:
                        if(currentProgress > 2)
                        {
                            lvlFourScreen.SetActive(true);
                            levelSelectionScreen.SetActive(false);
                        }
                        break;

                }
            }
        }
    }
}