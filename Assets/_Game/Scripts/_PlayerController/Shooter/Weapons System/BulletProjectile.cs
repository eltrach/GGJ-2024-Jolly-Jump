using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private float destroyBulletAfterXSec = 3f;
    public Transform impactDecal;
    public float velocity = 50f;
    public bool projectileDebug = false;
    public int damage = 10;
    private Rigidbody bulletRigidbody;
    private Vector3 startPos;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    private void Start()
    {
        bulletRigidbody.velocity = transform.forward * velocity;
        Destroy(gameObject, destroyBulletAfterXSec);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); return;
        }
        if (other.CompareTag("Bullet")) return;

        if (impactDecal != null) Instantiate(impactDecal, transform.position, Quaternion.identity);
        Destroy(gameObject);

        IHealthSystem hp = other.GetComponent<IHealthSystem>();
        AttackReceiver hitBox = other.GetComponent<AttackReceiver>();
        Debug.Log("Collided with ->  " + other.name);
        if (hp != null)
        {
            hp.TakeDamage(damage);
            Debug.Log(" ->  " + other.name);
        }
        else if (hitBox != null)
        {
            hitBox.OnRaycastHit(this);
        }
    }

    private void OnDrawGizmos()
    {
        if (projectileDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.1f);
            Gizmos.DrawLine(startPos, transform.position);
        }
    }
}
