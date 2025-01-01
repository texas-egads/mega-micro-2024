using UnityEngine;
using static IMinigamesManager;

public class Archerbody : MonoBehaviour
{

    public int speed;
    public Vector2 angleMinMax;
    public float difficulty, step, MaxLineHorizontal;
    public LineRenderer Line;
    public bool fired;

    float interp = 0f;
    bool spacePushed = false;
    public AudioClip shootSound;
    [SerializeField] Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        difficulty = setDifficulty(difficulty);
    }

    float setDifficulty(float difficulty)
    {
        Difficulty level = (Difficulty)(Managers.MinigamesManager.GetCurrentMinigameDifficulty());
        int Hardness = (int)(level) + 1;

        return Hardness * difficulty;
    }

    // Update is called once per frame
    void Update()
    {

        if (!spacePushed){
            RotationOscillation();
            Line.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else{
            fired = true;
        }
    }

    void RotationOscillation()
    {
        if (!spacePushed)
        {
            float deltaAngle = Mathf.Lerp(angleMinMax.x, angleMinMax.y, interp);
            transform.rotation = Quaternion.Euler(Vector3.forward * deltaAngle);
            DrawPath(step, transform.rotation.eulerAngles.z, MaxLineHorizontal);
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
                AudioSource shoot = Managers.AudioManager.CreateAudioSource();
                anim.SetBool("Released", true);
                shoot.PlayOneShot(shootSound);
            }
        }
    }

    void DrawPath(float Step, float angle, float distance)
    {

        step = Mathf.Max(0.01f, step);
        float Pathtime = distance / (speed * Mathf.Cos(angle * Mathf.Deg2Rad));
        Line.positionCount = (int)(Pathtime / Step) + 2;
        int count = 0;
        for (float i = 0f; i < Pathtime; i += Step)
        {
            float YDisplace = speed * i * Mathf.Sin(angle * Mathf.Deg2Rad) + (Physics2D.gravity.y * Mathf.Pow(i, 2) / 2f);
            float XDisplace = speed * i * Mathf.Cos(angle * Mathf.Deg2Rad);
            Vector3 pos = new Vector3(XDisplace, YDisplace, 0);
            Line.SetPosition(count, pos);
            count++;
        }
        //print("drawn");
        //final point
        float FinalYDisplace = speed * Pathtime * Mathf.Sin(angle * Mathf.Deg2Rad) + (Physics2D.gravity.y * Mathf.Pow(Pathtime, 2) / 2f);
        float FinalXDisplace = speed * Pathtime * Mathf.Cos(angle * Mathf.Deg2Rad);
        Vector3 Finalpos = new Vector3(FinalXDisplace, FinalYDisplace, 0);
        Line.SetPosition(count, Finalpos);

    }

}
