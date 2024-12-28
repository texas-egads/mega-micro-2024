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
                //int rInt = Random.Range(0, 4);
                int rInt = 0; //TEST
                grips[i] = Instantiate(gripsPrefabs[rInt], grips[i].transform.position, grips[i].transform.rotation);
            }
        }

        public void PlayerInputsRecording()
        {
            Grip.Inputs currentInput;
            if (Input.GetKey(KeyCode.W))
            {
                currentInput = Grip.Inputs.W;
                if (currentInput == grips[inputsPressed].GetComponent<Grip>().inputValue)
                {
                   transform.position = grips[inputsPressed].transform.position;
                }
                else
                {
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                }
            }
           
            inputsPressed++;
        }
    }
}
