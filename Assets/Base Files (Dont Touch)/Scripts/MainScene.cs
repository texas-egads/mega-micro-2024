﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainScene : MonoBehaviour
{
    public GameObject container;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI promptText;
    public InstructionText instructionText;
    public Image background;
    public Animator[] lifeAnims;
    public Animator pandaAnim;
    public Animator playerAnim;
    public Animator gameStartAnim;

    private Color normalBG;
    [SerializeField] private Color loseBG;
    [SerializeField] private Color winBG;

    private bool oldSpacePressed;
    private Action spacePressedAction;

    private String baseStatusText;
    private float lastPressTime = 0;

    private int prevLives = 3;

    private void Awake() {
        normalBG = background.color;
    }

    private void Start() {
        Managers.__instance.minigamesManager.OnStartMinigame += OnStartMinigame;
        Managers.__instance.minigamesManager.OnEndMinigame += OnEndMinigame;
        Managers.__instance.minigamesManager.OnBeginIntermission += OnBeginIntermission;

        Managers.__instance.minigamesManager.StartMinigames();
    }

    private void OnDestroy() {
        Managers.__instance.minigamesManager.OnStartMinigame -= OnStartMinigame;
        Managers.__instance.minigamesManager.OnEndMinigame -= OnEndMinigame;
        Managers.__instance.minigamesManager.OnBeginIntermission -= OnBeginIntermission;
    }


    private void Update() {
        // call the space pressed action whenever space is pressed
        bool spacePressed = Input.GetAxis("Space") > 0;
        if (spacePressed && !oldSpacePressed) {
            spacePressedAction?.Invoke();
            spacePressedAction = null;
        }

        float axis = Input.GetAxis("Horizontal");

        if (axis < 0 && Managers.__instance.minigamesManager.minigameDifficulty != IMinigamesManager.Difficulty.EASY && Time.time - lastPressTime > 0.5f)
        {
            Managers.__instance.minigamesManager.minigameDifficulty--;
            lastPressTime = Time.time;
        }
        else if (axis > 0 && Managers.__instance.minigamesManager.minigameDifficulty != IMinigamesManager.Difficulty.HARD && Time.time - lastPressTime > 0.5f)
        {
            Managers.__instance.minigamesManager.minigameDifficulty++;
            lastPressTime = Time.time;
        }

        SetStatusText();

        oldSpacePressed = spacePressed;
    }


    private void OnStartMinigame(MinigameDefinition _) {
        container.SetActive(false);
    }

    private void OnEndMinigame() {
        container.SetActive(true);

        // reset the prompt text
        promptText.text = "";
    }

    private void SetStatusText()
    {
        String difficultyString = "";

        switch(Managers.__instance.minigamesManager.minigameDifficulty)
        {
            case IMinigamesManager.Difficulty.EASY:
                difficultyString = "easy";
                break;
            case IMinigamesManager.Difficulty.MEDIUM:
                difficultyString = "medium";
                break;
            case IMinigamesManager.Difficulty.HARD:
                difficultyString = "hard";
                break;
        }

        String statusTextString =
            baseStatusText + $"\nCurrent Difficulty: {difficultyString} (use arrow keys or a/d to adjust)";

        statusText.text = statusTextString;
    }

    private void OnBeginIntermission(MinigameStatus status, Action intermissionFinishedCallback) {
        // write all of the status to the screen
        baseStatusText =
            $"Result of previous minigame: {(status.previousMinigameResult == WinLose.WIN ? "Won" : status.previousMinigameResult == WinLose.LOSE ? "Lost" : "N/A")}\n" +
            $"Rounds completed: {status.nextRoundNumber} out of {status.totalRounds}\n" +
            $"Lives: {status.currentHealth}\n" +
            $"Overall game status: {(status.gameResult == WinLose.WIN ? "Won" : status.gameResult == WinLose.LOSE ? "Lost" : "Playing")}";

        SetStatusText();

        // update old lives
        for (int i = prevLives; i < 3; i++)
        {
            lifeAnims[i].Play("lifeFullAppear");
        }
        // update new change in lives
        if (prevLives != status.currentHealth)
        {
            prevLives = status.currentHealth;
            lifeAnims[status.currentHealth].Play("lifeAppear");
        }

        // play character anims
        playerAnim.Play(status.previousMinigameResult == WinLose.WIN ? "playerWin" : status.previousMinigameResult == WinLose.LOSE ? "playerLose" : "N/A");
        pandaAnim.Play(status.previousMinigameResult == WinLose.WIN ? "pandaSad" : status.previousMinigameResult == WinLose.LOSE ? "pandaLaugh" : "N/A");

        // flash a color if the game was won/lost
        /*if (status.previousMinigameResult == WinLose.WIN) {
            background.color = winBG;
        }
        if (status.previousMinigameResult == WinLose.LOSE) {
            background.color = loseBG;
        }*/

        if (status.nextMinigame != null) {
            // prepare for the next minigame
            DOVirtual.DelayedCall(1f, () => {
                // return the background color to what it was before
                background.color = normalBG;

                // await input
                promptText.text = "Press SPACE to start next minigame";
                spacePressedAction = () => OnProceed(status, intermissionFinishedCallback);
            }, false);
        }
    }

    private void OnProceed(MinigameStatus status, Action intermissionFinishedCallback) {
        // start the sequence for the next minigame
        gameStartAnim.Play("gameStart");
        instructionText.ShowImpactText(status.nextMinigame.instruction);
        DOVirtual.DelayedCall(1f, () => intermissionFinishedCallback?.Invoke(), false);
    }


    /*
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        MainGameManager.Instance.GrowMainScene += GrowScene;
        MainGameManager.Instance.ShrinkMainScene += ShrinkScene;
    }

    private void GrowScene()
    {
        _animator.Play("main-scene-grow");
    }

    private void ShrinkScene()
    {
        _animator.Play("main-scene-shrink");
    }

    private void OnDestroy()
    {
        MainGameManager.Instance.GrowMainScene -= GrowScene;
        MainGameManager.Instance.ShrinkMainScene -= ShrinkScene;
    }
    */
}
