/*****************************************************************************
// File Name : MenuNavigator.cs
// Author : Robert Lee
// Creation Date : April 19th, 2020
//
// Brief Description :  Allows you to navigate the main menu using the arrow
                        keys or WASD.
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    public int index = 0;
    public int totalOptions = 4;
    public float yOffset = 1f;
    public float xOffset = 1f;
    public GameObject updownmenu;
    public GameObject leftrightmenu;
    public GameObject creditsScreen;
    public GameObject tutorialScreen;
    public GameObject levelOneScreen;
    public GameObject levelTwoScreen;
    public GameObject levelThreeScreen;
    public GameObject levelFourScreen;

    private RectTransform rectTransform;
    private CanvasScaler canvasScalar;
    private Vector2 screenScale;

    private int currentProgress;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        //Setting the screenScale vector. Will be used to scale x- and y-offset according to the resolution of the game.
        if (canvasScalar == null)
            canvasScalar = GetComponentInParent<CanvasScaler>();
        if (canvasScalar)
            screenScale = new Vector2(canvasScalar.referenceResolution.x / Screen.width, canvasScalar.referenceResolution.y / Screen.height);
        else
            screenScale = Vector2.one;

        print(screenScale);
    }

    private void Start()
    {
        currentProgress = PlayerPrefs.GetInt("Game Progress");
    }

    // Update is called once per frame
    void Update()
    {
        //Space and enter mimic left clicking.
        if ((Input.GetKeyDown("space")) || (Input.GetKeyDown("return")))
        {
            currentProgress = PlayerPrefs.GetInt("Game Progress");
            Select();
        }
        //Navigate up the appropriate menu.
        else if ((Input.GetKeyDown("up") || Input.GetKeyDown("w")) && updownmenu.activeInHierarchy)
        {
            if (index > 0)
            {
                index--;
                Vector2 position = rectTransform.position;
                position.y += yOffset;
                rectTransform.position = position;
            }
        }
        //Navigate down the approriate menu.
        else if ((Input.GetKeyDown("down") || Input.GetKeyDown("s")) && updownmenu.activeInHierarchy)
        {
            if (index <= (totalOptions - 1))
            {
                index++;
                Vector2 position = rectTransform.position;
                position.y -= yOffset;
                rectTransform.position = position;
            }
        }
        //Navigate left on the appropriate menu.
        else if ((Input.GetKeyDown("left") || Input.GetKeyDown("a")) && leftrightmenu.activeInHierarchy)
        {
            if (index > 0)
            {
                index--;
                Vector2 position = rectTransform.position;
                position.x -= xOffset;
                rectTransform.position = position;
            }
        }
        //Navigate right on the appropriate menu.
        else if ((Input.GetKeyDown("right") || Input.GetKeyDown("d")) && leftrightmenu.activeInHierarchy)
        {
            if (index <= (totalOptions - 1))
            {
                index++;
                Vector2 position = rectTransform.position;
                position.x += xOffset;
                rectTransform.position = position;
            }
        }
    }


    void Select()
    {
        if (updownmenu.activeInHierarchy)
        {
            if (index == 0)
            {
                updownmenu.SetActive(false);
                leftrightmenu.SetActive(true);
            }
            else if (index == 1)
            {
                updownmenu.SetActive(false);
                tutorialScreen.SetActive(true);
            }
            else if (index == 2)
            {
                updownmenu.SetActive(false);
                creditsScreen.SetActive(true);
            }
            else if (index == 3)
            {
                Application.Quit();
            }
        }
        else if (leftrightmenu.activeInHierarchy)
        {
            if (index == 0)
            {
                leftrightmenu.SetActive(false);
                levelOneScreen.SetActive(true);
            }
            else if (index == 1 && currentProgress > 0)
            {
                leftrightmenu.SetActive(false);
                levelTwoScreen.SetActive(true);
            }
            else if (index == 2 && currentProgress > 1)
            {
                leftrightmenu.SetActive(false);
                levelThreeScreen.SetActive(true);
            }
            else if (index == 3 && currentProgress > 2)
            {
                leftrightmenu.SetActive(false);
                levelFourScreen.SetActive(true);
            }
        }
        else if (creditsScreen.activeInHierarchy)
        {
            creditsScreen.SetActive(false);
            updownmenu.SetActive(true);
        }
        else if (tutorialScreen.activeInHierarchy)
        {
            tutorialScreen.SetActive(false);
            updownmenu.SetActive(true);
        }
    }
}