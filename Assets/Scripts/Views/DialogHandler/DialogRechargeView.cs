using UnityEngine;
using System.Collections;
using Puppet.Service;
using System.Collections.Generic;

public class DialogRechargeView : BaseDialog<DialogRecharge,DialogRechargeView> {

	#region UnityEditor
		#region Tab Recharge
			public UITable tableTypeRecharge;
			public UITable tableRechargeItem;
		#endregion
	#endregion
	public override void ShowDialog (DialogRecharge data)
	{
		base.ShowDialog (data);
	}
	private void initTab(){
		foreach (RechargeTab item in data.tabs) {
			GameObject gobj = GameObject.Instantiate( Resources.Load("Prefabs/Dialog/Recharge/RechargeChipTabType")) as GameObject;
			gobj.transform.parent = tableTypeRecharge;
			gobj.transform.localScale = Vector3.one;
			gobj.transform.localPosition = Vector3.zero;
		}
	}

}
public class DialogRecharge : AbstractDialogData{
	public List<RechargeTab> tabs;
	public List<CardRecharge> cards;
	public List<SMSRecharge> sms;
	public DialogRecharge(List<RechargeTab> tabs,List<CardRecharge> cards,List<SMSRecharge> sms):base(){
		this.tabs = tabs;
		this.cards = cards;
		this.sms = sms;
	}
	public override void ShowDialog ()
	{
		DialogRechargeView.Instance.ShowDialog (this);
	}
}
