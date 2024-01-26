# Currency

## Description

The **Currency** is a simple in game currency system that you can use. You can create as many currencies as you want. \
The interface used is **ICurrency**, the C# implementation is **CurrencyImpl** and the ScriptableObject implementation is **CurrencySOImpl**.

## Usage

### CurrencyImpl

CurrencyImpl is a native C# class implementing ICurrency, managed only via code. You can create a currency using :

```plaintext
ICurrency currency = new CurrencyImpl("currency_name");
```

### CurrencySOImpl

CurrencySOImpl is a scriptable object implementation of ICurrency. As a scriptable object, you can modify it directly in the editor without changing your scripts. You can create one inside your project with **Right Click -> Create -> Prototype Template -> Tools -> Currency**. You can then drag that currency wherever you need to have it referenced.

As ICurrency is not a MonoBehaviour, you cannot drag the scriptable object in an ICurrency field, you need to drag it inside a CurrencySOImpl field instead. Then, I would recommand to use the currency through its implementation, for example with a property :

```plaintext
[SerializeField] CurrencySOImpl _currency;

public ICurrency Currency => _currency; 
```

### ICurrency

Once your currency is created, you need to Init() before using it :

```plaintext
Currency.Init();
```

If you want to store the currency, you can pass him a IDataManager in Init(), it will only be in RAM without an IDataManager, in disk with IDataManager :

```plaintext
IDataManager dataManager;
Currency.Init(dataManager);
```

Then, there are a few methods that you can use :

```plaintext
event CurrencyEvent OnValueChanged;

string CurrencyName { get; }
int Value { get; }

void Init(IDataManager dataManager = null);
void SetCurrency(int value);
void GainCurrency(int gain);
bool SpendCurrency(int spend);
bool CanSpendCurrency(int spend);
void Reset();
```

* **OnValueChanged** is an event that you can subscribe to, it's called every time the value has changed, giving the new value as parameter
* **CurrencyName** represents the name that you can use to display the currency, it is also used as key with the IDataManager, be careful to use different currency names if you use multiple currencies
* **Value** represents the amount of currency at disposal
* **Init** is used to setup the currency, it must be called before using the currency\
  You can give it a IDataManager to store the Value inside with the CurrencyName as key
* **SetCurrency** is used to overwrite the currency's value to the value given in parameter
* **GainCurrency** is used to add currency to the current value
* **SpendCurrency** is used to remove currency to the current value, it returns true if the operation is a success
* **CanSpendCurrency** is used to see if there is enough currency to spend the value given in parameter
* **Reset** is used to reset the currency to zero