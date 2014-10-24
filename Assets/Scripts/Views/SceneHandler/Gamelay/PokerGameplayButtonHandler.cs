using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet.Poker;
using System;
using Puppet.Poker.Models;
using Puppet.Poker.Datagram;
using Puppet;
using Puppet.Service;
using System.Linq;

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
        InGame = 3,
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

    DialogBetting bettingDialog;

    void Start()
    {
        bettingDialog = new DialogBetting(500, 2000, (money) =>
        {
            Puppet.API.Client.APIPokerGame.PlayRequest(PokerRequestPlay.RAISE, money);
        }, Array.Find<ButtonItem>(itemButtons, button => button.slot == EButtonSlot.Third).button.transform);

        SetEnableButtonType(EButtonType.OutGame);
    }

    void OnEnable()
    {
        PokerObserver.Instance.dataTurnGame += Instance_dataTurnGame;
        PokerObserver.Instance.onNewRound += Instance_onNewRound;
        PokerObserver.Instance.onFinishGame += Instance_onFinishGame;
        PokerObserver.Instance.onPlayerListChanged += Instance_onPlayerListChanged;

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
        PokerObserver.Instance.dataTurnGame -= Instance_dataTurnGame;
        PokerObserver.Instance.onNewRound -= Instance_onNewRound;
        PokerObserver.Instance.onFinishGame -= Instance_onFinishGame;
        PokerObserver.Instance.onPlayerListChanged -= Instance_onPlayerListChanged;

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
            Puppet.API.Client.APIPokerGame.PlayRequest(PokerRequestPlay.CALL, 0);
        }
    }
    void OnClickButton2(GameObject go)
    {
        if (currentType == EButtonType.InTurn)
        {
            Puppet.API.Client.APIPokerGame.PlayRequest(PokerRequestPlay.FOLD, 0);
        }
    }

    void OnClickButton3(GameObject go)
    {
        if (currentType == EButtonType.InTurn)
        {
            if(DialogService.Instance.IsShowing(bettingDialog) == false)
            {
                DialogService.Instance.ShowDialog(bettingDialog);
            }
        }
        else if(currentType == EButtonType.OutGame)
        {
            Puppet.API.Client.APIPokerGame.AutoSitDown();
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

    void Instance_onPlayerListChanged(ResponsePlayerListChanged data)
    {
        if (PokerObserver.Instance.IsMainPlayer(data.player.userName))
        {
            switch (data.GetActionState())
            {
                case PokerPlayerChangeAction.playerAdded:
                    SetEnableButtonType(EButtonType.InGame);
                    break;
                case PokerPlayerChangeAction.waitingPlayerAdded:
                    SetEnableButtonType(EButtonType.OutGame);
                    break;
            }
        }
    }

    void Instance_dataTurnGame(ResponseUpdateTurnChange data)
    {
        if (PokerObserver.Instance.IsMainPlayerInGame())
        {
            if (data.toPlayer != null)
            {
                ButtonItem selectedButton = Array.Find<ButtonItem>(itemButtons, button => button.toggle.value);

                SetEnableButtonType(PokerObserver.Instance.IsMainPlayer(data.toPlayer.userName) ? EButtonType.InTurn : EButtonType.OutTurn);

                if (selectedButton != null)
                {
                    if (selectedButton.slot == EButtonSlot.First)
                        OnClickButton1(selectedButton.button);
                    else if (selectedButton.slot == EButtonSlot.Second)
                        OnClickButton2(selectedButton.button);
                    else if (selectedButton.slot == EButtonSlot.Third)
                        OnClickButton1(selectedButton.button);

                    selectedButton.toggle.value = selectedButton.slot == EButtonSlot.Third;
                }
            }
            else
                SetEnableButtonType(EButtonType.InGame);
        }
    }

    void Instance_onNewRound(ResponseWaitingDealCard data)
    {
        if (PokerObserver.Instance.IsMainPlayerInGame())
            SetEnableButtonType(EButtonType.InGame);
    }

    void Instance_onFinishGame(ResponseFinishGame obj)
    {
        if (PokerObserver.Instance.IsMainPlayerInGame())
            SetEnableButtonType(EButtonType.InGame);
    }
}