using UnityEditor.MPE;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Bar, wire;
    Vector2 SpawnRange;
    Collider2D[] colliders;
    int numberWires, count;
    public float radius;

    bool CanSpawn = false;
    //public Vector2 boxSize;

    enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    }
    void Start()
    {
       float wireHalfWidth = wire.transform.localScale.x/ 2f;
        float BarHalfWidth = Bar.transform.localScale.x / 2f;
        SpawnRange = new Vector2(Bar.transform.position.x - (BarHalfWidth- wireHalfWidth), Bar.transform.position.x + (BarHalfWidth - wireHalfWidth));
        numberWires = setDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
            if (count < numberWires)
            {
                Vector2 spawnPosition = new Vector2(Random.Range(SpawnRange.x, SpawnRange.y), Bar.transform.position.y);
                int SafetyBreak = 0;
                while (!CanSpawn) {
                    spawnPosition = new Vector2(Random.Range(SpawnRange.x, SpawnRange.y), Bar.transform.position.y);
                    CanSpawn = PreventSpawnOverlap(spawnPosition);

                    if (CanSpawn) {
                        break;
                    }

                    SafetyBreak++;

                    if (SafetyBreak > 50){
                        print("safetyBreak");
                        break;
                    }
                }
                Instantiate(wire, spawnPosition, Quaternion.Euler(Vector3.zero));
                count++;
            }
     
    }

     bool PreventSpawnOverlap(Vector2 spawnPos)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (BoxCollider2D collider in colliders){ 
            Vector3 centerPoint = collider.bounds.center;
            float width = collider.bounds.extents.x;

            float leftExtent = centerPoint.x - width*1.5f;
            float rightExtent = centerPoint.x + width *1.5f;

            if (spawnPos.x >= leftExtent && spawnPos.x <= rightExtent) {
                return false;
            }
        }
        return true;
    }

    int setDifficulty()
    {
        Difficulty level = (Difficulty)(Managers.MinigamesManager.GetCurrentMinigameDifficulty());
        int Hardness = (int)(level) + 2;

        return Hardness;
    }
}
