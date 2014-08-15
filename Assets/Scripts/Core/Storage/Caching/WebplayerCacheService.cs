﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Esimo.Core.Storage
{
    internal sealed class WebplayerCacheService : AbstractCacheService
    {
        private const string SAVE_FILE_KEY = "WebplayerCacheService-SaveFileKey";

        protected override bool LoadSync()
        {
            try
            {
                memCache = new Dictionary<string, CacheModel>();
                Dictionary<string, object> dict = JsonUtil.Deserialize(PlayerPrefStorage.Instance.GetString(SAVE_FILE_KEY)) as Dictionary<string, object>;
                foreach(string key in dict.Keys)
                    memCache[key] = dict[key] as CacheModel;
            }
            catch
            {
                return false;
            }
            return true;
        }

        protected override bool SaveSync()
        {
            try
            {
                string data = JsonUtil.Serialize(memCache);
                PlayerPrefStorage.Instance.SetString(SAVE_FILE_KEY, data);
            }
            catch
            {
                return false;
            }
            return true;
        }

        protected override bool DeleteFileSync()
        {
            PlayerPrefStorage.Instance.DeleteKey(SAVE_FILE_KEY);
            return true;
        }

        public override object GetObject(string key)
        {
            return base.GetObject(key);
        }

        public override void SetObject(string key, object value)
        {
            base.SetObject(key, value);
        }
    }
}
