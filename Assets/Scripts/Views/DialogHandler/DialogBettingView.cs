﻿using UnityEngine;
using System.Collections;
using Puppet.API.Client;
using Puppet;
using System;
using Puppet.Service;
[PrefabAttribute(Name = "Prefabs/Dialog/Gameplay/DialogGameplayBetting", Depth = 9)]
public class DialogBettingView : BaseDialog<DialogBetting, DialogBettingView>
{
    #region UnityEditor
    public UISlider sliderBar;
    public UILabel labelMoney;
    #endregion

    double smallBlind;
    EventDelegate del;
    void Awake()
    {
        del = new EventDelegate(this, "OnSliderChange");
        smallBlind = PokerObserver.Instance.gameDetails.customConfiguration.SmallBlind;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        sliderBar.onChange.Add(del);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        sliderBar.onChange.Remove(del);
    }

    double GetCurrentMoney
    {
        get
        {
            int index = (int)Mathf.Lerp(1, sliderBar.numberOfSteps, sliderBar.value);
			double money= (smallBlind * index);
			if(index == 1)
				money = data.MaxBinded + smallBlind;
			return money;
        }
    }

    void OnSliderChange()
    {
		if (GetCurrentMoney >= data.MaxBetting) {
			labelMoney.text = "All In";
			sliderBar.value = 1;
		} else {
			labelMoney.text = GetCurrentMoney.ToString("#,###");
		}
    }

    public override void ShowDialog(DialogBetting data)
    {
        base.ShowDialog(data);

        sliderBar.numberOfSteps = (int)(data.MaxBetting / smallBlind);
        gameObject.transform.parent = data.parent;
        gameObject.transform.localPosition = new Vector3(0f, 280f, 0f);
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.parent = null;
    }

    protected override void OnPressButton(bool? pressValue, DialogBetting data)
	{
        if (pressValue == true && data.onBetting != null)
            	data.onBetting(GetCurrentMoney);

	}
}

public class DialogBetting : AbstractDialogData
{
	public double MaxBinded, MaxBetting;
    public Action<double> onBetting;
    public Transform parent;

    public DialogBetting(double maxBinded, double max, Action<double> onBetting, Transform parent)
    {
		this.MaxBinded = maxBinded;
        this.MaxBetting = max;
        this.onBetting = onBetting;
        this.parent = parent;
    }

    public override void ShowDialog()
    {
        DialogBettingView.Instance.ShowDialog(this);
    }

    protected override string GetButtonName(bool? button)
    {
        return "Tố";
    }
}