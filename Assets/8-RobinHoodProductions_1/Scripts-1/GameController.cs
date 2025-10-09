using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace RobinHoodProductions_1
{
    public class GameController : MonoBehaviour
    {
        public GameObject[] wires;
        public float radius, delay;
        public Animator animator, anim;
        public AudioClip winSound;
        float timer = 0;
        public List<GameObject> WireActives = new List<GameObject>();
        bool gameOver = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            WireActives = WireStatus();

            if (WireActives.Count <= 0 && gameOver == false && timer >= delay)
            {
                animator.SetBool("Win", true);
                anim.SetBool("Won", true);
                AudioSource win = Managers.AudioManager.CreateAudioSource();
                win.PlayOneShot(winSound);
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame(1f);
                gameOver = true;
            }

            timer += Time.deltaTime;
        }

        public List<GameObject> getWireList()
        {
            return WireActives;
        }

        List<GameObject> WireStatus()
        {
            List<GameObject> ActiveWires = new List<GameObject>();
            foreach (GameObject wire in wires)
            {
                bool active = wire.GetComponent<Wire>().Active;
                if (active)
                {
                    ActiveWires.Add(wire);
                }

            }
            return ActiveWires;
        }
    }
}
