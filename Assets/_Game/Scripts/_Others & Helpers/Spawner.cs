using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private BoxCollider _boxCollider;
    [SerializeField] private float _frequoece = 0.2f;
    [SerializeField] private GameObject[] _spawnObjects;
    [SerializeField] private List<GameObject> _spawnedObj;
    float _timer;
    bool _startSpawner;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }
    public void Init()
    {
        _startSpawner = true;
    }

    void Update()
    {
        if (_startSpawner)
        {
            if (Time.time > _timer)
            {
                _timer = Time.time + _frequoece;
                GameObject tragertObj = _spawnObjects[Random.Range(0, _spawnObjects.Length)];
                GameObject spawned = Instantiate(tragertObj,transform);
                spawned.transform.localPosition = GetRandomPositionFromCollider()*0.5f;
            }
        }
    }
    Vector3 GetRandomPositionFromCollider()
    {
        Bounds bounds = _boxCollider.bounds;
        return new Vector3(
            Random.Range(-bounds.extents.x , bounds.extents.x),
            Random.Range(-bounds.extents.y, bounds.extents.y),
            Random.Range(-bounds.extents.z, bounds.extents.z)
        );

    }
    public void Stopped()
    {
        Debug.Log("Stopped()" + _startSpawner);
        _startSpawner = false;
        DestroyAllChildren();
        //for (int i = transform.childCount - 1; i >= 0; i--)
        //{
        //    Destroy(transform.GetChild(i).gameObject);
        //}
        //foreach (Transform transform in transform)
        //{
        //    Destroy(transform.gameObject);
        //}
    }
    void DestroyAllChildren()
    {
        // Get the transform of the current GameObject
        Transform transform = gameObject.transform;

        // Loop through all the children of the current GameObject
        for (int i = 0; i < transform.childCount; i++)
        {
            // Destroy the child GameObject
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
