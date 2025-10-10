using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public static AudioSource main_music;
    public AudioSource music;
    public GameObject coverPrefab;
    // TODO create a new title screen
    //public string sceneName;

    bool isLoading = false;
    bool isMain = false;
    private void Start()
    {
        isMain = SceneManager.GetActiveScene().name == "StartScreen";
        if (isMain)
        {
            if (main_music)
            {
                music.Stop();
                music = main_music;
            } else
            {
                main_music = music;
                DontDestroyOnLoad(main_music.gameObject);
            }
        } else if (main_music)
        {
            //in credits
            music = main_music;
        }
    }
    void Update()
    {
        if (isLoading)
        {
            music.volume -= Time.deltaTime * 2;
        }
    }
    public void StartGame(string sceneName)
    {
        //only do once
        if (isLoading) return;
        isLoading = true;

        Instantiate(coverPrefab, transform).GetComponent<Animator>().Play("fastClos");
        DOVirtual.DelayedCall(.5f, () => {
            //kill music on non-credits
            if (isMain && sceneName != "TestCredits")
            {
                Destroy(main_music.gameObject);
            }

            SceneManager.LoadScene(sceneName);
        }, false);
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
