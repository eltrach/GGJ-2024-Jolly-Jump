using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    private float rotationMultiplier = 100;
    [SerializeField] Vector3 rotationVector;

    private void Update()
    {
        transform.Rotate(rotationVector * rotationSpeed * rotationMultiplier * Time.deltaTime);
    }
}
