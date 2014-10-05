using UnityEngine;
using System.Collections;
using Puppet;

public class PromotionView : MonoBehaviour,IPromotionView {
	#region Unity Editor
	public UILabel lbTitle,lbMoney,lbMoneyShortcut;
	#endregion

	#region IPromotionView implementation
	private string[] name = {"icon_promotion_first","icon_promotion_center","icon_promotion_last"};
	private string[] status = {"_active","_deactive"};
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
		GameObject gobj = GameObject.Instantiate(Resources.Load("Prefabs/Others/PromotionItem")) as GameObject;
		PromotionView view = gobj.GetComponent<PromotionView> ();
		view.Presenter = pre as PromotionPresenter;
		view.Presenter.View = view;


	}
	public void ShowActive ()
	{
		gameObject.GetComponent<UISprite> ().spriteName = name[presenter.index] + status[0];
		gameObject.GetComponent<UISprite> ().MakePixelPerfect ();
	}

	
	public void ShowDeactive ()
	{
		gameObject.GetComponent<UISprite> ().spriteName = name[presenter.index] + status[1];
		gameObject.GetComponent<UISprite> ().MakePixelPerfect ();
		gameObject.collider.enabled = false;
	}
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
		gameObject.GetComponent<UISpriteAnimation> ().namePrefix = name[presenter.index];
	}

	public void ShowBackground ()
	{
		gameObject.GetComponent<UISpriteAnimation> ().namePrefix = name[presenter.index];
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
