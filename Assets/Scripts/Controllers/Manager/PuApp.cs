using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet;
using Puppet.Core;
using Puppet.Utils;
using Puppet.Utils.Threading;
using Puppet.Service;

public class PuApp : Singleton<PuApp>
{
    PuSetting setting;

    List<KeyValuePair<EMessage, string>> listMessage = new List<KeyValuePair<EMessage, string>>();
    protected override void Init()
    {
		setting = new PuSetting("puppet.esimo.vn", "puppet.esimo.vn");
    }

    public void StartApplication()
    {
        PuMain.Setting.Threading.QueueOnMainThread(() =>
        {
            PuMain.Dispatcher.onWarningUpgrade += Dispatcher_onWarningUpgrade;
        });
		SocialService.SocialStart ();
    }

    void Dispatcher_onWarningUpgrade(EUpgrade type, string message, string market)
    {
        PuMain.Setting.Threading.QueueOnMainThread (() =>
		{
				DialogService.Instance.ShowDialog (new DialogConfirm ("Kiểm tra phiên bản", message, delegate(bool? obj) {
	
			
				}));
		});
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

	public void OnApplicationQuit()
	{
		PuMain.Socket.Disconnect();
	}
}
