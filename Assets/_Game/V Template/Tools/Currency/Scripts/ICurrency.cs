namespace VTemplate
{
    public delegate void CurrencyEvent(int value);

    
    public interface ICurrency
    {
        /// <summary>
        /// Event that you can subscribe to, it's called every time the value has changed, giving the new value as parameter
        /// </summary>
        event CurrencyEvent OnValueChanged;

        /// <summary>
        /// Represents the name that you can use to display the currency, it is also used as key with the IDataManager
        /// Be careful to use different currency names if you use multiple currencies
        /// </summary>
        string CurrencyName { get; }
        /// <summary>
        /// Represents the amount of currency at disposal
        /// </summary>
        int Value { get; }

        /// <summary>
        /// Used to setup the currency, it must be called before using the currency
        /// You can give it a IDataManager to store the Value inside with the CurrencyName as key
        /// It will only be in RAM without an IDataManager, in disk with IDataManager
        /// </summary>
        void Init(IDataManager dataManager = null);
        /// <summary>
        /// Used to overwrite the currency's value to the value given in parameter
        /// </summary>
        /// <param name="value">New currency value</param>
        void SetCurrency(int value);
        /// <summary>
        /// Used to add currency to the current value
        /// </summary>
        /// <param name="gain">Currency gain</param>
        void GainCurrency(int gain);
        /// <summary>
        /// Used to remove currency to the current value
        /// </summary>
        /// <param name="spend">Currency to spend</param>
        /// <returns>Returns true if the operation is a success</returns>
        bool SpendCurrency(int spend);
        /// <summary>
        /// Used to see if there is enough currency to spend the value given in parameter
        /// </summary>
        /// <param name="spend">Currency to spend</param>
        /// <returns>Returns true if there is enough currency</returns>
        bool CanSpendCurrency(int spend);
        /// <summary>
        /// Used to reset the currency to zero
        /// </summary>
        void Reset();
    }
}