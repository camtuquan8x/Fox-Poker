using UnityEngine;
using System.Collections;
using Puppet.API.Client;
using Puppet;
using System;
using Puppet.Service;
[PrefabAttribute(Name = "Prefabs/Dialog/DialogRegister", Depth = 9, IsAttachedToCamera = true, IsUIPanel = true)]
public class DialogRegister : SingletonPrefab<DialogRegister>
{

    #region UnityEditor
    public UIInput userName, password, rePassword;
    public GameObject btnRegister, btnClose;
    Action<bool?, string, string> OnRegisterComplete;
    #endregion
    void Start () {
	
	}
    void OnEnable()
    {
        UIEventListener.Get(btnRegister).onClick += OnClickRegister;
        UIEventListener.Get(btnClose).onClick += OnClickClose;
    }


    void OnDisable()
    {
        UIEventListener.Get(btnRegister).onClick -= OnClickRegister;
        UIEventListener.Get(btnClose).onClick -= OnClickClose;
    }

    private void OnClickRegister(GameObject go)
    {
        string name = userName.value;
        string pass = password.value;
        string rePass = rePassword.value;
        if (pass.Equals(rePass))
        {
            APILogin.QuickRegister(name, pass, QuickRegisterCallBack);
        }
        else
        {
            Logger.Log("Mật khẩu không giống nhau");
        }
    }

    private void QuickRegisterCallBack(bool status, string message)
    {
        if (status)
        {
            PuMain.Setting.Threading.QueueOnMainThread(() =>
            {
                if (OnRegisterComplete != null)
                    OnRegisterComplete(status, userName.value, password.value);
                GameObject.Destroy(gameObject);
            });
        }
        else
        {
            PuMain.Setting.Threading.QueueOnMainThread(() =>
            {
                DialogService.Instance.ShowDialog(new DialogMessage("Lỗi", message, null));
            });
        }
    }
    public void ShowDialog( Action<bool?, string, string> OnRegisterComplete) {
        this.OnRegisterComplete = OnRegisterComplete;
    }


    private void OnClickClose(GameObject go)
    {
        GameObject.Destroy(gameObject);
    }

}
