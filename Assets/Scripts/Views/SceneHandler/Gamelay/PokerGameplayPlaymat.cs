using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Puppet.Poker;
using System;
using System.Linq; 
using Puppet.Poker.Models;
using Puppet.Poker.Datagram;
using Puppet;

public class PokerGameplayPlaymat : MonoBehaviour
{
    #region UNITY EDITOR
    public GameObject prefabBetObject;

    public Transform []positionDealCards;
    public PokerCurrentBet currentPot;
    public GameObject objectDealer;
    #endregion
    PokerGPSide[] arrayPokerSide;
    Dictionary<string, GameObject> dictPlayerObject = new Dictionary<string, GameObject>();

    void Awake()
    {
        currentPot.SetActive(false);
        objectDealer.SetActive(false);

        arrayPokerSide = GameObject.FindObjectsOfType<PokerGPSide>();

        PokerObserver.Instance.onFirstJoinGame += Instance_onFirstJoinGame;
        PokerObserver.Instance.onPlayerListChanged += Instance_onPlayerListChanged;
        PokerObserver.Instance.dataUpdateGameChange += Instance_dataUpdateGame;
        PokerObserver.Instance.onEventUpdateHand += Instance_onEventUpdateHand;
        PokerObserver.Instance.onTurnChange += Instance_dataTurnGame;
        PokerObserver.Instance.onNewRound += Instance_onNewRound;
        PokerObserver.Instance.onUpdatePot += Instance_onUpdatePot;
        PokerObserver.Instance.onFinishGame += Instance_onFinishGame;
    }

    void Instance_onUpdatePot(ResponseUpdatePot obj)
    {
        if (obj.pot != null && obj.pot.Length > 0)
        {
            currentPot.SetActive(true);
            double value = 0;
            Array.ForEach<ResponseUpdatePot.DataPot>(obj.pot, p => value += p.value);
            currentPot.SetBet(value);
        }
    }

    void Instance_onNewRound(ResponseWaitingDealCard data)
    {
        currentPot.SetActive(false);

        DestroyCardObject();
    }

    void DestroyCardObject()
    {
        countGenericCard = 0;
        for (int i = cardsDeal.Count - 1; i >= 0; i--)
            GameObject.Destroy(cardsDeal[i]);
        cardsDeal.Clear();
    }

    private void Instance_dataTurnGame(ResponseUpdateTurnChange data)
    {
        if(data.dealComminityCards != null && data.dealComminityCards.Length > 0)
            CreateCardDeal(data.dealComminityCards);
    }

    List<GameObject> cardsDeal = new List<GameObject>();
    int countGenericCard = 0;
    void CreateCardDeal(int [] cards)
    {
        for(int i=0;i<cards.Length;i++)
        {
            GameObject card = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Gameplay/CardUI"));
            card.GetComponent<PokerCardObject>().SetDataCard(new PokerCard(cards[i]));
            card.transform.parent = positionDealCards[countGenericCard++].transform;
            card.transform.localRotation = Quaternion.identity;
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one * 0.9f;
            cardsDeal.Add(card);
        }
    }

    void Instance_onEventUpdateHand(ResponseUpdateHand data)
    {
        CreateHand(data.players, data.hand);
    }

    void CreateHand(PokerPlayerController[] players, int [] hands)
    {
        foreach(PokerPlayerController p in players)
        {
            int handSize = p.handSize;
            GameObject[] cardObjects = new GameObject[handSize];
            for (int i = 0; i < handSize;i++)
                cardObjects[i] = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Gameplay/CardUI"));

            if (PokerObserver.Instance.mUserInfo.info.userName == p.userName)
                for(int i=0;i<handSize;i++)
                    cardObjects[i].GetComponent<PokerCardObject>().SetDataCard(new PokerCard(hands[i]), i);
            else
                for (int i = 0; i < handSize; i++)
                    cardObjects[i].GetComponent<PokerCardObject>().SetDataCard(new PokerCard(), i);

            dictPlayerObject[p.userName].GetComponent<PokerPlayerUI>().UpdateSetCardObject(cardObjects);

            cardsDeal.AddRange(cardObjects);
        }
    }

