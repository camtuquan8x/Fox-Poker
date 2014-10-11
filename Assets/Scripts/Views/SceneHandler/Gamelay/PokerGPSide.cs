using Puppet;
using Puppet.Poker;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PokerGPSide : MonoBehaviour
{
    public PokerSide CurrentSide;

    public GameObject[] positionCardMainPlayer;
	public GameObject[] positionCardFaceCards;
	public GameObject[] positionCardBackCards;
	public GameObject[] positionCardGameEnd;
	public GameObject positionMoney;
	public GameObject btnSit;

    void Awake()
    {
        sendSitdown = false;
    }

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

    static event Action<int> onPlayerPickSide;
    static bool sendSitdown = false;
    void PlayerPickSide(int slot)
    {
        if (sendSitdown == false)
        {
            PokerGameModel.Instance.SitDown(slot);
            sendSitdown = true;
        }
        NGUITools.SetActive(btnSit, false);
    }

    void OnClickSit(GameObject go)
    {
        if(onPlayerPickSide != null)
            onPlayerPickSide((int)CurrentSide);
    }
}
