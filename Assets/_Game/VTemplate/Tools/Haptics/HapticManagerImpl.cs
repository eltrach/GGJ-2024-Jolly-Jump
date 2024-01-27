// Uses a package (NiceVibrations)
#if false

using Lofelt.NiceVibrations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VTemplate.Haptics
{
    public class HapticManagerImpl : IHapticManager
    {
        private const string HapticsDataFileName = "haptics_data";
        private const string HapticsOnKey = "haptics_on";

        public bool IsHapticOn => _isHapticOn;
        private readonly IDataManager _hapticDataManager;
        private readonly bool _iOSEnabled;
        private readonly bool _androidEnabled;
        private readonly float _minDelayBetweenHaptics;
        private readonly Dictionary<HapticPatterns.PresetType, float> _hapticTimers;
        private bool _isHapticOn;
        private float _continuousHapticTime;

        

        public HapticManagerImpl(bool iOSEnabled, bool androidEnabled, float minDelayBetweenHapticsIos, float minDelayBetweenHapticsAndroid)
        {
            _hapticDataManager = new DataManagerImpl(HapticsDataFileName);
            SetHapticFromDataManager();
            _iOSEnabled = iOSEnabled;
            _androidEnabled = androidEnabled;
            _minDelayBetweenHaptics = minDelayBetweenHapticsIos;
            _hapticTimers = new Dictionary<HapticPatterns.PresetType, float>();
            _continuousHapticTime = 0;
        }


        public void EnableHaptic()
        {
            _isHapticOn = true;
            _hapticDataManager.SetData(HapticsOnKey, _isHapticOn);
            _hapticDataManager.SaveData();
        }

        public void DisableHaptic()
        {
            _isHapticOn = false;
            _hapticDataManager.SetData(HapticsOnKey, _isHapticOn);
            _hapticDataManager.SaveData();
        }

        public void Haptic(HapticPatterns.PresetType hapticType)
        {
            if (!ShouldVibrate())
                return;

            if (_hapticTimers.ContainsKey(hapticType) && _hapticTimers[hapticType] + _minDelayBetweenHaptics <= Time.unscaledTime)
            {
                _hapticTimers[hapticType] = Time.unscaledTime;
                HapticPatterns.PlayPreset(ConvertHapticTypeForAndroid(hapticType));
            }
            else if (!_hapticTimers.ContainsKey(hapticType))
            {
                _hapticTimers.Add(hapticType, Time.unscaledTime);
                //Graph.Haptics.Haptic(ConvertHapticTypeForAndroid(hapticType));
            }
        }

        public void ContinuousHaptic(float intensity, float sharpness, float duration)
        {
            if (!ShouldVibrate())
                return;

            if (_continuousHapticTime + _minDelayBetweenHaptics <= Time.unscaledTime)
            {
                _continuousHapticTime = Time.unscaledTime;
                //Graph.Haptics.ContinuousHaptic(intensity, sharpness, duration);
            }
        }

        private void SetHapticFromDataManager()
        {
            if (_hapticDataManager.HasData(HapticsOnKey))
            {
                bool isHapticOn = Convert.ToBoolean(_hapticDataManager.GetData(HapticsOnKey));
                if (isHapticOn)
                    EnableHaptic();
                else
                    DisableHaptic();
                return;
            }
#if UNITY_IOS
            EnableHaptic();
#endif
#if UNITY_ANDROID
            DisableHaptic();
#endif
        }

        private bool ShouldVibrate()
        {
            if (!_isHapticOn)
                return false;
#if UNITY_IOS
            if (!_iOSEnabled)
                return false;
#endif
#if UNITY_ANDROID
            if (!_androidEnabled)
                return false;
#endif
            return true;
        }

        private HapticPatterns.PresetType ConvertHapticTypeForAndroid(HapticPatterns.PresetType hapticType)
        {

            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return hapticType;
            if (hapticType == HapticPatterns.PresetType.LightImpact || hapticType == HapticPatterns.PresetType.MediumImpact || hapticType == HapticPatterns.PresetType.HeavyImpact)
                return hapticType;
            return HapticPatterns.PresetType.LightImpact;
        }
        /// <summary>
        /// Function only used to add Vibration permissions to the AndroidManifest. Do not call that
        /// </summary>
        private void AddVibrationsToAndroidManifest()
        {
            Handheld.Vibrate();
        }
    }
}
#endif