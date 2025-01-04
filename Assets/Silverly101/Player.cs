using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Silverly101
{
    public class Player : MonoBehaviour
    {
        [SerializeField] 
        private float controlTarget = 0;

        [SerializeField]
        private float controlSpeed = 0;

        public float width = 0;
        
        [SerializeField]
        private float maxSpeed;
        
        // PID controller out in the wild?
        public float p;
        
        public GameObject dummy;
        
        [SerializeField]
        private SpriteRenderer sr;

        public Sprite[] tailSprites;
        private int obstacleLayerMask;
        private int finishLayerMask;

        private bool gameEnded = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            obstacleLayerMask = LayerMask.GetMask("Object 1");
            finishLayerMask = LayerMask.GetMask("Object 2");
        }

        private float output;
        // Update is called once per frame
        void FixedUpdate()
        {
            
            // collision
            if (!gameEnded && Physics2D.OverlapCircle(transform.position, .65f, obstacleLayerMask))
            {
                gameEnded = true;
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                Managers.MinigamesManager.EndCurrentMinigame(0f);
            } else if (!gameEnded && Physics2D.OverlapCircle(transform.position, .7f, finishLayerMask))
            {
                gameEnded = true;
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
            }
            controlTarget += Input.GetAxisRaw("Horizontal") * Time.deltaTime * controlSpeed;
            controlTarget = Mathf.Clamp(controlTarget, -width, width);
            dummy.transform.position = new Vector3(controlTarget, dummy.transform.position.y);
            
            float error = controlTarget - transform.position.x;

            output = p * error;
            output = Mathf.Clamp(output, -maxSpeed, maxSpeed);
            
            
            transform.Translate(output * Time.deltaTime, 0 , 0, Space.World);
            transform.rotation = Quaternion.Euler(0, 0, output * -3);
        }

        private void LateUpdate()
        {
            if (output < -3f)
            {
                sr.sprite = tailSprites[0];
            }
            else if(output > 3f)
            {
                sr.sprite = tailSprites[2];
            }
            else
            {
                sr.sprite = tailSprites[1];
            }
            sr.transform.localRotation = transform.rotation;//Quaternion.Euler(0, 0, output * 3);;
        }
    }
}