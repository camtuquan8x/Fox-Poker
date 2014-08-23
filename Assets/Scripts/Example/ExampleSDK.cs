using UnityEngine;
using System.Collections;
using Puppet.Utils.Loggers;
using Puppet;
using System.IO;
using Puppet.Core.Network.Http;
using System.Collections.Generic;
using Puppet.Core.Network.Socket;
using Puppet.Utils;

public class ExampleSDK : MonoBehaviour
{

    CSmartFox sf;

    void Awake()
    {
        string pathCaching = Path.Combine(Application.dataPath, "Caching.save");
        PuMain.Setting = new Setting(EPlatform.Editor, pathCaching);
    }

    void Start()
    {
        TestGetToken();
    }

    void TestGetToken()
    {
        string accessToken = string.Empty;
        WWWRequest request = new WWWRequest(this, "?command=get_access_token", 30, 0);
        request.Method = HttpMethod.Post;
        request.PostData = new Dictionary<string, object>();
        request.PostData.Add("userName", "dungnv");
        request.PostData.Add("password", "puppet#89");

        request.onResponse = (IHttpRequest myRequest, IHttpResponse response) =>
        {
            Logger.Log("Data: " + response.Data);
            if (string.IsNullOrEmpty(response.Error))
            {
                accessToken = JsonUtil.Deserialize(response.Data)["token"].ToString();
                sf = new CSmartFox(accessToken);
                sf.Connect();
            }
        };
        PuMain.WWWHandler.Request(request);
    }

    void FixedUpdate()
    {
        if(sf != null)
            sf.FixedUpdate();
    }


    class Setting : IPuSettings
    {
        IServerMode server;
        IServerMode serverWeb;
        EPlatform _platform;
        string _pathCaching;

        public Setting(EPlatform platform, string pathCaching)
        {
            server = new ServerMode();
            serverWeb = new WebServerMode();
            _platform = platform;
            _pathCaching = pathCaching;
        }

        public EPlatform Platform
        {
            get { return _platform; }
        }

        public string PathCache
        {
            get  { return _pathCaching; }
        }

        public ServerEnvironment Environment
        {
            get { return ServerEnvironment.Dev; }
        }

        public IServerMode ServerModeHttp
        {
            get { return serverWeb; }
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
                get { return "9933"; }
            }

            public string Domain
            {
                //get { return "test.esimo.vn"; }
                get { return "192.168.10.126"; }
            }

            public string GetPath(string path)
            {
                return string.Format("{0}/{1}", GetBaseUrl(), path);
            }
        }

        class WebServerMode : IServerMode
        {
            public string GetBaseUrl()
            {
                return string.Format("http://{0}:{1}", Domain, Port);
            }

            public string Port
            {
                get { return "1990"; }
            }

            public string Domain
            {
                //get { return "test.esimo.vn"; }
                get { return "192.168.10.126"; }
            }

            public string GetPath(string path)
            {
                return string.Format("{0}/puppet/{1}", GetBaseUrl(), path);
            }
        }

        public void ActionChangeScene(string fromScene, string toScene)
        {

        }

        public void ActionPrintLog(ELogType type, object message)
        {
            if(message != null)
                Debug.Log(string.Format("{0}: {1}", type.ToString(), message.ToString()));
        }

        public Puppet.Utils.Storage.IStorage PlayerPref
        {
            get { return UnityPlayerPrefab.Instance; }
        }

        public Puppet.Utils.Threading.IThread Threading
        {
            get { return Puppet.Utils.Threading.CsharpThread.Instance; }
        }
    }

}
