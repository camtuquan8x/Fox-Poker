using Puppet.Utils.Threading;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puppet
{
    /// <summary>
    /// Prefab attribute. Use this on child classes
    /// to define if they have a prefab associated or not
    /// By default will attempt to load a prefab
    /// that has the same name as the class,
    /// otherwise [Prefab("path/to/prefab")] to define it specifically. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class PrefabAttribute : Attribute
    {
        public string Name;
        public bool IsUIPanel = false;
        public int Depth = 1;
        public bool IsAttachedToCamera = false;
    }

    public abstract class SingletonPrefab<T> : MonoBehaviour where T : SingletonPrefab<T>
    {
        private static T _instance = null;
        private static object _lock = new object();
        public static bool IsAwake
        {
            get
            {
                return (_instance != null);
            }
        }

        /// <summary>
        /// gets the instance of this Singleton
        /// use this for all instance calls:
        /// MyClass.Instance.MyMethod();
        /// or make your public methods static
        /// and have them use Instance internally
        /// for a nice clean interface
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        string singletonObjName = typeof(T).Name.ToString();
                        GameObject singletonObj = GameObject.Find(singletonObjName);

                        if (singletonObj == null) //if still not found try prefab or create
                        {
                            // checks if the [Prefab] attribute is set and pulls that if it can
                            bool hasPrefab = Attribute.IsDefined(typeof(T), typeof(PrefabAttribute));
                            if (hasPrefab)
                            {
                                PrefabAttribute attr = (PrefabAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(PrefabAttribute));
                                string prefabName = attr.Name;
                                try
                                {
                                    if (prefabName != "")
                                        singletonObj = (GameObject)Instantiate(Resources.Load(prefabName, typeof(GameObject)));
                                    else
                                        singletonObj = (GameObject)Instantiate(Resources.Load(singletonObjName, typeof(GameObject)));

                                    if(attr.IsAttachedToCamera)
                                    {
                                        UICamera camera = UICamera.current;
                                        if (camera == null) camera = GameObject.FindObjectOfType<UICamera>();
                                        singletonObj.transform.parent = camera.gameObject.transform;
                                        singletonObj.transform.localPosition = Vector3.zero;
                                        singletonObj.transform.localScale = Vector3.one;
                                    }
                                    if (attr.IsUIPanel)
                                        singletonObj.GetComponent<UIPanel>().depth = attr.Depth;

                                }
                                catch (Exception e)
                                {
                                    Debug.LogError("could not instantiate prefab even though prefab attribute was set: " + e.Message + "\n" + e.StackTrace);
                                }
                            }

                            if (singletonObj == null)
                            {
                                singletonObj = new GameObject();
                            }

                            DontDestroyOnLoad(singletonObj);
                            singletonObj.name = singletonObjName;
                        }

                        _instance = singletonObj.GetComponent<T>();
                        if (_instance == null)
                        {
                            _instance = singletonObj.AddComponent<T>();
                        }
                    }

                    return _instance;
                }
            }
        }

        // in your child class you can implement Awake()
        // and add any initialization code you want.

        // Helper function to initialize the Singleton object.
        public void Load(GameObject parentObj = null)
        {
            SetParent(parentObj);
        }


        /// <summary>
        /// Implement details in child class if it needs to be reset at some point.
        /// </summary>
        protected virtual void Reset() { }


        /// <summary>
        /// Parent this to another gameobject by string
        /// call from Awake if you so desire
        /// </summary>
        protected void SetParent(GameObject parentGO)
        {
            if (parentGO != null)
            {
                this.transform.parent = parentGO.transform;
            }
        }
    }
}
