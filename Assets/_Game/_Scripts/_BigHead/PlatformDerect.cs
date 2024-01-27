using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDerect : MonoBehaviour
{
    Platform _platform;
    CharacterController _characterController;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var collider = hit.collider;
        if (collider.CompareTag("Platform"))
        {
            _platform = collider.GetComponent<Platform>();
            _platform.OnTargetEnter(_characterController);
        }
        else
        {
            if (_platform)
            {
                _platform.OnTargetExit(_characterController);
                _platform = null;
            }
        }
    }
}
