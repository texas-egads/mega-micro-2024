using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Return0
{
    public class PandaClimber : MonoBehaviour
    {
        public TextMeshProUGUI UITextInstructions;
        public TextMeshProUGUI UITextWin;

        public string startText;
        public string winText;
        public bool gameLost = false;

        public AudioClip loopSound;
        public AudioClip winSound;


        private int inputsPressed = 0;
        [SerializeField] private GameObject[] gripsPrefabs = new GameObject[4];
        [SerializeField] private GameObject[] grips = new GameObject[10];

        [SerializeField] private Transform cameraPos;
        [SerializeField] private Transform originalPos;

        void Start()
        {
            originalPos = cameraPos;

            UITextInstructions.text = startText;
            UITextWin.text = "";

            AudioSource loop = Managers.AudioManager.CreateAudioSource();
            loop.loop = true;
            loop.clip = loopSound;
            loop.Play();

            RandomGripsGenerator();
        }

        void Update()
        {
            if (inputsPressed == 10) 
            {
                Debug.Log("game won");
                UITextWin.text = winText;

                AudioSource win = Managers.AudioManager.CreateAudioSource();
                win.PlayOneShot(winSound);
  
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(2); //2 = seconds delay for animation
                inputsPressed++;
            }
            else if (inputsPressed < 10)
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
                Debug.Log("Random Grips Generated");
                int rInt = Random.Range(0, 4);
                grips[i] = Instantiate(gripsPrefabs[rInt], grips[i].transform.position, grips[i].transform.rotation);
            }
        }

        public void UpdateCamera()
        {
            if (Camera.main.transform.position.y <= 0) //checking bottom bounds, resets y if below 0.
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 0, -9);
            }
            else if (Camera.main.transform.position != transform.position) //if they don't match, move camera towards player
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
                   transform.position = grips[inputsPressed].transform.position; //jump to next position
                }
                else
                {
                    GameOver();
                }
                inputsPressed++;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentInput = Grip.Inputs.A;

                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                    transform.position = grips[inputsPressed].transform.position; //jump to next position
                }
                else
                {
                    GameOver();
                }
                inputsPressed++;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                currentInput = Grip.Inputs.S;
         
                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                    transform.position = grips[inputsPressed].transform.position; //jump to next position
                }
                else
                {
                    GameOver();
                }
                inputsPressed++;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                currentInput = Grip.Inputs.D;

                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                    transform.position = grips[inputsPressed].transform.position; //jump to next position
                }
                else
                {
                    GameOver();
                }
                inputsPressed++;
            }
        }
        public void GameOver()
        {
            Debug.Log("Game lost");
            //cameraPos.localPosition = originalPos, Random.insideUnitSphere * 2f;
            gameLost = true;
            Managers.MinigamesManager.DeclareCurrentMinigameLost();
            Managers.MinigamesManager.EndCurrentMinigame(2); //2 = seconds delay for animation
        }
    }
}
