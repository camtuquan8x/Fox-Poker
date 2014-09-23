using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet;
using Puppet.Core.Model;
using Puppet.API.Client;

public class LobbyScene : MonoBehaviour
{
    public UIEventListener btnType, btnSearch, btnBack;
    public UILabel txtUsername, txtChip;
	public UITable tableType1,tableType2,tableTab;
    bool isShowType1 = true;
    List<LobbyRowType1> types1;
    List<LobbyRowType2> types2;

    List<DataChannel> dataChannel;
    DataChannel selectedChannel;
    List<DataLobby> lobbies;

    void Start()
    {
        types1 = new List<LobbyRowType1>();
        types2 = new List<LobbyRowType2>();
        APILobby.GetGroupsLobby(OnGetGroupLobbyCallback);
    }

    void OnEnable()
    {
        btnType.onClick += btnTypeLobbyClick;
        btnBack.onClick += OnClickBack;
    }

    void OnDisable()
    {
        btnType.onClick -= btnTypeLobbyClick;
        btnBack.onClick -= OnClickBack;
    }
	private void ClearAllRow(){
		while (types1.Count > 0)
		{
			GameObject.Destroy(types1[0].gameObject);
			types1.RemoveAt(0);
		}
		while (types2.Count > 0)
		{
			GameObject.Destroy(types2[0].gameObject);
			types2.RemoveAt(0);
		}
	}
    private void initShowRowType1()
    {
		ClearAllRow ();
        foreach (DataLobby item in lobbies)
        {
            types1.Add(LobbyRowType1.Create(item, tableType1));
        }
        tableType1.repositionNow = true;
        tableType1.GetComponent<UICenterOnChild>().CenterOn(tableType1.transform.GetChild(0));
    }
    private void initShowRowType2()
    {
		ClearAllRow ();
        foreach (DataLobby item in lobbies)
        {
            types2.Add(LobbyRowType2.Create(item, tableType2));
        }

        tableType2.repositionNow = true;
    }

    void btnTypeLobbyClick(GameObject go)
    {
        if (isShowType1)
        {
            isShowType1 = false;
            go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon_menu_type_2";
            tableType1.transform.parent.parent.gameObject.SetActive(false);
            tableType2.transform.parent.parent.gameObject.SetActive(true);
            initShowRowType2();
        }
        else
        {
            isShowType1 = true;
            go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon_menu";
            tableType1.transform.parent.parent.gameObject.SetActive(true);
            tableType2.transform.parent.parent.gameObject.SetActive(false);
            initShowRowType1();
        }
    }

    void OnClickBack(GameObject obj)
    {
        PuApp.Instance.BackScene();
    }

    void OnGetGroupLobbyCallback(bool status, string message, List<DataChannel> data)
    {
        if (status)
        {
            dataChannel = data;
			for (int i = 0; i < dataChannel.Count; i++) {
				LobbyTab tab = LobbyTab.Create(dataChannel[i],tableTab,i);	
				tab.SetEventChoiceTab( delegate(DataChannel channel) {
					APILobby.SetSelectChannel(channel, OnGetAllLobbyInChannel);
				});
			}
			tableTab.Reposition();
			Vector3 currentPosition = tableTab.transform.localPosition;
			tableTab.transform.localPosition = new Vector3(currentPosition.x,currentPosition.y-2,currentPosition.z);

        }
        else
            Logger.LogError(message);
    }

    void OnGetAllLobbyInChannel(bool status, string message, List<DataLobby> data)
    {
        if (status)
        {
            this.lobbies = data;
            initShowRowType1();
        }
        else
            Logger.LogError(message);
    }

}
