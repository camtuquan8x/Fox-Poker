
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using Puppet.Service;
using Puppet;
using Puppet.Core.Model;
using UnityEngine;


[PrefabAttribute(Name = "Prefabs/Dialog/UserInfo/DialogInfo", Depth = 7, IsAttachedToCamera = true, IsUIPanel = true)]
public class DialogInfoView : BaseDialog<DialogInfo,DialogInfoView>
{
	#region UnityEditor
	public UILabel lbUserName,lbChip,lbLevel,lbExp,lbMaxChip,lbWinMax,lbPrecentsWin, lbBetWin,lbStatictis;
	public UISlider sliderExp;
	public UITexture avatar;
	public GameObject btnViewStatictis,btnEditAvatar;
	protected override void OnEnable ()
	{
		base.OnEnable ();
		UIEventListener.Get (btnViewStatictis).onClick += onClickViewStatictis;
	}
	protected override void OnDisable ()
	{
		base.OnDisable ();
		UIEventListener.Get (btnViewStatictis).onClick -= onClickViewStatictis;
	}

	void onClickViewStatictis (GameObject go)
	{
		DialogService.Instance.ShowDialog (new DialogStatistic());
	}

	#endregion
	public override void ShowDialog (DialogInfo data)
	{
		base.ShowDialog (data);
		initData (data);
	}

	void initData (DialogInfo data)
	{
		lbUserName.text = data.info.info.userName;
		lbChip.text = data.info.assets.content [0].value.ToString ();
	}
}

public class DialogInfo : AbstractDialogData{
	public UserInfo info;
	public DialogInfo(UserInfo info) : base(){
		this.info = info;
	}
	public override void ShowDialog ()
	{
		DialogInfoView.Instance.ShowDialog (this);
	}
}
