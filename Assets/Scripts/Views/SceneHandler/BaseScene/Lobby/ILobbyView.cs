using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface ILobbyView : IView
{
    void ShowUserName(string userName);
    void ShowMoney(string money);
    void DrawChannels(List<DataChannel> channels);
    void DrawLobbies(List<DataLobby> lobbies);
    void RemoveLobby(DataLobby lobbies);
    void UpdateLobby(DataLobby lobbies);
    void AddLobby(DataLobby lobbies);
}

