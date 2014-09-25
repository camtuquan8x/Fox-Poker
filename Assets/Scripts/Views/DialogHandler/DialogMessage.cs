using UnityEngine;
using System.Collections;
using Puppet;

[PrefabAttribute(Name = "Prefabs/Dialog/DialogMessage", Depth = 10, IsAttachedToCamera = true, IsUIPanel = true)]
public class DialogMessage : SingletonPrefab<DialogMessage>
{

    #region Unity Editor
    public UILabel title, content;
    public UISprite backgroundTransparent;
    public GameObject btnClose;
    #endregion
    // Use this for initialization
	void Start () {
        UIPanel root = NGUITools.GetRoot(gameObject).GetComponent<UIPanel>();
        backgroundTransparent.SetAnchor(root.gameObject, 0, 0, 0, 0);
	}
    void OnEnable() {
        UIEventListener.Get(btnClose).onClick += OnCloseDialog;
    }
    void OnDisable() {
        UIEventListener.Get(btnClose).onClick -= OnCloseDialog;
    }
    private void OnCloseDialog(GameObject go)
    {
        GameObject.Destroy(gameObject);
    }
}
