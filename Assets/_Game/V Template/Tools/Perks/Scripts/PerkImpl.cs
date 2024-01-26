using System;


namespace VTemplate
{
    public class PerkImpl : IPerk
    {
        public event PerkLevelEvent OnPerkLevelChanged;

        public string PerkName { get; }
        public float BaseValue { get; }
        public float ValuePerLevel { get; }
        public int BaseCost { get; }
        public int CostPerLevel { get; }
        public int MaxLevel { get; }
        public int Level { get; private set; }
        public float Value => BaseValue + (Level * ValuePerLevel);
        public int Cost => BaseCost + (Level * CostPerLevel);

        private string LevelKey => $"Perks_{PerkName.ToLower()}_level";

        private IDataManager _dataManager;



        public PerkImpl(string perkName, float baseValue, float valuePerLevel, int baseCost, int costPerLevel, int maxLevel)
        {
            Precondition.CheckNotNull(perkName);
            Precondition.CheckNotNull(baseValue);
            Precondition.CheckNotNull(valuePerLevel);
            Precondition.CheckNotNull(baseCost);
            Precondition.CheckNotNull(costPerLevel);
            Precondition.CheckNotNull(maxLevel);

            PerkName = perkName;
            BaseValue = baseValue;
            ValuePerLevel = valuePerLevel;
            BaseCost = baseCost;
            CostPerLevel = costPerLevel;
            MaxLevel = maxLevel;
        }



        public void Init(IDataManager dataManager = null)
        {
            _dataManager = dataManager;
            SetLevelFromDataManager();
        }

        public void SetLevel(int level)
        {
            if (level < 0)
                throw new Exception($"Cannot set a negative level for the perk {PerkName}");

            Level = level;
            if (IsMaxLevel())
                Level = MaxLevel;
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