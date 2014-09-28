using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.API;

public interface IPlazaView
{
    void ShowUserName(string username);
    void ShowMoney(string money);
    void ShowEvent();
    void ShowQuestionToReceiverGold();
}

