using UnityEngine;
using System.Collections;

namespace Panoodlers
{

public class SpaceScript : MonoBehaviour
{

    public GameObject spaceText; // Whoops bad programming practice
    public GameObject spaceSmash;
    public GameObject spacebar;
    private SpriteRenderer spacebarImagesRenderer;
    public Sprite[] mySequence;

    private GMScript GameManagerScript;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spacebarImagesRenderer = GameObject.Find("SpaceImages").GetComponent<SpriteRenderer>();
        GameManagerScript = GameObject.Find("MyGameManager").GetComponent<GMScript>();
        // spaceText = gameObject.child("SpaceText");
        // spaceSmash = gameObject.child("SpaceSmash");
        // spacebar = gameObject.child("SpaceBar");
        HideSpace();
        
        
    }

    // Update is called once per frame
    void Update()
    {
         if (GameManagerScript.gamestage == Stage.pull) {
            if (Input.GetButtonDown("Space")) {
                 spaceSmash.GetComponent<SpriteRenderer>().enabled = true;
            } else if (Input.GetButtonUp("Space"))
            {
                spaceSmash.GetComponent<SpriteRenderer>().enabled = false;
            }
                

                
        }
        
    }

    public void ShowSpace()
    {
        spaceText.GetComponent<SpriteRenderer>().enabled =true;
        spacebarImagesRenderer.enabled = true;
    }

    public void HideSpace()
    {
        spaceText.GetComponent<SpriteRenderer>().enabled = false;
        spaceSmash.GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("SpaceImages").GetComponent<SpriteRenderer>().enabled = false;
    }

    public IEnumerator flashSmash()
    {
        spaceSmash.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(.5f);
        spaceSmash.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void setSprite(int frame)
    {
        if (frame < mySequence.Length)
        {
            spacebarImagesRenderer.sprite = mySequence[frame];
        }
        // Debug.Log("Attempted to SS.setSprite to frame " + frame);
    }


}



}
