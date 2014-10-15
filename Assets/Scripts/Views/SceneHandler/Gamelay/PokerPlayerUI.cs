using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PokerPlayerUI : MonoBehaviour
{
    public UITexture texture;
    public UILabel labelCurrentGold;
    public UILabel labelUsername;

    GameObject[] cardOnHands;
    PokerPlayerController data;
    [HideInInspector]
    public PokerGPSide side;

    public void SetData(PokerPlayerController player)
    {
        this.data = player;
        labelUsername.text = player.userName;
        labelCurrentGold.text = player.asset.GetAsset(Puppet.EAssets.Chip).value.ToString();
    }

    public void UpdateSetCardObject(GameObject [] cardOnHands)
    {
        this.cardOnHands = cardOnHands;

        for (int i = 0; i < cardOnHands.Length; i++)
        {
            cardOnHands[i].transform.parent = PokerObserver.Instance.IsMainPlayer(data.userName) ? side.positionCardMainPlayer[i].transform : side.positionCardFaceCards[i].transform;
            cardOnHands[i].transform.localRotation = Quaternion.identity;
            cardOnHands[i].transform.localPosition = Vector3.zero;
            cardOnHands[i].transform.localScale = PokerObserver.Instance.IsMainPlayer(data.userName) ? Vector3.one : Vector3.one /3;
        }
    }
}
