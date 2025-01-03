using System.Collections.Generic;
using UnityEngine;

public class ScrollBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 5;
    public GameObject bar, spawner;
    float BarHalfWidth, ScrollWidth, TotalDisplacement;
    Vector2 ScrollRange;

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
        ScrollWidth = transform.localScale.x / 2f;
        BarHalfWidth = bar.transform.localScale.x/2f;
        ScrollRange = new Vector2(-(BarHalfWidth-ScrollWidth), BarHalfWidth - ScrollWidth);
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = false;

        startingNumberofWires = setDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
       
        SetWireList();

        if (set)
        {
            currentWire = ActiveWires[wireIndex];
            KeyCode currentWireKey = currentWire.GetComponent<Wire>().keyCode;
            //print(currentWireKey);
            transform.position = currentWire.transform.position;
            //int input = (int)Input.GetAxisRaw("Horizontal");
            //wireIndex += input;
            /*
            if (wireIndex < ActiveWires.Count && wireIndex >= 0)
            {
                currentWire = ActiveWires[wireIndex];
                currentWireKey = currentWire.GetComponent<Wire>().keyCode;
                print(currentWireKey);
                transform.position = currentWire.transform.position;
            }
            else {
                wireIndex -= input;
            }
            */
            //Vector2 direction = input.normalized;
            //Vector2 velocity = direction * speed;
            //Vector2 displacement = velocity * Time.deltaTime;

            /*
            TotalDisplacement += displacement.x;
            if (TotalDisplacement < ScrollRange.y && TotalDisplacement > ScrollRange.x)
            {
                transform.Translate(displacement);
            }
            else {
                TotalDisplacement -= displacement.x;
            }
            */

            if (Input.GetKeyDown(currentWireKey))
            {
                defusingTime = 0.1f;

            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
            { 
                penaltyTime = 0.5f;
                //Debug.Log("penalty");
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
            if (wireIndex < startingNumberofWires-1)
            {
                wireIndex += 1;
            }
        }
    }

    int setDifficulty()
    {
        Difficulty level = (Difficulty)(Managers.MinigamesManager.GetCurrentMinigameDifficulty());
        int Hardness = (int)(level) + 2;

        return Hardness;
    }
    void SetWireList()
    { 
            ActiveWires = spawner.GetComponent<Spawner>().getActiveWires();
            if (ActiveWires.Count == startingNumberofWires)
            {
                set = true;
            }
            //Debug.Log(ActiveWires.Count);
    }

}
