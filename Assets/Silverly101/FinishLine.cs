using System;
using UnityEngine;

namespace Silverly101
{
    public class FinishLine : MonoBehaviour
    {
        private float speed = 1;
        public float scale;
        public float acceleration;
        public Spawner spawner;
        void Start()
        {
            if (Managers.MinigamesManager.GetCurrentMinigameDifficulty() == IMinigamesManager.Difficulty.EASY)
            {
                acceleration = 2;
            }
            if (Managers.MinigamesManager.GetCurrentMinigameDifficulty() == IMinigamesManager.Difficulty.MEDIUM)
            {
                acceleration = 2.5f;
            }
        }
        

        private void FixedUpdate()
        {
            transform.Translate(0, -1 * Time.deltaTime * speed, 0);
            speed += Time.deltaTime * acceleration;
            transform.localScale = Vector3.Lerp(new Vector3(3.3f, .08f, 0), new Vector3(17f, .3f, 0), Mathf.InverseLerp(1.5f,-4,transform.position.y));
        }
        
        
    }
}