using UnityEngine;
using System.Collections;
using Puppet.Core.Model;

public class GameItem : MonoBehaviour
{
    #region Unity Editor
    public UITexture icon, logo;
    #endregion
    void Start() {
        UIEventListener.Get(gameObject).onClick += onClickByMe;
    }

    private void onClickByMe(GameObject go)
    {
        Puppet.API.Client.APIWorldGame.JoinRoom(data, onJoinRoomCallBack);
    }

    private void onJoinRoomCallBack(bool status, string message)
    {
        
    }
    public static GameItem Create(DataGame data,Transform parent) {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/WorldGame/GameItem"));
        obj.name="00" + data.roomId + " - "+ data.roomName;
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
    }

    public DataGame data { get; set; }
}
