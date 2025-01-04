using System.Collections.Generic;
using UnityEngine;

public class ScrollBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 5;
    public GameObject bar, gameController;
    public Animator animator, anim;
    public AudioClip loseSound, cutWire;
    public bool contact;
    float defusingTime, penaltyTime;
    BoxCollider2D boxCollider2D;

    public List<GameObject> ActiveWires = new List<GameObject>();
    GameObject currentWire;
    int startingNumberofWires, wireIndex = 0;
    bool set = false;

    enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    }
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = false;

        startingNumberofWires = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (!set)
        {
            SetWireList();
        }

        if (set)
        {
            currentWire = ActiveWires[wireIndex];
            KeyCode currentWireKey = currentWire.GetComponent<Wire>().keyCode;
            transform.position = currentWire.transform.position;
           

            if (Input.GetKeyDown(currentWireKey))
            {
                defusingTime = 0.1f;
                AudioSource cut = Managers.AudioManager.CreateAudioSource();
                cut.PlayOneShot(cutWire);

            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
            {
                penaltyTime = 0.5f;
                animator.SetBool("Lose", true);
                anim.SetBool("Exploded", true);
                AudioSource lose = Managers.AudioManager.CreateAudioSource();
                lose.PlayOneShot(loseSound);
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                Managers.MinigamesManager.EndCurrentMinigame(1f);
            }


            if (defusingTime > 0 && penaltyTime <= 0)
            {
                boxCollider2D.isTrigger = true;
                defusingTime -= Time.deltaTime;
            }
            else
            {
                boxCollider2D.isTrigger = false;
                if (penaltyTime > 0)
                {
                    penaltyTime -= Time.deltaTime;

                }
            }

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Object 1")
        {
            collision.gameObject.GetComponent<Wire>().Active = false;
            defusingTime = 0f;
            if (wireIndex < startingNumberofWires - 1)
            {
                wireIndex += 1;
            }
        }
    }

    void SetWireList()
    { 
            ActiveWires = gameController.GetComponent<GameController>().getWireList();
            if (ActiveWires.Count == startingNumberofWires && !set)
            {
                set = true;
            }
    }

}
