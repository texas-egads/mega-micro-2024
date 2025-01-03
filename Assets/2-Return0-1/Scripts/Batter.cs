using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Return0 
{
    
    public class Batter : MonoBehaviour
    {
        public float swingDelay = 0f;
        public bool canSwing;
        Animator animator;
        public GameObject hitBox;
        public List<Transform> hitBoxStances;
        [SerializeField] float cameraShakeStrenght;
        [SerializeField] float cameraShakeDelay;
        [SerializeField] int cameraShakeCycles;
        private int stanceIndex;
        [SerializeField] AudioClip baseballHit;
        [SerializeField] ParticleSystem homerunParticle;
        private bool canAim;
        [SerializeField] AudioClip baseballIntro;


        void Start()
        {
            Debug.Log("Hello!");
            canSwing = true;
            if (this.gameObject.GetComponent<Animator>()) animator = this.gameObject.GetComponent<Animator>();
            else Debug.Log("<color=red>Animator not found!</color>");
            animator.SetBool("CanSwing", true);
            SetPlayerControl();

            AudioSource organ = Managers.AudioManager.CreateAudioSource();
            if (baseballIntro) organ.PlayOneShot(baseballIntro);
            else Debug.Log("<color=red>Intro Music not Initiallized</color>");
        }

        private void Update()
        {
            float axisSpace = Input.GetAxisRaw("Space");
            float axisVert = Input.GetAxisRaw("Vertical");

            if (canSwing)
            {
                if (canAim)
                {
                    if (axisVert > 0) ChangeHitBoxStance(0);
                    else if (axisVert == 0) ChangeHitBoxStance(1);
                    else if (axisVert < 0) ChangeHitBoxStance(2);
                }
                else
                {
                    ChangeHitBoxStance(BaseballManager.pitchType);
                }


                if (axisSpace > 0 && animator)  //swing bat when space is pressed
                {
                    //Debug.Log("SPace hit");
                    animator.SetInteger("StanceIndex", stanceIndex);
                    animator.SetBool("SpacePressed", true);

                    canSwing = false;
                    StartCoroutine(StartSwingDelay());
                    
                }
            }


        }

        IEnumerator StartSwingDelay()
        {
            yield return new WaitForSeconds(0.25f);
            animator.SetBool("CanSwing", false);
            yield return new WaitForSeconds(swingDelay);
            animator.SetBool("SpacePressed", false);
            
        }

        private void ChangeHitBoxStance (int _index)
        {
            hitBox.transform.position = hitBoxStances[_index].position;
            //Debug.Log("hitBoxStances[_index].name");
            stanceIndex = _index;
        }

        //TODO: hit the ball succesfully, add camera shake!!!!
        public void OnBallHit()
        {
            Debug.Log("<color=yellow>Home Run!</color>");
            homerunParticle.Play();
            StartCoroutine(ShakeCamera());

            //store win condition
            AudioSource win = Managers.AudioManager.CreateAudioSource();
            if(baseballHit) win.PlayOneShot(baseballHit);

            Managers.MinigamesManager.DeclareCurrentMinigameWon();
            Managers.MinigamesManager.EndCurrentMinigame(1f);
        }

        private IEnumerator ShakeCamera()
        {
            //cameraTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount; //cam shake
            Vector3 originalPos = Camera.main.transform.localPosition;
            for (int i = 0; i < cameraShakeCycles; i++)
            {
                Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * cameraShakeStrenght;
                yield return new WaitForSeconds(cameraShakeDelay);
            }
            Camera.main.transform.localPosition = originalPos;
        }

        private void SetPlayerControl()
        {
            if (Managers.MinigamesManager.GetCurrentMinigameDifficulty().ToString() == "EASY")
            {
                canAim = false;
            }
            else canAim = true;
        }
    }

}

