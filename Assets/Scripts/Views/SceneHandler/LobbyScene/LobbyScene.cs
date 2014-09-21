using UnityEngine;
using System.Collections;
using Puppet.Core.Model;

public class LobbyScene : MonoBehaviour {
	public UIEventListener tabBeginner,tabAmature,tabProfessional,tabMaster;
	public UIEventListener  btnType,btnSearch,btnBack;
	public UILabel txtUsername, txtChip;
	public UITable tableType1;
	public UITable tableType2;
	bool isShowType1 = true;
	void Start () {
        Puppet.API.Client.APILobby.GetAllLobby(onGetAllLobby);
		tabBeginner.onHover += onTabHover;
		tabMaster.onHover += onTabHover;
		tabProfessional.onHover += onTabHover;
		tabAmature.onHover += onTabHover;
		btnType.onClick += btnTypeLobbyClick;
	}

    private void onGetAllLobby(bool status, string message, System.Collections.Generic.List<Puppet.Core.Model.DataLobby> data)
    {
        this.lobbies = data;
        initShowRowType1();
    }
    private void initShowRowType1()
    {
        for (int i = 0; i < tableType2.transform.childCount; i++)
        {
            Destroy(tableType2.transform.GetChild(i));
        }
        foreach (DataLobby item in lobbies)
        {
            LobbyRowType1.Create(item, tableType1);
        }
        tableType1.repositionNow = true;
        tableType1.GetComponent<UICenterOnChild>().CenterOn(tableType1.transform.GetChild(0));
    }
    private void initShowRowType2()
    {
        for (int i = 0; i < tableType1.transform.childCount; i++)
        {
            Destroy(tableType1.transform.GetChild(i));
        }
        foreach (DataLobby item in lobbies)
        {
            LobbyRowType1.Create(item, tableType1);
        }
        
        tableType2.repositionNow = true;
    }

	void OnDestroy(){
		tabBeginner.onHover -= onTabHover;
		tabMaster.onHover -= onTabHover;
		tabProfessional.onHover -= onTabHover;
		tabAmature.onHover -= onTabHover;
		btnType.onClick -= btnTypeLobbyClick;
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



    public System.Collections.Generic.List<Puppet.Core.Model.DataLobby> lobbies { get; set; }
}
