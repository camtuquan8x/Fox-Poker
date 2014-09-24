using UnityEngine;
using System.Collections;
using System;
using Puppet;

[PrefabAttribute(Name = "Prefabs/Dialog/DialogConfirm", Depth = 10, IsAttachedToCamera = true, IsUIPanel = true)]
public class DialogConfirm : SingletonPrefab<DialogConfirm>
{
    #region UnityEditor
    public UILabel title, message;
    public GameObject btnConfirm, btnCancel, btnClose;
    #endregion

    Action<bool?> onClickButton;

    void OnEnable() {
        UIEventListener.Get(btnConfirm).onClick += onConfirmClickHandler;
        UIEventListener.Get(btnCancel).onClick += onCancelClickHandler;
        UIEventListener.Get(btnClose).onClick += onCloseClickHandler;
    }

    void OnDisable() {
        UIEventListener.Get(btnConfirm).onClick -= onConfirmClickHandler;
        UIEventListener.Get(btnCancel).onClick -= onCancelClickHandler;
        UIEventListener.Get(btnClose).onClick -= onCloseClickHandler;
    }

    private void onConfirmClickHandler(GameObject go)
    {
        if (onClickButton != null)
            onClickButton(true);

        GameObject.Destroy(gameObject);
    }
    private void onCancelClickHandler(GameObject go)
    {
        if (onClickButton != null)
            onClickButton(false);

        GameObject.Destroy(gameObject);
    }

    private void onCloseClickHandler(GameObject go)
    {
        if (onClickButton != null)
            onClickButton(null);

        GameObject.Destroy(gameObject);
    }

	public void ShowConfirm(string title, string content, Action<bool?> onClickButton)
    {
        this.title.text = title;
        this.message.text = content;
        this.onClickButton = onClickButton;
    }
}
