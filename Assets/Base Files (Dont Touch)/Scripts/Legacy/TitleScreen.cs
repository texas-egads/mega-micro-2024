using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    
    // TODO create a new title screen
    public string sceneName;

    
    public void StartGame()
    {
        //GameManager.instance.LoadScene("TestMain");
        SceneManager.LoadScene(sceneName);
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
