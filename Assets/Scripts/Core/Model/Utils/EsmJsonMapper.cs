using System;
using LitJson;
using System.Reflection;

namespace Esimo
{
    public class EsmJsonMapper : JsonMapper
    {
        public static object ToObject(string type, string json)
        {
            JsonReader reader = new JsonReader(json);
            Logger.Log("type name: {0}", type);
            return ReadValue(GetType(type), reader);
        }

        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null)
                return type;

            var typeWithNamespace = Type.GetType("Esimo." + typeName);
            if (typeWithNamespace != null)
                return typeWithNamespace;

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                typeWithNamespace = a.GetType("Esimo." + typeName);
                if (type != null)
                    return type;
                if (typeWithNamespace != null)
                    return typeWithNamespace;
            }
            Logger.Log("type is null");
            return null;
        }

    }
}