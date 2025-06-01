using UnityEngine;

namespace Panoodlers {

    
    
    public class PlayerController : MonoBehaviour
    {
        private GMScript GameManagerScript;
        private Vector2[] denPositions = new Vector2[3];
        public int currentDen; // Used by game manager
        private Vector2 pos;
        private const int numDens = 3;
        private GameObject den; // For initializing denPositions from object locations for panda

        private float oldInputAxis; // for input
        private float input; // to save input
        private bool justMoved; // to keep track of movement
        public int test;
        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Set player current den
            currentDen = 1;

            // Acquire game manager script to get gamestage
            GameManagerScript = GameObject.Find("MyGameManager").GetComponent<GMScript>();

            // Get a reference to the player's location
            pos = GetComponent<Transform>().position;
            
            // Grab den locations so the player can choose between them
            for (int i = 0; i < numDens; i++) {
                
                den = GameObject.Find("Den" + i);
                // Debug.Log(den);
                denPositions[i] = den.GetComponent<Transform>().position;
            }
                
            // Set old Input axis
            oldInputAxis = 0;

            // testing
            test = 0;
            
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log("PC sees gamestage as " + GameManagerScript.gamestage);
            //Debug.Log("test: " + test);
            test++;

            // Allow moving only when choosing den
            if (GameManagerScript.gamestage == Stage.chooseden) {
                input = Input.GetAxis("Horizontal");

                // Move left or right by changing den number
                if (input < 0 && input < oldInputAxis && !justMoved) { // Go left on key up
                    currentDen -= 1;
                    justMoved = true;
                    // Debug.Log(currentDen);
                    
                    if (currentDen < 0) {
                        currentDen = 0;
                    }
                    
                }

                if (input > 0 && input > oldInputAxis && !justMoved) {
                    currentDen += 1;
                    justMoved = true;
                    // Debug.Log(currentDen);
                    
                    if (currentDen >= numDens) {
                        currentDen = numDens - 1;
                    }
                    
                }

                // Reset justMoved if no key is pressed (moving toward zero)
                if ((input < 0 && input > oldInputAxis) || (input > 0 && input < oldInputAxis)) {
                    justMoved = false;
                }

                oldInputAxis = Input.GetAxis("Horizontal");
                //Debug.Log(oldInputAxis);

                // Update position of panda
                transform.position = denPositions[currentDen];
                //Debug.Log(transform.position);

            }

            // Change sprite when pulling fish
            if (GameManagerScript.gamestage == Stage.pull) {
                
                // Change sprite
                // Debug.Log("Panda is pulling");
            }

            //Debug.Log("Gamestage: " + GameManagerScript.gamestage + " CD: " + currentDen + " Input: " + input + " JM: " + justMoved);
            
        }
    }


}
