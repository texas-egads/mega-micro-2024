using UnityEngine;

namespace RedRagers
{
    public class Log : MonoBehaviour
    {
        Transform tf;
        public Player player;
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
                    rotationSpeed -= 60 * Time.deltaTime * Input.GetAxis("Horizontal");
                }
                else {
                    rotationSpeed -= 60 * Time.deltaTime * Input.GetAxis("Horizontal");
                }
            }
            else
            {
                rotationSpeed += 60 * Time.deltaTime;
            }
            player.value += -rotationSpeed / player.rotationLimit * Time.deltaTime;
            player.value = Mathf.Clamp(player.value, 0f, 1f);
            tf.Rotate(Time.deltaTime * rotationSpeed, 0, 0);
        }
    }
}
