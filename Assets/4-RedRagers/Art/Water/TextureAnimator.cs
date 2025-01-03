using UnityEngine;

public class TextureAnimator : MonoBehaviour
{
    public Sprite[] animationFrames;
    public float frameRate = 60f;
    public Material targetMaterial;

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

            targetMaterial.mainTexture = animationFrames[currentFrame].texture;
        }
    }
}
