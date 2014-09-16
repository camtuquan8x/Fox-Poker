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
    void Awake()
    {
        PuMain.Instance.Init();
    }

    void Start()
    {
        TestGetToken();
    }

    void TestGetToken()
    {
        Puppet.API.Client.APILogin.GetAccessToken("dungnv", "puppet#89", (IHttpResponse response, bool status, string token) =>
        {
            if (string.IsNullOrEmpty(response.Error))
                Logger.Log("Status:{0} - Token:{1}", status, token);

            if (status)
            {
                Puppet.API.Client.APILogin.Login(token, null);
            }
        });
    }
}
