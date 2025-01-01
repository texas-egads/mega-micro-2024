using UnityEngine;

public class StumpHeightSet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SpriteRenderer spriteRenderer; // variable used to access the sprite renderer.
    [SerializeField] Sprite shortStump; // the sprite used for the short stump.
    [SerializeField] Sprite tallStump; // the sprite used for the tall stump.
    [SerializeField] GameObject[] raiseList; // the objects to move up if the stump exists, and if it's extra tall.
    private int spriteId;
    private float raiseAmount;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        // on start, set the stump's height randomly, and then move the apple, panda, archer, and/or arrow accordingly.
        spriteId = Random.Range(0,3); // get a random number for the sprite. 0 - doesn't exist. 1 - short stump. 2 - tall stump.
        switch (spriteId) // depending on the sprite id, set the stump's position, sprite, and amount to raise objects on top.
        {
            case 0: // if the stump doesn't exist, move it below the screen.
                this.transform.position += Vector3.up * -10.0f; // just move the stump out of the way if it's not supposed to be there.
                raiseAmount = 0.0f;
                break;
            case 1: // if the stump is short, give it the short stump sprite, and raise the specified objects up by 1 unit.
                spriteRenderer.sprite = shortStump;
                raiseAmount = 1.0f;
                break;
            case 2: // if the stump is tall, give it the tall stump sprite, and raise the specified objects up by 3 units.
                spriteRenderer.sprite = tallStump;
                raiseAmount = 2.5f;
                break;
            default:
                break;

        }
        foreach (var raiseObject in raiseList){
            raiseObject.transform.position += Vector3.up * raiseAmount;
        }
    }

}
