using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip loopSound;
    void Start()
    {
        AudioSource loop = Managers.AudioManager.CreateAudioSource();
        loop.loop = true;
        loop.clip = loopSound;
        loop.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
