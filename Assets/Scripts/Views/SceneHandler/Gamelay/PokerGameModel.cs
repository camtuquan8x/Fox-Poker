using UnityEngine;
using System.Collections;
using Puppet.Poker;
using Puppet.Poker.Datagram;
using System;
using Puppet.API.Client;
using Puppet;

public class PokerGameModel
{
    public event Action<ResponseUpdateGame> dataFirstJoinGame;
    public event Action<ResponseUpdateGame> dataUpdateGameChange;
    public event Action<ResponsePlayerListChanged> dataPlayerListChanged;

    PokerGameplay pokerGame;

    static PokerGameModel _instance;
    public static void NewInstance()
    {
        _instance = new PokerGameModel();
    }
    public static PokerGameModel Instance
    {
        get { return _instance; }
    }

    public void StartGame()
    {
        Puppet.Poker.EventDispatcher.onGameEvent += EventDispatcher_onGameEvent;
        pokerGame = Puppet.API.Client.APIPokerGame.GetGameplay();
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
            dataPlayerListChanged((ResponsePlayerListChanged)data);
    }

    public void QuitGame()
    {
        APIGeneric.BackScene(null);
    }

    public void SitDown(int slotServer)
    {
        APIPokerGame.SitDown(slotServer);
    }
}
