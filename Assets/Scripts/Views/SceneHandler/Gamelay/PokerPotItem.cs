using Puppet.Poker.Datagram;
using Puppet.Poker.Models;
using System.Collections.Generic;
using UnityEngine;

public class PokerPotItem : MonoBehaviour
{
    #region UNITY EDITOR
    public UISprite spriteIcon;
    public UILabel labelCurrentbet;
    #endregion
    private string[] nameChipArr = { "icon_chip_1", "icon_chip_2", "icon_chip_3", "icon_chip_4" };
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
        currentBet = value;
        labelCurrentbet.text = string.Format("{0:f2}{1}", money[0], money[1]);
    }
    
    double currentBet = 0;
    public double CurrentBet { get { return currentBet; } }
}
