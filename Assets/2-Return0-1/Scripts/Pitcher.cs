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
        float maxThrowTime;
        public bool ballIsThrown;
        float currentTime;
        string difficulty;
        [SerializeField] float easyMultiplier;
        [SerializeField] float mediumMultiplier;
        [SerializeField] float hardMultiplier;
        //bool pitchStored;
        bool canCountTime;
        

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

            DifficutlyMultiplier();
            throwTime = Random.Range(0.5f, maxThrowTime);
            ball.SetActive(false);
            ballIsThrown = false;
            canCountTime = true;
            
        }

        // Update is called once per frame
        void Update()
        {

            if (canCountTime) currentTime += Time.deltaTime;

            if (currentTime >= throwTime)
            {
                ThrowBall();
                ballIsThrown = true;
                currentTime = 0;        // set current time to 0 so throw ball isn't called again
                canCountTime = false;   // set bool to false so that time isn't counted anymore
            }
        }

        void ThrowBall()
        {
            
            //read an index and set the animation controller to pitch a certain ball
            if (!ballIsThrown)
            {
                Debug.Log("Throw!");
                ball.SetActive(true);
                //Ball index goes 0 for hi, 1 fo mid & 2 for lo, subject to change
                
                // choose random int for pitch type
                int pitchType = Random.Range(0, 3);
                //Debug.Log(pitchType);

                BaseballManager.pitchType = pitchType;
                //pitchStored = true;
                Debug.Log("<color=yellow> pitch stored as </color>" + pitchType);

                if (ballAnimator)
                {
                    // choose different pitch based on pitchType
                    switch (pitchType) 
                    {
                        case 0:
                            //Debug.Log("High ball!");
                            ballAnimator.SetInteger("PitchIndex", 0);
                            break;
                        case 1:
                            //Debug.Log("Fast ball!");
                            ballAnimator.SetInteger("PitchIndex", 1);
                            break;
                        case 2:
                            //Debug.Log("Low ball!");
                            ballAnimator.SetInteger("PitchIndex", 2);
                            break;
                        default:
                            break;
                    }

                    if (pitcherAnimator) pitcherAnimator.SetBool("isThrown", true);
                    ballIsThrown = false;
                }
                

            }
        }

        void DifficutlyMultiplier()
        {
            difficulty = Managers.MinigamesManager.GetCurrentMinigameDifficulty().ToString();
            Debug.Log(difficulty);
            if (ballAnimator)
            {
                switch (difficulty)
                {
                    case "EASY":
                        ballAnimator.speed *= easyMultiplier;
                        maxThrowTime = 1f;
                        break;
                    case "MEDIUM":
                        ballAnimator.speed *= mediumMultiplier;
                        maxThrowTime = 2.5f;
                        break;
                    case "HARD":
                        ballAnimator.speed *= hardMultiplier;
                        maxThrowTime = 3.5f;
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
}
