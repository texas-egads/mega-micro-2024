using UnityEngine;


public class Target : MonoBehaviour
{
    BoxCollider2D boxCollider;
    Rigidbody2D rb;
    [SerializeField] Animator anim;
    public AudioClip winSound;
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
            collision.gameObject.tag = "Object 2";
            HingeJoint2D hj;
            hj = gameObject.AddComponent<HingeJoint2D>();
            GetComponent<HingeJoint2D>().connectedBody = collision.rigidbody;
            rb.mass = 0.00001f;

            collision.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            collision.rigidbody.linearVelocity = new Vector2(0, 0);
            collision.gameObject.transform.parent = transform;

            AudioSource win = Managers.AudioManager.CreateAudioSource();
            win.PlayOneShot(winSound);

            anim.SetBool("TargetHit", true);
            Managers.MinigamesManager.DeclareCurrentMinigameWon();
            Managers.MinigamesManager.EndCurrentMinigame(1f);
        }
    }
}
