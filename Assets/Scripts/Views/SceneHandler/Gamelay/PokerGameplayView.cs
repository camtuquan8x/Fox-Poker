using Puppet.Poker.Datagram;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PokerGameplayView : MonoBehaviour
{
    #region UnityEditor
    public GameObject btnView, btnLeaveTurn, btnAddBet, btnFollowBet, btnConvertMoney, btnGameMini, btnRule, btnSendMessage;
	public GameObject btnViewCheckBox, btnFollowBetCheckBox, btnFollowAllBetCheckbox;
    public UIInput txtMessage;
    public PokerGameplayPlaymat playmat;
    #endregion
    
    void Awake()
    {
		ShowOrHideCheckboxInButton (false);
    }

    void Start()
    {
		HeaderMenuView.Instance.ShowInGameplay ();	
        PokerObserver.Instance.StartGame();
    }

    void OnEnable() {


        //UIEventListener.Get(btnView).onClick += OnButtonViewClickCallBack;
        //UIEventListener.Get(btnLeaveTurn).onClick += OnButtonLeaveTurnClickCallBack;
        //UIEventListener.Get(btnAddBet).onClick += OnButtonAddBetClickCallBack;
        //UIEventListener.Get(btnFollowBet).onClick += OnButtonFollowBetClickCallBack;
        //UIEventListener.Get(btnConvertMoney).onClick += OnButtonConvertMoneyClickCallBack;
        //UIEventListener.Get(btnGameMini).onClick += OnButtonGameMiniClickCallBack;
        //UIEventListener.Get(btnRule).onClick += OnButtonRuleClickCallBack;
        //UIEventListener.Get(btnSendMessage).onClick += OnButtonSendMessageClickCallBack;
        //UIEventListener.Get(headerMenuBtnBack).onClick += OnButtonHeaderBackClickCallBack;
        //UIEventListener.Get(headerMenuBtnUp).onClick += OnButtonHeaderUpClickCallBack;
        //UIEventListener.Get(headerMenuBtnRecharge).onClick += OnButtonHeaderRechargeClickCallBack;
        //UIEventListener.Get(headerMenuBtnSettings).onClick += OnButtonHeaderSettingClickCallBack;

    }
    void OnDisable()
    {
        //UIEventListener.Get(btnView).onClick -= OnButtonViewClickCallBack;
        //UIEventListener.Get(btnLeaveTurn).onClick -= OnButtonLeaveTurnClickCallBack;
        //UIEventListener.Get(btnAddBet).onClick -= OnButtonAddBetClickCallBack;
        //UIEventListener.Get(btnFollowBet).onClick -= OnButtonFollowBetClickCallBack;
        //UIEventListener.Get(btnConvertMoney).onClick -= OnButtonConvertMoneyClickCallBack;
        //UIEventListener.Get(btnGameMini).onClick -= OnButtonGameMiniClickCallBack;
        //UIEventListener.Get(btnRule).onClick -= OnButtonRuleClickCallBack;
        //UIEventListener.Get(btnSendMessage).onClick -= OnButtonSendMessageClickCallBack;
        //UIEventListener.Get(headerMenuBtnBack).onClick -= OnButtonHeaderBackClickCallBack;
        //UIEventListener.Get(headerMenuBtnUp).onClick -= OnButtonHeaderUpClickCallBack;
        //UIEventListener.Get(headerMenuBtnRecharge).onClick -= OnButtonHeaderRechargeClickCallBack;
        //UIEventListener.Get(headerMenuBtnSettings).onClick -= OnButtonHeaderSettingClickCallBack;
    }

    void OnButtonQuitClick(GameObject go)
    {
    }

    private void OnButtonHeaderSettingClickCallBack(GameObject go)
    {
    }

    private void OnButtonHeaderRechargeClickCallBack(GameObject go)
    {
    }

    private void OnButtonHeaderUpClickCallBack(GameObject go)
    {
    }

    private void OnButtonHeaderBackClickCallBack(GameObject go)
    {
    }

    private void OnButtonSendMessageClickCallBack(GameObject go)
    {
    }

    private void OnButtonRuleClickCallBack(GameObject go)
    {
    }

    private void OnButtonGameMiniClickCallBack(GameObject go)
    {
    }

    private void OnButtonConvertMoneyClickCallBack(GameObject go)
    {
    }

    private void OnButtonFollowBetClickCallBack(GameObject go)
    {
    }

    private void OnButtonAddBetClickCallBack(GameObject go)
    {
    }

    private void OnButtonLeaveTurnClickCallBack(GameObject go)
    {
    }

    private void OnButtonViewClickCallBack(GameObject go)
    {
    }
	void ShowOrHideCheckboxInButton(bool isShow){
		if (!isShow) {
			btnViewCheckBox.transform.FindChild ("Checkbox").gameObject.SetActive (false);
			btnFollowBetCheckBox.transform.FindChild ("Checkbox").gameObject.SetActive (false);
			btnFollowAllBetCheckbox.transform.FindChild ("Checkbox").gameObject.SetActive (false);
		} else {
			
			btnViewCheckBox.transform.FindChild ("Checkbox").gameObject.SetActive (true);
			btnFollowBetCheckBox.transform.FindChild ("Checkbox").gameObject.SetActive (true);
			btnFollowAllBetCheckbox.transform.FindChild ("Checkbox").gameObject.SetActive (true);
		}
	}
}

