using Unity.Mathematics.Geometry;
using UnityEngine;

namespace Silverly101
{
    public class Spawner : MonoBehaviour
    {
        public GameObject[] obstacles;
        public GameObject finishLine;
        public Transform player;
        public float elapsedTime = 0;
        public float objectSpeed;
        public float spawnCooldown;
        private float spawnTimer = 0;
        void Start()
        {
            elapsedTime = 0;
        }

        private bool spawnFinish = false;

        // Update is called once per frame
        void Update()
        {
            if (elapsedTime <= .4f)
            {
                
            }
            else if (elapsedTime <= 8)
            {
                if (spawnTimer <= 0)
                {
                    if (Random.value <= .7)
                    {
                        spawnTimer += Random.Range(.3f, spawnCooldown);
                        float position = Random.value;
                        Instantiate(obstacles[Random.Range(0, obstacles.Length)],
                            transform.position + new Vector3(Mathf.Lerp(-1.3f, 1.3f, position), 0, 0), Quaternion.Euler(0, 0, Mathf.Lerp(-49, 49, position)));
                    }
                    else
                    {
                        spawnTimer += Random.Range(.7f, spawnCooldown);
                        float position = Mathf.InverseLerp(-4, 4, player.position.x);
                        Instantiate(obstacles[Random.Range(0, obstacles.Length)],
                            transform.position + new Vector3(Mathf.Lerp(-1.3f, 1.3f, position), 0, 0), Quaternion.Euler(0, 0, Mathf.Lerp(-49, 49, position)));
                    }
                    
                }
            }
            else if(!spawnFinish)
            {
                Instantiate(finishLine, new Vector3(0, 1.5f, 0), Quaternion.identity);
                spawnFinish = true;
            }
            
            elapsedTime += Time.deltaTime;
            spawnTimer -= Time.deltaTime;
        }
        
    }
}