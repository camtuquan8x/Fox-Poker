using UnityEngine;
using System.Collections;
using Puppet;
using Puppet.Core.Model;


public class FriendViewItem : MonoBehaviour {
	#region Unity Editor
	UILabel lbUserName;
	UIToggle tgIsOnline;
	#endregion
	EventDelegate toggleChange;

	UserInfo userInfo;

	public void SetData(UserInfo userInfo,bool isOnline ){
		this.userInfo = userInfo;
		lbUserName.text = userInfo.info.userName;
		tgIsOnline.value = isOnline;
	}
	void Start(){
		toggleChange = new EventDelegate(this,"OnSelectedFriend");
	}
	void OnEnable(){
		gameObject.GetComponent<UIToggle> ().onChange.Add (toggleChange);
	}
	void OnDisable(){
		gameObject.GetComponent<UIToggle> ().onChange.Remove(toggleChange);
	}
	void OnSelectedFriend(){

	}
}
