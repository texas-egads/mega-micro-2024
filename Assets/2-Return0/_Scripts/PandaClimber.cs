using System.Collections;
using TMPro;
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

        [SerializeField] private Animator _animator;

        [SerializeField]private GameObject[] levelSprites = new GameObject[3];

        AudioSource jump;

        private int inputsPressed = 0;
        private int rInt;

        [SerializeField] private GameObject[] normalGripsPrefabs = new GameObject[4];
        [SerializeField] private GameObject[] goldGripsPrefabs = new GameObject[4];
        public static GameObject[] grips;


        void Start()
        {
            DifficultyChanger.GetGripsCount(); //easy = 8 inputs(grips), medium = 12, hard = 15
            grips = new GameObject[DifficultyChanger.count];
            DifficultyChanger.AssignGrips(); //now we have an "empty" array with a certain amt of elements. Waiting to be filled
            RandomGripsGenerator();
            WallChanger();

            UITextWin.text = "";

            AudioSource loop = Managers.AudioManager.CreateAudioSource();
            loop.loop = true;
            loop.clip = loopSound;
            loop.Play();

            jump = Managers.AudioManager.CreateAudioSource();
            jump.volume = 0.5f;

        }

        void Update()
        {

            if (inputsPressed == DifficultyChanger.count) 
            {//winning conditions met

                _animator.SetBool("GameWonAnimation", true);
                UITextWin.text = winText;

                AudioSource win = Managers.AudioManager.CreateAudioSource();
                win.PlayOneShot(winSound);
  
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(2f); //2 = seconds delay for animation
                inputsPressed++; //this is a lazy way to execute this if only once
            }
            else if (inputsPressed < DifficultyChanger.count)
            {
                if (!gameLost)
                {
                    UpdateCamera();
                    PlayerInputsReading();
                }
                if (gameLost)
                {
                    FallingMovement();
                }
            }
            else
            {
                this.enabled = false;
            }
        }

        public void WallChanger()
        {
            switch (DifficultyChanger.dif)
            {
                case "EASY":
                    levelSprites[0].SetActive(true);
                    levelSprites[1].SetActive(false);
                    levelSprites[2].SetActive(false);
                    break;
                case "MEDIUM":
                    levelSprites[0].SetActive(false);
                    levelSprites[1].SetActive(true);
                    levelSprites[2].SetActive(false);
                    break;
                case "HARD":
                    levelSprites[0].SetActive(false);
                    levelSprites[1].SetActive(false);
                    levelSprites[2].SetActive(true);
                    break;
            }
        }

        public void RandomGripsGenerator()
        {
            
            for (int i = 0; i < grips.Length; i++)
            {
                rInt = Random.Range(0, 4);
                grips[i] = Instantiate(normalGripsPrefabs[rInt], grips[i].transform.position, grips[i].transform.rotation);
            }
            NextInputGolden(); //visual cue to help the player detecting the next input
        }

        public void NextInputGolden()
        {
            if (inputsPressed == DifficultyChanger.count)
            {
                return;
            }
            else
            {
                Grip.Inputs nextInput = grips[inputsPressed].GetComponent<Grip>().inputValue;
                int n = (int)nextInput;
                grips[inputsPressed] = Instantiate(goldGripsPrefabs[n], grips[inputsPressed].transform.position, grips[inputsPressed].transform.rotation);
            }
           
        }

        public void UpdateCamera()
        {

            if (Camera.main.transform.position.y <= 0) //checking bottom bounds, resets y if below 0.
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 0, -9);
            }
            if (Camera.main.transform.position != transform.position) //if they don't match, move camera towards player
            {
                Vector2 offsetPos = new Vector2(transform.position.x, transform.position.y+3.03f);
                Vector2 newPos = Vector2.Lerp(Camera.main.transform.position, offsetPos, Time.deltaTime+0.005f); //lerp needs to be stored first, then used through Vector 3 because we are using a 3D camera.
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
            _animator.SetBool("hasJumped",true);
            Invoke("CancelJump", 0.1f);

            //sound
            jump.pitch += 0.2f;
            jump.PlayOneShot(jumpSound);
            
            transform.position = grips[inputsPressed].transform.position; //jump to next position
            inputsPressed++;
            NextInputGolden();
        }

        public void CancelJump()
        {
            _animator.SetBool("hasJumped", false);
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

        private void FallingMovement()
        {
            if (transform.position.x == 0f && transform.position.y == 0f)
            {
                return;
            }
            else
            {
                //player falling
                Vector3 fallinPos = new Vector3(transform.position.x, transform.position.y -2f, transform.position.z);
                Vector2 newPos = Vector2.Lerp(transform.position, fallinPos, Time.deltaTime+ 0.005f); //lerp needs to be stored first, then used through Vector 3 because we are using a 3D camera.
                transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            }
        }
        public void GameOver()
        {
            if (transform.position.y != 0f)
            {
                _animator.SetBool("GameFailedAnimation", true); 
            }

            StartCoroutine(ShakeCamera());
            gameLost = true;
            Managers.MinigamesManager.DeclareCurrentMinigameLost();
            Managers.MinigamesManager.EndCurrentMinigame(2f); //1 = seconds delay for animation
        }
    }
}
