using UnityEngine;

namespace Return0 
{
    public class Ball : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("<color=yellow>collision detected");

            if (collision.gameObject.GetComponentInParent<Batter>())
            {
                Debug.Log("Batter Found");
                Batter batter = collision.gameObject.GetComponentInParent<Batter>();
                batter.OnBallHit();
            }
            else
            {
                Debug.Log("<color=red>Batter not found</color>");
            }
        }
    }
}


