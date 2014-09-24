using UnityEngine;
using System.Collections;
using Puppet;
using Puppet.Core;
using Puppet.Utils;

public class PuApp : Singleton<PuApp>
{
    PuSetting setting;
    protected override void Init()
    {
        setting = new PuSetting("test.esimo.vn");
    }

    public void StartApplication()
    {

    }

    void FixedUpdate()
    {
        if (setting != null)
            setting.Update();
    }

    public void BackScene()
    {
        Puppet.API.Client.APIGeneric.BackScene((bool status, string message) => {
            if (!status)
                Logger.Log(message);
        });
    }
}
