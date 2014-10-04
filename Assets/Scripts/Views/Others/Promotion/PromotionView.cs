using UnityEngine;
using System.Collections;

public class PromotionView : MonoBehaviour,IPromotionView {

	#region IPromotionView implementation

	PromotionPresenter presenter;
	public PromotionPresenter Presenter {
		set{
			this.presenter = value;
		}
		get{
			return presenter;
		}
	}
	public static void newInstance (IPromotionPresenter pre)
	{
		GameObject gobj = GameObject.Instantiate(Resources.Load("/Prefabs/Others/PromotionItem")) as GameObject;
		PromotionView view = gobj.GetComponent<PromotionView> ();
		view.Presenter = pre as PromotionPresenter;
		view.Presenter.View = view;

	}

	#endregion



	#region Unity Editor
	public UILabel lbTitle,lbMoney,lbMoneyShortcut;
	#endregion

	void OnEnable(){
		UIEventListener.Get (gameObject).onClick += OnClickGetPromotion;
	}
	void OnDisable(){
		UIEventListener.Get (gameObject).onClick -= OnClickGetPromotion;
	}

	#region IPromotionView implementation

	public void ShowInfo (string day, string money, string shortcut)
	{
		lbTitle.text = day;
		lbMoney.text = money;
		lbMoneyShortcut.text = shortcut;
	}

	public void ShowBackground (string name)
	{
		gameObject.GetComponent<UISprite> ().spriteName = name;
		gameObject.GetComponent<UISpriteAnimation> ().namePrefix = getNamePrefix(name);
	}
	string getNamePrefix(string name){
		string[] names = name.Split('_');
		string namePrefix = "";
		for (int i = 0; i < names.Length-1; i++) {
			namePrefix += name[i] + "_";
		}
		return namePrefix;
	}

	public void ShowAnmation (bool show)
	{
		gameObject.GetComponent<UISpriteAnimation> ().enabled = show;
	}


	void OnClickGetPromotion (GameObject go)
	{
		presenter.GetPromotion ();
	}
	#endregion
}
