using System;


namespace VTemplate
{
    public class CurrencyImpl : ICurrency
    {
        public event CurrencyEvent OnValueChanged;
        public string CurrencyName { get; }
        public int Value { get; private set; }
        private string CurrencyKey => $"Currency_{CurrencyName}_value";
        private IDataManager _dataManager;
        public CurrencyImpl (string currencyName)
        {
            Precondition.CheckNotNull(currencyName);
            CurrencyName = currencyName;
        }
        public void Init (IDataManager dataManager = null)
        {
            _dataManager = dataManager;
            Value = GetCurrencyFromDataManager();
        }
        public void SetCurrency (int value)
        {
            Value = value;
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