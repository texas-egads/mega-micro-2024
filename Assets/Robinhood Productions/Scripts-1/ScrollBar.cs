using UnityEngine;

public class ScrollBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 5, delay= 0.5f;
    float timer = 0;
    public GameObject bar;
    float BarHalfWidth, ScrollWidth, TotalDisplacement;
    Vector2 ScrollRange;
    public bool contact;
    public float defusingTime;
    BoxCollider2D boxCollider2D;
    void Start()
    {
        ScrollWidth = transform.localScale.x / 2f;
        BarHalfWidth = bar.transform.localScale.x/2f;
        ScrollRange = new Vector2(-(BarHalfWidth-ScrollWidth), BarHalfWidth - ScrollWidth);
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = false;
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
            defusingTime = 0.1f;
        }

        if (defusingTime > 0)
        {
            boxCollider2D.isTrigger = true;
            defusingTime -= Time.deltaTime;
        }
        else
        {
            boxCollider2D.isTrigger = false;
        }

        //timer += Time.deltaTime; 



        //defusing = false;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Object 1")
        {
            collision.gameObject.GetComponent<Wire>().Active = false;
        }
    }

}
