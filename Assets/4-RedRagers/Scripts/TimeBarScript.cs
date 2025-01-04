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
        public float startTime = 6.0f;
        public float timeRemaining;
        public bool timerIsRunning = false;
        public bool won = false;
        private bool animationCheck = true;
        public Player player;
        public Animator splashAnimator;
        public Animator splashAnimator2;
        [HideInInspector] public AudioSource music;
        [HideInInspector] public AudioSource loseSound;
        public AudioClip musicClip;
        public AudioClip loseSoundClip;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //makes timer start automatically
            timerIsRunning = true;

            timeRemaining = startTime;

            music = Managers.AudioManager.CreateAudioSource();
            music.clip = musicClip;
            music.Play();
            loseSound = Managers.AudioManager.CreateAudioSource();
            loseSound.clip = loseSoundClip;
        }

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
                }
                else
                {
                    Managers.MinigamesManager.DeclareCurrentMinigameWon();
                    won = true;
                    
                    timerIsRunning = false;
                    transform.localScale = new Vector3(timeRemaining, 0, 0);
                    Managers.MinigamesManager.EndCurrentMinigame(1f);
                }
                if (player.value == 0.0 || player.value == 1.0f) {
                    music.Stop();
                    loseSound.Play();
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                    timeRemaining = 0f;
                    timerIsRunning = false;
                    transform.localScale = new Vector3(timeRemaining, 0, 0);
                    Managers.MinigamesManager.EndCurrentMinigame(1f);
                    if (player.value == 0.0) {
                        if (animationCheck)
                        {
                            splashAnimator.SetBool("splash", true);
                            animationCheck = false;
                        }
                        else
                        {
                            splashAnimator.SetBool("splash", false);
                        }
                    }
                    else {
                        if (animationCheck)
                        {
                            splashAnimator2.SetBool("splash", true);
                            animationCheck = false;
                        }
                        else
                        {
                            splashAnimator2.SetBool("splash", false);
                        }
                    }
                }
            }
            

        }
        
    }
}
