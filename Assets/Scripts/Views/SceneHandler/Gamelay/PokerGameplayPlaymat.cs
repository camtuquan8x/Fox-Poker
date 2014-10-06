using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet.Poker;
using System;
using Puppet.Poker.Models;
using Puppet.Poker.Datagram;
using Puppet;

public class PokerGameplayPlaymat : MonoBehaviour 
{
    PokerGPSide[] arrayPokerSide;
    Dictionary<string, GameObject> dictPlayerObject = new Dictionary<string, GameObject>();

    void Awake()
    {
        arrayPokerSide = GameObject.FindObjectsOfType<PokerGPSide>();
        PokerGameModel.Instance.dataFirstJoinGame += Instance_dataFirstJoinGame;
        PokerGameModel.Instance.dataPlayerListChanged += Instance_dataPlayerListChanged;
    }

    void Instance_dataFirstJoinGame(ResponseUpdateGame data)
    {
        foreach (PokerPlayerController player in data.players)
        {
            SetPositionAvatarPlayer(player);
        }
    }

    void Instance_dataPlayerListChanged(ResponsePlayerListChanged dataPlayer)
    {
        PokerPlayerChangeAction state = dataPlayer.GetActionState();
        if(state == PokerPlayerChangeAction.playerAdded)
        {
            SetPositionAvatarPlayer(dataPlayer.player);
        }
        else if(state == PokerPlayerChangeAction.playerRemoved)
        {
            GameObject.Destroy(dictPlayerObject[dataPlayer.player.userName]);
            dictPlayerObject.Remove(dataPlayer.player.userName);
        }
    }

    public PokerGPSide GetPokerSide(PokerSide side)
    {
        return Array.Find<PokerGPSide>(arrayPokerSide, s => s.CurrentSide == side);
    }

    public void SetPositionAvatarPlayer(PokerPlayerController player)
    {
        GameObject obj;
        if (dictPlayerObject.ContainsKey(player.userName))
            obj = dictPlayerObject[player.userName];
        else
        {
            obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Gameplay/PlayerUI"));
            dictPlayerObject.Add(player.userName, obj);
        }

        Logger.Log("Server: {0} - Client: {1}", player.slotIndex, player.GetSide().ToString());

        PokerGPSide playerSide = GetPokerSide(player.GetSide());
        obj.transform.parent = playerSide.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }


}
