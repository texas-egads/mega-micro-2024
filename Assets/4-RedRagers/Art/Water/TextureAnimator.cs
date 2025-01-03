using UnityEngine;

public class TextureAnimator : MonoBehaviour
{
    public Sprite[] animationFrames; // Assign your PNG sprites in the inspector
    public float frameRate = 60f; // Frames per second
    public Material targetMaterial; // The material of your 3D object

    private int currentFrame;
    private float timer;

    void Update()
    {
        if (animationFrames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % animationFrames.Length;

            // Update the material's texture with the current sprite
            targetMaterial.mainTexture = animationFrames[currentFrame].texture;
        }
    }
}
