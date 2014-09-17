using UnityEngine;
using System.Collections;

public class LobbyScene : MonoBehaviour {
	public UIEventListener tabBeginner,tabAmature,tabProfessional,tabMaster;
	public UIEventListener  btnType,btnSearch,btnBack;
	public UILabel txtUsername, txtChip;
	public UITable tableType1;
	public UITable tableType2;
	public static Vector3 centerObject = Vector3.one;
	bool isShowType1 = true;
	void Start () {
		tableType1.GetComponent<UICenterOnChild> ().onFinished = OnDragFinishGift;
		tableType1.GetComponent<UICenterOnChild> ().CenterOn (tableType1.transform.GetChild (0));
		tabBeginner.onHover += onTabHover;
		tabMaster.onHover += onTabHover;
		tabProfessional.onHover += onTabHover;
		tabAmature.onHover += onTabHover;
		btnType.onClick += btnTypeLobbyClick;
	}
	void OnDestroy(){
		tabBeginner.onHover -= onTabHover;
		tabMaster.onHover -= onTabHover;
		tabProfessional.onHover -= onTabHover;
		tabAmature.onHover -= onTabHover;
		btnType.onClick -= btnTypeLobbyClick;
	}
	void OnDragFinishGift ()
	{
		centerObject = tableType1.GetComponent<UICenterOnChild> ().centeredObject.transform.position;
	}

	void btnTypeLobbyClick (GameObject go)
	{
		if (isShowType1) {
			isShowType1 = false;
			go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon_menu_type_2";
			tableType1.transform.parent.parent.gameObject.SetActive (false);
			tableType2.transform.parent.parent.gameObject.SetActive (true);
		} else {
			isShowType1 = true;
			go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon_menu";
			tableType1.transform.parent.parent.gameObject.SetActive (true);
			tableType2.transform.parent.parent.gameObject.SetActive (false);
		}
	}
	void onTabHover (GameObject go, bool state)
	{
		if (state) {
				if(go.name.Contains("1")){
					go.GetComponent<UISprite> ().spriteName = "tab_select_first";		
				}
				else {
					go.GetComponent<UISprite> ().spriteName = "tab_select_center";		
				}
				go.GetComponent<UISprite> ().MakePixelPerfect ();
				go.transform.localPosition = new Vector3(go.transform.localPosition.x,-38,go.transform.localPosition.z);
		} else {
			if(go.name.Contains("1")){
				go.GetComponent<UISprite> ().spriteName = "tab_normal_first";
			}
			else {
				go.GetComponent<UISprite> ().spriteName = "tab_normal_center";	
			}	
			go.GetComponent<UISprite> ().MakePixelPerfect ();
			go.transform.localPosition = new Vector3(go.transform.localPosition.x,-36,go.transform.localPosition.z);
		}
	}


}
