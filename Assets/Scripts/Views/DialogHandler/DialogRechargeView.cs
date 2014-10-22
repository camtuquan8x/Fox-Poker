using UnityEngine;
using System.Collections;
using Puppet.Service;
using System.Collections.Generic;

public class DialogRechargeView : BaseDialog<DialogRecharge,DialogRechargeView> {

	#region UnityEditor
		#region Tab Recharge
			public UITable tableTypeRecharge;
			public UIGrid tableRechargeItem;
		#endregion
	#endregion
	List<GameObject> lstRechargeGobj = new List<GameObject>();
	public override void ShowDialog (DialogRecharge data)
	{
		base.ShowDialog (data);

	}
	void Start(){
		initTab ();
	}
	private void initTab(){
		foreach (RechargeTab item in data.tabs) {
			GameObject gobj = GameObject.Instantiate( Resources.Load("Prefabs/Dialog/Recharge/RechargeChipTab")) as GameObject;
			gobj.transform.parent = tableTypeRecharge.transform;
			gobj.transform.localScale = Vector3.one;
			gobj.transform.localPosition = Vector3.zero;
			gobj.GetComponent<RechargeTabView>().SetData(item,OnTabRechargeSelected);
		}
		tableTypeRecharge.Reposition ();
		OnTabRechargeSelected (true, data.tabs [0]);
	}
	private void OnTabRechargeSelected(bool isSelected,RechargeTab tab){

		if(isSelected){
			while (lstRechargeGobj.Count > 0) {
				GameObject.Destroy(lstRechargeGobj[0]);
				lstRechargeGobj.RemoveAt(0);
			}
			if (tab.typeRecharge == 0) {
			
			foreach (SMSRecharge item in data.sms) {
				GameObject gobj = GameObject.Instantiate( Resources.Load("Prefabs/Dialog/Recharge/SMSItem")) as GameObject;
				gobj.transform.parent = tableTypeRecharge.transform;
				gobj.transform.localScale = Vector3.one;
				gobj.transform.localPosition = Vector3.zero;
				gobj.GetComponent<SMSRechargeView>().SetData(item,OnSMSClickListener);
				lstRechargeGobj.Add(gobj);
			}

			}else if(tab.typeRecharge == 1){
				foreach (CardRecharge item in data.cards) {
					GameObject gobj = GameObject.Instantiate( Resources.Load("Prefabs/Dialog/Recharge/CardItem")) as GameObject;
					gobj.transform.parent = tableTypeRecharge.transform;
					gobj.transform.localScale = Vector3.one;
					gobj.transform.localPosition = Vector3.zero;
					gobj.GetComponent<CardRechargeView>().SetData(item,OnCardClickListener);
					lstRechargeGobj.Add(gobj);
				}
			}
			tableRechargeItem.Reposition();
		}

	}
	private void OnSMSClickListener(SMSRecharge model){
		
	}
	private void OnCardClickListener(CardRecharge model){

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
	public DialogRecharge() : base(){
		tabs = new List<RechargeTab> ();
		sms = new List<SMSRecharge> ();
		cards = new List<CardRecharge> ();
		RechargeTab tabSMS = new RechargeTab ();
		tabSMS.title = "Nạp chip qua SMS";
		tabSMS.typeRecharge = 0;
		RechargeTab tabCard = new RechargeTab ();
		tabSMS.title = "Nạp chip qua thẻ cào";
		tabSMS.typeRecharge = 1;
		tabs.Add (tabSMS);
		tabs.Add (tabCard);

		SMSRecharge sms1 = new SMSRecharge ();
		sms1.money = "15k Chip";
		sms1.texture = (Texture)Resources.LoadAssetAtPath ("Art_Assets/Others/Recharge/sms_low",typeof(Texture));
		SMSRecharge sms2 = new SMSRecharge ();
		sms2.money = "25k Chip";
		sms2.texture = (Texture)Resources.LoadAssetAtPath ("Art_Assets/Others/Recharge/sms_medium",typeof(Texture));
		SMSRecharge sms3 = new SMSRecharge ();
		sms3.money = "50k Chip";
		sms3.texture = (Texture)Resources.LoadAssetAtPath ("Art_Assets/Others/Recharge/sms_high",typeof(Texture));

		sms.Add (sms1);
		sms.Add (sms2);
		sms.Add (sms3);

	}
	public override void ShowDialog ()
	{
		DialogRechargeView.Instance.ShowDialog (this);
	}
}
