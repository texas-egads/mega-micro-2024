using UnityEngine;

public class Button : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    SpriteRenderer spriteRenderer;
    public KeyCode keyCode;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(keyCode))
        {
            spriteRenderer.color = Color.red;
        }
        else {
            spriteRenderer.color = Color.white;
        }
    }
}
