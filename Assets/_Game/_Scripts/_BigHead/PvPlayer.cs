using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvPlayer : MonoBehaviour
{

    [SerializeField] private GameObject _player;

    private void Start()
    {
        Instantiate(_player, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }
}
