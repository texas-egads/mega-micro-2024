using UnityEngine;

public class BystanderPanda2 : MonoBehaviour
{
    BoxCollider2D boxCollider;
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Object 1")
        {
            anim.SetBool("Hit", true);
            Managers.MinigamesManager.EndCurrentMinigame(1f);
            //print("You shot the Panda!");
        }
    }
}
