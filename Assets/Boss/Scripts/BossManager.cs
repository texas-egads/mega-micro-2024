using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    // state managent 
    public enum State
    {
        CUTSCENE,
        PLAY
    }
    [HideInInspector] public State state;
    public float startTimer;
    public bool suddenDeath;

    public Animator cover;
    public GameObject KO;
    public GameObject[] winIndicators;
    public Player player;
    public Boss boss;
    public TextMeshProUGUI timerText;

    private float countdownTimer;
    private bool inCountdown;
    private float gameTimer;
    private int[] winCount;

    public static BossManager Instance;
    void Start()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        state = State.CUTSCENE;
    }

    void Update()
    {
        if(startTimer > 0)
        {
            startTimer -= Time.deltaTime;
            if(startTimer <= 0)
            {
                countdownTimer = 3;
                timerText.fontSize = 36;
                inCountdown = false;
            }
            return;
        }
        if(countdownTimer > 0)
        {
            if (!inCountdown)
            {
                inCountdown = true;
                BudioManager.Instance.PlayRoundStart(0);
            }
            timerText.text = Mathf.Ceil(countdownTimer).ToString("D1");
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0)
            {
                BudioManager.Instance.PlayRoundStart(1);
                timerText.fontSize = 28;
                timerText.text = "FIGHT";
                gameTimer = 61;
                state = State.PLAY;
            }
            return;
        }
        if(gameTimer > 0)
        {
            if(gameTimer < 60)
            {
                timerText.fontSize = 36;
                timerText.text = "0:" + countdownTimer.ToString("D2");
            }
            if(gameTimer <= 0)
            {
                timerText.text = "SD";
                suddenDeath = true;
                timerText.color = Color.red;
                //TODO enter sudden death
            }
        }
    }

    public IEnumerator ResetGame(int winner)
    {
        yield return new WaitForSeconds(2.6f);
        cover.Play("fastClos");
        yield return new WaitForSeconds(.8f);
        winCount[winner]++;
        if(winCount[winner] == 2)
        {
            if(winner == 0)
            {
                SceneManager.LoadScene("WinScreen");
            } else
            {
                SceneManager.LoadScene("LoseScreen");
            }
        }
        winIndicators[winner].SetActive(true);
        cover.Play("fastOpen");

        KO.SetActive(false);
        suddenDeath = false;
        timerText.color = Color.white;
        timerText.text = "";

        player.ResetState();
        boss.ResetState();
        state = State.CUTSCENE;
        startTimer = 2;
    }
    public void ShowKO()
    {
        KO.SetActive(true);
    }
}
