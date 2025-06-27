using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // TODO make a new end screen
    public string sceneName;
    public string sceneNameTwo;
    
    public void LoadTitle()
    {
        //GameManager.Instance.LoadScene("TitleScreen");
        SceneManager.LoadScene(sceneName);
    }
    
    
    
}
