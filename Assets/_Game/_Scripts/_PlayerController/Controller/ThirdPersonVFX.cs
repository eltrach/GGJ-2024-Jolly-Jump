using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _bloodFX;
    [SerializeField] private ParticleSystem _healFX;


    public void PlayDamageFX() { if (_bloodFX) _bloodFX.Emit(10); }
    public void PlayHealFX() { if (_healFX) _healFX.Emit(10); }
}
