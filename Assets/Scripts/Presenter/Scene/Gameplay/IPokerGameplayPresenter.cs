using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IPokerGameplayPresenter : IGameplayPresenter
{
    void LeaveTurn();
    void AddBet(float bet);
    void FollowBet();
}

