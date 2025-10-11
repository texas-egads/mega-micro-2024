using UnityEngine;

public class BudioManager : MonoBehaviour
{
    public static BudioManager Instance;

    public AudioSource musicSource;
    public AudioSource source;
    public AudioClip[] roundStart; // round start, fight, KO
    public AudioClip[] attackSounds; // light, heavy
    public AudioClip[] moveSounds; // step, jump, land

    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        musicSource.pitch = Time.timeScale;
    }

    public void PlayRoundStart(int i)
    {
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(roundStart[i]);
    }
    public void PlayAttack(int i)
    {
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(attackSounds[i]);
    }
    public void PlayMove(int i)
    {
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(moveSounds[i]);
    }
}
