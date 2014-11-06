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
    /// <summary>
    /// Info main player
    /// </summary>
    public PokerPlayerController mainPlayer;
    /// <summary>
    /// Info player in current turn
    /// </summary>
    public PokerPlayerController lastPlayer, currentPlayer;
    /// <summary>
    /// Info Setting Poker Game
    /// </summary>
    public PokerGameDetails gameDetails;



    public event Action<ResponseUpdateGame> onFirstJoinGame;
    public event Action<ResponseUpdateGame> dataUpdateGameChange;
    public event Action<ResponsePlayerListChanged> onPlayerListChanged;
    public event Action<ResponseUpdateHand> onEventUpdateHand;
    public event Action<ResponseUpdateTurnChange> onTurnChange;
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
                gameDetails = dataGame.gameDetails;
                ResetCurrentBetting();
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
        else if (data is ResponseUpdateTurnChange && onTurnChange != null)
        {
            ResponseUpdateTurnChange dataTurn = (ResponseUpdateTurnChange)data;

            if (dataTurn != null)
            {
                currentPlayer = dataTurn.toPlayer;
                lastPlayer = dataTurn.fromPlayer;
                if (currentPlayer != null && IsMainPlayer(currentPlayer.userName))
                    mainPlayer = dataTurn.toPlayer;    
            }

            if(onTurnChange != null)
                onTurnChange(dataTurn);
        }
        else if (data is ResponseFinishGame && onFinishGame != null)
            onFinishGame((ResponseFinishGame)data);
        else if (data is ResponseWaitingDealCard && onNewRound != null)
            onNewRound((ResponseWaitingDealCard)data);
        else if (data is ResponseUpdatePot && onUpdatePot != null)
        {
            ResetCurrentBetting();
            onUpdatePot((ResponseUpdatePot)data);
        }
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

    #region HANDLE BUTTON
    public void QuitGame()
    {
        APIGeneric.BackScene(null);
        _instance = null;
    }

    public void SitDown(int slotServer)
    {
        APIPokerGame.SitDown(slotServer, gameDetails.customConfiguration.SmallBlind * 20);
    }
    #endregion

    #region PLAYER
    public bool IsMainPlayer(string userName)
    {
        return mUserInfo.info.userName == userName;
    }

    public bool IsMainPlayerInGame()
    {
        return listPlayers.Contains(mUserInfo.info.userName);
    }

    public bool IsMainTurn { get { return currentPlayer.userName == mainPlayer.userName; } }
    #endregion

    #region BETTING VALUE
    double _maxCurrentBetting;
    public double MaxCurrentBetting
    {
        get { return _maxCurrentBetting; }
        set
        {
            if (_maxCurrentBetting < value)
                _maxCurrentBetting = value;
        }
    }
    void ResetCurrentBetting ()
    {
        _maxCurrentBetting = gameDetails.customConfiguration.SmallBlind;
    }

    public double Difference
    {
        get
        {
            double maxBettingInGame = PokerObserver.Instance.MaxCurrentBetting;
            double yourMoney = PokerObserver.Instance.mainPlayer.GetMoney();
            double pay = maxBettingInGame - PokerObserver.Instance.mainPlayer.currentBet;
            if (yourMoney < pay)
                pay = yourMoney;
            return pay;
        }
    }
    #endregion
}
