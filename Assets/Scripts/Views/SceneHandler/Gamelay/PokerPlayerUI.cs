﻿using Puppet;
using Puppet.Poker.Datagram;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using Puppet.Poker;
using System.Collections;

public class PokerPlayerUI : MonoBehaviour
{
    #region UNITY EDITOR
    public UITexture texture;
    public UILabel labelCurrentGold;
    public UILabel labelUsername;
    public UISlider timerSlider;
    public GameObject btnGift;
    public UISprite spriteResultIcon;
    #endregion

    [HideInInspector]
    public GameObject[] cardOnHands;
    PokerPlayerController data;
    PokerGameplayPlaymat playmat;
    [HideInInspector]
    public PokerGPSide side;
    public PokerPotItem currentBet;
    protected double lastMyBetting = 0;
    public string UserName
    {
        get { return data.userName; }
    }

    void Awake()
    {
        this.SetResult(false);
        playmat = GameObject.FindObjectOfType<PokerGameplayPlaymat>();
    }

    void OnEnable()
    {
        PokerObserver.Instance.onTurnChange += Instance_dataTurnGame;
        PokerObserver.Instance.onUpdatePot += Instance_onUpdatePot;
        PokerObserver.Instance.onUpdateUserInfo += Instance_onUpdateUserInfo;
        PokerObserver.Instance.onFinishGame += Instance_onFinishGame;
    }

    void OnDisable()
    {
        PokerObserver.Instance.onTurnChange -= Instance_dataTurnGame;
        PokerObserver.Instance.onUpdatePot -= Instance_onUpdatePot;
        PokerObserver.Instance.onUpdateUserInfo -= Instance_onUpdateUserInfo;
        PokerObserver.Instance.onFinishGame -= Instance_onFinishGame;
    }

