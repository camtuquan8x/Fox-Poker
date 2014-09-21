using UnityEngine;
using System.Collections;
using Puppet.Core.Model;
using Puppet;

public class LobbyRowType1 : MonoBehaviour
{
    #region Unity Editor
    public GameObject[] slots;
    public UILabel title;
    #endregion
    public static LobbyRowType1 Create(DataLobby data, UITable parent)
    {
        GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Lobby/LobbyRowType1")) as GameObject;
        go.transform.parent = parent.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.GetComponent<UIDragScrollView>().scrollView = parent.GetComponentInParent<UIScrollView>();
        go.name = data.displayName;
        LobbyRowType1 item = go.GetComponent<LobbyRowType1>();
        item.setData(data);
        return item;
    }
    public void setData(DataLobby lobby)
    {
        this.lobby = lobby;
        title.text = lobby.displayName;
    }
    void Start()
    {
        gameObject.GetComponent<UIEventListener>().onClick += onTableClick;
        NGUITools.FindInParents<UICenterOnChild>(gameObject).onCenter += onParentSetCenterToChild;
    }

    
    void OnDestroy()
    {
        gameObject.GetComponent<UIEventListener>().onClick -= onTableClick;
        NGUITools.FindInParents<UICenterOnChild>(gameObject).onCenter -= onParentSetCenterToChild;
    }
    private void onTableClick(GameObject go)
    {

    }

    private void onParentSetCenterToChild(GameObject centeredObject)
    {
        float value = Mathf.Lerp(0.731f, 1.0f, 0.2f / Vector3.SqrMagnitude(gameObject.transform.position - centeredObject.transform.position));
        gameObject.transform.localScale = new Vector3(value, value, 1f);
        try
        {
            if (!gameObject.transform.name.Equals(centeredObject.transform.name))
            {
                gameObject.GetComponent<UISprite>().color = new Color(69f / 255f, 69f / 255f, 69f / 255f);
            }
            else
            {
                gameObject.GetComponent<UISprite>().color = new Color(1f, 1f, 1f);
            }
        }
        catch (System.Exception)
        {

            throw;
        }
    }
    public DataLobby lobby { get; set; }
}

