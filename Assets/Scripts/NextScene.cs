using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NextScene : MonoBehaviour
{

    public string newGameSceneName;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(newGameSceneName);
        //Debug.Log("wwwwwwwwww");
    }


}
