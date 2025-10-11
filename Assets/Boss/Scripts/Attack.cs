using UnityEngine;

public class Attack : MonoBehaviour
{
    public float lifespan;
    public float damage;
    public bool friendly;
    void Start()
    {

    }

    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(friendly && collision.CompareTag("Boss"))
        {
            collision.GetComponent<Boss>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!friendly && collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
