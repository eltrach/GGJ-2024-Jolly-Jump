using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;


public class LookAt : MonoBehaviour
{

    [Title("You must tag the player as !! Player !!")]
    [Tooltip("make sure that there is only one player taged in the scene")]
    [SerializeField] private bool lookAtPlayer = true;
    [SerializeField] Transform targetObject;

    [SerializeField] private float lookAtSpeed = 5f;


    private void Awake() 
    {
        if (lookAtPlayer)
        {
            targetObject = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }


    private void Update() 
    {
        // Quaternion targetRotation = Quaternion.LookRotation(targetObject.position - transform.position);
        // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lookAtSpeed);
        transform.LookAt(targetObject);
        
    }
}
