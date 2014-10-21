using UnityEngine;
using System.Collections;
using System;

public class CardRechargeView : MonoBehaviour {
	#region UnityEditor
	public UITexture texture;
	#endregion	
	CardRecharge model;
	Action<CardRecharge> action;
	void OnEnable(){
		UIEventListener.Get (gameObject).onClick += OnClicked;
	}
	void OnDisable(){
		UIEventListener.Get (gameObject).onClick -= OnClicked;
	}
	public void SetData(CardRecharge model,Action<CardRecharge> action){
		this.model = model;
		texture.mainTexture = model.texture;
		this.action = action;
		NGUITools.AddWidgetCollider (gameObject);
	}

	void OnClicked (GameObject go)
	{
		if (action != null)
			action (model);
	}
}
public class CardRecharge{
	public Texture texture;
}
