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
    Action onDestroyDialog;
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
    void OnDestroy() {
        if (onDestroyDialog != null)
            onDestroyDialog();
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

    public void ShowConfirm(DialogConfirmModel model, Action onDestroyDialog)
    {
        Logger.Log("chay vao show confirm không em ơi");
        this.title.text = model.Title;
        this.message.text = model.Content;
        this.onClickButton = model.OnButtonClick;
        this.onDestroyDialog = onDestroyDialog;
    }

    
}
