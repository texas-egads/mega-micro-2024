using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform player;
    public Transform boss;
    private Vector3 refPos;
    void Start()
    {
        refPos = new Vector3(0, transform.position.y, transform.position.z);
    }

    void Update()
    {
        transform.position = refPos + Vector3.right * Mathf.Clamp((player.position.x + boss.position.x) / 2, -2.6f, 2.6f);
    }
}
