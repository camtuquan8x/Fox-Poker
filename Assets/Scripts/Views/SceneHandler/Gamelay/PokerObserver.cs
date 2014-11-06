using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet.Poker;
using Puppet.Poker.Datagram;
using System;
using Puppet.API.Client;
using Puppet;
using Puppet.Core.Model;
using Puppet.Poker.Models;

public class PokerObserver
{
    public double[] arrBettings;
    public PokerPlayerController playerData;

    public event Action<ResponseUpdateGame> onFirstJoinGame;
    public event Action<ResponseUpdateGame> dataUpdateGameChange;
    public event Action<ResponsePlayerListChanged> onPlayerListChanged;
    public event Action<ResponseUpdateHand> onEventUpdateHand;
    public event Action<ResponseUpdateTurnChange> dataTurnGame;
    public event Action<ResponseFinishGame> onFinishGame;
    public event Action<ResponseWaitingDealCard> onNewRound;
    public event Action<ResponseUpdatePot> onUpdatePot;
    public event Action<ResponseUpdateUserInfo> onUpdateUserInfo;
    public event Action<ResponseError> onEncounterError;

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
            ResponseUpdateGame dataGame = (ResponseUpdateGame)data;
            if (command == "updateGame" && dataUpdateGameChange != null)
                dataUpdateGameChange(dataGame);
            else if (command == "updateGameToWaitingPlayer" && onFirstJoinGame != null)
            {
                LastBetting = dataGame.gameDetails.customConfiguration.betting / 2;
                arrBettings = dataGame.gameDetails.configuration.betting;

                string str = string.Empty;
                foreach(int i in arrBettings)
                    str += i + ",";
                Logger.Log(str);

                onFirstJoinGame(dataGame);
            }
        }
        else if (data is ResponsePlayerListChanged && onPlayerListChanged != null)
        {
            ResponsePlayerListChanged dataPlayerChange = (ResponsePlayerListChanged)data;
            UpdatePlayerInRoom(dataPlayerChange);
            onPlayerListChanged(dataPlayerChange);
        }
        else if (data is ResponseUpdateHand && onEventUpdateHand != null)
            onEventUpdateHand((ResponseUpdateHand)data);
        else if (data is ResponseUpdateTurnChange && dataTurnGame != null)
        {
            ResponseUpdateTurnChange dataTurn = (ResponseUpdateTurnChange)data;
            if (dataTurn.toPlayer != null && PokerObserver.Instance.IsMainPlayer(dataTurn.toPlayer.userName))
                PokerObserver.Instance.playerData = dataTurn.toPlayer;
            else if (dataTurn.fromPlayer != null && dataTurn.fromPlayer.currentBet > 0)
                PokerObserver.Instance.LastBetting = dataTurn.fromPlayer.currentBet;

            dataTurnGame(dataTurn);
        }
        else if (data is ResponseFinishGame && onFinishGame != null)
            onFinishGame((ResponseFinishGame)data);
        else if (data is ResponseWaitingDealCard && onNewRound != null)
            onNewRound((ResponseWaitingDealCard)data);
        else if (data is ResponseUpdatePot && onUpdatePot != null)
            onUpdatePot((ResponseUpdatePot)data);
        else if (data is ResponseUpdateUserInfo && onUpdateUserInfo != null)
            onUpdateUserInfo((ResponseUpdateUserInfo)data);
        else if (data is ResponseError && onEncounterError != null)
            onEncounterError((ResponseError)data);
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
        APIPokerGame.SitDown(slotServer, 10000);
    }

    public bool IsMainPlayer(string userName)
    {
        return mUserInfo.info.userName == userName;
    }

    public bool IsMainPlayerInGame()
    {
        return listPlayers.Contains(mUserInfo.info.userName);
    }

    double _lastBetting;
    double _maxBetting;
    public double LastBetting
    {
        get
        {
            return _lastBetting;
        }
        set
        {
            _lastBetting = value;
            if (LastBetting > MaxBetting)
                _maxBetting = _lastBetting;
        }
    }
    public double MaxBetting
    {
        get { return _maxBetting; }
    }
    
}
