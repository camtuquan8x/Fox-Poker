using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Esimo.Core.Storage
{
    internal abstract class AbstractCacheService : IStorage, IStorageFile
	{
        #region Cache Model
        [Serializable()]
        internal class CacheModel : DataModel
        {
            public string type;
            public string value;
            public CacheModel() : base() { }
            public CacheModel(string type, string value)
            {
                this.type = type;
                this.value = value;
            }
            public CacheModel(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
        }
        #endregion

        private delegate bool delegateRunAsync();

        protected const string KEY_VALUE_INT = "INT";
        protected const string KEY_VALUE_STRING = "STRING";
        protected const string KEY_VALUE_FLOAT = "FLOAT";
        protected const string KEY_VALUE_OBJECT = "OBJECT";

        protected const string DUPLICATE_KEY_FOR_VALUE_TYPES_EXCEPTION = "Key has already been used for other value types";
        protected const string KEY_NOT_FOUND = "Key not found";

        public Dictionary<string, CacheModel> memCache = new Dictionary<string, CacheModel>();
        public string filePath = string.Empty;

        private void SetKeyValue(string key, string value, string type)
        {
            if (HasKey(key))
            {
                CacheModel tmp = memCache[key];
                if (tmp.type != type)
                    throw new ArgumentException(DUPLICATE_KEY_FOR_VALUE_TYPES_EXCEPTION);
                else
                {
                    CacheModel model = new CacheModel(type, value);
                    memCache[key] = model;
                }

            }
            else
            {
                CacheModel model = new CacheModel(type, value);
                memCache.Add(key, model);
            }
        }

        private void RunAync(delegateRunAsync action, Action<bool> callback)
        {
            Loom.RunAsync(() =>
            {
                bool result = false;
                try
                {
                    result = action();
                }
                finally
                {
                    if (callback != null)
                        callback(result);
                }
            });
        }

        public void SetInt(string key, int value)
        {
            SetKeyValue(key, value.ToString(), KEY_VALUE_INT);
        }

        public void SetFloat(string key, float value)
        {
            SetKeyValue(key, value.ToString(), KEY_VALUE_FLOAT);
        }

        public void SetString(string key, string value)
        {
            SetKeyValue(key, value, KEY_VALUE_STRING);
        }

        public virtual void SetObject(string key, object value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, value);
                SetKeyValue(key, Convert.ToBase64String(ms.ToArray()), KEY_VALUE_OBJECT);
            }
        }

        public int GetInt(string key)
        {
            if (HasKey(key))
            {
                CacheModel model = memCache[key];
                if (model.type == KEY_VALUE_INT)
                    return Convert.ToInt32(model.value);
                else
                    throw new ArgumentException(KEY_NOT_FOUND);
            }
            else
                throw new ArgumentException(KEY_NOT_FOUND);
        }

        public float GetFloat(string key)
        {
            if (HasKey(key))
            {
                CacheModel model = memCache[key];
                if (model.type == KEY_VALUE_FLOAT)
                    return (float)Convert.ToDouble(model.value);
                else
                    throw new ArgumentException(KEY_NOT_FOUND);
            }
            else
                throw new ArgumentException(KEY_NOT_FOUND);
        }

        public string GetString(string key)
        {
            if (HasKey(key))
            {
                CacheModel model = memCache[key];
                if (model.type == KEY_VALUE_STRING)
                    return model.value;
                else
                    throw new ArgumentException(KEY_NOT_FOUND);
            }
            else
                throw new ArgumentException(KEY_NOT_FOUND);
        }

        public virtual object GetObject(string key)
        {
            if (HasKey(key))
            {
                CacheModel model = memCache[key];
                if (model.type == KEY_VALUE_OBJECT)
                {
                    byte[] bytes = Convert.FromBase64String(model.value);
                    using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
                    {
                        ms.Write(bytes, 0, bytes.Length);
                        ms.Position = 0;
                        return new BinaryFormatter().Deserialize(ms);
                    }
                }
                else
                    throw new ArgumentException(KEY_NOT_FOUND);
            }
            else
                throw new ArgumentException(KEY_NOT_FOUND);
        }

        public void DeleteAll()
        {
            memCache = new Dictionary<string, CacheModel>();
        }

        public void DeleteKey(string key)
        {
            if (HasKey(key))
                memCache.Remove(key);
            else
                throw new KeyNotFoundException("");
        }

        public bool HasKey(string key)
        {
            return memCache.ContainsKey(key);
        }

        public void SaveFile(Action<bool> callback)
        {
            RunAync(SaveSync, callback);
        }

        public void LoadFile(Action<bool> callback)
        {
            RunAync(LoadSync, callback);
        }

        public void DeleteFile(Action<bool> callback)
        {
            RunAync(DeleteFileSync, callback);
        }

        protected abstract bool LoadSync();

        protected abstract bool SaveSync();

        protected abstract bool DeleteFileSync();
    }
}
