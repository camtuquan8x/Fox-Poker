using UnityEngine;
using System.Collections;
using Puppet;
using System;
[PrefabAttribute(Name = "Prefabs/Dialog/SearchView/SearchView", Depth = 9)]
public class SearchView : SingletonPrefab<SearchView>
{

    #region UnityEditor
    public UIInput txtInput;
    public GameObject btnSearch, btnExits;
    public UIToggle cbTwoPeople, cbFivePeople, cbNinePeople;
    #endregion
    Action<string, bool[] > onSearchSubmit;
    void Start() {
        btnExits.GetComponent<UISprite>().SetAnchor(NGUITools.GetRoot(gameObject).transform);
        btnExits.GetComponent<UISprite>().topAnchor.absolute = 0;
        btnExits.GetComponent<UISprite>().leftAnchor.absolute = 0;
        btnExits.GetComponent<UISprite>().rightAnchor.absolute = 0;
        btnExits.GetComponent<UISprite>().bottomAnchor.absolute = 0;
    }
    public void SetActionSubmit(  Action<string, bool[] > onSearchSubmit) {
        this.onSearchSubmit = onSearchSubmit;
    }
    void OnEnable()
    {
        UIEventListener.Get(btnSearch).onClick += OnSearchClick;
        UIEventListener.Get(btnExits).onClick += OnCloseSearchView;
    }
    void OnDisable()
    {
        UIEventListener.Get(btnSearch).onClick -= OnSearchClick;
        UIEventListener.Get(btnExits).onClick -= OnCloseSearchView;
    }
    private void OnCloseSearchView(GameObject go)
    {
        GameObject.Destroy(gameObject);
    }

    private void OnSearchClick(GameObject go)
    {
        bool[] arrayCheckbox = new bool[3];
        arrayCheckbox[0] = cbTwoPeople.value;
        arrayCheckbox[2] = cbFivePeople.value;
        arrayCheckbox[1] = cbNinePeople.value;
        string text = txtInput.value;
        if (onSearchSubmit != null)
            onSearchSubmit(text, arrayCheckbox);
    }
    void Update()
    {

    }
}
