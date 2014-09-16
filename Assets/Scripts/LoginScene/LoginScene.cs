using UnityEngine;
using System.Collections;
using Puppet.Core.Network.Http;
using Puppet;

public class LoginScene : MonoBehaviour {
	public UIEventListener btnLogin,btnForgot,btnFacebook,btnGuest;
	public UIInput txtUsername,txtPassword;

    void Awake()
    {
        PuApp.Instance.StartApplication();
    }

	void Start () {
		btnLogin.onClick += this.onBtnLoginClick;
		btnForgot.onClick += this.onBtnForgotClick;
		btnFacebook.onClick += this.onBtnFacebookClick;
		btnGuest.onClick += this.onBtnGuestClick;
	}
	void OnDestroy(){
		btnLogin.onClick -= this.onBtnLoginClick;
		btnForgot.onClick -= this.onBtnForgotClick;
		btnFacebook.onClick -= this.onBtnFacebookClick;
		btnGuest.onClick -= this.onBtnGuestClick;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void onBtnLoginClick (GameObject gobj)
	{
		string userName = txtUsername.value;
		string password = txtPassword.value;
		if(!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            Puppet.API.Client.APILogin.GetAccessToken(userName, password, GetAccessTokenResponse);
		}
	}

    void GetAccessTokenResponse(bool status, string message, IHttpResponse response)
    {
        if (status == false)
            Logger.Log(message);
        else
            Puppet.API.Client.APILogin.Login(message, LoginResponse);
    }

    void LoginResponse(bool status, string message)
    {
        if (status == false)
            Logger.Log(message);
    }


	void onBtnForgotClick (GameObject gobj)
	{

	}
	void onBtnFacebookClick (GameObject gobj)
	{
		Debug.Log ("=========> " + btnFacebook);
	}
	void onBtnGuestClick (GameObject gobj)
	{
		Debug.Log ("=========> " + btnGuest);
	}
}
