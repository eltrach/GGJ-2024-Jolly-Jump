using System.Collections.Generic;
using UnityEngine;

public class SmileySpawnSys : MonoBehaviour
{
    [SerializeField] Smiley _smilyGameObjGenerator;
    [SerializeField] float _smilySpawnTimer = 0.6f;
    [SerializeField] float _yOffset = 0.6f;

    SmileySpanPoint[] _spawnPosition;

    float _spawnTime;

    List<Smiley> _spawnedObjects = new();

    private void Start()
    {
        _spawnPosition = GetComponentsInChildren<SmileySpanPoint>(true);
    }

    private void Update()
    {
        if (_spawnTime < Time.time)
        {
            _spawnTime = Time.time + _smilySpawnTimer;

            SmileySpanPoint smileySpan = _spawnPosition[Random.Range(0, _spawnPosition.Length)];
            if (smileySpan.IsFree)
            {
                Transform targetP = smileySpan.transform;
                Vector3 spawnPosition = targetP.position;
                spawnPosition.y += _yOffset;

                Smiley obj = Instantiate(_smilyGameObjGenerator, spawnPosition, transform.rotation, transform);
                _spawnedObjects.Add(obj);

                smileySpan.smileyObj = obj;

                if (_spawnedObjects.Count > 5)
                {
                    var toDestroy = _spawnedObjects[0];
                    if (toDestroy && toDestroy.canCollect)
                        toDestroy.Hide();
                    _spawnedObjects.RemoveAt(0);
                }
            }
        }
    }
}