    void UpdateUI(PokerPlayerController player)
    {
        if (player != null && player.userName == data.userName)
        {
            double money = player.GetMoney();
            labelCurrentGold.text = money > 0 ? money.ToString("#,###") : "All In";

            string customTitle = string.Empty;
            if (PokerObserver.Instance.isWaitingFinishGame || (PokerObserver.Game.CurrentPlayer != null && PokerObserver.Game.CurrentPlayer.userName == player.userName))
                labelUsername.text = data.userName;
            else if (PokerObserver.Game.ListPlayerWaitNextGame.Contains(player.userName))
                customTitle = "Chờ ván mới";
            else if (PokerObserver.Game.IsPlayerInGame(player.userName))
            {
                if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.fold)
                    customTitle = "Bỏ bài";
                else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.bigBlind)
                    customTitle = "Big Blind";
                else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.smallBlind)
                    customTitle = "Small Blind";
                else if (PokerObserver.Game.LastPlayer != null && PokerObserver.Game.LastPlayer.userName == player.userName)
                {
                    if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.call)
                        customTitle = "Theo cược";
                    else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.allIn ||
                        (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.raise && player.currentBet == 0))
                        customTitle = "All-in";
                    else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.raise)
                        customTitle = "Thêm cược";
                    else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.check)
                        customTitle = "Xem bài";
                }
                else if (PokerObserver.Game.IsPlayerInGame(player.userName) && player.currentBet == 0)
                    customTitle = "Chờ đặt cược";
                else
                    labelUsername.text = data.userName;
            }
            else
                labelUsername.text = data.userName;

            if (!string.IsNullOrEmpty(customTitle))
                labelUsername.text = string.Format("[FFFF50]{0}[-]", customTitle);

            LoadCurrentBet(player.currentBet);

            if (PokerObserver.Game.Dealer == player.userName)
                playmat.SetDealerObjectToPlayer(player);
        }
    }

    public void SetTitle(string title)
    {
        labelUsername.text = string.IsNullOrEmpty(title) ? data.userName : title;
    }

    void Instance_onUpdatePot(ResponseUpdatePot data)
    {
    }

    void Instance_onUpdateUserInfo(ResponseUpdateUserInfo data)
    {
    }

    void Instance_onFinishGame(ResponseFinishGame data)
    {
        StopTimer();

        if (currentBet != null)
            currentBet.gameObject.SetActive(false);

        ResponseFinishCardPlayer cardPlayer = Array.Find<ResponseFinishCardPlayer>(data.players, p => p.userName == this.data.userName);
        if (cardPlayer != null && cardPlayer.cards != null)
        {
            bool isFoldAll = PokerObserver.Game.ListPlayer.FindAll(p => p.GetPlayerState() == PokerPlayerState.fold).Count == 0;
            if (isFoldAll || PokerObserver.Game.ListPlayer.FindAll(p => p.userName != PokerObserver.Game.MainPlayer.userName).Count == 0)
            { }
            else
            {
                for (int i = 0; i < cardPlayer.cards.Length; i++)
                {
                    cardOnHands[i].GetComponent<PokerCardObject>().SetDataCard(new PokerCard(cardPlayer.cards[i]));
                    cardOnHands[i].transform.parent = side.positionCardGameEnd[i].transform;
                    cardOnHands[i].transform.localRotation = Quaternion.identity;
                    cardOnHands[i].transform.localPosition = Vector3.zero;
                    cardOnHands[i].transform.localScale = Vector3.one;
                }
            }
        }
    }

    private void Instance_dataTurnGame(ResponseUpdateTurnChange responseData)
    {
        NGUITools.SetActive(timerSlider.gameObject, responseData.toPlayer != null && responseData.toPlayer.userName == this.data.userName);
        if (responseData.toPlayer != null && responseData.toPlayer.userName == this.data.userName)
            StartTimer(responseData.time > 1000 ? responseData.time / 1000f : responseData.time);
        else
            StopTimer();
    }


    void LoadCurrentBet(double value)
    {
        if (side != null)
        {
            if (currentBet == null)
            {
                currentBet = NGUITools.AddChild(side.positionMoney, playmat.prefabBetObject).GetComponent<PokerPotItem>();
            }
            else
            {
                currentBet.transform.parent = side.positionMoney.transform;
                currentBet.transform.localPosition = Vector3.zero;
            }
        }

        if (currentBet != null)
        {
            addBetAnim(value);
        }
    }
    public void addMoneyToMainPot() {
        currentBet.labelCurrentbet.transform.parent.gameObject.SetActive(false);
        iTween.MoveTo(currentBet.gameObject, iTween.Hash("position", playmat.potContainer.tablePot.transform.position, "time", 1.0f, "oncomplete", "onMoneyToMainPotComplete", "oncompletetarget", gameObject));

    }
    void onMoneyToMainPotComplete()
    {
        currentBet.transform.localPosition = Vector3.zero;
        currentBet.labelCurrentbet.transform.parent.gameObject.SetActive(true);
        SetCurrentBet(0);
    }
    IEnumerator SetCurrentBet(double value)
    {
        yield return new WaitForSeconds(value > 0 ? 1.0f:0f);
        currentBet.gameObject.SetActive(value > 0);
        currentBet.SetBet(value);

    }
    PokerPotItem betAnim;
    public void addBetAnim(double value)
    {
        if (currentBet.CurrentBet < value)
        {
            if (betAnim != null && betAnim.gameObject != null)
                tweenComplete();
            betAnim = NGUITools.AddChild(gameObject, playmat.prefabBetObject).GetComponent<PokerPotItem>();
            betAnim.labelCurrentbet.transform.parent.gameObject.SetActive(false);
            iTween.MoveTo(betAnim.gameObject, iTween.Hash("position", side.positionMoney.transform.localPosition, "islocal", true, "time", 1.0f, "oncomplete", "tweenComplete", "oncompletetarget", gameObject));
         
        }
        StartCoroutine(SetCurrentBet(value));

    }
    void tweenComplete()
    {
        GameObject.Destroy(betAnim.gameObject);
    }
    public void SetResult(bool isWinner)
    {
        NGUITools.SetActive(spriteResultIcon.gameObject, isWinner);
    }

    void OnDestroy()
    {
        GameObject.Destroy(currentBet.gameObject);
        currentBet = null;

        data.onDataChanged -= playerModel_onDataChanged;
    }

    public void SetData(PokerPlayerController player)
    {
        bool addEvent = data == null;
        this.data = player;
        if (addEvent)
            data.onDataChanged += playerModel_onDataChanged;

        UpdateUI(player);

        Vector3 giftPosition = btnGift.transform.localPosition;
        if ((int)player.GetSide() > (int)Puppet.Poker.PokerSide.Slot_5)
        {
            float x = Math.Abs(giftPosition.x);
            giftPosition.x = x;
        }
        btnGift.transform.localPosition = giftPosition;
    }

    void playerModel_onDataChanged()
    {
        UpdateUI(data);
    }

    public void UpdateSetCardObject(GameObject[] cardOnHands)
    {
        this.cardOnHands = cardOnHands;

        for (int i = 0; i < cardOnHands.Length; i++)
        {
            cardOnHands[i].transform.parent = PokerObserver.Instance.IsMainPlayer(data.userName) ? side.positionCardMainPlayer[i].transform : side.positionCardFaceCards[i].transform;
            cardOnHands[i].transform.localRotation = Quaternion.identity;
            cardOnHands[i].transform.localPosition = Vector3.zero;
            cardOnHands[i].transform.localScale = PokerObserver.Instance.IsMainPlayer(data.userName) ? Vector3.one : Vector3.one / 3;
        }
    }

    #region TIMER
    float totalCountDown = 0f;
    float timeCountDown = 0f;
    float realtime = 0f;
    void StartTimer(float time, float remainingTime = 0f)
    {
        if (time > 0)
        {
            totalCountDown = timeCountDown = time;
            if (remainingTime > 0)
                timeCountDown = remainingTime;
            realtime = Time.realtimeSinceStartup;
        }
    }
    void StopTimer()
    {
        timeCountDown = -1f;
        realtime = 0f;
        timerSlider.value = 0;
    }
    #endregion

    void Update()
    {
        if (timeCountDown >= 0 && realtime >= 0)
        {
            timeCountDown -= (Time.realtimeSinceStartup - realtime);
            realtime = Time.realtimeSinceStartup;
            timerSlider.value = timeCountDown / totalCountDown;
        }
    }
}
