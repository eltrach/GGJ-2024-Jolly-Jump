using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void Awake()
    {
        if (GetComponent<MeshRenderer>())
            Destroy(GetComponent<MeshRenderer>());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GlobalRoot.Instance.ReloadLevel();
            AudioManager.Play("Laff" + Random.Range(1, 3));
        }
    }



}
