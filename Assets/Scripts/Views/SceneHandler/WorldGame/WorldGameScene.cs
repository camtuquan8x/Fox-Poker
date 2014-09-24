using UnityEngine;
using System.Collections;
using Puppet.Core.Model;

public class WorldGameScene : MonoBehaviour
{

    #region Unity Editor
    public UITable tableGame;
    #endregion
    void Start () {
        Puppet.API.Client.APIWorldGame.GetListGame(onGetListGame);
	}

	// Update is called once per frame
	void Update () {
	
	}
    private void onGetListGame(bool status, string message, System.Collections.Generic.List<Puppet.Core.Model.DataGame> data)
    {
        foreach (DataGame item in data)
        {
            GameItem.Create(item, tableGame.transform);
        }
        //tableGame.Reposition();
    }
}
