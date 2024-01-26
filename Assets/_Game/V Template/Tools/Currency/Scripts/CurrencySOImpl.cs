using System;
using UnityEngine;
using Sirenix.OdinInspector;


namespace VTemplate
{
    [CreateAssetMenu(fileName = "Currency", menuName = "V Template/Tools/Currency", order = 0)]
    public class CurrencySOImpl : ScriptableObject, ICurrency
    {
        public event CurrencyEvent OnValueChanged;
        public string CurrencyName => _currencyName;
        public int Value => _value;
        
        private string CurrencyKey => $"Currency_{CurrencyName}_value";

        [SerializeField, Required, BoxGroup("Currency")] private string _currencyName;
        [SerializeField, BoxGroup("Currency"), ReadOnly] private int _value;
        
        private IDataManager _dataManager;

        public void Init (IDataManager dataManager = null)
        {
            _dataManager = dataManager;
            _value = GetCurrencyFromDataManager();
        }

        [Button(Expanded = true), BoxGroup("Currency")]
        public void SetCurrency (int value)
        {
            _value = value;
            OnValueChanged?.Invoke(Value);
            _dataManager?.SetData(CurrencyKey, Value);
        }

        public void GainCurrency (int gain)
        {
            SetCurrency(Value + gain);
        }

        public bool SpendCurrency (int spend)
        {
            if (!CanSpendCurrency(spend))
                return false;
            SetCurrency(Value - spend);
            return true;
        }

        public bool CanSpendCurrency (int spend)
        {
            return Value >= spend;
        }

        public void Reset ()
        {
            SetCurrency(0);
        }

        private int GetCurrencyFromDataManager ()
        {
            if (_dataManager == null)
                return 0;
            return _dataManager.HasData(CurrencyKey) ? Convert.ToInt32(_dataManager.GetData(CurrencyKey)) : 0;
        }
    }
}