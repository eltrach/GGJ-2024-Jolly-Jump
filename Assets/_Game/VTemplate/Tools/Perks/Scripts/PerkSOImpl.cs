using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace VTemplate
{
    [CreateAssetMenu(fileName = "Perk", menuName = "Prototype Template/Tools/Perk", order = 1)]
    public class PerkSOImpl : ScriptableObject, IPerk
    {
        public event PerkLevelEvent OnPerkLevelChanged;

        [SerializeField, Required, BoxGroup("Perk")] private string _perkName;
        [SerializeField, BoxGroup("Perk")] private float _baseValue;
        [SerializeField, BoxGroup("Perk")] private float _valuePerLevel;
        [SerializeField, BoxGroup("Perk")] private int _baseCost;
        [SerializeField, BoxGroup("Perk")] private int _costPerLevel;
        [SerializeField, BoxGroup("Perk")] private int _maxLevel;
        [SerializeField, BoxGroup("Perk"), ReadOnly] private int _level;

        public string PerkName => _perkName;
        public float BaseValue => _baseValue;
        public float ValuePerLevel => _valuePerLevel;
        public int BaseCost => _baseCost;
        public int CostPerLevel => _costPerLevel;
        public int MaxLevel => _maxLevel;
        public int Level => _level;
        public float Value => BaseValue + (Level * ValuePerLevel);
        public int Cost => BaseCost + (Level * CostPerLevel);
        private string LevelKey => $"{PerkName.ToLower()}_level";

        private IDataManager _dataManager;
        public void Init(IDataManager dataManager = null)
        {
            _dataManager = dataManager;
            SetLevelFromDataManager();
        }

        [Button(Expanded = true), BoxGroup("Values")]
        public void SetLevel(int level)
        {
            if (level < 0)
                throw new Exception($"Cannot set a negative level for the perk {PerkName}");

            _level = level;
            if (IsMaxLevel())
                _level = MaxLevel;
            OnPerkLevelChanged?.Invoke(Level);
            _dataManager?.SetData(LevelKey, Level);
        }

        public void LevelUp()
        {
            SetLevel(Level + 1);
        }

        public void Reset()
        {
            SetLevel(0);
        }

        public bool IsMaxLevel()
        {
            return Level >= MaxLevel;
        }

        private void SetLevelFromDataManager()
        {
            if (_dataManager != null && _dataManager.HasData(LevelKey))
                SetLevel(Convert.ToInt32(_dataManager.GetData(LevelKey)));
            else
                Reset();
        }
    }
}