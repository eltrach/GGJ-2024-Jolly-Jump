using UnityEngine;

public class Throwable : MonoBehaviour
{
    public ParticleSystem collideParticlePrefab;
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("BigHead"))
        {
            BigHead bigHead = collision.transform.GetComponentInParent<BigHead>();
            bigHead.Hit(damage);

            if (collideParticlePrefab != null)
                Instantiate(collideParticlePrefab,
                    collision.contacts[0].point,
                    Quaternion.identity);
        }
        else Destroy(gameObject);
    }
}
