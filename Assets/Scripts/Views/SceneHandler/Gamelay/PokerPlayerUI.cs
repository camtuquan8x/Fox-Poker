using Puppet;
using Puppet.Poker.Datagram;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    PokerPotItem currentBet;

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

            if (PokerObserver.Instance.isWaitingFinishGame || (PokerObserver.Game.CurrentPlayer != null && PokerObserver.Game.CurrentPlayer.userName == player.userName))
                labelUsername.text = data.userName;
            else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.none)
                labelUsername.text = "Chờ ván mới";
            else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.fold)
                labelUsername.text = "Bỏ bài";
            else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.bigBlind)
                labelUsername.text = "Big Blind";
            else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.smallBlind)
                labelUsername.text = "Small Blind";
            else if (PokerObserver.Game.LastPlayer != null && PokerObserver.Game.LastPlayer.userName == player.userName)
            {
                if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.call)
                    labelUsername.text = "Theo cược";
                else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.allIn || 
                    (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.raise && player.currentBet == 0))
                    labelUsername.text = "All-in";
                else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.raise)
                    labelUsername.text = "Thêm cược";
                else if (player.GetPlayerState() == Puppet.Poker.PokerPlayerState.check)
                    labelUsername.text = "Xem bài";
            }
            else if (player.currentBet == 0)
                labelUsername.text = "Chờ đặt cược";
            else
                labelUsername.text = data.userName;

            LoadCurrentBet(player.currentBet);
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
        if(cardPlayer != null && cardPlayer.cards != null)
        {
            for(int i=0;i<cardPlayer.cards.Length;i++)
            {
                cardOnHands[i].GetComponent<PokerCardObject>().SetDataCard(new PokerCard(cardPlayer.cards[i]));
                cardOnHands[i].transform.parent = side.positionCardGameEnd[i].transform;
                cardOnHands[i].transform.localRotation = Quaternion.identity;
                cardOnHands[i].transform.localPosition = Vector3.zero;
                cardOnHands[i].transform.localScale = Vector3.one;
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
            if(currentBet == null)
                currentBet = NGUITools.AddChild(side.positionMoney, playmat.prefabBetObject).GetComponent<PokerPotItem>();
            else
            {
                currentBet.transform.parent = side.positionMoney.transform;
                currentBet.transform.localPosition = Vector3.zero;
            }
        }

        if (currentBet != null)
        {
            currentBet.gameObject.SetActive(value > 0);
            currentBet.SetBet(value);
        }
    }

    public void SetResult(bool isWinner)
    {
        NGUITools.SetActive(spriteResultIcon.gameObject, isWinner);
    }

    void OnDestroy()
    {
        data.onDataChanged -= playerModel_onDataChanged;
    }

    public void SetData(PokerPlayerController player)
    {
        bool addEvent = data == null;
        this.data = player;
        if(addEvent)
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

    #region TIMER
    float totalCountDown = 0f;
    float timeCountDown = 0f;
    float realtime = 0f;
    void StartTimer(float time, float remainingTime = 0f)
    {
        if(time > 0)
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
