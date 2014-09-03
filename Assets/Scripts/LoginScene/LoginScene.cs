using UnityEngine;
using System.Collections;

public class LoginScene : MonoBehaviour {
	public UIButton btnLogin,btnForgot,btnFacebook,btnGuest;
	public UIInput txtUsername,txtPassword;
	void Start () {
		EventDelegate.Add (btnLogin.onClick, this.onBtnLoginClick);
		EventDelegate.Add (btnForgot.onClick, this.onBtnForgotClick);
		EventDelegate.Add (btnFacebook.onClick, this.onBtnFacebookClick);
		EventDelegate.Add (btnGuest.onClick, this.onBtnGuestClick);
	}
	void OnDestroy(){
		EventDelegate.Remove(btnLogin.onClick, this.onBtnLoginClick);
		EventDelegate.Remove (btnForgot.onClick, this.onBtnForgotClick);
		EventDelegate.Remove (btnFacebook.onClick, this.onBtnFacebookClick);
		EventDelegate.Remove (btnGuest.onClick, this.onBtnGuestClick);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void onBtnLoginClick ()
	{
		string userName = txtUsername.value;
		string password = txtPassword.value;
		if(!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password)){
			Debug.Log("========> " + Scene.PokerHallScene.ToString());
			Application.LoadLevel(Scene.PokerHallScene.ToString());
		}
	}
	void onBtnForgotClick ()
	{
		Debug.Log ("=========> " + btnForgot);
	}
	void onBtnFacebookClick ()
	{
		Debug.Log ("=========> " + btnFacebook);
	}
	void onBtnGuestClick ()
	{
		Debug.Log ("=========> " + btnGuest);
	}
}
