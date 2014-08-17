using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Esimo.Core.Storage
{
    public class PlayerPrefStorage : BaseSingleton<PlayerPrefStorage>, IStorage
    {
        public void SetInt(string key, int value)
        {
            UnityEngine.PlayerPrefs.SetInt(key, value);
        }

        public void SetFloat(string key, float value)
        {
            UnityEngine.PlayerPrefs.SetFloat(key, value);
        }

        public void SetString(string key, string value)
        {
            UnityEngine.PlayerPrefs.SetString(key, value);
        }

        public void SetObject(string key, object value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, value);
                SetString(key, Convert.ToBase64String(ms.ToArray()));
            }
        }

        public int GetInt(string key)
        {
            return UnityEngine.PlayerPrefs.GetInt(key);
        }

        public float GetFloat(string key)
        {
            return UnityEngine.PlayerPrefs.GetFloat(key);
        }

        public string GetString(string key)
        {
            return UnityEngine.PlayerPrefs.GetString(key);
        }

        public object GetObject(string key)
        {
            byte[] bytes = Convert.FromBase64String(GetString(key));
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }
        }

        public void DeleteAll()
        {
            UnityEngine.PlayerPrefs.DeleteAll();
        }

        public void DeleteKey(string key)
        {
            UnityEngine.PlayerPrefs.DeleteKey(key);
        }

        public bool HasKey(string key)
        {
            return UnityEngine.PlayerPrefs.HasKey(key);
        }
    }
}
