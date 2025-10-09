using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    
    // TODO create a new title screen
    //public string sceneName;

    
    public void StartGame(string sceneName)
    {
        //GameManager.instance.LoadScene("TestMain");
        SceneManager.LoadScene(sceneName);
    }
    public void SetDifficulty(int difficulty)
    {
        //GameManager.instance.LoadScene("TestMain");
        PlayerPrefs.GetInt("difficulty", difficulty);
    }


    public void LoadMenu(GameObject newg)
    {
        newg.SetActive(true);
    }
    public void DeloadMenu(GameObject oldg)
    {
        oldg.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Debug.Log("Game is over (once the game is built)");
        Application.Quit();
    }
    
}
