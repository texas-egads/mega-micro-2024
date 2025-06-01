using UnityEngine;
using System.Collections;

namespace Panoodlers {

public enum Stage {
    chooseden,
    swimin, // Skipped for now
    pull
}

public enum Animal {
    platypus,
    catfish,
    turtle
}

// Increase sprite frequency or swish frequency for higher difficulty games

public class GMScript : MonoBehaviour
{
    public Stage gamestage;
    //Managers.MinigamesManager.Difficulty difficulty; //Doesn't work?
    private int difficulty; // hmm this for now, I guess
   
    // Den resident data
    public int[,] orderings; // for reference only
    public int orderings2;
    private int orderNum;
    private GameObject den;

    // For player's den
    private PlayerController PCscript;

    // For space stuff

    private SpaceScript spaceItems;

    // For counting the pulling
    private int pullThreshold = 10;
    private int pullCount; // Space count
    private int pullProgress; // Distance on progress bar for sprite frames

    public AudioClip music;
    public AudioClip correct;
    public AudioClip wrong;
    public AudioClip smash;
    
    private AudioSource musicplayer;
    private AudioSource effects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get reference to playercontroller script;
        PCscript = GameObject.Find("Player").GetComponent<PlayerController>();

        // Get reference to space stuff
        spaceItems = GameObject.Find("SpaceItems").GetComponent<SpaceScript>(); //or cheat
        // Debug.Log("Space items: " + spaceItems);
        spaceItems.HideSpace();

        // Set orderings
        orderings = new int[,] {{0, 1, 2}, {0, 2, 1}, {1, 0, 2}, {1, 2, 0}, {2, 0, 1}, {2, 1, 0}};
        //orderings = new int
       

        // Get difficulty
        difficulty = (int)Managers.MinigamesManager.GetCurrentMinigameDifficulty();

        // Set gamestage to chooseden and pick a den ordering
        gamestage = Stage.chooseden;
        orderNum = Random.Range(0, orderings.GetLength(0)); // maxExclusive, int overload

        // Testing orderings
        // Debug.Log("Order num: " + orderNum + " current den: " + PCscript.currentDen);
        // Debug.Log(orderings[orderNum,0] + " " + orderings[orderNum,1] + " " + orderings[orderNum,2]);

        // Set pull numbers to zero
        pullCount = 0;
        pullProgress = 0;

        // Set animal
            for (int i = 0; i < 3; i++) {
            
            den = GameObject.Find("Den" + i);
            den.GetComponent<DenScript>().myanimal = (Animal)orderings[orderNum, i];
            // Debug.Log("Den " + i + "has animal " + den.GetComponent<DenScript>().myanimal);
        }

        musicplayer = Managers.AudioManager.CreateAudioSource();
        effects = Managers.AudioManager.CreateAudioSource();

        musicplayer.PlayOneShot(music, 1);

        
    }

    // Update is called once per frame
    void Update()
    {
        // Change game stage to "pull" once a den has been chosen
        if (gamestage == Stage.chooseden) {
            if (Input.GetButtonDown("Space")) {
                gamestage = Stage.pull;
                // Debug.Log("Space pressed");
                // Debug.Log(gamestage);
                spaceItems.ShowSpace();
            }
        }

        if (gamestage == Stage.pull) {
            if (Input.GetButtonDown("Space")) {
                pullCount += 1;
                effects.PlayOneShot(smash, 1);
                spaceItems.flashSmash(); // Briefly show smash sprite
                if (pullCount % (difficulty + 1) == 0) {
                    pullProgress += 1;
                }
                // Update bar sprite here
                spaceItems.setSprite(pullProgress);

                if (pullProgress == pullThreshold) {
                    // Debug.Log("Order num: " + orderNum + " current den: " + PCscript.currentDen);
                    // Debug.Log(orderings[orderNum,0]);
                    // Debug.Log(orderings[orderNum,PCscript.currentDen]);
                    if (orderings[orderNum,PCscript.currentDen] == (int)Animal.catfish) {

                        // Win game!
                        // Debug.Log("You win!");
                        Managers.MinigamesManager.DeclareCurrentMinigameWon();
                        effects.PlayOneShot(correct, 1);

                        //DeclareCurrentMinigameLost(); Don't need to do this
                        Managers.MinigamesManager.EndCurrentMinigame(2); // number is delay

                    } else { // Reset to chooseden stage
                        // Debug.Log("Choose a different den");
                        musicplayer.PlayOneShot(wrong, 1);
                        

                        // Reset pull numbers
                        pullCount = 0;
                        pullProgress = 0;
                    
                    }
                    spaceItems.HideSpace();
                    StartCoroutine(PullAnimal(PCscript.currentDen, (Animal)orderings[orderNum, PCscript.currentDen])); // Delay so you don't accidentally re-choose the den

                    
                }
            }
        }
        
        
    }

    
    // Delay and display animal
    IEnumerator PullAnimal(int den, Animal animal) 
    {
        var denobj = GameObject.Find("Den" + den);
        // Display animal sprite
        if (animal == Animal.catfish) {
            denobj.transform.Find("Fish").gameObject.GetComponent<SpriteRenderer>().enabled = true;
        } else if (animal == Animal.platypus) {
            denobj.transform.Find("Platypus").gameObject.GetComponent<SpriteRenderer>().enabled = true;
        } else if (animal == Animal.turtle) {
            denobj.transform.Find("Turtle").gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        yield return new WaitForSeconds(1f);
        gamestage = Stage.chooseden;
        denobj.transform.Find("Turtle").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        denobj.transform.Find("Platypus").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        denobj.transform.Find("Fish").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log("Gamestage is " + gamestage);
    }



}

}
