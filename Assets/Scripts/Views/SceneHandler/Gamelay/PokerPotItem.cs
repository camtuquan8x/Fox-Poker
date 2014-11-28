using Puppet.Poker.Datagram;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PokerPotItem : MonoBehaviour
{
    #region UNITY EDITOR
    public UISprite spriteIcon;
    public UILabel labelCurrentbet;
    #endregion
    Puppet.Poker.Datagram.ResponseUpdatePot.DataPot pot;
    public Puppet.Poker.Datagram.ResponseUpdatePot.DataPot Pot
    {
        get { return pot; }
    }
    public static PokerPotItem Create(Puppet.Poker.Datagram.ResponseUpdatePot.DataPot pot)
    {
        GameObject gobj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Gameplay/PotItem"));
        gobj.name = "" + pot.id;
        PokerPotItem item = gobj.GetComponent<PokerPotItem>();
        item.SetValue(pot);
        return item;
    }
    public void SetValue(Puppet.Poker.Datagram.ResponseUpdatePot.DataPot pot)
    {
        this.pot = pot;
        SetBet(pot.value);

    }
    public void SetBet(double value)
    {
        string[] money = Utility.Convert.ConvertMoneyAndShortCut(value);

        labelCurrentbet.text = string.Format("{0:f2}{1}", money[0], money[1]);
    }
}
