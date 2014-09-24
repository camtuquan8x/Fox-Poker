using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet;
using Puppet.Core.Model;
using Puppet.API.Client;

public class LobbyScene : MonoBehaviour
{
    public GameObject btnType, btnSearch, btnBack, btnCreateGame;
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
    void FixedUpdate() {
        if (tableType1.GetComponent<UICenterOnChild>().centeredObject != null) {
            btnCreateGame.SetActive(tableType1.GetComponent<UICenterOnChild>().centeredObject.GetComponent<LobbyRowType1>().lobby.roomId == types1[0].lobby.roomId);
        }
    }
    void OnEnable()
    {
        UIEventListener.Get(btnType).onClick += btnTypeLobbyClick;
        UIEventListener.Get(btnBack).onClick += OnClickBack;
        UIEventListener.Get(btnCreateGame).onClick += OnClickCreateGame;
        
    }

    private void OnClickCreateGame(GameObject go)
    {
        APILobby.CreateLobby(OnCreateLobbyHandler);
    }

    private void OnCreateLobbyHandler(bool status, string message)
    {
        if (!status)
            Logger.LogError(message);
    }


    void OnDisable()
    {
        UIEventListener.Get(btnType).onClick -= btnTypeLobbyClick;
        UIEventListener.Get(btnBack).onClick -= OnClickBack;
        UIEventListener.Get(btnCreateGame).onClick += OnClickCreateGame;
        //tableType1.GetComponent<UICenterOnChild>().onFinished -= onGotoCenterType1;
    }
    private void onGotoCenterType1()
    {
        if (tableType1.GetComponent<UICenterOnChild>().centeredObject != null)
            tableType1.GetComponent<UICenterOnChild>().CenterOn(tableType1.GetComponent<UICenterOnChild>().centeredObject.transform);
        tableType1.GetComponent<UICenterOnChild>().onFinished -= onGotoCenterType1;
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
    IEnumerator initShowRowType1()
    {
		ClearAllRow ();
        yield return new WaitForEndOfFrame();
        foreach (DataLobby item in lobbies)
        {
            types1.Add(LobbyRowType1.Create(item, tableType1));
        }
        tableType1.repositionNow = true;
        tableType1.GetComponent<UICenterOnChild>().onFinished += onGotoCenterType1;
        //tableType1.GetComponent<UICenterOnChild>().CenterOn(tableType1.transform.GetChild(0));
    }
    IEnumerator  initShowRowType2()
    {
		ClearAllRow ();
        yield return new WaitForEndOfFrame();
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
            StartCoroutine( initShowRowType2());
        }
        else
        {
            isShowType1 = true;
            go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon_menu";
            tableType1.transform.parent.parent.gameObject.SetActive(true);
            tableType2.transform.parent.parent.gameObject.SetActive(false);
            StartCoroutine(initShowRowType1());
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
            APILobby.SetSelectChannel(dataChannel[0], OnGetAllLobbyInChannel);

        }
        else
            Logger.LogError(message);
    }

    void OnGetAllLobbyInChannel(bool status, string message, List<DataLobby> data)
    {
        if (status)
        {
            this.lobbies = data;
            StartCoroutine(initShowRowType1());
        }
        else
            Logger.LogError(message);
    }

}
