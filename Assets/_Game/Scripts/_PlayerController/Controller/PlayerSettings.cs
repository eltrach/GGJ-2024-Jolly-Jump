using UnityEngine;

namespace VTemplate.Input
{
    [CreateAssetMenu(menuName = "Player/Movements/Settings")]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField] private float _walkSpeed = 2f;
        [SerializeField] private float _runSpeed = 6f;
        [SerializeField] private float _jumpForce = 13f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _speedMultiplier = 100f;

        public float WalkSpeed { get => _walkSpeed; set => _walkSpeed = value; }
        public float RunSpeed { get => _runSpeed; set => _runSpeed = value; }
        public float JumpForce { get => _jumpForce; set => _jumpForce = value; }
        public float Gravity { get => _gravity; set => _gravity = value; }
        public float SpeedMultiplier { get => _speedMultiplier; set => _speedMultiplier = value; }
    }
}