using UnityEngine;
namespace RobinHoodProductions_0
{
    public class Background : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public AudioClip loopSound;
        public GameObject Arrow;
        bool Hit;
        AudioSource loop;
        void Start()
        {
            loop = Managers.AudioManager.CreateAudioSource();
            loop.loop = true;
            loop.clip = loopSound;
            loop.Play();
        }

        // Update is called once per frame
        void Update()
        {
            Hit = Arrow.GetComponent<MovingArrow>().hit;
            if (Hit)
            {
                loop.loop = false;
                loop.Stop();
            }
        }
    }
}
