﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet;
using Puppet.Core.Model;
using Puppet.API.Client;
using Puppet.Service;

public class LobbyScene : MonoBehaviour,ILobbyView
{
    public GameObject btnType, btnSearch, btnBack, btnCreateGame;
    public UILabel txtUsername, txtChip;
	public UITable tableType1,tableType2,tableTab;
    bool isShowType1 = true;

    List<LobbyRowType1> types1 = new List<LobbyRowType1>();
    List<LobbyRowType2> types2 = new List<LobbyRowType2>();
    List<LobbyTab> tabs = new List<LobbyTab>();
    void Start()
    {
        presenter = new PokerLobbyPresenter(this);
    }
    void OnDestroy() {
        presenter.ViewEnd();
    }
    void FixedUpdate() {
        if(types1.Count == 0)
        {
            btnCreateGame.SetActive(true);
            return;
        }
        if (tableType1.GetComponent<UICenterOnChild>().centeredObject != null) {
            btnCreateGame.SetActive(tableType1.GetComponent<UICenterOnChild>().centeredObject.GetComponent<LobbyRowType1>().data.roomId == types1[0].data.roomId);
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
    IEnumerator initShowRowType1(List<DataLobby> lobbies)
    {
		ClearAllRow ();
        yield return new WaitForEndOfFrame();
        foreach (DataLobby item in lobbies)
        {
            types1.Add(LobbyRowType1.Create(item, tableType1));
        }
        tableType1.repositionNow = true;
        tableType1.GetComponent<UICenterOnChild>().onFinished += onGotoCenterType1;
    }
    IEnumerator initShowRowType2(List<DataLobby> lobbies)
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
            StartCoroutine(initShowRowType2(presenter.Lobbies));
        }
        else
        {
            isShowType1 = true;
            go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon_menu";
            tableType1.transform.parent.parent.gameObject.SetActive(true);
            tableType2.transform.parent.parent.gameObject.SetActive(false);
            StartCoroutine(initShowRowType1(presenter.Lobbies));
        }
       
    }

    void OnClickBack(GameObject obj)
    {
        presenter.BackScene();
    }

    public void DrawChannels(List<DataChannel> channels)
    {
        for (int i = 0; i < channels.Count; i++)
        {
            LobbyTab tab = LobbyTab.Create(channels[i], tableTab, i);
            tab.SetEventChoiceTab(delegate(DataChannel channel)
            {
                presenter.LoadLobbiesByChannel(channel);
            });
            tabs.Add(tab);
        }
        tableTab.Reposition();
        Vector3 currentPosition = tableTab.transform.localPosition;
        tableTab.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y - 2, currentPosition.z);
    }

    public void DrawLobbies(List<DataLobby> lobbies)
    {
        tabs.Find(s => s.data.name == presenter.SelectedChannel.name).GetComponent<UIToggle>().value = true;
        if (isShowType1){
            StartCoroutine(initShowRowType1(lobbies));
        }
        else
        {
            StartCoroutine(initShowRowType2(lobbies));
        }
    }

    public void RemoveLobby(DataLobby lobbies)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateLobby(DataLobby lobbies)
    {
        throw new System.NotImplementedException();
    }

    public void AddLobby(DataLobby lobbies)
    {
        throw new System.NotImplementedException();
    }

    public void ShowError(string message)
    {
        PuMain.Setting.Threading.QueueOnMainThread(() => { 
            DialogService.Instance.ShowDialog(new DialogMessage("Lỗi",message,null));
        });
    }

    public void ShowConfirm(string message, System.Action<bool?> action)
    {
        throw new System.NotImplementedException();
    }

    public PokerLobbyPresenter presenter { get; set; }

    public void ShowUserName(string userName)
    {
        txtUsername.text = userName;

    }

    public void ShowMoney(string money)
    {
        txtChip.text = money;
    }

    public void JoinGame(DataLobby lobby)
    {
        presenter.JoinToGame(lobby);
    }
}
