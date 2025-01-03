using UnityEngine;

public class Wire : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool Active;
    SpriteRenderer sprite;
    void Start()
    {
        Active = true;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) {
            sprite.color = Color.green;
        }
    }
}
