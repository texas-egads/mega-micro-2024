using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public float radius, delay;
    float timer = 0;
    public List<GameObject> WireActives = new List<GameObject>();
    bool gameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        WireActives = WireStatus();

        if (WireActives.Count <= 0 && gameOver == false && timer >= delay)
        {
            Managers.MinigamesManager.DeclareCurrentMinigameWon();
            Managers.MinigamesManager.EndCurrentMinigame(1f);
            gameOver = true;
        }

        timer += Time.deltaTime;
    }

    List<GameObject> WireStatus()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        List<GameObject> ActiveWires = new List<GameObject>();
        foreach (BoxCollider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Object 1")
            {
                bool active = collider.gameObject.GetComponent<Wire>().Active;
                if (active)
                {
                    ActiveWires.Add(collider.gameObject);
                }
            }

        }
        return ActiveWires;
    }
}
