using UnityEngine;
using System.Collections;
using Puppet.Core.Model;
using Puppet.API.Client;

public class WorldGameScene : MonoBehaviour
{

    #region Unity Editor
    public UITable tableGame;
	public UILabel lbUserName,lbMoney,lbLevel;
	public UISlider expSlider;
    #endregion
    void Start () {
        APIWorldGame.GetListGame(onGetListGame);
		UserInfo user = APIUser.GetUserInformation ();
		lbUserName.text = user.info.userName;
		lbMoney.text = user.assets.content [0].value.ToString();
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
