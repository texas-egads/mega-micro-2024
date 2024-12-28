using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Return0
{
    public class PandaClimber : MonoBehaviour
    {
        private int inputsPressed = 0;

        [SerializeField] private GameObject[] gripsPrefabs = new GameObject[4];
        [SerializeField] private GameObject[] grips = new GameObject[10];

        void Start()
        {
            RandomGripsGenerator();
        }

        void Update()
        {
            PlayerInputsRecording();
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

        public void PlayerInputsRecording()
        {
            Grip.Inputs currentInput;
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("inputs " + inputsPressed);
                currentInput = Grip.Inputs.W;
                Debug.Log(grips[inputsPressed].GetComponent<Grip>().inputValue);
                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                   transform.position = grips[inputsPressed].transform.position; //jump to next position
                }
                else
                {
                    Debug.Log("Game lost");
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                }
                inputsPressed++;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("inputs " + inputsPressed);
                currentInput = Grip.Inputs.A;
                Debug.Log(grips[inputsPressed].GetComponent<Grip>().inputValue);
                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                    transform.position = grips[inputsPressed].transform.position; //jump to next position
                }
                else
                {
                    Debug.Log("Game lost");
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                }
                inputsPressed++;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                currentInput = Grip.Inputs.S;
                Debug.Log(grips[inputsPressed].GetComponent<Grip>().inputValue);
                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                    transform.position = grips[inputsPressed].transform.position; //jump to next position
                }
                else
                {
                    Debug.Log("Game lost");
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                }
                inputsPressed++;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                currentInput = Grip.Inputs.D;
                Debug.Log(grips[inputsPressed].GetComponent<Grip>().inputValue);
                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                    transform.position = grips[inputsPressed].transform.position; //jump to next position
                }
                else
                {
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                }
                inputsPressed++;
            }
        }
    }
}
