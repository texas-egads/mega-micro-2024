using UnityEngine;
using System.Collections;

namespace Panoodlers {

public class DenScript : MonoBehaviour
{
    //public tailSprite
    private const int maxDelay = 5;
    public Animal myanimal; // Assigned by game manager on start
    private Animator animAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(flashTail());
        animAnimator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator flashTail() 
    {
        float delay = Random.Range (0, maxDelay);
        yield return new WaitForSeconds(delay);

        // flash tail
        // Debug.Log("flash tail");

        switch (myanimal) {
            case Animal.platypus:
                animAnimator.SetTrigger("PlayPlaty");
                break;
            case Animal.catfish:
                animAnimator.SetTrigger("PlayFish");
                break;
            case Animal.turtle:
                animAnimator.SetTrigger("PlayTurtle");
                break;
        }

        
        StartCoroutine(flashTail());

    }
}

}
