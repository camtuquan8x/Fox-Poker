﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PokerGameplayView : MonoBehaviour
{
    #region UnityEditor
    public GameObject btnView, btnLeaveTurn, btnAddBet, btnFollowBet, btnConvertMoney, btnGameMini, btnRule, btnSendMessage;
    public GameObject headerMenuBtnBack, headerMenuBtnUp, headerMenuBtnRecharge, headerMenuBtnSettings;
    public UIInput txtMessage;
    #endregion

    void OnEnable() {
        UIEventListener.Get(btnView).onClick += OnButtonViewClickCallBack;
        UIEventListener.Get(btnLeaveTurn).onClick += OnButtonLeaveTurnClickCallBack;
        UIEventListener.Get(btnAddBet).onClick += OnButtonAddBetClickCallBack;
        UIEventListener.Get(btnFollowBet).onClick += OnButtonFollowBetClickCallBack;
        UIEventListener.Get(btnConvertMoney).onClick += OnButtonConvertMoneyClickCallBack;
        UIEventListener.Get(btnGameMini).onClick += OnButtonGameMiniClickCallBack;
        UIEventListener.Get(btnRule).onClick += OnButtonRuleClickCallBack;
        UIEventListener.Get(btnSendMessage).onClick += OnButtonSendMessageClickCallBack;
        UIEventListener.Get(headerMenuBtnBack).onClick += OnButtonHeaderBackClickCallBack;
        UIEventListener.Get(headerMenuBtnUp).onClick += OnButtonHeaderUpClickCallBack;
        UIEventListener.Get(headerMenuBtnRecharge).onClick += OnButtonHeaderRechargeClickCallBack;
        UIEventListener.Get(headerMenuBtnSettings).onClick += OnButtonHeaderSettingClickCallBack;

    }
    void OnDisable()
    {
        UIEventListener.Get(btnView).onClick -= OnButtonViewClickCallBack;
        UIEventListener.Get(btnLeaveTurn).onClick -= OnButtonLeaveTurnClickCallBack;
        UIEventListener.Get(btnAddBet).onClick -= OnButtonAddBetClickCallBack;
        UIEventListener.Get(btnFollowBet).onClick -= OnButtonFollowBetClickCallBack;
        UIEventListener.Get(btnConvertMoney).onClick -= OnButtonConvertMoneyClickCallBack;
        UIEventListener.Get(btnGameMini).onClick -= OnButtonGameMiniClickCallBack;
        UIEventListener.Get(btnRule).onClick -= OnButtonRuleClickCallBack;
        UIEventListener.Get(btnSendMessage).onClick -= OnButtonSendMessageClickCallBack;
        UIEventListener.Get(headerMenuBtnBack).onClick -= OnButtonHeaderBackClickCallBack;
        UIEventListener.Get(headerMenuBtnUp).onClick -= OnButtonHeaderUpClickCallBack;
        UIEventListener.Get(headerMenuBtnRecharge).onClick -= OnButtonHeaderRechargeClickCallBack;
        UIEventListener.Get(headerMenuBtnSettings).onClick -= OnButtonHeaderSettingClickCallBack;
    }
    private void OnButtonHeaderSettingClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonHeaderRechargeClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonHeaderUpClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonHeaderBackClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonSendMessageClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonRuleClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonGameMiniClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonConvertMoneyClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonFollowBetClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonAddBetClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonLeaveTurnClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }

    private void OnButtonViewClickCallBack(GameObject go)
    {
        throw new NotImplementedException();
    }
    
}
