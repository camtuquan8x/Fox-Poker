using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet.Poker;
using System;
using Puppet.Poker.Models;

public class PokerGameplayPlaymat : MonoBehaviour 
{
    PokerGPSide[] arrayPokerSide;
    Dictionary<PokerPlayerController, GameObject> dictPlayerObject = new Dictionary<PokerPlayerController,GameObject>();

    void Awake()
    {
        arrayPokerSide = GameObject.FindObjectsOfType<PokerGPSide>();
    }

    public PokerGPSide GetPokerSide(PokerSide side)
    {
        return Array.Find<PokerGPSide>(arrayPokerSide, s => s.CurrentSide == side);
    }

    public void SetPositionAvatarPlayer(PokerPlayerController player)
    {
        GameObject obj;
        if (dictPlayerObject.ContainsKey(player))
            obj = dictPlayerObject[player];
        else
        {
            obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Gameplay/PlayerUI"));
            dictPlayerObject.Add(player, obj);
        }
        PokerGPSide playerSide = GetPokerSide(player.GetSide());
        obj.transform.parent = playerSide.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }
}
