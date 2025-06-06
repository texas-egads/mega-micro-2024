using UnityEngine;

namespace RobinHoodProductions_1
{
    public class Background1 : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public AudioClip loopSound1, loopSound2;
        AudioSource loop1, loop2;
        void Start()
        {
            loop1 = Managers.AudioManager.CreateAudioSource();
            loop1.loop = true;
            loop1.clip = loopSound1;
            loop1.Play();

            loop2 = Managers.AudioManager.CreateAudioSource();
            loop2.loop = true;
            loop2.clip = loopSound2;
            loop2.Play();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
