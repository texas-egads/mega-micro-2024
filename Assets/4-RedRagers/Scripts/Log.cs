using UnityEngine;

namespace RedRagers
{
    public class Log : MonoBehaviour
    {
        Transform tf;
        float rotationSpeed;

        void Start()
        {
            tf = gameObject.GetComponent<Transform>();
            rotationSpeed = 10;
        }

        void Update()
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                if (rotationSpeed/Mathf.Abs(rotationSpeed) == Input.GetAxis("Horizontal")) {
                    rotationSpeed -= 120 * Time.deltaTime * Input.GetAxis("Horizontal");
                }
                else {
                    rotationSpeed -= 60 * Time.deltaTime * Input.GetAxis("Horizontal");
                }
            }
            else
            {
                rotationSpeed += 30 * Time.deltaTime;
            }
            tf.Rotate(Time.deltaTime * rotationSpeed, 0, 0);
            if (Input.GetButtonDown("Space"))
            {
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(1f);
            }
        }
    }
}
