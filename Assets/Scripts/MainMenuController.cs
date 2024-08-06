using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    public string newGameSceneName;

    [Header("Options Panel")]
    public GameObject MainPanel;
    public GameObject Options;
    public GameObject LoadGamePanel;


    public void newGame()
    {
        if (!string.IsNullOrEmpty(newGameSceneName))
            SceneManager.LoadScene(newGameSceneName, LoadSceneMode.Single);

        else
            Debug.Log("Please write a scene name");
    }

    public void loadShow()
    {
        MainPanel.transform.localPosition = new Vector3(0, -300, 0);
        LoadGamePanel.transform.localPosition = new Vector3(0, 0, 0);

        //play click sfx
        playClickSound();


    }

    public void optionsShow()
    {
        MainPanel.transform.localPosition = new Vector3(0, -300, 0);
        Options.transform.localPosition = new Vector3(300, 0, 0);

        //play click sfx
        playClickSound();


    }

    public void backLoadGamePanel()
    {
        LoadGamePanel.transform.localPosition = new Vector3(589, 0, 0);
        MainPanel.transform.localPosition = new Vector3(0, -70, 0);

        //play click sfx
        playClickSound();
    }


    public void backOptionsPanel()
    {
        Options.transform.localPosition = new Vector3(-376, 0, 0);
        MainPanel.transform.localPosition = new Vector3(0, -70, 0);

        //play click sfx
        playClickSound();
    }

    public void Quit()
    {
        Application.Quit();
    }


    void playClickSound()
    {

    }



}
