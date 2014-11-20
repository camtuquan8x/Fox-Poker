using UnityEngine;
using System.Collections;

public class LeagueItemRow : MonoBehaviour
{
    #region Unity Editor
    public UILabel lbPrize, lbCondition, lbTime, lbNumber,lbBtnTitle;
    public GameObject btnLeague;
    public UITexture avatar;
    #endregion
    string[] spriteName = { "btn_7","btn_17","btn_18","btn_19","btn_20"};
    string[] title = { "Tham Gia", "Đã đăng ký", "Trường Đấu", "Xem giải", "Kết Quả" };
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
