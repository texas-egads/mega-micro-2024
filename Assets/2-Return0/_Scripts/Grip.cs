using UnityEngine;
namespace Return0
{
    public class Grip : MonoBehaviour
    {
        public Inputs inputValue;
        public bool pressed = false;

        public enum Inputs
        {
            notDefined,
            W,
            A,
            S,
            D
        }

        public Grip(Inputs inputValue, bool pressed, Sprite sprite)
        {
            this.inputValue = inputValue;
            this.pressed = pressed;
        }
    }
}

