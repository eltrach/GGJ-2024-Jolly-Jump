namespace VTemplate
{
    public delegate void DataManagerSet(string key, object data);
    
    
    public interface IDataManager
    {
        event DataManagerSet OnDataSet;
        
        /// <summary>
        /// Is there a data for the given key ?
        /// </summary>
        /// <param name="key">The key to retrieve data from</param>
        /// <returns>True if theres a data, false if not</returns>
        bool HasData(string key);
        /// <summary>
        /// Returns the data for the given key
        /// Throws an exception if there is no data
        /// </summary>
        /// <param name="key">The key to retrieve data from</param>
        /// <returns>The data as object</returns>
        object GetData(string key);
        /// <summary>
        /// Modifies the data for the given key if there is already a data for that key
        /// Creates a new data if there was no data for that key
        /// </summary>
        /// <param name="key">The key to set data</param>
        /// <param name="data">The data to set</param>
        void SetData(string key, object data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void RemoveData(string key);
        /// <summary>
        /// Serializes the dictionary
        /// </summary>
        void SaveData();
        /// <summary>
        /// Delete the file saved
        /// </summary>
        void DeleteSave();

        string GetDataJson(string key);

        string GetString(string key);

        T GetObject<T>(string key);
    }
}