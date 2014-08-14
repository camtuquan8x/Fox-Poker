using System;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Esimo
{
    public class JsonDataModelFactory
    {
        public static T CreateDataModel<T>(string json) where T : IDataModel
        {
            return (T)EsmJsonMapper.ToObject<T>(json);
        }

        public static T CreateDataModel<T>(Dictionary<string, System.Object> dict) where T : IDataModel
        {
            return (T)JsonDataModelFactory.CreateDataModel<T>(JsonUtil.Serialize(dict));
        }
    }
}

