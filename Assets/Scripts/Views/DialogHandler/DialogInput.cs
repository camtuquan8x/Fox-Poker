using UnityEngine;
using System.Collections;
using Puppet;
using System;

[PrefabAttribute(Name = "Prefabs/Dialog/DialogInput", Depth = 10, IsAttachedToCamera = true, IsUIPanel = true)]
public class DialogInput : SingletonPrefab<DialogInput>
{

    #region Unity Editor
    public UILabel title, message;
    public UIInput input;
    public UISprite backgroundTransparent;
    public GameObject btnConfirm, btnCancel, btnClose;
    #endregion
    Action<bool?,string> onClickButton;
	void Start () {
        UIPanel root = NGUITools.GetRoot(gameObject).GetComponent<UIPanel>();
        backgroundTransparent.SetAnchor(root.gameObject, 0, 0, 0, 0);
	}
    void OnEnable()
    {
        UIEventListener.Get(btnConfirm).onClick += onConfirmClickHandler;
        UIEventListener.Get(btnCancel).onClick += onCancelClickHandler;
        UIEventListener.Get(btnClose).onClick += onCloseClickHandler;
    }

    void OnDisable()
    {
        UIEventListener.Get(btnConfirm).onClick -= onConfirmClickHandler;
        UIEventListener.Get(btnCancel).onClick -= onCancelClickHandler;
        UIEventListener.Get(btnClose).onClick -= onCloseClickHandler;
    }
    private void onConfirmClickHandler(GameObject go)
    {
        if(string.IsNullOrEmpty( input.value)){

        }
        else
        {
            if (onClickButton != null)
                onClickButton(true, input.value);

            GameObject.Destroy(gameObject);
        }
        
    }
    private void onCancelClickHandler(GameObject go)
    {
        if (onClickButton != null)
            onClickButton(false,null);

        GameObject.Destroy(gameObject);
    }

    private void onCloseClickHandler(GameObject go)
    {
        if (onClickButton != null)
            onClickButton(null,null);

        GameObject.Destroy(gameObject);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
