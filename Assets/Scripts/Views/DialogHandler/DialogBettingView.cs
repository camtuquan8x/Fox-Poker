using UnityEngine;
using System.Collections;
using Puppet.API.Client;
using Puppet;
using System;
using Puppet.Service;
[PrefabAttribute(Name = "Prefabs/Dialog/DialogGameplayBetting", Depth = 9)]
public class DialogBettingView : BaseDialog<DialogBetting, DialogBettingView>
{
    #region UnityEditor
    public UISlider sliderBar;
    public UILabel labelMoney;
    #endregion

    EventDelegate del;
    void Awake()
    {
        del = new EventDelegate(this, "OnSliderChange");
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

    long GetCurrentMoney
    {
        get
        {
            return Mathf.RoundToInt((data.MaxBetting - data.MinBetting) * sliderBar.value) + data.MinBetting;
        }
    }

    void OnSliderChange()
    {
        labelMoney.text =(GetCurrentMoney >= data.MaxBetting) ? "All In" : GetCurrentMoney.ToString("#,###");
    }

    public override void ShowDialog(DialogBetting data)
    {
        base.ShowDialog(data);

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
    public long MinBetting, MaxBetting;
    public Action<long> onBetting;
    public Transform parent;

    public DialogBetting(long min, long max, Action<long> onBetting, Transform parent)
    {
        this.MinBetting = min;
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