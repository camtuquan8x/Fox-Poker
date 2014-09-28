using Puppet;
using Puppet.API.Client;
using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class WorldGamePresenter : IWorldGamePresenter
{
    public WorldGamePresenter(IWorldGameView view) {
        this.view = view;
        ViewStart();
    }
    public void GetGameItem()
    {
        APIWorldGame.GetListGame(onGetListGame);
    }

    private void onGetListGame(bool status, string message, List<DataGame> data)
    {
        if (status) {
            view.OnLoadGame(data);
        }
    }

    public void ViewStart()
    {
        GetGameItem();
        UserInfo info = APIUser.GetUserInformation();
        view.ShowUserName(info.info.userName);
        view.ShowChip(info.assets.content[0].value.ToString());
    }
    public void ViewEnd()
    {

    }
    public IWorldGameView view { get; set; }

    public void BackScene()
    {
        PuApp.Instance.BackScene();
    }
}

