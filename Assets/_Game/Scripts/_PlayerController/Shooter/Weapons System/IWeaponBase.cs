using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace VTemplate.Shooter
{
    public class IWeaponBase : MonoBehaviour
    {

        [TabGroup("General")]
        [Tooltip("The category of the weapon\n Used for the IK offset system. \nExample: HandGun, Pistol, Machine-Gun")]
        public string weaponCategory = "MyCategory";

        [TabGroup("General")]
        [Tooltip("Frequency of shots")]
        public float shootFrequency;

        [TabGroup("Ammo")]
        public bool isInfinityAmmo;

        [TabGroup("Ammo"), ShowIf("isInfinityAmmo", false)]
        [Tooltip("Starting ammo")]
        public int ammo;

        [TabGroup("Ammo")]
        public List<string> ignoreTags = new List<string>();

        [TabGroup("Ammo")]
        public LayerMask hitLayer = 1 << 0;

        [TabGroup("Projectile")]
        [Tooltip("Prefab of the projectile")]
        public BulletProjectile projectile;

        [TabGroup("Projectile")]
        [Tooltip("Assign the muzzle of your weapon")]
        public Transform muzzle;

        [TabGroup("Projectile")]
        [Range(1, 20)]
        public int projectilesPerShot = 1;

        [TabGroup("Projectile")]
        [Range(0, 90)]
        [Tooltip("how much dispersion the weapon has")]
        public float dispersion = 0;

        [TabGroup("Projectile")]
        [Range(0, 1000)]
        [Tooltip("Velocity of your projectile")]
        public float velocity = 380;

        [TabGroup("Damage")]
        [Tooltip("Use the DropOffStart and DropOffEnd to calculate damage by distance using min and max damage")]
        public bool damageByDistance = true;

        [TabGroup("Damage")]
        [FormerlySerializedAs("DropOffStart")]
        public float minDamageDistance = 8f;

        [TabGroup("Damage")]
        [FormerlySerializedAs("DropOffEnd")]
        public float maxDamageDistance = 50f;

        [TabGroup("Damage")]
        public int minDamage;

        [TabGroup("Damage")]
        public int maxDamage;

        [TabGroup("Audio")]
        public AudioSource source;

        [TabGroup("Audio")]
        public AudioClip fireClip;

        [TabGroup("Audio")]
        public AudioClip emptyClip;

        [TabGroup("Effects")]
        public bool testShootEffect;

        [TabGroup("Effects")]
        public Light lightOnShot;

        [TabGroup("Effects"), SerializeField]
        public ParticleSystem[] emittShurykenParticle;

        [HideInInspector]
        public OnDestroyEvent onDestroy;

        [System.Serializable]
        public class OnDestroyEvent : UnityEvent<GameObject> { }

        protected float _nextShootTime;
        protected float _nextEmptyClipTime;

        protected Transform sender;
        #region Public Methods

        /// <summary>
        /// Apply additional velocity to the Shot projectile 
        /// </summary>
        public virtual float velocityMultiplierMod { get; set; }

        /// <summary>
        /// Apply additional damage to the projectile
        /// </summary>
        public virtual float damageMultiplierMod { get; set; }

        /// <summary>
        /// Weapon Name
        /// </summary>
        public virtual string weaponName
        {
            get
            {
                var value = gameObject.name.Replace("(Clone)", string.Empty);
                return value;
            }
        }

        /// <summary>
        /// Shoot to direction of the muzzle forward
        /// </summary>
        public virtual void Shoot()
        {
            Shoot(muzzle.position + muzzle.forward * 100f);
        }

        /// <summary>
        /// Shoot to direction of the muzzle forward
        /// </summary>
        /// <param name="sender">Sender to reference of the damage</param>
        /// <param name="successfulShot">Action to check if shoot is sucessful</param>
        public virtual void Shoot(Transform _sender = null, UnityAction<bool> successfulShot = null)
        {
            Shoot(muzzle.position + muzzle.forward * 100f, _sender, successfulShot);
        }

        /// <summary>
        /// Shoot to direction of the aim Position
        /// </summary>
        /// <param name="aimPosition">Aim position to override direction of the projectile</param>
        /// <param name="sender">ender to reference of the damage</param>
        /// <param name="successfulShot">Action to check if shoot is sucessful</param>
        public virtual void Shoot(Vector3 aimPosition, Transform _sender = null, UnityAction<bool> successfulShot = null)
        {
            if (HasAmmo())
            {
                if (!CanDoShot)
                {
                    return;
                }

                UseAmmo();
                this.sender = _sender != null ? _sender : transform;
                var dir = aimPosition - muzzle.position;
                HandleShot(dir);
                if (successfulShot != null)
                {
                    successfulShot.Invoke(true);
                }

                _nextShootTime = Time.time + shootFrequency;
                _nextEmptyClipTime = _nextShootTime;
            }
            else
            {
                if (!CanDoEmptyClip)
                {
                    return;
                }

                EmptyClipEffect();
                if (successfulShot != null)
                {
                    successfulShot.Invoke(false);
                }

                _nextEmptyClipTime = Time.time + shootFrequency;
            }
        }

        /// <summary>
        /// Check if can shoot by <seealso cref="shootFrequency"/>
        /// </summary>
        public virtual bool CanDoShot
        {
            get
            {
                bool _canShot = _nextShootTime < Time.time;
                return _canShot;
            }
        }
        /// <summary>
        /// Check if can do empty clip effect, <seealso cref="shootFrequency"/>
        /// </summary>
        public virtual bool CanDoEmptyClip
        {
            get
            {
                bool _canShot = _nextEmptyClipTime < Time.time;
                return _canShot;
            }
        }

        /// <summary>
        /// Use weapon Ammo
        /// </summary>
        /// <param name="count">count to use</param>
        public virtual void UseAmmo(int count = 1)
        {
            if (ammo <= 0)
            {
                return;
            }

            ammo -= count;
            if (ammo <= 0)
            {
                ammo = 0;
            }
        }

        /// <summary>
        /// Check if Weapon Has Ammo
        /// </summary>
        /// <returns></returns>
        public virtual bool HasAmmo()
        {

            return isInfinityAmmo || ammo > 0;
        }
        #endregion

        #region Protected Methods

        protected virtual void OnDestroy()
        {
            onDestroy.Invoke(gameObject);
        }

        private void OnApplicationQuit()
        {
            onDestroy.RemoveAllListeners();
        }
        protected virtual void HandleShot(Vector3 aimDir)
        {
            ShootBullet(aimDir);
            ShotEffect();
        }

        protected virtual Vector3 Dispersion(Vector3 aim, float distance, float variance)
        {
            aim.Normalize();
            Vector3 v3 = Vector3.zero;
            do
            {
                v3 = Random.insideUnitSphere;
            }
            while (v3 == aim || v3 == -aim);
            v3 = Vector3.Cross(aim, v3);
            v3 = v3 * Random.Range(0f, variance);
            return aim * distance + v3;
        }

        int countShootedBullets = 0;
        protected virtual void ShootBullet(Vector3 dir)
        {
            //var dir = aimPosition - muzzle.position;
            var rotation = Quaternion.LookRotation(dir);
            GameObject bulletObject = null;
            if (dispersion > 0 && projectile)
            {
                for (int i = 0; i < projectilesPerShot; i++)
                {
                    var dispersionDir = Dispersion(dir.normalized, maxDamageDistance, dispersion);
                    var spreadRotation = Quaternion.LookRotation(dispersionDir);
                    Instantiate(projectile, muzzle.transform.position, spreadRotation);
                    //Debug.Log("Shooting Multiple : " + countShootedBullets++);
                }
            }
            else if (projectilesPerShot > 0 && projectile)
            {
                bulletObject = Instantiate(projectile.gameObject, muzzle.position, Quaternion.LookRotation(dir, Vector3.up));
                //Debug.Log("Shooting Single : " + countShootedBullets++);
            }
        }
        protected virtual IEnumerator ApplyForceToBullet(GameObject bulletObject, Vector3 direction, float velocityChanged)
        {
            yield return new WaitForSeconds(0.01f);
            try
            {
                var _rigidbody = bulletObject.GetComponent<Rigidbody>();
                _rigidbody.mass = _rigidbody.mass / projectilesPerShot;//Change mass per projectiles count.

                _rigidbody.AddForce((direction.normalized * velocityChanged), ForceMode.VelocityChange);
            }
            catch
            {

            }
        }

        protected virtual float damageMultiplier
        {
            get
            {
                return 1 + damageMultiplierMod;
            }
        }
        protected virtual float velocityMultiplier
        {
            get
            {
                return 1 + velocityMultiplierMod;
            }
        }

        #region Effects
        protected virtual void ShotEffect()
        {
            StopCoroutine(LightOnShoot());
            if (source && fireClip)
            {

                source.PlayOneShot(fireClip);
            }

            StartCoroutine(LightOnShoot(0.037f));
            StartEmitters();
        }

        protected virtual void StopSound()
        {
            if (source)
            {
                source.Stop();
            }
        }

        protected virtual IEnumerator LightOnShoot(float time = 0)
        {
            if (lightOnShot)
            {
                lightOnShot.enabled = true;

                yield return new WaitForSeconds(time);
                lightOnShot.enabled = false;
            }
        }

        protected virtual void StartEmitters()
        {
            if (emittShurykenParticle != null)
            {
                foreach (ParticleSystem pe in emittShurykenParticle)
                {
                    pe.Emit(1);
                }
            }
        }

        protected virtual void StopEmitters()
        {
            if (emittShurykenParticle != null)
            {
                foreach (ParticleSystem pe in emittShurykenParticle)
                {
                    pe.Stop();
                }
            }
        }

        protected virtual void EmptyClipEffect()
        {
            if (source && emptyClip)
            {
                source.PlayOneShot(emptyClip);
            }
        }
        #endregion

        #endregion
    }
}