using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet.Poker;
using Puppet.Poker.Datagram;
using System;
using Puppet.API.Client;
using Puppet;
using Puppet.Core.Model;

public class PokerObserver
{
    public event Action<ResponseUpdateGame> dataFirstJoinGame;
    public event Action<ResponseUpdateGame> dataUpdateGameChange;
    public event Action<ResponsePlayerListChanged> dataPlayerListChanged;
    public event Action<ResponseUpdateHand> onEventUpdateHand;
    public event Action<ResponseUpdateTurnChange> dataTurnGame;
    public event Action<ResponseFinishGame> onFinishGame;
    public event Action<ResponseWaitingDealCard> onNewRound;
    public event Action<ResponseUpdatePot> onUpdatePot;

    public List<string> listPlayers = new List<string>();
    public List<string> listWaitingPlayers = new List<string>();

    public PokerGameplay pokerGame;
    public UserInfo mUserInfo;

    static PokerObserver _instance;
    public static PokerObserver Instance
    {
        get 
        {
            if (_instance == null)
                _instance = new PokerObserver();
            return _instance; 
        }
    }

    public void StartGame()
    {
        mUserInfo = Puppet.API.Client.APIUser.GetUserInformation();
        Puppet.Poker.EventDispatcher.onGameEvent += EventDispatcher_onGameEvent;
        pokerGame = Puppet.API.Client.APIPokerGame.GetPokerGameplay();
        Puppet.API.Client.APIPokerGame.StartListenerEvent();
    }

    void EventDispatcher_onGameEvent(string command, object data)
    {
        if (data is ResponseUpdateGame)
        {
            if (command == "updateGame" && dataUpdateGameChange != null)
                dataUpdateGameChange((ResponseUpdateGame)data);
            else if (command == "updateGameToWaitingPlayer" && dataFirstJoinGame != null)
                dataFirstJoinGame((ResponseUpdateGame)data);
        }
        else if (data is ResponsePlayerListChanged && dataPlayerListChanged != null)
        {
            ResponsePlayerListChanged dataPlayerChange = (ResponsePlayerListChanged)data;
            UpdatePlayerInRoom(dataPlayerChange);
            dataPlayerListChanged(dataPlayerChange);
        }
        else if (data is ResponseUpdateHand && onEventUpdateHand != null)
            onEventUpdateHand((ResponseUpdateHand)data);
        else if (data is ResponseUpdateTurnChange && dataTurnGame != null)
            dataTurnGame((ResponseUpdateTurnChange)data);
        else if (data is ResponseFinishGame && onFinishGame != null)
            onFinishGame((ResponseFinishGame)data);
        else if (data is ResponseWaitingDealCard && onNewRound != null)
            onNewRound((ResponseWaitingDealCard)data);
        else if (data is ResponseUpdatePot && onUpdatePot != null)
            onUpdatePot((ResponseUpdatePot)data);
    }

    void UpdatePlayerInRoom(ResponsePlayerListChanged dataPlayerChange)
    {
        switch (dataPlayerChange.GetActionState())
        {
            case PokerPlayerChangeAction.playerAdded:
                listPlayers.Add(dataPlayerChange.player.userName);
                break;
            case PokerPlayerChangeAction.playerRemoved:
                listPlayers.Remove(dataPlayerChange.player.userName);
                break;
            case PokerPlayerChangeAction.waitingPlayerAdded:
                listWaitingPlayers.Add(dataPlayerChange.player.userName);
                break;
            case PokerPlayerChangeAction.waitingPlayerRemoved:
                listWaitingPlayers.Remove(dataPlayerChange.player.userName);
                break;
        }
    }

    public void QuitGame()
    {
        APIGeneric.BackScene(null);
        _instance = null;
    }

    public void SitDown(int slotServer)
    {
        APIPokerGame.SitDown(slotServer);
    }

    public bool IsMainPlayer(string userName)
    {
        return mUserInfo.info.userName == userName;
    }

    public bool IsMainPlayerInGame()
    {
        return listPlayers.Contains(mUserInfo.info.userName);
    }
}
