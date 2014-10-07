using Puppet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IWorldGameView : IView
{
    void OnLoadGame(List<DataGame> datas);
}

