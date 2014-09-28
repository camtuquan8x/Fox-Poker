using UnityEngine;
using System.Collections;

public class CardObject : MonoBehaviour 
{
    static string[] RANK_IMAGE = new string[] { "char_ace", "char_2", "char_3", "char_4", "char_5", "char_6", "char_7", "char_8", "char_9", "char_10", "char_j", "char_q", "char_k" };
    static string[] SUIT_IMAGE = new string[] { "bitch_icon", "spade_icon", "diamond_icon", "heart_icon" };
    static string[] ICON_IMAGE = new string[] { "jack_icon", "queen_icon", "king_icon" };

    public UISprite 
        spriteBackground, 
        spriteRank, 
        spriteSuit, 
        spriteIcon;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
