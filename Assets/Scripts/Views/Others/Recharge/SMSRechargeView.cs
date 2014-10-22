using UnityEngine;
using System.Collections;
using System;

public class SMSRechargeView : MonoBehaviour {

	#region UnityEditor
	public UITexture texture;
	public UILabel lbMoney;
	#endregion

	Action<SMSRecharge> actionClick;

	SMSRecharge model;
	void OnEnable(){
		UIEventListener.Get (gameObject).onClick += OnClicked;
	}
	void OnDisable(){
		UIEventListener.Get (gameObject).onClick -= OnClicked;
	}
	
	void OnClicked (GameObject go)
	{
		if (actionClick != null)
			actionClick (model);
	}
	

	public void SetData(SMSRecharge model,Action<SMSRecharge> action){
		this.model = model;
		texture.mainTexture = this.model.texture;
		lbMoney.text = model.money;
		NGUITools.AddWidgetCollider (gameObject);
		this.actionClick = action;
	}

}
public class SMSRecharge{
	public string money;
	public Texture texture;
}