using UnityEngine;
using System.Collections;
using Puppet.Core.Network.Http;
using Puppet;
using Puppet.API.Client;
using Puppet.Utils;
using System.Collections.Generic;
using Puppet.Service;
using System;

public class LoginScene : MonoBehaviour,ILoginView
{
    public UIEventListener btnLogin, btnForgot, btnFacebook, btnGuest,btnRegister;

    public UIInput txtUsername, txtPassword;

	LoginPresenter presenter;

	void Awake()
	{
		PuSetting.UniqueDeviceId = SystemInfo.deviceUniqueIdentifier;
	}

    void Start()
    {   
		presenter = new LoginPresenter (this);
        presenter.ViewStart();
        btnLogin.onClick += this.onBtnLoginClick;
        btnForgot.onClick += this.onBtnForgotClick;
        btnFacebook.onClick += this.onBtnFacebookClick;
        btnGuest.onClick += this.onBtnGuestClick;
        btnRegister.onClick += this.onBtnRegisterClick;

    }

	void OnGUI()
	{
		PuSetting.UniqueDeviceId = GUI.TextField(new Rect(0, 0, Screen.width / 4, Screen.height/15), PuSetting.UniqueDeviceId);
	}

    void OnDestroy()
    {
		presenter.ViewEnd ();
        btnLogin.onClick -= this.onBtnLoginClick;
        btnForgot.onClick -= this.onBtnForgotClick;
        btnFacebook.onClick -= this.onBtnFacebookClick;
        btnGuest.onClick -= this.onBtnGuestClick;
        btnRegister.onClick -= this.onBtnRegisterClick;
    }


    void onBtnLoginClick(GameObject gobj)
    {
        string userName = txtUsername.value;
        string password = txtPassword.value;
        if (string.IsNullOrEmpty(userName))
            userName = "cong91";
        if (string.IsNullOrEmpty(password))
            password = "cong";
		presenter.LoginWithUserName (userName, password);
    }

    void onBtnForgotClick(GameObject gobj)
    {
        
    }
    private void onBtnRegisterClick(GameObject go)
    {
        presenter.ShowRegister();
    }
    void onBtnFacebookClick(GameObject gobj)
    {
		presenter.LoginFacebook ();
    }
    void onBtnGuestClick(GameObject gobj)
    {
		presenter.LoginTrail ();
    }


	#region ILoginView implementation

	public void ShowError (string message)
	{
		DialogService.Instance.ShowDialog(new DialogMessage("Lỗi", message, null));
	}

	public void ShowRegister (Action<bool?, string, string> OnRegisterComplete)
	{
        DialogRegister.Instance.ShowDialog(OnRegisterComplete);
	}
	#endregion


    public void ShowConfirm(string message, Action<bool?> action)
    {
        throw new NotImplementedException();
    }
}
