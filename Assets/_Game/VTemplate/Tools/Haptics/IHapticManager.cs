#if false

using Lofelt.NiceVibrations;

namespace VTemplate.Haptics
{
    public interface IHapticManager
    {
        /// <summary>
        /// Did the user agree to have haptics ?
        /// </summary>
        bool IsHapticOn { get; }

        /// <summary>
        /// Call when the users asks to have haptics
        /// </summary>
        void EnableHaptic();
        /// <summary>
        /// Call when the users asks not to have haptics
        /// </summary>
        void DisableHaptic();
        /// <summary>
        /// Triggers an haptic vibration
        /// </summary>
        /// <param name="hapticType">HapticType from NiceVibration</param>
        void Haptic(HapticPatterns.PresetType hapticType);
        /// <summary>
        /// Trigger a continuous haptic vibration
        /// </summary>
        void ContinuousHaptic(float intensity, float sharpness, float duration);
    }
}
#endif