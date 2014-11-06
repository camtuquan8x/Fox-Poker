using Puppet.Poker.Datagram;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PokerCurrentBet : MonoBehaviour
{
    #region UNITY EDITOR
    public UISprite spriteIcon;
    public UILabel labelCurrentbet;
    #endregion

    public void SetActive(bool state)
    {
        NGUITools.SetActive(gameObject, state);
    }

    public void SetBet(double value)
    {
        string unit = string.Empty;
        double money = value;
        if (money >= 100000000) { money /= 100000000f; unit = "M"; }
        else if (money >= 100000) { money /= 100000f; unit = "K"; }

        labelCurrentbet.text = string.Format("{0:#,##}{1}", Math.Round(money, 1), unit);
    }
}
