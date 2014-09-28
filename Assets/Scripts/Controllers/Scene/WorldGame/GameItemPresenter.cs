using Puppet;
using Puppet.API.Client;
using Puppet.Core.Model;
using Puppet.Core.Network.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GameItemPresenter : IGameItemPresenter
{
    private DataGame data;
    public DataGame Data {
        get {
            return data;
        }
        set {
            this.data = value;
            LoadImage();
        }
    }
    public GameItemPresenter(IGameItemView view)
    {
        this.view = view;
    }

    public void JoinToGame()
    {
        APIWorldGame.JoinRoom(data, OnJoinRoomCallBack);
    }

    private void OnJoinRoomCallBack(bool status, string message)
    {
        if (!status)
        {
            PuMain.Setting.Threading.QueueOnMainThread(() =>
            {
                view.ShowError(message);
            });
        }
    }

    public void LoadImage()
    {
        WWWRequest request = new WWWRequest((GameItem)view, data.icon, 30f, 0);
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

}

