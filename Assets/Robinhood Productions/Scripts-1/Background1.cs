using UnityEngine;

public class Background1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip loopSound;
    AudioSource loop;
    void Start()
    {
        loop =  Managers.AudioManager.CreateAudioSource();
        loop.loop = true;
        loop.clip = loopSound;
        loop.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
