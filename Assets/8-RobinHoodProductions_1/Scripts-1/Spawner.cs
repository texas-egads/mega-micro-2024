using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

namespace RobinHoodProductions_1
{
    public class Spawner : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public GameObject Bar, wire;
        Vector2 SpawnRange;
        Collider2D[] colliders;
        public int numberWires, count;
        public float radius;
        Dictionary<Color, KeyCode> WireType = new Dictionary<Color, KeyCode>();
        List<Color> colors = new List<Color>();
        public List<GameObject> ActiveWires = new List<GameObject>();
        bool CanSpawn = false;
        float wireHalfWidth;
        float BarHalfWidth;


        enum Difficulty
        {
            EASY,
            MEDIUM,
            HARD
        }

        void Start()
        {
            createDictionary(WireType);
            createColorList(colors);
            wireHalfWidth = wire.transform.localScale.x / 2f;
            BarHalfWidth = Bar.transform.localScale.x / 2f;
            SpawnRange = new Vector2(Bar.transform.position.x - (BarHalfWidth - wireHalfWidth), Bar.transform.position.x);
            numberWires = setDifficulty();
        }

        // Update is called once per frame
        void Update()
        {
            if (count < numberWires)
            {
                Vector2 spawnPosition = new Vector2(Random.Range(SpawnRange.x, SpawnRange.y), Bar.transform.position.y);
                int SafetyBreak = 0;
                while (!CanSpawn)
                {
                    spawnPosition = new Vector2(Random.Range(SpawnRange.x, SpawnRange.y), Bar.transform.position.y);
                    CanSpawn = PreventSpawnOverlap(spawnPosition);

                    if (CanSpawn)
                    {
                        break;
                    }

                    SafetyBreak++;

                    if (SafetyBreak > 50)
                    {
                        print("safetyBreak");
                        break;
                    }
                }
                GameObject wireClone = Instantiate(wire, spawnPosition, Quaternion.Euler(Vector3.zero));
                int RandomColor = Random.Range(0, colors.Count);
                wireClone.GetComponent<Wire>().keyCode = WireType[colors[RandomColor]];
                wireClone.GetComponent<Wire>().code = colors[RandomColor];
                ActiveWires.Add(wireClone);
                count++;
                if (count == numberWires - 1)
                {
                    SpawnRange = new Vector2(wireClone.transform.position.x + wireClone.transform.localScale.x, Bar.transform.position.x + (BarHalfWidth - wireHalfWidth * 2));
                }
                else
                {
                    SpawnRange = new Vector2(wireClone.transform.position.x + wireClone.transform.localScale.x, wireClone.transform.position.x + (5 * wireHalfWidth) * count);
                    print("last: " + SpawnRange + "at " + count);
                }
            }

        }

        public List<GameObject> getActiveWires()
        {
            return ActiveWires;
        }

        void createDictionary(Dictionary<Color, KeyCode> WireType)
        {
            WireType.Add(Color.red, KeyCode.W);
            WireType.Add(Color.blue, KeyCode.A);
            WireType.Add(Color.green, KeyCode.S);
            WireType.Add(Color.yellow, KeyCode.D);
        }

        void createColorList(List<Color> colorTypes)
        {
            colorTypes.Add(Color.red);
            colorTypes.Add(Color.blue);
            colorTypes.Add(Color.green);
            colorTypes.Add(Color.yellow);
        }


        bool PreventSpawnOverlap(Vector2 spawnPos)
        {
            colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in colliders)
            {
                Vector3 centerPoint = collider.bounds.center;
                float width = collider.bounds.extents.x;
                float leftExtent = centerPoint.x - width * 10f;
                float rightExtent = centerPoint.x + width * 10f;

                if (spawnPos.x >= leftExtent && spawnPos.x <= rightExtent)
                {
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
}
