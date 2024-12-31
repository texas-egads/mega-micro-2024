using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    Transform tf;
    float rotationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
        rotationSpeed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            rotationSpeed -= 20 * Time.deltaTime * Input.GetAxis("Horizontal");
        }
        else {
            rotationSpeed += 5 * Time.deltaTime;
        }
        Debug.Log(rotationSpeed);
        tf.Rotate(Time.deltaTime * rotationSpeed,0,0);
    }
}
