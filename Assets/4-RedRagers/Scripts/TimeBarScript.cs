using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace RedRagers
{
    

    public class TimeBarScript : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //makes timer start automatically
            timerIsRunning = true;
            
            timeRemaining = startTime;
        }
        public float startTime = 6.0f;
        public float timeRemaining;
        public bool timerIsRunning = false;
        

        
        // Update is called once per frame
        //final timer action should only happen once
        void Update()
        {
            if (timerIsRunning)
            {
                if (timeRemaining > 0f)
                {
                    timeRemaining -= Time.deltaTime;
                    //bar depletes to zero with time
                    transform.localScale -= new Vector3(Time.deltaTime / startTime, 0, 0);
                    Managers.MinigamesManager.DeclareCurrentMinigameWon();
                }
                else
                {
                    Debug.Log("You Win!");
                    timeRemaining = 0f;
                    timerIsRunning = false;
                    transform.localScale = new Vector3(timeRemaining, 0, 0);
                    Managers.MinigamesManager.EndCurrentMinigame(1f);
                }
            }
        }
        
    }
}
