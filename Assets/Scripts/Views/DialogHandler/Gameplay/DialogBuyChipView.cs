using UnityEngine;
using System.Collections;
using Puppet.Service;
using Puppet.API.Client;
using Puppet;

[PrefabAttribute(Name = "Prefabs/Dialog/Gameplay/DialogGamePlayBuyChip", Depth = 7, IsAttachedToCamera = true, IsUIPanel = true)]
public class DialogBuyChipView : BaseDialog<DialogBuyChip,DialogBuyChipView>
{
    #region UnityEditor
    public UILabel minChip,maxChip,money;
    public UISlider slider;
    public UIToggle autoBuy;
    public GameObject btnMinChip, btnMaxChip;
    #endregion
    // Use this for initialization
    public override void ShowDialog(DialogBuyChip data)
    {
        base.ShowDialog(data);
        initData();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventDelegate.Add(slider.onChange, onSliderChange);
        UIEventListener.Get(btnMinChip).onClick += onBtnMinClick;
        UIEventListener.Get(btnMaxChip).onClick += onBtnMaxClick;
    }

    private void onSliderChange()
    {
        int index = (int)Mathf.Lerp(1, slider.numberOfSteps, slider.value);
		if (int.Parse (minChip.text) * index > APIUser.GetUserInformation ().assets.content [0].value) {
			string[] moneyAndShortcut = Utility.Convert.ConvertMoneyAndShortCut (APIUser.GetUserInformation ().assets.content [0].value);
			money.text = "$" + moneyAndShortcut [0].Trim() + moneyAndShortcut [1].Trim();

		} else {
			string[] moneyAndShortcut = Utility.Convert.ConvertMoneyAndShortCut (int.Parse (minChip.text) * index);
			money.text = "$" + moneyAndShortcut [0].Trim() + moneyAndShortcut [1].Trim();
		}
       
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventDelegate.Remove(slider.onChange, onSliderChange);
        UIEventListener.Get(btnMinChip).onClick -= onBtnMinClick;
        UIEventListener.Get(btnMaxChip).onClick -= onBtnMaxClick;
    }
    private void initData()
    {
        minChip.text = (data.smallBind * 20).ToString();
        maxChip.text = (data.smallBind * 400).ToString();
        slider.numberOfSteps = (int)((data.smallBind * 400) / (data.smallBind * 20));
		slider.value = 0.5f;
		string[] moneyAndShortcut = Utility.Convert.ConvertMoneyAndShortCut(APIUser.GetUserInformation ().assets.content [0].value);
        labelTitle.text = "Số Gold hiện tại của bạn: $" + moneyAndShortcut[0] + moneyAndShortcut[1];
    }
    protected override void OnPressButton(bool? pressValue, DialogBuyChip data)
    {
        base.OnPressButton(pressValue, data);
        if (pressValue == true)
        {
            int index = (int)Mathf.Lerp(1, slider.numberOfSteps, slider.value);
			int value = 0;
			if (int.Parse (minChip.text) * index > APIUser.GetUserInformation ().assets.content [0].value) {
				value = int.Parse(""+APIUser.GetUserInformation ().assets.content [0].value);
			}else{
				value = int.Parse(minChip.text) * index;
			}
            if(data.onChooise != null)
                data.onChooise(value, autoBuy.value);
        }
        else if (data.onChooise != null)
            data.onChooise(0, false);
    }
    private void onBtnMaxClick(GameObject go)
    {
        slider.value = 1;
    }

    private void onBtnMinClick(GameObject go)
    {
        slider.value = 0;
    }
}
public class DialogBuyChip : AbstractDialogData
{
    public DialogBuyChip(double smallBind, System.Action<int, bool> onChooise)
    {
        this.smallBind = smallBind;
        this.onChooise = onChooise;
    }
    public System.Action<int, bool> onChooise;
    public int slot;
    public double smallBind;
    public double currentChip;
    public override void ShowDialog()
    {
        DialogBuyChipView.Instance.ShowDialog(this);
    }
}