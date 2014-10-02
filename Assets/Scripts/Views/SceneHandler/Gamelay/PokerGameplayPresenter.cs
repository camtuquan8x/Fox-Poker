using UnityEngine;
using System.Collections;
using Puppet.Poker;
using Puppet.Poker.Datagram;
using System;

public class PokerGameplayPresenter 
{
    PokerGameplay pokerGame;
    public event Action<ResponseUpdateGame> dataUpdateGameChange;

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
        Puppet.API.Client.APIGeneric.BackScene(null);
    }
}
