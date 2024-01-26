using UnityEngine;

public class HitBox : MonoBehaviour
{
    protected Collider _trigger;
    public AttackReceiver attackObject;

    public Collider trigger
    {
        get
        {
            _trigger = gameObject.GetComponent<Collider>();

            if (!_trigger) _trigger = gameObject.AddComponent<BoxCollider>();
            return _trigger;
        }
    }
    private void Start()
    {
        if (!_trigger) _trigger = gameObject.AddComponent<BoxCollider>();
        if(attackObject == null) attackObject = GetComponentInParent<AttackReceiver>();
    }
    void OnDrawGizmos()
    {

        Color color = Color.green;
        color.a = 0.6f;
        Gizmos.color = color;
        if (!Application.isPlaying && trigger && !trigger.enabled) trigger.enabled = true;
        if (trigger && trigger.enabled)
        {
            if (trigger as BoxCollider)
            {
                BoxCollider box = trigger as BoxCollider;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawCube(box.center, Vector3.Scale(Vector3.one, box.size));
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (attackObject == null) Debug.Log("<color=red> ATTACKOBJECT (type:AttackReceiver) IS NULL </color> ");
        Debug.Log(other);
        if (attackObject != null && (attackObject.thirdPersonMelee == null
            || other.gameObject != attackObject.thirdPersonMelee.gameObject) && other.GetComponent<HealthSystem>())
        {
            if (attackObject != null)
            {
                attackObject.OnHit(this, other);
            }
        }
    }
    bool TriggerCondictions(Collider other)
    {
        return
           attackObject != null && (attackObject.thirdPersonMelee == null || other.gameObject != attackObject.thirdPersonMelee.gameObject);
    }
}