using UnityEngine;
using System.Collections;
using Puppet.Core.Network.Http;
using Puppet;
using Puppet.API.Client;
using Puppet.Utils;
using System.Collections.Generic;
using Puppet.Services;

public class LoginScene : MonoBehaviour,ILoginView
{
    public UIEventListener btnLogin, btnForgot, btnFacebook, btnGuest,btnRegister;

    public UIInput txtUsername, txtPassword;
    void Start()
    {   
		LoginPresenter presenter = new LoginPresenter (this);

        btnLogin.onClick += this.onBtnLoginClick;
        btnForgot.onClick += this.onBtnForgotClick;
        btnFacebook.onClick += this.onBtnFacebookClick;
        btnGuest.onClick += this.onBtnGuestClick;
        btnRegister.onClick += this.onBtnRegisterClick;

    }


    void OnDestroy()
    {
        btnLogin.onClick -= this.onBtnLoginClick;
        btnForgot.onClick -= this.onBtnForgotClick;
        btnFacebook.onClick -= this.onBtnFacebookClick;
        btnGuest.onClick -= this.onBtnGuestClick;
        btnRegister.onClick -= this.onBtnRegisterClick;
		SocialService.Instance.onLoginComplete -= onLoginComplete;
    }

    void Update()
    {

    }

    void onBtnLoginClick(GameObject gobj)
    {
        string userName = txtUsername.value;
        string password = txtPassword.value;
        if (string.IsNullOrEmpty(userName))
            userName = "dungnv";
        if (string.IsNullOrEmpty(password))
            password = "puppet#89";
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            APILogin.GetAccessToken(userName, password, GetAccessTokenResponse);
        }
    }

    void GetAccessTokenResponse(bool status, string message, IHttpResponse response)
    {
        if (status == false)
        {
            PuMain.Setting.Threading.QueueOnMainThread(() =>
            {
                DialogService.Instance.ShowDialog(new DataDataDialogMessage("Error", message, null));
            });
        }
        else
            APILogin.Login(message, LoginResponse);
    }

    void LoginResponse(bool status, string message)
    {
        if (status == false)
            Logger.Log(message);
    }


    void onBtnForgotClick(GameObject gobj)
    {
        
    }
    private void onBtnRegisterClick(GameObject go)
    {
        DialogRegister.Instance.ShowDialog(delegate(bool? status,string userName,string password){
            if (status == true)
                APILogin.GetAccessToken(userName, password, GetAccessTokenResponse);
        });
    }
    void onBtnFacebookClick(GameObject gobj)
    {
		SocialService.SocialLogin (SocialType.Facebook);
    }
    void onBtnGuestClick(GameObject gobj)
    {
        APILogin.LoginTrial((bool status, string message) =>
        {
            if(status == false)
                DialogService.Instance.ShowDialog(new DataDataDialogMessage("Lỗi", message, null));
        });
    }

	void onLoginComplete (SocialType arg1, bool arg2)
	{
		switch (arg1) 
		{
			case SocialType.Facebook:
				if(arg2)
					APILogin.GetAccessTokenFacebook(SocialService.GetSocialNetwork(arg1).AccessToken, onReceiveredAccessToken);
				break;
		}
	}

	void onReceiveredAccessToken (bool status, string message, Dictionary<string, object> data)
	{
		foreach (string key in data.Keys) {
			Logger.Log ("=======> " + key + " - " + data[key].ToString());	
		}

	}

	#region ILoginView implementation

	public void ShowLoginError (string message)
	{

	}

	public void ShowRegister ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion
}
