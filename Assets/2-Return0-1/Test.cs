using System.Collections;
using UnityEngine;


namespace Return0
{
    public class Test : MonoBehaviour
    {
        public float swingDelay=0f;
        public bool canSwing;
        Animator animator;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Debug.Log("Hello!");
            canSwing = true;
            if (this.gameObject.GetComponent<Animator>()) animator = this.gameObject.GetComponent<Animator>();
            else Debug.Log("<color=red>Animator not found!</color>");
        }

        private void Update()
        {
            float axisSpace = Input.GetAxisRaw("Space");

            if (axisSpace > 0 && canSwing && animator)
            {
                Debug.Log("SPace hit");
                canSwing = false;
                if(animator) animator.SetBool("SpacePressed", true);
                StartCoroutine(StartSwingDelay());
            }
        }

        IEnumerator StartSwingDelay()
        {
            
            yield return new WaitForSeconds(swingDelay);
            if (animator) animator.SetBool("SpacePressed", false);
            canSwing = true;
            Debug.Log("can swing again");
        }
    }
}

