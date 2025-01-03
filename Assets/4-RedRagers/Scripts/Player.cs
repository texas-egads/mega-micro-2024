using RedRagers;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float value = 0.5f;
    public float rotationSpeed = 0.1f;
    public float rotationLimit = 140f;
    public Animator animator;
    public GameObject pivotObject;
    public TimeBarScript timeBar;

    // Update is called once per frame
    void Update()
    {
        if (timeBar.timerIsRunning)
        {
            // Sets animator position and pivot point
            animator.SetFloat("position", value);
            Vector3 pivotPoint = pivotObject.transform.position;
            float input = Input.GetAxis("Horizontal");


            // Increases value by the rotationSpeed. Value between 0-1, with the position being mapped from 70 to -70.=
            if (input != 0)
            {
                value += input * rotationSpeed * Time.deltaTime;
            }
            value = Mathf.Clamp(value, 0f, 1f);

            // Rotates the Player.
            float rotationAngle = 90f + (-1f * value * rotationLimit) - (180 - rotationLimit) * 0.5f;
            Vector3 direction = Quaternion.Euler(0f, 0f, rotationAngle) * Vector3.up;
            Vector3 newPos = pivotPoint + direction * Vector3.Distance(pivotPoint, transform.position);
            transform.position = newPos;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        }
        else {
            if (!timeBar.won) {
                transform.position = new Vector3(transform.position.x, transform.position.y - 10 * Time.deltaTime, transform.position.z);
            }
        }
    }
}
