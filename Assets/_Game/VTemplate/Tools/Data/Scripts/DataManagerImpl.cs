// required package :  install it from git => com.unity.nuget.newtonsoft-json
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace VTemplate
{
    public class DataManagerImpl : IDataManager
    {
        private const string Extension = ".data";

        private static readonly LoggerSettings LogSettings = new LoggerSettings("DataManager", new Color32(24, 230, 241, 255));

        public event DataManagerSet OnDataSet;

        public string Path => $"{Application.persistentDataPath}/{_fileName}{Extension}";

        private readonly string _fileName;
        [SerializeField]
        private Dictionary<string, object> _datas;

        private static readonly string EncryptionKey = "1278980083";
        private static readonly string IV = "56";

        public DataManagerImpl(string fileName)
        {
            Precondition.CheckNotNull(fileName);
            _fileName = fileName;
            LoadData();
        }


        public bool HasData(string key)
        {
            Precondition.CheckNotNull(key);
            return _datas.ContainsKey(key) && _datas[key] != null;
        }

        public object GetData(string key)
        {
            Precondition.CheckNotNull(key);
            if (!HasData(key))
            {
                Debug.LogWarning($"The {_fileName}DataManager does not contains this key. Use HasData() method before calling GetData()");
                return null;
            }
            return _datas[key];
        }

        public string GetDataJson(string key)
        {
            Precondition.CheckNotNull(key);
            if (!HasData(key))
                Debug.LogWarning($"The {_fileName}DataManager does not contains this key. Use HasData() method before calling GetData()");
            return JsonConvert.SerializeObject(_datas[key]);
        }

        public T GetObject<T>(string key)
        {
            Dictionary<string, T> objectTypeData = new Dictionary<string, T>();
            string jsonData = File.ReadAllText(Path);
            objectTypeData = JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonData);

            Precondition.CheckNotNull(key);
            if (!objectTypeData.ContainsKey(key) || objectTypeData[key] == null)
                throw new Exception($"The {_fileName}DataManager does not contains this key. Use HasData() method before calling GetData()");

            return objectTypeData[key];
        }

        public void SetData(string key, object data)
        {
            Precondition.CheckNotNull(key);
            Precondition.CheckNotNull(data);

            if (_datas.ContainsKey(key))
                _datas[key] = data;
            else
                _datas.Add(key, data);
            OnDataSet?.Invoke(key, data);
        }

        public void RemoveData(string key)
        {
            Precondition.CheckNotNull(key);
            if (!_datas.ContainsKey(key))
                throw new Exception($"The {_fileName} DataManager does not contains this key");
            _datas.Remove(key);
        }

        public void SaveData()
        {
#if UNITY_EDITOR
            // Save data without encryption in the editor
            string jsonData = JsonConvert.SerializeObject(_datas);
            File.WriteAllText(Path, jsonData);
            Logger.Log($"File saved at {Path}", LogLevel.Verbose, LogSettings);
#else
            // Save data with encryption during build time
            string jsonData = JsonConvert.SerializeObject(_datas);
            string encryptedData = EncryptString(jsonData, EncryptionKey, IV);
            File.WriteAllText(Path, encryptedData);
#endif
        }

        private void LoadData()
        {
            if (!File.Exists(Path))
            {
                _datas = new Dictionary<string, object>();
                return;
            }

#if UNITY_EDITOR
            // Load data without decryption in the editor
            string jsonData = File.ReadAllText(Path);
#else
            // Load data with decryption during build time
            string encryptedData = File.ReadAllText(Path);
            string jsonData = DecryptString(encryptedData, EncryptionKey, IV);
#endif

            _datas = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
            Logger.Log($"File loaded at {Path}", LogLevel.Verbose, LogSettings);
        }


        private static string EncryptString(string plainText, string key, string IV)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = GenerateValidKey(key, aesAlg.KeySize);
            aesAlg.IV = GenerateValidIV(IV, aesAlg.BlockSize);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new MemoryStream();
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        private static string DecryptString(string cipherText, string key, string IV)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = GenerateValidKey(key, aesAlg.KeySize);
            aesAlg.IV = GenerateValidIV(IV, aesAlg.BlockSize);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }

        private static byte[] GenerateValidKey(string key, int keySize)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            Array.Resize(ref keyBytes, keySize / 8);
            return keyBytes;
        }

        private static byte[] GenerateValidIV(string IV, int blockSize)
        {
            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);
            Array.Resize(ref ivBytes, blockSize / 8);
            return ivBytes;
        }
        public void DeleteSave()
        {
            if (!File.Exists(Path))
            {
                Logger.Log($"No save file to delete", LogLevel.Verbose, LogSettings);
                return;
            }
            File.Delete(Path);
            Logger.Log($"File deleted at {Path}", LogLevel.Verbose, LogSettings);
        }
        public string GetString(string key)
        {
            return Convert.ToString(GetData(key));
        }
    }
}