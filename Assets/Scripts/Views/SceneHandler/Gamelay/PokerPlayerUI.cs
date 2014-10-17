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
    #endregion

    GameObject[] cardOnHands;
    PokerPlayerController data;
    [HideInInspector]
    public PokerGPSide side;

    void OnEnable()
    {
        PokerObserver.Instance.dataTurnGame += Instance_dataTurnGame;
    }

    void OnDisable()
    {
        PokerObserver.Instance.dataTurnGame -= Instance_dataTurnGame;
    }

    private void Instance_dataTurnGame(ResponseUpdateTurnChange data)
    {
        NGUITools.SetActive(timerSlider.gameObject, data.toPlayer != null && data.toPlayer.userName == this.data.userName);

        if (data.toPlayer != null && data.toPlayer.userName == this.data.userName)
        {
            StartTimer(data.time > 1000 ? data.time / 1000f : data.time);
        }
        else
        {
            StopTimer();
        }
    }

    public void SetData(PokerPlayerController player)
    {
        this.data = player;
        labelUsername.text = player.userName;
        labelCurrentGold.text = player.asset.GetAsset(Puppet.EAssets.Chip).value.ToString();

        Vector3 giftPosition = btnGift.transform.localPosition;
        if ((int)player.GetSide() > (int)Puppet.Poker.PokerSide.Slot_5)
        {
            float x = Math.Abs(giftPosition.x);
            giftPosition.x = x;
        }
        btnGift.transform.localPosition = giftPosition;
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
