using UnityEngine;

public class Smiley : MonoBehaviour
{
    [SerializeField] GameObject[] _emojisType;
    [SerializeField] int value = 1;

    int _indexImoji;

    private void Start()
    {
        foreach (GameObject emoji in _emojisType)
            emoji.SetActive(false);

        _indexImoji = Random.Range(0, _emojisType.Length);
        _emojisType[_indexImoji].SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        PlayerRoot.ThirdPersonAiming.IncreaseShots(value);

    }
}
