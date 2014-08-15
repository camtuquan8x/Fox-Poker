using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Esimo.Core.Storage
{
    internal sealed class DeviceCacheService : AbstractCacheService
    {
        protected override bool LoadSync()
        {
            string json = string.Empty;
            if (filePath == null || !File.Exists(filePath))
                throw new FileNotFoundException();
            else
            {
                Stream stream = File.Open(filePath, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Binder = new VersionDeserializationBinder();
                try
                {
                    json = (string)bformatter.Deserialize(stream);
                }
                finally
                {
                    stream.Close();
                }

                bool success = false;
                if (json != null)
                {
                    Dictionary<string, object> jsonDict = JsonUtil.Deserialize(json) as Dictionary<string, object>;
                    memCache = new Dictionary<string, CacheModel>();
                    foreach (var item in jsonDict)
                    {
                        CacheModel model = JsonDataModelFactory.CreateDataModel<CacheModel>(item.Value as Dictionary<string, object>);
                        memCache.Add(item.Key, model);
                        Logger.Log("Key: " + item.Key);
                        Logger.Log("Value: " + model.value);
                    }
                    success = true;
                }
                return success;
            }
        }

        protected override bool SaveSync()
        {
            if (memCache == null)
                throw new ArgumentNullException("data cannot be null.");
            string data = JsonUtil.Serialize(memCache);
            Logger.Log("data: " + data);

            Stream stream = File.Open(filePath, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            bformatter.Binder = new VersionDeserializationBinder();
            bformatter.Serialize(stream, data);
            stream.Close();
            return true;
        }

        protected override bool DeleteFileSync()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            else
                throw new FileNotFoundException();
        }

        private sealed class VersionDeserializationBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
                {
                    Type typeToDeseralize = null;
                    assemblyName = Assembly.GetExecutingAssembly().FullName;
                    typeToDeseralize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
                    return typeToDeseralize;
                }
                return null;
            }
        }
    }
}
