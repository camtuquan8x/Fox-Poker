using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IGameItemPresenter
{
     void JoinLobby(DataGame data);
     void LoadImage(string url);
}

