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
    public GameObject creditsScreen;
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

    private int currentProgress;

    private void Start()
    {
        GameControllerScript.instance.transform.GetChild(1).GetComponent<AudioSource>().Stop();
        currentProgress = PlayerPrefs.GetInt("Game Progress");

        titleScreen.SetActive(true);
        levelSelect.SetActive(false);
        creditsScreen.SetActive(false);
        tutorialScreen.SetActive(false);
        levelOneCanvas.SetActive(false);
        levelTwoCanvas.SetActive(false);
        levelThreeCanvas.SetActive(false);
        LevelFourCanvas.SetActive(false);

        UpdateProgress();
    }

    private void Update()
    {
        if (Input.GetKeyUp("escape"))
        {
            EscManagement();
        }
        
        //Cheat codes.
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ++currentProgress;
            PlayerPrefs.SetInt("Game Progress", currentProgress);
            GameControllerScript.instance.transform.GetChild(0).GetComponent<AudioController>().PlayClip(AudioController.GameManagerSFX.finishLevel);
            UpdateProgress();
        }
    }

    //Updates menu items according to player progression
    private void UpdateProgress()
    {
        Image sr2 = levelTwoPicture.GetComponent<Image>();
        Image sr3 = levelThreePicture.GetComponent<Image>();
        Image sr4 = levelFourPicture.GetComponent<Image>();

        print(currentProgress);
        if (currentProgress > 2)
        {
            LevelFourButton.interactable = true;
            sr4.color = Color.white;
            LevelThreeButton.interactable = true;
            sr3.color = Color.white;
            LevelTwoButton.interactable = true;
            sr2.color = Color.white;
        }
        else if (currentProgress > 1)
        {
            LevelThreeButton.interactable = true;
            sr3.color = Color.white;
            LevelTwoButton.interactable = true;
            sr2.color = Color.white;
        }
        else if (currentProgress > 0)
        {
            LevelTwoButton.interactable = true;
            sr2.color = Color.white;
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
        else if (creditsScreen.activeInHierarchy)
        {
            titleScreen.SetActive(true);
            creditsScreen.SetActive(false);
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
            ExitGame();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}