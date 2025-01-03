using UnityEngine;

namespace RedRagers
{
    public class Log : MonoBehaviour
    {
        Transform tf;
        public Player player;
        float rotationSpeed;
        public Animator front_water;
        public Animator back_water;

        void Start()
        {
            tf = gameObject.GetComponent<Transform>();
            rotationSpeed = 10;
        }

        void Update()
        {
            float input = Input.GetAxis("Horizontal");
            front_water.SetFloat("horizontal", input);
            back_water.SetFloat("horizontal", input);
            if (input != 0)
            {
                if (rotationSpeed/Mathf.Abs(rotationSpeed) == Input.GetAxis("Horizontal")) {
                    rotationSpeed -= 60 * Time.deltaTime * Input.GetAxis("Horizontal");
                }
                else {
                    rotationSpeed -= 80 * Time.deltaTime * Input.GetAxis("Horizontal");
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
