using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // TODO make a new end screen
    public string titleSceneName;
    public string gameSceneName;
    
    public void LoadTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }



}
