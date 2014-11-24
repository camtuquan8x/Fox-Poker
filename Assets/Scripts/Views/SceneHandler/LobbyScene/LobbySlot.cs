using UnityEngine;
using System.Collections;
using Puppet.Core.Model;

public class LobbySlot : MonoBehaviour {

	public GameObject btnDown;
	public LobbySlotUserInfo userInfo;
	public void setData(DataPlayerController controller){
		userInfo.gameObject.SetActive (true);
		userInfo.showUserInfo (controller);
	}
}
