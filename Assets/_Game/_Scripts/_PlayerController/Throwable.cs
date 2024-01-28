using UnityEngine;

public class Throwable : MonoBehaviour
{
    public ParticleSystem collideParticlePrefab;
    public float damage;

    public FadeOut fadeOut;

    private void Start()
    {
        fadeOut = GetComponent<FadeOut>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (damage == 0)
            return;
        if (collision.transform.CompareTag("BigHead"))
        {
            BigHead bigHead = collision.transform.GetComponentInParent<BigHead>();
            bigHead.Hit(damage);

            if (collideParticlePrefab != null)
                Instantiate(collideParticlePrefab,
                    collision.contacts[0].point,
                    Quaternion.identity);

            damage = 0;
            Destroy(gameObject);
        }
        else 
            fadeOut.FadeOutStart();

        AudioManager.Play("Put");
    }
}
