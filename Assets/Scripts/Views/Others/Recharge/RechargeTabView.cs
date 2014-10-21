using UnityEngine;
using System.Collections;
using System;

public class RechargeTabView : MonoBehaviour {

	#region UnityEditor
	public UILabel lbTitle;
	#endregion

	private Action<bool,RechargeTab> actionSelectToTab;

	RechargeTab model;
	void OnEnable(){
		UIEventListener.Get (gameObject).onSelect += onSelectedValue;
	}
	void OnDisable(){
		UIEventListener.Get(gameObject).onSelect -= onSelectedValue;
	}
	public void SetData(RechargeTab model,Action<bool,RechargeTab> action){
		this.model = model;
		lbTitle.text = this.model.title;
		this.actionSelectToTab = action;
	}

	void onSelectedValue (GameObject go, bool state)
	{
		if (state) {
			lbTitle.color = Color.white;
		} else {
			lbTitle.color = new Color(11f/255f,73f/255f,102f/255f);
		}
		if (actionSelectToTab != null)
			actionSelectToTab (state, model);
	}
}
public class RechargeTab
{
	public string title;
	public int typeRecharge;
}
