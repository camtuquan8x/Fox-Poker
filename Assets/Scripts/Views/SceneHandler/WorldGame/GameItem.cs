using UnityEngine;
using System.Collections;
using Puppet.Core.Model;
using Puppet.Core.Network.Http;
using Puppet;

public class GameItem : MonoBehaviour
{
    #region Unity Editor
    public UITexture icon;
    #endregion
    void Start()
    {
        UIEventListener.Get(gameObject).onClick += onClickByMe;
    }

    private void onClickByMe(GameObject go)
    {
        Puppet.API.Client.APIWorldGame.JoinRoom(data, onJoinRoomCallBack);
    }

    private void onJoinRoomCallBack(bool status, string message)
    {

    }
    public static GameItem Create(DataGame data, Transform parent)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/WorldGame/GameItem"));
        obj.name = "00" + data.roomId + " - " + data.roomName;
        obj.transform.parent = parent;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        GameItem item = obj.GetComponent<GameItem>();
        item.setData(data);
        return item;
    }
    public void setData(DataGame data)
    {
        this.data = data;
        LoadImage();
    }

    public DataGame data { get; set; }
    void LoadImage()
    {
        WWWRequest request = new WWWRequest(this, data.icon, 30f, 0);
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
                icon.mainTexture = texture;
                icon.MakePixelPerfect(); 
                NGUITools.AddWidgetCollider(gameObject);
                gameObject.transform.parent.GetComponent<UITable>().Reposition();
            }
        };
        PuMain.WWWHandler.Request(request);
    }
}
