using System;

namespace Esimo
{
    public class BaseSingleton<T> where T : class, new()
    {
        BaseSingleton() { }

        class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly T instance = new T();
        }

        public static T Instance
        {
            get { return SingletonCreator.instance; }
        }
    }
}
