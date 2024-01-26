using Sirenix.OdinInspector;
using UnityEngine;
using VTemplate.Shooter;

public class Weapon : IWeaponBase
{

    [TabGroup("Customize")] public bool isLeftWeapon = false;
    [TabGroup("Customize")] public bool chargeWeapon = false;
    [TabGroup("Customize")] public bool autoShotOnFinishCharge = false;
    [TabGroup("Customize")] public float chargeSpeed = 0.1f;
    [TabGroup("Customize")] public float chargeDamageMultiplier = 2;
    [TabGroup("Customize")] public bool changeVelocityByCharge = true;
    [TabGroup("Customize")] public float chargeVelocityMultiplier = 2;
    [TabGroup("Customize")] public bool automaticWeapon;
                         
    [TabGroup("Customize")] public float reloadTime = 1f;
    [TabGroup("Customize")] public bool reloadOneByOne;
    [TabGroup("Customize")] public int clipSize;
    [TabGroup("Customize")] public bool dontUseReload;
    [TabGroup("Customize"), Tooltip("Automatically reload the weapon when it's empty")] public bool autoReload;

    [TabGroup("Audio")]
    public AudioSource reloadSource;
    [TabGroup("Audio")]
    public AudioClip reloadClip;
    [TabGroup("Audio")] public AudioClip finishReloadClip;

    //right
    [TabGroup("IK")] public Transform rightHandIKTarget;
    [TabGroup("IK")] public Transform LeftHandIKTarget;

    [TabGroup("IK")] public Transform rightHandIKHint;
    [TabGroup("IK")] public Transform LeftHandIKHint;





#if false
    [Title("Right Hand - Position")]
    [TabGroup("IK")] public Vector3 rightHandIKTargetPosition;
    [TabGroup("IK")] public Vector3 rightHandIKHintPosition;
    [Title("Right Hand - Rotation")]
    [TabGroup("IK")] public Vector3 rightHandIKTargetEulerAngles;
    [TabGroup("IK")] public Vector3 rightHandIKHintRotation;

    // left
    [Title("Left Hand - Position")]
    [TabGroup("IK")] public Vector3 leftHandIKTargetPosition;
    [TabGroup("IK")] public Vector3 leftHandIKHintPosition;
    [Title("Left Hand - Rotation")]
    [TabGroup("IK")] public Vector3 leftHandIKTargetRotation;
    [TabGroup("IK")] public Vector3 leftHandIKHintRotation;
#endif
    private void Start()
    {
        projectile.GetComponent<BulletProjectile>().velocity = velocity;
    }

    public virtual void ReloadEffect()
    {
        if (reloadSource && reloadClip)
        {
            reloadSource.Stop();
            reloadSource.PlayOneShot(reloadClip);
        }
    }

    public virtual void FinishReloadEffect()
    {
        if (reloadSource && finishReloadClip)
        {
            reloadSource.Stop();
            reloadSource.PlayOneShot(finishReloadClip);
        }
    }

}
