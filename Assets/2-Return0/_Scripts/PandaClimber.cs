using System.Collections;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Return0
{
    public class PandaClimber : MonoBehaviour
    {
        public TextMeshProUGUI UITextWin;

        public string startText;
        public string winText;
        public bool gameLost = false;

        public AudioClip loopSound;
        public AudioClip winSound;
        public AudioClip jumpSound;
        public AudioClip jumpSound2;

        AudioSource jump;

        private int inputsPressed = 0;
        private int rInt;

        [SerializeField] private GameObject[] normalGripsPrefabs = new GameObject[4];
        [SerializeField] private GameObject[] goldGripsPrefabs = new GameObject[4];
        public static GameObject[] grips;


        void Start()
        {
            DifficultyChanger.GetGripsCount();
            grips = new GameObject[DifficultyChanger.count];
            DifficultyChanger.AssignGrips();

            UITextWin.text = "";

            AudioSource loop = Managers.AudioManager.CreateAudioSource();
            loop.loop = true;
            loop.clip = loopSound;
            loop.Play();

            jump = Managers.AudioManager.CreateAudioSource();
            jump.volume = 0.5f;

            RandomGripsGenerator();

        }

        void Update()
        {

            if (inputsPressed == DifficultyChanger.count) 
            {
                Debug.Log("game won");
                UITextWin.text = winText;

                AudioSource win = Managers.AudioManager.CreateAudioSource();
                win.PlayOneShot(winSound);
  
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(2); //2 = seconds delay for animation
                inputsPressed++;
            }
            else if (inputsPressed < DifficultyChanger.count)
            {
                if (!gameLost)
                {
                    UpdateCamera();
                    PlayerInputsReading();
                }
            }
            else
            {
                this.enabled = false;
            }
        }

        public void RandomGripsGenerator()
        {
            
            for (int i = 0; i < grips.Length; i++)
            {
                rInt = Random.Range(0, 4);
                grips[i] = Instantiate(normalGripsPrefabs[rInt], grips[i].transform.position, grips[i].transform.rotation);
                Debug.Log("<color=yellow>grips: </color>" + grips[i]);
            }
            NextInputGolden();
        }

        public void NextInputGolden()
        {
            Grip.Inputs nextInput = grips[inputsPressed].GetComponent<Grip>().inputValue;
            Debug.Log("<color=red>next Input: </color>" + nextInput);
            int n = (int)nextInput;
            grips[inputsPressed] = Instantiate(goldGripsPrefabs[n], grips[inputsPressed].transform.position, grips[inputsPressed].transform.rotation);
        }

        public void UpdateCamera()
        {

            if (Camera.main.transform.position.y <= 0) //checking bottom bounds, resets y if below 0.
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 0, -9);
            }
            if (Camera.main.transform.position != transform.position) //if they don't match, move camera towards player
            {
                Vector2 newPos = Vector2.Lerp(Camera.main.transform.position, transform.position, Time.deltaTime); //lerp needs to be stored first, then used through Vector 3 because we are using a 3D camera.
                Camera.main.transform.position = new Vector3(newPos.x,newPos.y,-9);
            }
            
        }

        public void PlayerInputsReading()
        {
            Grip.Inputs currentInput;

            if (Input.GetKeyDown(KeyCode.W))
            {
                currentInput = Grip.Inputs.W;

                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                    JumptToNextPosition();
                }
                else
                {
                    GameOver();
                }

            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentInput = Grip.Inputs.A;

                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                    JumptToNextPosition();
                }
                else
                {
                    GameOver();
                }

            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                currentInput = Grip.Inputs.S;
         
                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                    JumptToNextPosition();
                }
                else
                {
                    GameOver();
                }

            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                currentInput = Grip.Inputs.D;

                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                    JumptToNextPosition();
                }
                else
                {
                    GameOver();
                }
            }

        }


        private void JumptToNextPosition()
        {
            jump.pitch += 0.2f;
            jump.PlayOneShot(jumpSound);
            
            transform.position = grips[inputsPressed].transform.position; //jump to next position
            inputsPressed++;
            NextInputGolden();
        }
        private IEnumerator ShakeCamera()
        {
            Vector3 originalPos = Camera.main.transform.localPosition;
            for (int i = 0; i < 10; i++)
            {
                Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * 0.5f;
                yield return new WaitForSeconds(0.025f);
            }
            Camera.main.transform.localPosition = originalPos;
        }

        public void GameOver()
        {
            Debug.Log("Game lost");
            StartCoroutine(ShakeCamera());
            gameLost = true;
            Managers.MinigamesManager.DeclareCurrentMinigameLost();
            Managers.MinigamesManager.EndCurrentMinigame(2); //2 = seconds delay for animation
        }
    }
}
