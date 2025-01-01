using DG.Tweening;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;
//using UnityEngine.Windows;

public class MovingArrow : MonoBehaviour
{
    public int speed;
    public Vector2 angleMinMax;
    public float difficulty, step, MaxLineHorizontal;
    public LineRenderer Line;
    public bool hit;

    float YDisplace, XDisplace;
    float interp = 0f;
    bool spacePushed = false;
    float time = 0;
    float fireAngle;

    Vector2 initialPos;
    Rigidbody2D ArrowBody;

    enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    void Start()
    {
        ArrowBody = GetComponent<Rigidbody2D>();
        initialPos = new Vector2(transform.position.x, transform.position.y);
        difficulty = setDifficulty(difficulty);
    }


    float setDifficulty(float difficulty)
    {
        Difficulty level = (Difficulty)(Managers.MinigamesManager.GetCurrentMinigameDifficulty());
        int Hardness = (int)(level)+1;

        return Hardness* difficulty;
    }

    void Update()
    {
        RotationOscillation();
        if (spacePushed)
        {
            if (Line.enabled == true){
                Line.enabled = !Line.enabled;
            }
            YDisplace = speed * time * Mathf.Sin(fireAngle * Mathf.Deg2Rad) + (Physics2D.gravity.y * Mathf.Pow(time, 2) / 2f);
            XDisplace = speed * time * Mathf.Cos(fireAngle * Mathf.Deg2Rad);
            if (time < 100)
            {
                time += Time.deltaTime;
            }
            //transform.position = new Vector3(XDisplace + initialPos.x, YDisplace + initialPos.y, 0);
            //transform.rotation = Quaternion.Euler(Vector3.forward * FlyingRotation(fireAngle, time));
        }
    }

    private void FixedUpdate()
    {
        if (!hit){
            ArrowBody.position = new Vector3(XDisplace + initialPos.x, YDisplace + initialPos.y, 0);
            ArrowBody.rotation = FlyingRotation(fireAngle, time);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Object 2") {
            hit = true;
        }
    }

    void RotationOscillation(){
        if (!spacePushed)
        {
            float deltaAngle = Mathf.Lerp(angleMinMax.x, angleMinMax.y, interp);
            transform.rotation = Quaternion.Euler(Vector3.forward * deltaAngle);
            transform.position = new Vector3(transform.position.x, Line.transform.position.y, transform.position.z);
            interp += Time.deltaTime * difficulty;
            if (interp > 1.0f)
            {
                float tempHold = angleMinMax.x;
                angleMinMax.x = angleMinMax.y;
                angleMinMax.y = tempHold;
                interp = 0f;
            }
            if (Input.GetButtonDown("Space"))
            {
                spacePushed = true;
                fireAngle = deltaAngle;
            }
        }
    }

    float FlyingRotation(float angle, float time){
        float Xvelocity = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
        float Yvelocity = speed * Mathf.Sin(angle * Mathf.Deg2Rad)+ Physics2D.gravity.y*time;
        
        float Rotation = Mathf.Atan2(Yvelocity, Xvelocity);

        return Rotation * Mathf.Rad2Deg;
    }
}
