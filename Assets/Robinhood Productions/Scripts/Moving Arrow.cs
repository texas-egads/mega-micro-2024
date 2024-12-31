using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
//using UnityEngine.Windows;

public class MovingArrow : MonoBehaviour
{
    public int speed;
    public Vector2 angleMinMax;
    public float Difficulty, step;
    float interp = 0f;
    bool spacePushed = false;
    float time = 0;
    float fireAngle;
    Vector2 initialPos;
    public LineRenderer Line;
    Rigidbody2D ArrowBody;
    float YDisplace, XDisplace;
    public bool hit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ArrowBody = GetComponent<Rigidbody2D>();
        initialPos = new Vector2(transform.position.x, transform.position.y);

    }

    // Update is called once per frame
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
        hit = true;
    }

    void RotationOscillation(){
        if (!spacePushed)
        {
            float deltaAngle = Mathf.Lerp(angleMinMax.x, angleMinMax.y, interp);
            transform.rotation = Quaternion.Euler(Vector3.forward * deltaAngle);
            DrawPath(step, transform.rotation.eulerAngles.z, 10);
            interp += Time.deltaTime * Difficulty;
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

    void DrawPath(float Step, float angle, float time){

        step = Mathf.Max(0.01f, step);
        time = 10;
        Line.positionCount = (int)(time/ Step) + 2;
        int count = 0;
        for (float i = 0f; i < time; i += Step){
            float YDisplace = speed * i * Mathf.Sin(angle*Mathf.Deg2Rad) + (Physics2D.gravity.y * Mathf.Pow(i, 2) / 2f);
            float XDisplace = speed * i * Mathf.Cos(angle * Mathf.Deg2Rad);
            float tan = YDisplace / XDisplace;
            Vector3 pos = new Vector3(XDisplace, YDisplace, 0);
            Line.SetPosition(count, pos);
            count++;
        }
        //print("drawn");
        //final point
        float Y = Mathf.Tan((angle) * Mathf.Deg2Rad); //x is always 1
        Vector2 Finaldir= new Vector2(1, Y);
        Vector2 Finalvelocity = Finaldir * speed;
        Vector2 Finaldisplacement = Finalvelocity * time + (Physics2D.gravity * Mathf.Pow(time, 2) / 2);
        Vector3 Finalpos = new Vector3(initialPos.x + Finaldisplacement.x, initialPos.y + Finaldisplacement.y, 0);
        Line.SetPosition(count, Finalpos);

    }
    float FlyingRotation(float angle, float time){
        float Xvelocity = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
        float Yvelocity = speed * Mathf.Sin(angle * Mathf.Deg2Rad)+ Physics2D.gravity.y*time;
        
        float Rotation = Mathf.Atan2(Yvelocity, Xvelocity);

        return Rotation * Mathf.Rad2Deg;
    }
}
