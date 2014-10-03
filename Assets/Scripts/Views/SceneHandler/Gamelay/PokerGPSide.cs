using Puppet;
using Puppet.Poker;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PokerGPSide : MonoBehaviour
{
    public PokerSide CurrentSide;
	public GameObject[] positionCardFaceCards;
	public GameObject[] positionCardBackCards;
	public GameObject[] positionCardGameEnd;
	public GameObject positionMoney;
	public GameObject btnSit;

    void OnEnable()
    {
        onPlayerPickSide += PlayerPickSide;
        UIEventListener.Get(btnSit).onClick += OnClickSit;
    }

    void OnDisable()
    {
        onPlayerPickSide -= PlayerPickSide;
        UIEventListener.Get(btnSit).onClick -= OnClickSit;
    }

    static event Action<PokerSide> onPlayerPickSide;
    static bool sendSitdown = false;
    void PlayerPickSide(PokerSide side)
    {
        if (sendSitdown == false)
        {
            PokerGameModel.Instance.SitDown(side);
            sendSitdown = true;
        }
        NGUITools.SetActive(btnSit, false);
    }

    void OnClickSit(GameObject go)
    {
        if(onPlayerPickSide != null)
            onPlayerPickSide(CurrentSide);
    }
}
