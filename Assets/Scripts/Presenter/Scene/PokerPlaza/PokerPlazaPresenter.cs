using Puppet.API.Client;
using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PokerPlazaPresenter : IPlazaPresenter
{
    public PokerPlazaPresenter(IPlazaView view)
    {
        Puppet.Poker.PokerMain.Instance.EnterPoker();
        this.view = view;
        ViewStart();
    }

    public void GetEvent()
    {
        throw new NotImplementedException();
    }

    public void JoinLobby()
    {
        Application.LoadLevel(Scene.LobbyScene.ToString());
    }

    public void PlayNow()
    {
        APIPlaza.Play();
    }

    public void GetListQuest()
    {
        throw new NotImplementedException();
    }

    public IPlazaView view { get; set; }


    public void ShowUserName()
    {
        UserInfo info = APIUser.GetUserInformation();
        view.ShowUserName(info.info.userName);
    }

    public void ShowMoney()
    {
        UserInfo info = APIUser.GetUserInformation();
		if(info.assets !=null && info.assets.content.Length > 0)
        	view.ShowMoney(info.assets.content[0].value.ToString());
    }



    public void JoinToEvent()
    {
        throw new NotImplementedException();
    }

    public void ViewStart()
    {
        ShowUserName();
        ShowMoney();
    }
    public void ViewEnd()
    {
        throw new NotImplementedException();
    }

    public void BackScene()
    {
        PuApp.Instance.BackScene();
    }
}

