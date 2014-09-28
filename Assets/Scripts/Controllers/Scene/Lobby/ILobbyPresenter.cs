using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface ILobbyPresenter : IPresenter
{
    void LoadChannels();
    void LoadAllLobbies();
    void LoadLobbiesByChannel(DataChannel channel);
    void JoinToGame(DataLobby lobby);
    void CreateLobby();
}

