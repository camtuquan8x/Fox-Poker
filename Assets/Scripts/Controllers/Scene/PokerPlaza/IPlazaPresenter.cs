using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IPlazaPresenter : IScenePresenter
{
    void GetEvent();
    void JoinLobby();
    void PlayNow();
    void GetListQuest();
    void ShowUserName();
    void ShowMoney();
    void JoinToEvent();

}
