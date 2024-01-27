using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileySpawnSys : MonoBehaviour
{
    [SerializeField] GameObject _smilyGameObjGenerator;
    [SerializeField] float _smilySpawnTimer = 0.6f;

    SmileySpanPoint[] _spawnPosition;

    float _spawnTime;

    List<GameObject> _spawnedObjects = new List<GameObject>();

    private void Start()
    {
        _spawnPosition = GetComponentsInChildren<SmileySpanPoint>(true);
    }

    private void Update()
    {
        if(_spawnTime < Time.time)
        {
            _spawnTime = Time.time + _smilySpawnTimer;

            SmileySpanPoint smileySpan = _spawnPosition[Random.Range(0, _spawnPosition.Length)];
            if (smileySpan.IsFree)
            {
                Transform targetP = smileySpan.transform;
                var obj = Instantiate(_smilyGameObjGenerator, targetP.position, transform.rotation, transform);
                _spawnedObjects.Add(obj);

                smileySpan.smileyObj = obj;

                if(_spawnedObjects.Count > 5)
                {
                    GameObject toDestroy = _spawnedObjects[0];
                    if(toDestroy)
                        Destroy(toDestroy);
                    _spawnedObjects.RemoveAt(0);
                }
            }
        }
    }
}
