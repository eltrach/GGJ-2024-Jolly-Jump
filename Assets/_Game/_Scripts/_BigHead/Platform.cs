using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 axe;
    [Range(0.3f, 25f)]
    public float distance = 1.5f;
    [Range(0.3f, 15f)]
    public float speed = 1.3f;

    Vector3 _startPosition;
    internal Vector3 moveDt;

    private void Awake()
    {
        _startPosition = transform.position;
        tag = "Platform";
    }

    private void Update()
    {
        Vector3 newPosition = _startPosition + axe * (Mathf.Cos(Time.time * speed / distance) * distance);
        moveDt = (newPosition - transform.position);
        transform.position = newPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 p1 = transform.position + axe * distance;
        Vector3 p2 = transform.position - axe * distance;
        Gizmos.DrawSphere(p1, 0.5f);
        Gizmos.DrawSphere(p2, 0.5f);
        Gizmos.DrawLine(p1, p2);
    }
}
