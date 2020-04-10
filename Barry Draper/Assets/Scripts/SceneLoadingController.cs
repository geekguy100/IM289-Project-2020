/*****************************************************************************
// File Name : SceneLoadingController
// Author : Kyle Grenier
// Creation Date : April 08, 2020
//
// Brief Description : Controls preparing the level each time a scene related to a playable level has been loaded.
*****************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingController : MonoBehaviour
{
    //Preparing the level once the scene has been loaded.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //Update player prefs
        PlayerPrefs.SetFloat("Game Progress", ProgressCheck.progress);
        print("Ur progress: " + ProgressCheck.progress);

        if (GameControllerScript.instance != null)
            GameControllerScript.instance.transform.GetChild(1).GetComponent<AudioController>().StopBackgroundMusic();
        if (scene.name != "Main Menu")
        {
            //print("Scene Preparer: Calling PrepareLevel()...");
            GameControllerScript.instance.PrepareLevel();
        }
    }
}