using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public float scalar;
    public Transform cam;
    private Vector3 initPos;

    void Start()
    {
        initPos = transform.position;
    }

    void Update()
    {
        transform.position = initPos + Vector3.right * cam.position.x * -scalar;
    }
}
