using UnityEngine;
using System.Collections;

public class PokerGamePlayRule : MonoBehaviour {

	#region Unity Editor
	public UILabel lbPercent;
	public UISprite hightLight;
	void Start(){
		SetPercent (0f, false);
	}
	#endregion
	public void SetPercent(float percent,bool isShowHighLight){
		lbPercent.text = percent + "%";
		if (isShowHighLight)
			hightLight.alpha = 1f;
		else 
			hightLight.alpha = 0f;

	}
}
