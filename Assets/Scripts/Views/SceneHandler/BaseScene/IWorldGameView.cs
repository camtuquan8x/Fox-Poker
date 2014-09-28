using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IWorldGameView : IView
{
    void OnLoadGame(List<DataGame> datas);
    void ShowUserName(string userName);
    void ShowChip(string chip);
    void ShowLevel(string level);
    void ShowExp(float currentExp,float nextExp,float currentMaxExp);
}

