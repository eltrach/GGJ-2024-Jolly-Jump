using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    private Spell spell;

    private Vector3 targetPosition;
    public float speed = 10f;
    public float lifetime = 2f;

    private float timer;

    public void Initialize(Spell spell, Vector3 targetPosition)
    {
        this.spell = spell;
        this.targetPosition = targetPosition;
        // Customize the behavior of the spell projectile based on the spellSO properties
    }

    private void Start() => timer = lifetime;

    private void Update()
    {
        // Move the spell projectile towards the target position
        Vector3 direction = targetPosition - transform.position;
        transform.Translate(translation: speed * Time.deltaTime * direction.normalized, relativeTo: Space.World);

        // Update the timer 
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Destroy the spell projectile when its lifetime expires
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Spawn a decal at the hit position
        if (spell.decalPrefab != null)
        {
            Instantiate(spell.decalPrefab, transform.position, Quaternion.LookRotation(other.transform.position - transform.position));
        }
        // Destroy the spell projectile on collision
        Destroy(gameObject);
    }
}
