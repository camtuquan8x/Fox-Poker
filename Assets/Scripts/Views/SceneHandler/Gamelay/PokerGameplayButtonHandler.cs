using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet.Poker;
using System;
using Puppet.Poker.Models;
using Puppet.Poker.Datagram;
using Puppet;

public class PokerGameplayButtonHandler : MonoBehaviour 
{
    public ButtonStepData [] dataButtons;
    public ButtonItem[] itemButtons;

    public enum EButtonSlot
    {
        First = 0,
        Second = 1,
        Third = 2,
    }

    public enum EButtonType
    {
        InTurn = 0,
        OutTurn = 1,
        OutGame = 2,
    }

    [Serializable()]
    public class ButtonStepData
    {
        public string text;
        public EButtonSlot slot;
        public EButtonType type;
        public Vector3 labelPosition;
        public int labelFontSize;
        public bool enableCheckBox;
        public Vector3 checkBoxPosition;
    }

    [Serializable()]
    public class ButtonItem
    {
        public EButtonSlot slot;
        public GameObject button;
        public UILabel label;
        public UIToggle toggle;
        public GameObject checkboxObject;
    }

    EButtonType currentType;

    void Start()
    {
        SetEnableButtonType(EButtonType.OutGame);
    }

    void OnEnable()
    {
        PokerGameModel.Instance.dataTurnGame += Instance_dataTurnGame;

        foreach(ButtonItem item in itemButtons)
        {
            if (item.slot == EButtonSlot.First)
                UIEventListener.Get(item.button).onClick += OnClickButton1;
            else if(item.slot == EButtonSlot.Second)
                UIEventListener.Get(item.button).onClick += OnClickButton2;
            else if (item.slot == EButtonSlot.Third)
                UIEventListener.Get(item.button).onClick += OnClickButton3;
        }
    }
    void OnDisable()
    {
        PokerGameModel.Instance.dataTurnGame -= Instance_dataTurnGame;

        foreach (ButtonItem item in itemButtons)
        {
            if (item.slot == EButtonSlot.First)
                UIEventListener.Get(item.button).onClick -= OnClickButton1;
            else if (item.slot == EButtonSlot.Second)
                UIEventListener.Get(item.button).onClick -= OnClickButton2;
            else if (item.slot == EButtonSlot.Third)
                UIEventListener.Get(item.button).onClick -= OnClickButton3;
        }
    }

    void OnClickButton1(GameObject go)
    {
        if(currentType == EButtonType.InTurn)
        {
            Puppet.API.Client.APIPokerGame.PlayRequest(PokerRequestPlay.CALL, 500);
        }
    }
    void OnClickButton2(GameObject go)
    {
        if (currentType == EButtonType.InTurn)
        {
            Puppet.API.Client.APIPokerGame.PlayRequest(PokerRequestPlay.FOLD, 500);
        }
    }
    void OnClickButton3(GameObject go)
    {
        if (currentType == EButtonType.InTurn)
        {
            Puppet.API.Client.APIPokerGame.PlayRequest(PokerRequestPlay.RAISE, 500);
        }
    }

    void SetEnableButtonType(EButtonType type)
    {
        this.currentType = type;
        ButtonStepData[] buttonData = Array.FindAll<ButtonStepData>(dataButtons, b => b.type == type);
        foreach(ButtonItem item in itemButtons)
        {
            ButtonStepData data = Array.Find<ButtonStepData>(buttonData, b => b.slot == item.slot);
            NGUITools.SetActive(item.button, data != null);
            if (data != null)
            {
                item.label.text = data.text;
                item.label.fontSize = data.labelFontSize;
                item.label.transform.localPosition = data.labelPosition;
                NGUITools.SetActive(item.checkboxObject.gameObject, data.enableCheckBox);
                item.checkboxObject.transform.localPosition = data.checkBoxPosition;
                item.button.GetComponent<UIToggle>().enabled = data.enableCheckBox;
            }
        }
    }

    void Instance_dataTurnGame(ResponseUpdateTurnChange data)
    {
        SetEnableButtonType(data.toPlayer.userName == PokerGameModel.Instance.mUserInfo.info.userName ? EButtonType.InTurn : EButtonType.OutTurn);
    }

}