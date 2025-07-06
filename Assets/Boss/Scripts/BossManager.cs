using UnityEngine;

public class BossManager : MonoBehaviour
{
    // state managent 
    public enum State
    {
        CUTSCENE,
        PLAY
    }
    public State state;


    public static BossManager Instance;
    void Start()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
