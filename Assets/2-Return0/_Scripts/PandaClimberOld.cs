using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Return0
{
    public class PandaClimberOld : MonoBehaviour
    {
        public enum Inputs
        {
            W,
            A,
            S,
            D
        }

        private int inputsDone = 0;

        List<Inputs> randomList = new List<Inputs>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            RandomListGenerator();
        }

        // Update is called once per frame
        void Update()
        {
            PlayerInputsRecording();
        }

        public void RandomListGenerator()
        {
            for (int i = 0; i < 10; i++)
            {
                int tempRandom = Random.Range(0, 4);
                switch (tempRandom)
                {
                    case 0:
                        randomList.Add(Inputs.W);
                        break;
                    case 1:
                        randomList.Add(Inputs.A);
                        break;
                    case 2:
                        randomList.Add(Inputs.S);
                        break;
                    case 3:
                        randomList.Add(Inputs.D);
                        break;

                }
            } 
        }
        public void PlayerInputsRecording()
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (randomList[inputsDone] != Inputs.W)
                {
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                }
                inputsDone++;
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (randomList[inputsDone] != Inputs.A)
                {
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                }
                inputsDone++;
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (randomList[inputsDone] != Inputs.S)
                {
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                }
                inputsDone++;
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (randomList[inputsDone] != Inputs.D)
                {
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                }
                inputsDone++;
            }
        }
    }
}
