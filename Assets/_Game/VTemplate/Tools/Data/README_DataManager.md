# Data Manager

## Description

The **DataManager** is a simple way to store any kind of data you want and have it **serialized** on demand.\
The interface used is **IDataManager** and the implementation is **DataManagerImpl**.

## Usage

To use a DataManager, you must create one somewhere (in a Graph for example) and give it a file name, it will automatically load the data if there is a file with that file name :

```plaintext
IDataManager _dataManager;
_dataManager = new DataManagerImpl("my_file_name");
```

All datas are contained inside a **Dictionary<string, object>**, meaning you have to know which type of data was stored for a given key to convert it into the correct type. **Be careful when converting certain C# native types like float, don't cast them with (float), use System.Convert.ToSingle() instead.**

You have access to a few methods to use the DataManager :

```plaintext
bool HasData(string key);
object GetData(string key);
void SetData(string key, object data);
void RemoveData(string key);
void SaveData();
void DeleteSave();
```

* **HasData** is used to know if the data exists for a given key

  ```plaintext
  object data;
  if (_dataManager.HasData(key))
     data = _dataManager.GetData("key");
  ```
* **GetData** returns the data for a given key, will return an Exception if there is no data for that key

  ```plaintext
  float levelId = Convert.ToSingle(_dataManager.GetData("level_id"));
  ```
* **SetData** modifies the data for a given key if there is already a data for that key or it creates a new data if there was no data for that key

  ```plaintext
  float levelId = 1;
  _dataManager.SetData("level_id", levelId);
  ```
* **RemoveData** removes the data for a given key, will return an Exception if there is no data for that key

  ```
  _dataManager.RemoveData("data_to_remove");
  ```
* **SaveData** serializes the Dictionary<string, object> as a json file on the PersistentDataPath. **You must call SaveData to save the file, there is no automatic saving done by the template**

  ```plaintext
  _dataManager.SaveData();
  ```
* **DeleteSave** deletes the json file

  ```plaintext
  _dataManager.DeleteSave();
  ```

## Tips

You can **create multiple DataManagers** to handle different types of data (useful if you want one system to manage one DataManager on its own without having to look for it somewhere) but you can also **use only one DataManager** that contains all datas of your game and that is accessible from anywhere.

For example, in the PrototypeTemplate, the [ProtoTimeManager](../../Essentials/GameCoordinator/README_GameCoordinator.md) has its own DataManager that contains only 2 floats.\
The [HapticManager](../Haptics/README_Haptics.md) also has its own DataManager only containing one boolean so far. \
On the other hand, the [Currency](../Currency/README_Currency.md) and the [Perk](../Perks/README_Perks.md) system both requires to have a DataManager given to them at Init(), they do not have one of their own.