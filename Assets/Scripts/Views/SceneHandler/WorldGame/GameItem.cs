using UnityEngine;
using System.Collections;
using Puppet.Core.Model;
using Puppet.Core.Network.Http;
using Puppet;
using Puppet.Service;

public class GameItem : MonoBehaviour,IGameItemView
{
    #region Unity Editor
    public UITexture icon;
    #endregion
    void Start()
    {
        
    
    }
    void OnEnable() {
        UIEventListener.Get(gameObject).onClick += onClickToMe;
    }
    void OnDisable()
    {
        UIEventListener.Get(gameObject).onClick -= onClickToMe;
    }

    private void onClickToMe(GameObject go)
    {
        presenter.JoinToGame();
    }
    public static GameItem Create(DataGame data, Transform parent)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/WorldGame/GameItem"));
        obj.name = "00" + data.roomId + " - " + data.roomName;
        obj.transform.parent = parent;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        GameItem item = obj.GetComponent<GameItem>();
        item.Presenter = new GameItemPresenter(item);
        item.setData(data);
        return item;
    }
    public void setData(DataGame data)
    {
        presenter.Data = data;
    }

    

    public void ShowImage(Texture2D texture)
    {
        icon.mainTexture = texture;
        icon.MakePixelPerfect();
        NGUITools.AddWidgetCollider(gameObject);
        gameObject.transform.parent.GetComponent<UITable>().Reposition();
    }


    public void ShowError(string message)
    {
        DialogService.Instance.ShowDialog(new DialogMessage("Lỗi", message, null));
    }

    public void ShowConfirm(string message, System.Action<bool?> action)
    {

    }
    private GameItemPresenter presenter;
    public GameItemPresenter Presenter {
        get { return presenter; }
        set {
            this.presenter = value;
        }
    }
}
