using UnityEngine;

public class ScrollBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 5;
    //public float DefusingTime = 1;
    public GameObject bar;
    float BarHalfWidth, ScrollWidth, TotalDisplacement;
    Vector2 ScrollRange;
    public bool defusing;
    void Start()
    {
        ScrollWidth = transform.localScale.x / 2f;
        BarHalfWidth = bar.transform.localScale.x/2f;
        ScrollRange = new Vector2(-(BarHalfWidth-ScrollWidth), BarHalfWidth - ScrollWidth);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        Vector2 direction = input.normalized;
        Vector2 velocity = direction * speed;
        Vector2 displacement = velocity * Time.deltaTime;

        TotalDisplacement += displacement.x;
        if (TotalDisplacement < ScrollRange.y && TotalDisplacement > ScrollRange.x)
        {
            transform.Translate(displacement);
        }
        else {
            TotalDisplacement -= displacement.x;
        }

        if (Input.GetButtonDown("Space"))
        {
            defusing = true;
        }
        else
        {
            defusing = false;
        }
    }
}
