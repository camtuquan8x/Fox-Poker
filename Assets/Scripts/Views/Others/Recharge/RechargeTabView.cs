using UnityEngine;
using System.Collections;
using System;
using Puppet;

public class RechargeTabView : MonoBehaviour
{

    #region UnityEditor
    public UILabel lbTitle;
    #endregion

    private Action<bool, RechargeTab> actionSelectToTab;

    RechargeTab model;
    void Start()
    {
      
    }
    void OnEnable()
    {
        EventDelegate.Add(gameObject.GetComponent<UIToggle>().onChange, onSelectedValue);
    }

    void OnDisable()
    {
        EventDelegate.Remove(gameObject.GetComponent<UIToggle>().onChange, onSelectedValue);
    }
    public void SetData(RechargeTab model, Action<bool, RechargeTab> action)
    {
        this.model = model;
        lbTitle.text = this.model.title;
        this.actionSelectToTab = action;
    }

    void onSelectedValue()
    {
        if (gameObject.GetComponent<UIToggle>().value)
            lbTitle.color = Color.white;
        else
            lbTitle.color = new Color(11f / 255f, 73f / 255f, 102f / 255f);
        if (actionSelectToTab != null)
            actionSelectToTab(gameObject.GetComponent<UIToggle>().value, model);
    }

    public void setTextTitleGreen()
    {
        lbTitle.color = new Color(11f / 255f, 73f / 255f, 102f / 255f);
    }
    public void setTextTitleWhite()
    {
        lbTitle.color = Color.white;
    }

    public EventDelegate valueChange { get; set; }
}
public class RechargeTab
{
    public string title;
    public int typeRecharge;
}
