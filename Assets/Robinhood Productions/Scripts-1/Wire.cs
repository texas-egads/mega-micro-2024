using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Wire : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool Active;
    SpriteRenderer sprite;
    public KeyCode keyCode;
    public Color code;
    BoxCollider2D boxCollider;
    //Dictionary<string, KeyCode> WireType = new Dictionary<string, KeyCode>();
    void Start()
    {
        Active = true;
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        //createDictionary(WireType);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active)
        {
            sprite.color = Color.grey;
        }
        else if (Active)
        {
            sprite.color = code;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "object 1")
        { 
            Destroy(collision.gameObject);
        }
    }


}
