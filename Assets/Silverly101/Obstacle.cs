using System;
using UnityEngine;

namespace Silverly101
{
    public class Obstacle : MonoBehaviour
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
            transform.GetChild(0).rotation = Quaternion.identity;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void FixedUpdate()
        {
            transform.Translate(0, -1 * Time.deltaTime * speed, 0);
            speed += Time.deltaTime * acceleration;
            transform.localScale = Vector3.one * (speed * scale);
            if (transform.position.y <= -5)
            {
                Destroy(this.gameObject);
            }
        }
        
        
    }
}