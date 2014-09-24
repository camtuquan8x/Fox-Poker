using UnityEngine;
using System.Collections;

public delegate void ConfirmClick();
public delegate void CancelClick();
public class DialogConfirm : MonoBehaviour
{

    #region UnityEditor
    public UILabel title, message;
    public GameObject btnConfirm, btnCancel, btnClose;
    #endregion
    private event ConfirmClick OnClickConfirmListener;
    private event CancelClick OnClickCancelListener;

    public void SetOnClickConfirmListener(ConfirmClick OnClickConfirmListener)
    {
        this.OnClickConfirmListener = OnClickConfirmListener;
    }
    public void SetOnClickCancelListener(CancelClick OnClickCancelListener)
    {
        this.OnClickCancelListener = OnClickCancelListener;
    }
	void Start () {
	}
    
    void onEnable() {
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
        if (OnClickConfirmListener != null)
            OnClickConfirmListener();
    }
    private void onCancelClickHandler(GameObject go)
    {
        if (OnClickCancelListener != null)
            OnClickCancelListener();
    }

    private void onCloseClickHandler(GameObject go)
    {
        GameObject.Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
