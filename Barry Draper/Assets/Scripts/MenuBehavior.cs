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
        SpriteRenderer sr2 = levelTwoPicture.GetComponent<SpriteRenderer>();
        SpriteRenderer sr3 = levelThreePicture.GetComponent<SpriteRenderer>();
        SpriteRenderer sr4 = levelFourPicture.GetComponent<SpriteRenderer>();

        titleScreen.SetActive(true);
        levelSelect.SetActive(false);
        levelOneCanvas.SetActive(false);
        levelTwoCanvas.SetActive(false);
        levelThreeCanvas.SetActive(false);
        LevelFourCanvas.SetActive(false);

        if (ProgressCheck.progress < 0.5f)
        {
            Object.Destroy(LevelTwoButton);
            sr2.color = Color.grey;
            Object.Destroy(LevelThreeButton);
            sr3.color = Color.grey;
            Object.Destroy(LevelFourButton);
            sr4.color = Color.grey;
        }
        else if (ProgressCheck.progress < 9.5f)
        {
            Object.Destroy(LevelThreeButton);
            sr3.color = Color.grey;
            Object.Destroy(LevelFourButton);
            sr4.color = Color.grey;
        }
        else if (ProgressCheck.progress < 99.5f)
        {
            Object.Destroy(LevelFourButton);
            sr4.color = Color.grey;
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
}
