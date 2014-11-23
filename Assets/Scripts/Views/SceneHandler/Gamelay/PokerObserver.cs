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
    public event Action<ResponseUpdateRoomMaster> onUpdateRoomMaster;

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

            foreach (PokerPlayerController p in dataGame.players) if (IsMainPlayer(p.userName)) { mainPlayer = p; break; }

            if (command == "updateGame" && dataUpdateGameChange != null)
            {
                dataUpdateGameChange(dataGame);
            }
            else if (command == "updateGameToWaitingPlayer")
            {
                gameDetails = dataGame.gameDetails;
                ResetCurrentBetting();
                if (onFirstJoinGame != null)
                    onFirstJoinGame(dataGame);
            }
        }
        else if (data is ResponsePlayerListChanged)
        {
            ResponsePlayerListChanged dataPlayerChange = (ResponsePlayerListChanged)data;
            if (onPlayerListChanged != null)
                onPlayerListChanged(dataPlayerChange);
        }
        else if (data is ResponseUpdateHand && onEventUpdateHand != null)
            onEventUpdateHand((ResponseUpdateHand)data);
        else if (data is ResponseUpdateTurnChange)
        {
            ResponseUpdateTurnChange dataTurn = (ResponseUpdateTurnChange)data;

            if (dataTurn != null)
            {
                currentPlayer = dataTurn.toPlayer;
                lastPlayer = dataTurn.fromPlayer;
                if (currentPlayer != null && IsMainPlayer(currentPlayer.userName))
                    mainPlayer = dataTurn.toPlayer;

                if (currentPlayer != null)
                    MaxCurrentBetting = currentPlayer.currentBet;
                if (lastPlayer != null)
                    MaxCurrentBetting = lastPlayer.currentBet;
                if (dataTurn.firstTurn && dataTurn.bigBlind != null)
                    MaxCurrentBetting = dataTurn.bigBlind.currentBet;
            }

            if (onTurnChange != null)
                onTurnChange(dataTurn);
        }
        else if (data is ResponseFinishGame && onFinishGame != null)
            onFinishGame((ResponseFinishGame)data);
        else if (data is ResponseWaitingDealCard && onNewRound != null)
            onNewRound((ResponseWaitingDealCard)data);
        else if (data is ResponseUpdateRoomMaster && onUpdateRoomMaster != null)
            onUpdateRoomMaster((ResponseUpdateRoomMaster)data);
        else if (data is ResponseUpdatePot)
        {
            ResetCurrentBetting();
            if (onUpdatePot != null)
                onUpdatePot((ResponseUpdatePot)data);
        }
        else if (data is ResponseUpdateUserInfo)
        {
            ResponseUpdateUserInfo dataUserInfo = (ResponseUpdateUserInfo)data;

            if (IsMainPlayer(dataUserInfo.userInfo.userName))
                mainPlayer = dataUserInfo.userInfo;

            if (onUpdateUserInfo != null)
                onUpdateUserInfo(dataUserInfo);
        }
        else if (data is ResponseError && onEncounterError != null)
            onEncounterError((ResponseError)data);
    }

    #region HANDLE BUTTON
    public void QuitGame()
    {
        APIGeneric.BackScene(null);
        _instance = null;
    }

    public void SitDown(int slotServer, double betting)
    {
        APIPokerGame.SitDown(slotServer, betting);
    }
    #endregion

    #region PLAYER
    public bool IsMainPlayer(string userName)
    {
        return mUserInfo.info.userName == userName;
    }

    public bool IsMainPlayerInGame()
    {
        return pokerGame.ListPlayer.Find(p => p.userName == mUserInfo.info.userName) != null;
    }

    public bool IsMainTurn { get { try { return currentPlayer.userName == mainPlayer.userName; } catch { return false; } } }
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
        _maxCurrentBetting = 0;

        if(mainPlayer != null)
            mainPlayer.currentBet = 0;
        if (lastPlayer != null)
            lastPlayer.currentBet = 0;
        if (currentPlayer != null)
            currentPlayer.currentBet = 0;
    }

    public double CurrentBettingDiff
    {
        get
        {
            double leftMoney = currentPlayer.GetMoney();
            double diff = PokerObserver.Instance.MaxCurrentBetting - PokerObserver.Instance.mainPlayer.currentBet;
            return leftMoney > diff ? diff : leftMoney;
        }
    }
    #endregion
}
