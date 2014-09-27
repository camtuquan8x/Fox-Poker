using UnityEngine;
using System.Collections;
using Puppet.Core.Network.Http;
using Puppet;
using Puppet.API.Client;
using Puppet.Utils;
using System.Collections.Generic;
using Puppet.Service;

public class LoginScene : MonoBehaviour,ILoginView
{
    public UIEventListener btnLogin, btnForgot, btnFacebook, btnGuest,btnRegister;

    public UIInput txtUsername, txtPassword;

	LoginPresenter presenter;

    void Start()
    {   
		presenter = new LoginPresenter (this);

        btnLogin.onClick += this.onBtnLoginClick;
        btnForgot.onClick += this.onBtnForgotClick;
        btnFacebook.onClick += this.onBtnFacebookClick;
        btnGuest.onClick += this.onBtnGuestClick;
        btnRegister.onClick += this.onBtnRegisterClick;

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
            userName = "dungnv";
        if (string.IsNullOrEmpty(password))
            password = "puppet#89";
		presenter.LoginWithUserName (userName, password);
    }

    void GetAccessTokenResponse(bool status, string message, IHttpResponse response)
    {
        if (status == false)
        {
            PuMain.Setting.Threading.QueueOnMainThread(() =>
            {
                DialogService.Instance.ShowDialog(new DialogMessage("Error", message, null));
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
//        DialogRegister.Instance.ShowDialog(delegate(bool? status,string userName,string password){
//            if (status == true)
//                APILogin.GetAccessToken(userName, password, GetAccessTokenResponse);
//        });
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

	public void ShowLoginError (string message)
	{
		DialogService.Instance.ShowDialog(new DialogMessage("Lỗi", message, null));
	}

	public void ShowRegister ()
	{

	}

	#endregion
}
