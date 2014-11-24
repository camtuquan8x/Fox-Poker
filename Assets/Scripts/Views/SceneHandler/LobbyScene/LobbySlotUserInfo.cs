using UnityEngine;
using System.Collections;
using Puppet.Core.Model;

public class LobbySlotUserInfo : MonoBehaviour {

	#region Unity Editor
	public UITexture avatar;
	public UILabel userName;
	#endregion
	public void showUserInfo(DataPlayerController user){
		userName.text = user.userName;
	}

}
