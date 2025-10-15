using UnityEngine;
using UnityEngine.UI;

public class UltimateCharge : MonoBehaviour
{
    public float appearTime;
    private Image sprite;
    private float appearTimer;
    private bool active;
    void Start()
    {
        sprite = GetComponent<Image>();
        appearTimer = appearTime;
        sprite.color = Color.clear;
    }

    void Update()
    {
        appearTimer += Time.deltaTime;
        if (active)
        {
            sprite.color = Color.Lerp(new Color(1,1,1,0), Color.white, Mathf.Clamp01(appearTimer/appearTime));
        } else
        {
            sprite.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), Mathf.Clamp01(appearTimer / appearTime));
        }
    }

    public void Activate()
    {
        active = true;
        appearTimer = 0;
    }
    public void Use()
    {
        active = false;
        appearTimer = 0;
    }
    public void Deactivate()
    {
        active = false;
        appearTimer = appearTime;
        sprite.color = Color.clear;
    }
}
