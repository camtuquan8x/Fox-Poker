using Puppet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PuSetting
{
    public PuSetting(string domain)
    {
        PuMain.Setting.ServerModeBundle = new WebServerMode(domain);
        PuMain.Setting.ServerModeHttp = new WebServerMode(domain);
        PuMain.Setting.ServerModeSocket = new ServerMode(domain);
        PuMain.Instance.Load();
        PuMain.Instance.Dispatcher.onChangeScene += ChangeScene;
    }

    void ChangeScene(EScene fromScene, EScene toScene)
    {
        if(toScene == EScene.LoginScreen)
            Application.LoadLevel(Scene.LoginScene.ToString());
        else if (toScene == EScene.World_Game)
            Application.LoadLevel(Scene.ChoiceGameScene.ToString());
        else if (toScene == EScene.Pocker_Plaza)
            Application.LoadLevel(Scene.PokerHallScene.ToString());
        else if (toScene == EScene.Pocker_Lobby)
            Application.LoadLevel(Scene.LobbyScene.ToString());
        else if (toScene == EScene.Pocker_Gameplay)
            Application.LoadLevel(Scene.GameplayScene.ToString());
        else if (toScene == EScene.SplashScreen)
            Application.LoadLevel(Scene.SplashScene.ToString());
    }

    public void Update()
    {
        if (PuMain.Setting.ActionUpdate != null)
            PuMain.Setting.ActionUpdate();
    }

    class ServerMode : IServerMode
    {
        string domain;
        public ServerMode(string domain)
        {
            this.domain = domain;
        }

        public string GetBaseUrl() { return string.Format("https://{0}:{1}", Domain, Port); }

        public int Port { get { return 9933; } }

        public string Domain { get { return domain; } }

        public string GetPath(string path) { return string.Format("{0}/{1}", GetBaseUrl(), path); }
    }

    class WebServerMode : IServerMode
    {
        string domain;
        public WebServerMode(string domain)
        {
            this.domain = domain;
        }

        public string GetBaseUrl() { return string.Format("http://{0}:{1}", Domain, Port); }

        public int Port { get { return 1990; } }

        public string Domain { get { return domain; } }

        public string GetPath(string path) { return string.Format("{0}/puppet/{1}", GetBaseUrl(), path); }
    }

}
