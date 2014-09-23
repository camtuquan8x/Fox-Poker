using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet;
using Puppet.Core.Model;
using Puppet.API.Client;

public class LobbyScene : MonoBehaviour
{
    public UIEventListener tabBeginner, tabAmature, tabProfessional, tabMaster;
    public UIEventListener btnType, btnSearch, btnBack;
    public UILabel txtUsername, txtChip;
    public UITable tableType1;
    public UITable tableType2;
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
        tabBeginner.onHover += OnTabHover;
        tabMaster.onHover += OnTabHover;
        tabProfessional.onHover += OnTabHover;
        tabAmature.onHover += OnTabHover;

        tabBeginner.onClick += OnClickChangeTab;
        tabAmature.onClick += OnClickChangeTab;
        tabProfessional.onClick += OnClickChangeTab;
        tabMaster.onClick += OnClickChangeTab;

        btnType.onClick += btnTypeLobbyClick;
        btnBack.onClick += OnClickBack;
    }

    void OnDisable()
    {
        tabBeginner.onHover -= OnTabHover;
        tabMaster.onHover -= OnTabHover;
        tabProfessional.onHover -= OnTabHover;
        tabAmature.onHover -= OnTabHover;

        tabBeginner.onClick -= OnClickChangeTab;
        tabAmature.onClick -= OnClickChangeTab;
        tabProfessional.onClick -= OnClickChangeTab;
        tabMaster.onClick -= OnClickChangeTab;

        btnType.onClick -= btnTypeLobbyClick;
        btnBack.onClick -= OnClickBack;
    }

    private void initShowRowType1()
    {
        while (types2.Count > 0)
        {
            GameObject.Destroy(types2[0].gameObject);
            types2.RemoveAt(0);
        }
       
        foreach (DataLobby item in lobbies)
        {
            types1.Add(LobbyRowType1.Create(item, tableType1));
        }
        tableType1.repositionNow = true;
        tableType1.GetComponent<UICenterOnChild>().CenterOn(tableType1.transform.GetChild(0));
    }
    private void initShowRowType2()
    {
        while (types1.Count > 0)
        {
            GameObject.Destroy(types1[0].gameObject);
            types1.RemoveAt(0);
        }
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
    void OnTabHover(GameObject go, bool state)
    {
        if (state)
        {
            go.GetComponent<UISprite>().spriteName = go.name.Contains("1") ? "tab_select_first" : "tab_select_center";
            go.GetComponent<UISprite>().MakePixelPerfect();
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, -38, go.transform.localPosition.z);
        }
        else
        {
            go.GetComponent<UISprite>().spriteName = go.name.Contains("1") ? "tab_normal_first" : "tab_normal_center";
            go.GetComponent<UISprite>().MakePixelPerfect();
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, -36, go.transform.localPosition.z);
        }
    }

    void OnClickBack(GameObject obj)
    {
        PuApp.Instance.BackScene();
    }

    void OnClickChangeTab(GameObject obj)
    {
        if (dataChannel.Count > 0)
        {
            if (obj == tabBeginner.gameObject)
                selectedChannel = dataChannel[0];
            else if (obj == tabAmature.gameObject)
                selectedChannel = dataChannel[1];
            else if (obj == tabProfessional.gameObject)
                selectedChannel = dataChannel[2];
            else if (obj == tabMaster.gameObject)
                selectedChannel = dataChannel[3];

            APILobby.SetSelectChannel(selectedChannel, OnGetAllLobbyInChannel);
        }
    }

    void OnGetGroupLobbyCallback(bool status, string message, List<DataChannel> data)
    {
        if (status)
        {
            dataChannel = data;
            OnClickChangeTab(tabBeginner.gameObject);
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