    void Instance_onFinishGame(ResponseFinishGame responseData)
    {
        StartCoroutine(_onFinishGame(responseData));
    }

    IEnumerator _onFinishGame(ResponseFinishGame responseData)
    {
        float time = responseData.time/1000f;
        float waitTimeViewCard = time > 1 ? 1f : 0f;
        float timeEffectPot = responseData.pots.Length > 0 ? time - (waitTimeViewCard / responseData.pots.Length) : time - waitTimeViewCard;
        yield return new WaitForSeconds(waitTimeViewCard /2f);

        foreach(ResponseResultSummary summary in responseData.pots)
        {
            ResponseMoneyExchange playerWin = Array.Find<ResponseMoneyExchange>(summary.players, p => p.winner);
            if(currentPot != null && playerWin != null)
            {
                dictPlayerObject[playerWin.userName].GetComponent<PokerPlayerUI>().SetResult(true);

                List<int> list = new List<int>(playerWin.cards);
                List<GameObject> listCardObject = cardsDeal.FindAll(o => list.Contains(o.GetComponent<PokerCardObject>().card.cardId));
                //GameObject obj = NGUITools.AddChild(currentPot.transform.parent.gameObject, currentPot.gameObject);
                //obj.transform.parent = dictPlayerObject[playerWin.userName].transform.parent;
                //iTween.MoveTo(obj, iTween.Hash("islocal", true, "time", timeEffectPot, "position", Vector3.zero));
                for (int i = 0; i < 20; i++ )
                {
                    listCardObject.ForEach(o => o.GetComponent<PokerCardObject>().SetHighlight(i % 2 == 0));
                    yield return new WaitForSeconds(timeEffectPot / 20f);
                }
                //GameObject.Destroy(obj);
                listCardObject.ForEach(o => o.GetComponent<PokerCardObject>().SetHighlight(false));
                dictPlayerObject[playerWin.userName].GetComponent<PokerPlayerUI>().SetResult(false);
            }
        }
        yield return new WaitForSeconds(waitTimeViewCard / 2);
    }

    void Instance_onFirstJoinGame(ResponseUpdateGame data)
    {
        foreach (PokerPlayerController player in data.players)
        {
            if (player.isMaster)
                SetDealerObjectToPlayer(player);
            SetPositionAvatarPlayer(player);
        }

        CreateHand(data.players, null);
        CreateCardDeal(data.dealComminityCards);
    }

    void Instance_dataUpdateGame(ResponseUpdateGame data)
    {
        DestroyCardObject();
        Instance_onFirstJoinGame(data);
    }

    void Instance_onPlayerListChanged(ResponsePlayerListChanged dataPlayer)
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

        if(dataPlayer.player.isMaster)
            SetDealerObjectToPlayer(dataPlayer.player);
    }

    public void SetDealerObjectToPlayer(PokerPlayerController player)
    {
        objectDealer.SetActive(true);
        PokerGPSide playerSide = Array.Find<PokerGPSide>(arrayPokerSide, s => s.CurrentSide == player.GetSide());
        objectDealer.transform.parent = playerSide.positionDealer.transform;
        objectDealer.transform.localPosition = Vector3.zero;
        objectDealer.transform.localScale = Vector3.one;
    }

    public PokerGPSide GetPokerSide(PokerSide side)
    {
        return Array.Find<PokerGPSide>(arrayPokerSide, s => s.CurrentSide == side);
    }

    void SetPositionAvatarPlayer(PokerPlayerController player)
    {
        GameObject obj;
        if (dictPlayerObject.ContainsKey(player.userName))
            obj = dictPlayerObject[player.userName];
        else
        {
            obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Gameplay/PlayerUI"));
            dictPlayerObject.Add(player.userName, obj);
        }

        PokerGPSide playerSide = Array.Find<PokerGPSide>(arrayPokerSide, s => s.CurrentSide == player.GetSide());
        obj.GetComponent<PokerPlayerUI>().side = playerSide;
        obj.GetComponent<PokerPlayerUI>().SetData(player);
        obj.transform.parent = playerSide.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }


}
