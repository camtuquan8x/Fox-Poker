using Puppet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PuSetting
{
    public PuSetting(string domain)
    {
        DefaultSetting.domain = "puppet.esimo.vn";

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
}
