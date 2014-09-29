using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IGameplayPresenter : IScenePresenter
{
    void SitDownToSlot(int index);
    void ShowProfile();
    void SendMessage(string message);
    void ShowMiniGame(int type);
    void WifiChange();
    void ShowTime();
}
