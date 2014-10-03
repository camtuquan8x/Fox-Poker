using UnityEngine;
using System.Collections;
using Puppet.Poker;
using Puppet.Poker.Datagram;
using System;
using Puppet.API.Client;
using Puppet;

public class PokerGameModel
{
    public event Action<ResponseUpdateGame> dataUpdateGameChange;
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
        if (pokerGame.dataUpdateGame != null && dataUpdateGameChange != null)
            dataUpdateGameChange(pokerGame.dataUpdateGame);
    }

    void EventDispatcher_onGameEvent(string command, object data)
    {
        if(data is ResponseUpdateGame && dataUpdateGameChange != null)
            dataUpdateGameChange(pokerGame.dataUpdateGame);
    }

    public void QuitGame()
    {
        APIGeneric.BackScene(null);
    }

    public void SitDown(PokerSide side)
    {
        APIPokerGame.SitDown(side);
    }
}
