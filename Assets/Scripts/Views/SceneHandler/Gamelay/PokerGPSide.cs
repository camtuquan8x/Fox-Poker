using Puppet;
using Puppet.Poker;
using Puppet.Poker.Datagram;
using Puppet.Service;
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
	public GameObject positionMoney,positionDealer;
	public GameObject btnSit;

    void Awake()
    {
        wasBuyChip = false;
    }

    void OnEnable()
    {
        onPlayerPickSide += PlayerPickSide;
        UIEventListener.Get(btnSit).onClick += OnClickSit;

        PokerObserver.Instance.onPlayerListChanged += Instance_onPlayerListChanged;
    }

    void OnDisable()
    {
        onPlayerPickSide -= PlayerPickSide;
        UIEventListener.Get(btnSit).onClick -= OnClickSit;

        PokerObserver.Instance.onPlayerListChanged -= Instance_onPlayerListChanged;
    }

    void Instance_onPlayerListChanged(ResponsePlayerListChanged data)
    {
        if(PokerObserver.Instance.IsMainPlayer(data.player.userName))
        {
            bool showSit = false;
            switch(data.GetActionState())
            {
                case PokerPlayerChangeAction.playerRemoved:
                case PokerPlayerChangeAction.waitingPlayerAdded:
                    showSit = true;
                    wasBuyChip = false;
                    break;
            }
            NGUITools.SetActive(btnSit, showSit);
        }
    }

    static event Action<int> onPlayerPickSide;
    static bool wasBuyChip = false;
    void PlayerPickSide(int slot)
    {
        if (wasBuyChip == false)
        {
            //PokerObserver.Instance.SitDown(slot, gameDetails.customConfiguration.SmallBlind * 20);
            DialogBuyChip dialog = new DialogBuyChip(PokerObserver.Instance.gameDetails.customConfiguration.SmallBlind, (betting, autoBuy) =>
            {
                if (betting >= PokerObserver.Instance.gameDetails.customConfiguration.SmallBlind)
                {
                    PokerObserver.Instance.SitDown(slot, betting);
                    Puppet.API.Client.APIPokerGame.SetAutoBuy(autoBuy);
                    wasBuyChip = true;
                }
            });
            DialogService.Instance.ShowDialog(dialog);
        }
        NGUITools.SetActive(btnSit, false);
    }

    void OnClickSit(GameObject go)
    {
        if(onPlayerPickSide != null)
            onPlayerPickSide((int)CurrentSide);
    }
}
