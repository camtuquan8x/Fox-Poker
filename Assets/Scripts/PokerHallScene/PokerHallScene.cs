using UnityEngine;
using System.Collections;

public class PokerHallScene : MonoBehaviour {
	public UIButton btnPlayNow,btnLeague,btnLobby,btnEvent;

	void Start () {
		EventDelegate.Add (btnPlayNow.onClick, this.OnBtnPlayNowClick);
		EventDelegate.Add (btnLobby.onClick,this.OnBtnLobbyClick);
	}
	void OnDestroy(){
		EventDelegate.Remove (btnPlayNow.onClick, this.OnBtnPlayNowClick);
		EventDelegate.Remove (btnLobby.onClick,this.OnBtnLobbyClick);
	}
	// Update is called once per frame
	void Update () {
	
	}
	void OnBtnPlayNowClick(){
		Application.LoadLevel (Scene.GameplayScene.ToString());
	}
	void OnBtnLobbyClick(){
		Application.LoadLevel (Scene.LobbyScene.ToString());
	}
}
