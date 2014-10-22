using UnityEngine;
using System.Collections;
using Puppet.Service;
using System.Collections.Generic;
using Puppet;

[PrefabAttribute(Name = "Prefabs/Dialog/DialogRechargeChip", Depth = 7, IsAttachedToCamera = true, IsUIPanel = true)]
public class DialogRechargeView : BaseDialog<DialogRecharge, DialogRechargeView>
{

    #region UnityEditor
    #region Tab Recharge
    public UITable tableTypeRecharge;
    public UIGrid tableRechargeItem;
    #endregion
    #endregion
    List<GameObject> lstRechargeGobj = new List<GameObject>();
    public override void ShowDialog(DialogRecharge data)
    {
        base.ShowDialog(data);
        initTab();
    }

    private void initTab()
    {
        foreach (RechargeTab item in data.tabs)
        {
            GameObject gobj = GameObject.Instantiate(Resources.Load("Prefabs/Dialog/Recharge/RechargeChipTab")) as GameObject;
            gobj.transform.parent = tableTypeRecharge.transform;
            gobj.transform.localScale = Vector3.one;
            gobj.transform.localPosition = Vector3.zero;
            gobj.GetComponent<RechargeTabView>().SetData(item, OnTabRechargeSelected);
        }
        tableTypeRecharge.Reposition();
        tableTypeRecharge.transform.GetChild(0).GetComponent<UIToggle>().value = true;
    }
    private void OnTabRechargeSelected(bool isSelected, RechargeTab tab)
    {
        if (isSelected)
        {
            StartCoroutine(ShowTabRecharge(tab));
        }
    }
    IEnumerator ShowTabRecharge(RechargeTab tab)
    {

        while (lstRechargeGobj.Count > 0)
        {
            GameObject.Destroy(lstRechargeGobj[0]);
            lstRechargeGobj.RemoveAt(0);
        }
        lstRechargeGobj.Clear();
        yield return new WaitForSeconds(0.1f);
        if (tab.typeRecharge == 0)
        {
            foreach (SMSRecharge item in data.sms)
            {
                GameObject gobj = GameObject.Instantiate(Resources.Load("Prefabs/Dialog/Recharge/SMSItem")) as GameObject;
                gobj.transform.parent = tableRechargeItem.transform;
                gobj.transform.localScale = Vector3.one;
                gobj.transform.localPosition = Vector3.zero;
                gobj.GetComponent<SMSRechargeView>().SetData(item, OnSMSClickListener);
                lstRechargeGobj.Add(gobj);
            }
        }
        else if (tab.typeRecharge == 1)
        {
            foreach (CardRecharge item in data.cards)
            {
                GameObject gobj = GameObject.Instantiate(Resources.Load("Prefabs/Dialog/Recharge/CardItem")) as GameObject;
                gobj.transform.parent = tableRechargeItem.transform;
                gobj.transform.localScale = Vector3.one;
                gobj.transform.localPosition = Vector3.zero;
                gobj.GetComponent<CardRechargeView>().SetData(item, OnCardClickListener);
                lstRechargeGobj.Add(gobj);
            }
        }
        tableRechargeItem.Reposition();
    }
    private void OnSMSClickListener(SMSRecharge model)
    {

    }
    private void OnCardClickListener(CardRecharge model)
    {

    }
}
public class DialogRecharge : AbstractDialogData
{
    public List<RechargeTab> tabs;
    public List<CardRecharge> cards;
    public List<SMSRecharge> sms;
    public DialogRecharge(List<RechargeTab> tabs, List<CardRecharge> cards, List<SMSRecharge> sms)
        : base()
    {
        this.tabs = tabs;
        this.cards = cards;
        this.sms = sms;
    }
    public DialogRecharge()
        : base()
    {
        tabs = new List<RechargeTab>();
        sms = new List<SMSRecharge>();
        cards = new List<CardRecharge>();
        RechargeTab tabSMS = new RechargeTab();
        tabSMS.title = "Nạp chip qua SMS";
        tabSMS.typeRecharge = 0;
        RechargeTab tabCard = new RechargeTab();
        tabCard.title = "Nạp chip qua thẻ cào";
        tabCard.typeRecharge = 1;
        tabs.Add(tabSMS);
        tabs.Add(tabCard);

        SMSRecharge sms1 = new SMSRecharge();
        sms1.money = "15k Chip";
        sms1.texture = (Texture2D)Resources.LoadAssetAtPath("Assets/Art_Assets/Others/Recharge/sms_low.png", typeof(Texture2D));
        SMSRecharge sms2 = new SMSRecharge();
        sms2.money = "25k Chip";
        sms2.texture = (Texture2D)Resources.LoadAssetAtPath("Assets/Art_Assets/Others/Recharge/sms_medium.png", typeof(Texture2D));
        SMSRecharge sms3 = new SMSRecharge();
        sms3.money = "50k Chip";
        sms3.texture = (Texture2D)Resources.LoadAssetAtPath("Assets/Art_Assets/Others/Recharge/sms_high.png", typeof(Texture2D));
        sms.Add(sms1);
        sms.Add(sms2);
        sms.Add(sms3);

        CardRecharge cardViettel = new CardRecharge();
        cardViettel.texture = (Texture2D)Resources.LoadAssetAtPath("Assets/Art_Assets/Others/Recharge/card_viettel.png", typeof(Texture2D));

        CardRecharge cardMobile = new CardRecharge();
        cardMobile.texture = (Texture2D)Resources.LoadAssetAtPath("Assets/Art_Assets/Others/Recharge/card_mobi.png", typeof(Texture2D));

        CardRecharge cardVcoin = new CardRecharge();
        cardVcoin.texture = (Texture2D)Resources.LoadAssetAtPath("Assets/Art_Assets/Others/Recharge/card_vcoin.png", typeof(Texture2D));

        CardRecharge cardGate = new CardRecharge();
        cardGate.texture = (Texture2D)Resources.LoadAssetAtPath("Assets/Art_Assets/Others/Recharge/card_gate.png", typeof(Texture2D));

        CardRecharge cardVina = new CardRecharge();
        cardVina.texture = (Texture2D)Resources.LoadAssetAtPath("Assets/Art_Assets/Others/Recharge/card_vina.png", typeof(Texture2D));
        cards.Add(cardViettel);
        cards.Add(cardMobile);
        cards.Add(cardVcoin);
        cards.Add(cardGate);
        cards.Add(cardVina);

    }
    public override void ShowDialog()
    {
        DialogRechargeView.Instance.ShowDialog(this);
    }
}
