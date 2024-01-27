# Perks

## Description

The Perk system allows you to create a variable that can be purchased and increased by the player (in an incremental game for example). \
The interface used is **IPerk**, the C# implementation is **PerkImpl** and the ScriptableObject implementation is **PerkSOImpl**.

## Usage

### PerkImpl

PerkImpl is a native C# class implementing IPerk, managed only via code. You can create a perk by giving it 6 parameters using :

```plaintext
IPerk perk = new PerkImpl(perkName, baseValue, valuePerLevel, baseCost, costPerLevel, maxLevel);
```

### PerkSOImpl

PerkSOImpl is a scriptable object implementation of IPerk. As a scriptable object, you can modify it directly in the editor without changing your scripts. You can create one inside your project with **Right Click -> Create -> Prototype Template -> Tools -> Perk**. You can then drag that currency wherever you need to have it referenced.

As IPerk is not a MonoBehaviour, you cannot drag the scriptable object in an IPerk field, you need to drag it inside a PerkSOImpl field instead. Then, I would recommand to use the currency through its implementation, for example with a property :

```
[SerializeField] PerkSOImpl _perk;

public IPerk Perk => _perk;
```

### IPerk

Once your perk is created, you need to Init() before using it :

```plaintext
Perk.Init();
```

If you want to store the currency, you can pass him a IDataManager in Init(), it will only be in RAM without an IDataManager, in disk with IDataManager:

```plaintext
IDataManager dataManager;
Perk.Init(dataManager);
```

Then, there are a few methods that you can use :

```
event PerkLevelEvent OnPerkLevelChanged;
        
string PerkName { get; }
float BaseValue { get; }
float ValuePerLevel { get; }
int BaseCost { get; }
int CostPerLevel { get; }
int MaxLevel { get; }
int Level { get; }
float Value { get; }
int Cost { get; }

void Init(IDataManager dataManager = null);
void SetLevel(int level);
void LevelUp();
void Reset();
bool IsMaxLevel();
```

* **OnPerkLevelChanged** is an event you can subscribe to, it's called every time the value has changed, giving the new value as parameter
* **PerkName** represents the name that you can use to display the perk, it is also used as key with the IDataManager, be careful to use different perk names if you use multiple perks
* **BaseValue** represents the value at level 0
* **ValuePerLevel** represents the value that you add for each level
* **BaseCost** represents the cost at level 0
* **CostPerLevel** represents the cost that you add for each level
* **MaxLevel** represents the maximum level the perk can reach
* **Level** represents the current perk level
* **Value** represents the value of the current level
* **Cost** represents the cost of the current level
* **Init** is used to setup the perk, it must be called before using the perk\
  You can give it a IDataManager to store the Value inside with the PerkName as key
* **SetLevel** is used to overwrite the perk's value to the value given in parameter
* **LevelUp** is used to increase the level by 1 if possible
* **Reset** is used to reset the perk at level 0
* **IsMaxLevel** is used to determine whether or not the perk has reached max level