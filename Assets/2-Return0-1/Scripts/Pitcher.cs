using System.Collections;
using UnityEngine;

namespace Return0
{
    public class Pitcher : MonoBehaviour
    {
        public GameObject ball;
        Animator ballAnimator;
        float throwTime;
        public float maxThrowTime;
        public bool ballThrow;
        float currentTime;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (this.gameObject.GetComponentInChildren<Animator>())
            {
                ballAnimator = this.gameObject.GetComponentInChildren<Animator>();
                Debug.Log("found");
            }
            else
            {
                Debug.Log("not found");
            }
            throwTime = Random.Range(0.5f, maxThrowTime);
            ball.SetActive(false);
            ballThrow = false;
        }

        // Update is called once per frame
        void Update()
        {
            currentTime += Time.deltaTime;
            if (currentTime >= throwTime) ballThrow = true;
            ThrowBall();
        }

        void ThrowBall()
        {
            if (ballThrow)
            {
                Debug.Log("Throw!");
                ball.SetActive(true);
                if (ballAnimator) ballAnimator.SetBool("Throw", true);
            }
        }
    }
}
