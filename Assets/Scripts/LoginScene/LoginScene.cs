using UnityEngine;
using System.Collections;

public class LoginScene : MonoBehaviour {
	public UIEventListener btnLogin,btnForgot,btnFacebook,btnGuest;
	public UIInput txtUsername,txtPassword;


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
		if(!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password)){
			Application.LoadLevel(Scene.PokerHallScene.ToString());
		}
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
