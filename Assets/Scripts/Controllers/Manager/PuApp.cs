using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet;
using Puppet.Core;
using Puppet.Utils;
using Puppet.Utils.Threading;

public class PuApp : Singleton<PuApp>
{
    PuSetting setting;

    List<KeyValuePair<EMessage, string>> listMessage = new List<KeyValuePair<EMessage, string>>();
    protected override void Init()
    {
        setting = new PuSetting("test.esimo.vn");
    }

    public void StartApplication()
    {
        PuMain.Setting.Threading.QueueOnMainThread(() =>
        {
        PuMain.Instance.Dispatcher.onNoticeMessage += Dispatcher_onNoticeMessage;
        });
    }

    void Dispatcher_onNoticeMessage(EMessage type, string message)
    {
        listMessage.Add(new KeyValuePair<EMessage, string>(type, message));
    }

    void FixedUpdate()
    {
        if (setting != null)
            setting.Update();

        if (listMessage.Count > 0)
        {
            //DialogConfirm.Instance.ShowConfirm("Kiểm tra phiên bản", "content: " + listMessage[0].Value , null);
            listMessage.Clear();
        }
    }

    public void BackScene()
    {
        Puppet.API.Client.APIGeneric.BackScene((bool status, string message) => {
            if (!status)
                Logger.Log(message);
        });
    }
    void OnDestroy() {
        DialogFactory.ApplicationDestroy();
    }
}
