using UnityEngine;


public class Target2 : MonoBehaviour
{
    BoxCollider2D boxCollider;
    Rigidbody2D rb;
    [SerializeField] Animator anim;
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
        if (collision.gameObject.tag == "Object 1")
        {
            HingeJoint2D hj;
            hj = gameObject.AddComponent<HingeJoint2D>();
            GetComponent<HingeJoint2D>().connectedBody = collision.rigidbody;
            rb.mass = 0.00001f;
            //boxCollider.material.bounciness = 0;
            collision.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            collision.rigidbody.linearVelocity = new Vector2(0, 0);
            //rb.freezeRotation = true;
            //rb.linearVelocity = new Vector2(0, 0);
            //rb.bodyType = RigidbodyType2D.Static;
            collision.gameObject.transform.parent = transform;
            Managers.MinigamesManager.DeclareCurrentMinigameWon();
            Managers.MinigamesManager.EndCurrentMinigame(1f);
            //print("You shot the Apple! Congrats!");
        }
    }
}
