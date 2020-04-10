/*****************************************************************************
// File Name : MenuBehavior.cs
// Author : Robert Lee
// Creation Date : March 2, 2020
//
// Brief Description :  Controls how the player can navigate the menu, as well
                        as what each option on the menu can do.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject levelSelect;
    public GameObject tutorialScreen;
    public GameObject levelOneCanvas;
    public GameObject levelTwoCanvas;
    public GameObject levelTwoPicture;
    public GameObject levelThreeCanvas;
    public GameObject levelThreePicture;
    public GameObject LevelFourCanvas;
    public GameObject levelFourPicture;
    public Button LevelTwoButton;
    public Button LevelThreeButton;
    public Button LevelFourButton;

    private void Start()
    {
        Image sr2 = levelTwoPicture.GetComponent<Image>();
        Image sr3 = levelThreePicture.GetComponent<Image>();
        Image sr4 = levelFourPicture.GetComponent<Image>();

        titleScreen.SetActive(true);
        levelSelect.SetActive(false);
        tutorialScreen.SetActive(false);
        levelOneCanvas.SetActive(false);
        levelTwoCanvas.SetActive(false);
        levelThreeCanvas.SetActive(false);
        LevelFourCanvas.SetActive(false);

        if (ProgressCheck.progress < 1f)
        {
            Destroy(LevelTwoButton);
            sr2.color = Color.grey;
            Destroy(LevelThreeButton);
            sr3.color = Color.grey;
            Destroy(LevelFourButton);
            sr4.color = Color.grey;
        }
        else if (ProgressCheck.progress < 2f)
        {
            Destroy(LevelThreeButton);
            sr3.color = Color.grey;
            Destroy(LevelFourButton);
            sr4.color = Color.grey;
        }
        else if (ProgressCheck.progress < 3f)
        {
            Destroy(LevelFourButton);
            sr4.color = Color.grey;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp("escape"))
        {
            EscManagement();
        }
    }

    /// <summary>
    /// Loads into the level of your choosing
    /// </summary>
    /// <param name="levelname">Name of level being loaded into</param>
    public void LoadLevel(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }


    /// <summary>
    /// Decides on what to do once the player hits Escape on the main menu
    /// </summary>
    private void EscManagement()
    {
        if (levelSelect.activeInHierarchy)
        {
            titleScreen.SetActive(true);
            levelSelect.SetActive(false);
        }
        else if (tutorialScreen.activeInHierarchy)
        {
            titleScreen.SetActive(true);
            tutorialScreen.SetActive(false);
        }
        else if (levelOneCanvas.activeInHierarchy ||
                 levelTwoCanvas.activeInHierarchy ||
                 levelThreeCanvas.activeInHierarchy ||
                 LevelFourCanvas.activeInHierarchy)
        {
            levelOneCanvas.SetActive(false);
            levelTwoCanvas.SetActive(false);
            levelThreeCanvas.SetActive(false);
            LevelFourCanvas.SetActive(false);
            levelSelect.SetActive(true);
        }
        else if (titleScreen.activeInHierarchy)
        {
            Application.Quit();
        }
    }
}
