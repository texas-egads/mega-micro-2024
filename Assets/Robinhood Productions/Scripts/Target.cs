using UnityEngine;


public class Target : MonoBehaviour
{
    BoxCollider2D boxCollider;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            HingeJoint2D hj;
            hj = gameObject.AddComponent<HingeJoint2D>();
            GetComponent<HingeJoint2D>().connectedBody = collision.rigidbody;
            rb.mass = 0.00001f;
            //boxCollider.material.bounciness = 0;
            collision.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            collision.rigidbody.linearVelocity = new Vector2(0, 0);
            rb.freezeRotation = true;
            rb.linearVelocity = new Vector2(0, 0);
            rb.bodyType = RigidbodyType2D.Static;
            collision.gameObject.transform.parent = transform;
            print("You shot the Apple! Congrats!");
        }
    }
}
