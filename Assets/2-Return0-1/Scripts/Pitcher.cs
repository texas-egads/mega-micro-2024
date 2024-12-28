using System.Collections;
using UnityEngine;

namespace Return0
{
    public class Pitcher : MonoBehaviour
    {
        public GameObject ball;
        [SerializeField] Animator ballAnimator;
        [SerializeField] Animator pitcherAnimator;
        float throwTime;
        public float maxThrowTime;
        public bool ballThrow;
        float currentTime;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
            //if (this.gameObject.GetComponent<Animator>())
            //{
            //    pitcherAnimator = this.gameObject.GetComponent<Animator>();
            //    Debug.Log(pitcherAnimator.name);
            //}
            //else Debug.Log("<color=red>Animator for pitcher not found</color>");
            //if (this.gameObject.GetComponentInChildren<Animator>() && this.gameObject.GetComponentInChildren<Animator>() != pitcherAnimator)
            //{
            //    ballAnimator = this.gameObject.GetComponentInChildren<Animator>();
            //    Debug.Log(ballAnimator.name);
            //}
            //else Debug.Log("<color=red>Animator for ball not found</color>");


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
            
            //read an index and set the animation controller to pitch a certain ball
            if (ballThrow)
            {
                //Debug.Log("Throw!");
                ball.SetActive(true);
                //Ball index goes 0 for hi, 1 fo mid & 2 for lo, subject to change



                //TODO: add SetInteger lines into a switch statement for the different pitches
                if (ballAnimator) ballAnimator.SetInteger("PitchIndex", 1);
                if (pitcherAnimator) pitcherAnimator.SetBool("isThrown", true);
            }
        }
    }
}
