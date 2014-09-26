using Puppet;
using Puppet.Utils.Loggers;
using Puppet.Utils.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PuSetting
{
    public PuSetting(string domain)
    {
        PuMain.Setting = new CurrentSetting("puppet.esimo.vn");
        PuMain.Setting.Init();
        PuMain.Instance.Load();
        PuMain.Instance.Dispatcher.onChangeScene += ChangeScene;
    }

    void ChangeScene(EScene fromScene, EScene toScene)
    {
        if(toScene == EScene.LoginScreen)
            Application.LoadLevel(Scene.LoginScene.ToString());
        else if (toScene == EScene.World_Game)
            Application.LoadLevel(Scene.WorldGame.ToString());
        else if (toScene == EScene.Pocker_Plaza)
            Application.LoadLevel(Scene.Pocker_Plaza.ToString());
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

    class CurrentSetting : DefaultSetting
    {
        public override bool UseUnity
        {
            get { return true; }
        }

        public CurrentSetting(string domain)
        {
            DefaultSetting.domain = domain;
        }

        protected override void AfterInit()
        {

        }

        public override void ActionPrintLog(ELogType type, object message)
        {
            if (!IsDebug) return;

            switch(type)
            {
                case ELogType.Info:
                    UnityEngine.Debug.Log(message);
                    break;
                case ELogType.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case ELogType.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
                case ELogType.Exception:
                    UnityEngine.Debug.LogException((Exception)message);
                    break;
            }
        }

        public override Puppet.Utils.Threading.IThread Threading
        {
            get { return UnityThread.Instance; }
        }

        public override Puppet.Utils.Storage.IStorage PlayerPref
        {
            get { return UnityPlayerPrefab.Instance; }
        }

        public override string PathCache
        {
            get { return Path.Combine(Application.persistentDataPath, "Caching.save"); }
        }

        public override string UniqueDeviceIdentification
        {
            get
            {
                return SystemInfo.deviceUniqueIdentifier;
            }
        }
    }
}
