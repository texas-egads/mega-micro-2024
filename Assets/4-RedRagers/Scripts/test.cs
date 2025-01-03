using UnityEngine;

public class test : MonoBehaviour
{
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1);
            animator.Play("walk",0, animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1);
        }
    }
}
