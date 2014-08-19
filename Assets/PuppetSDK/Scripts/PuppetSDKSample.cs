using UnityEngine;
using System.Collections;
using Puppet;
using Puppet.Utils;

public class PuppetSDKSample : MonoBehaviour {

	void Awake() {
		string cachingPath = System.IO.Path.Combine (Application.temporaryCachePath, "Caching.save");
		PuMain.Setting = new Setting(EPlatform.Editor, EEngine.Unity, cachingPath);

	}

	// Use this for initialization
	void Start () {
		TestCaching ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region IMPLEMENT PUPPET SDK SETTING 
	class Setting : IPuSettings
	{
		ServerMode server;
		EPlatform _platform;
		EEngine _engine;
		string pathCache;
		
		public Setting(EPlatform platform, EEngine engine, string path)
		{
			server = new ServerMode();
			_platform = platform;
			_engine = engine;
			pathCache = path;
		}
		
		public EPlatform Platform
		{
			get { return _platform; }
		}
		
		public EEngine Engine
		{
			get { return _engine; }
		}
		
		public string PathCache
		{
			get  { return pathCache; }
		}
		
		public ServerEnvironment Environment
		{
			get { return ServerEnvironment.Dev; }
		}
		
		public IServerMode ServerModeWeb
		{
			get { return server; }
		}
		public IServerMode ServerModeBundle
		{
			get { return server; }
		}
		public IServerMode ServerModeSocket
		{
			get { return server; }
		}
		
		class ServerMode : IServerMode
		{
			public string GetBaseUrl()
			{
				return string.Format("https://{0}:{1}", Domain, Port);
			}
			
			public string Port
			{
				get { return "8888"; }
			}
			
			public string Domain
			{
				get { return "localhost"; }
			}
			
			public string GetPath(string path)
			{
				return string.Format("{0}/{1}", GetBaseUrl(), path);
			}
		}
	}
	#endregion

	void TestCaching()
	{
		CacheHandler.Instance.SetInt ("1", 1);
		CacheHandler.Instance.SaveFile ((bool status) => Logger.Log ("Save cache file {0}", status));
	}
}
