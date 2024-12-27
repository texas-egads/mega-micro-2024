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
        }

        private void Update()
        {
            float axisSpace = Input.GetAxisRaw("Space");

            if (axisSpace > 0 && canSwing)
            {
                Debug.Log("SPace hit");
                canSwing = false;
                StartCoroutine(StartSwingDelay());
            }
        }

        IEnumerator StartSwingDelay()
        {
            yield return new WaitForSeconds(swingDelay);
            canSwing = true;
            Debug.Log("can swing again");
        }
    }
}

