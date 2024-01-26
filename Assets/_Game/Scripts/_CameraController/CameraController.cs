using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 100f;

    [SerializeField] private ThirdPersonInputs _inputManager;
    [SerializeField] private Transform playerBody;
    [SerializeField] private float minX = -90f;
    [SerializeField] private float maxX = 90f;
    [SerializeField] private bool isFPP = false;
    // private Unserialized
    private float _xRotation;
    private float mouseX;
    private float mouseY;

    void Awake()
    {
        _inputManager = GetComponentInParent<ThirdPersonInputs>();
    }
    void LateUpdate()
    {
        HandleInputs();
        if(isFPP) FirstPersonPerspective();
        else ThirdPersonPerspective();
    }
    private void ThirdPersonPerspective()
    {

    }
    private void FirstPersonPerspective()
    {
        _xRotation -= mouseY * _sensitivity;
        _xRotation = Mathf.Clamp(_xRotation, minX, maxX);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX * _sensitivity);
    }
    private void HandleInputs()
    {
        mouseX = _inputManager.Look.x * Time.fixedDeltaTime;
        mouseY = _inputManager.Look.y * Time.fixedDeltaTime;
    }
}
