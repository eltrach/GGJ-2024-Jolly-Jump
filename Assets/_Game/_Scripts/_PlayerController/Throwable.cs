using UnityEngine;

public class Throwable : MonoBehaviour
{
    public ParticleSystem CollideParticle;
    public float damage;



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("BigHead"))
        {
            BigHead bigHead = collision.transform.GetComponentInParent<BigHead>();
            bigHead.Hit(damage);
        }

    }
}
