using Puppet;
using Puppet.Core.Model;
using Puppet.Core.Network.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GameItemPresenter : IGameItemPresenter
{
    public GameItemPresenter(IGameItemView view,IWorldGamePresenter presenter)
    {
        this.view = view;
        this.worldgamepre = presenter;
    }

    public void JoinLobby(DataGame data)
    {
        worldgamepre.OnJoinGame(data);   
    }

    public void LoadImage(string url)
    {
        WWWRequest request = new WWWRequest((GameItem)view, url, 30f, 0);
        request.isFullUrl = true;
        request.onResponse += (IHttpRequest currentRequest, IHttpResponse currentResponse) =>
        {
            WWWResponse response = (WWWResponse)currentResponse;
            if (response.State == System.Net.HttpStatusCode.OK)
            {
                UnityEngine.Texture2D texture = response.www.texture;
                texture.filterMode = FilterMode.Point;
                texture.anisoLevel = 0;
                texture.wrapMode = TextureWrapMode.Clamp;
                view.ShowImage(texture);
               
            }
        };
        PuMain.WWWHandler.Request(request);
    }

    private IGameItemView view { get; set; }

    private IWorldGamePresenter worldgamepre { get; set; }
}

