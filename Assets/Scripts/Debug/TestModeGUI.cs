using Puppet;
using Puppet.API.Client;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TestModeGUI : MonoBehaviour
{
    public UITexture background;
    public static string KEY_COMUTITY_CARD = "comunity_card";
    List<PokerCard> lstCard = new List<PokerCard>();
    string strPick = "";

    string[] playerPick = new string[9];
    Action<Dictionary<string, int[]>> actionOrderHand;

    string comunityCard = "";
    public Action<Dictionary<string, int[]>> ActionOrderHand
    {
        set
        {
            actionOrderHand = value;
        }
    }
    void Update() { 
        if (Input.GetKey(KeyCode.Escape)) 
        {
            GameObject.Destroy(gameObject);
        } 
    }


    public static void Create(Action<Dictionary<string, int[]>> actionOrderHand)
    {
        if (GameObject.Find("__Prefab Test Pick Card") != null) return;

        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Others/Debug/TestMode"));
        obj.GetComponent<TestModeGUI>().ActionOrderHand = actionOrderHand;
        obj.name = "__Prefab Test Pick Card";
    }
    void Start()
    {
        for (int i = 0; i < 52; i++)
            lstCard.Add(new PokerCard(i));
        foreach (PokerPlayerUI item in GameObject.FindObjectsOfType<PokerPlayerUI>())
        {
            playerPick[item.data.slotIndex] = "";
        }
    }
    const float SPACE_TOP = 50f;
    const float SPACE_LEFT = 50f;
    void OnGUI()
    {
        float BOX_WIDTH = Screen.width - SPACE_LEFT * 2;
        float BOX_HEIGHT = Screen.height - SPACE_TOP * 2;

        Rect rect = new Rect(SPACE_LEFT, SPACE_TOP, BOX_WIDTH, BOX_HEIGHT);
        GUI.DrawTexture(rect, background.mainTexture);
        GUI.Box(rect, "");
        {
            GUILayout.BeginArea(rect, "");
            {
                #region LINE 1
                GUILayout.BeginHorizontal(GUILayout.Height(BOX_HEIGHT / 3));
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal();
                        {
                            float contentWidth = BOX_WIDTH - 100f;
                            GUILayout.Space(30f);
                            GUILayout.Label("COMUNITY CARD", GUILayout.Width(150f));
                            GUILayout.TextField(comunityCard, GUILayout.Width(contentWidth - 150f));
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.BeginVertical();

                    foreach (PokerPlayerUI p in GameObject.FindObjectsOfType<PokerPlayerUI>())
                    {
                        GUILayout.BeginVertical();
                        {
                            GUILayout.BeginHorizontal();
                            {
                                float contentWidth = BOX_WIDTH - 100f;
                                GUILayout.Space(30f);
                                GUILayout.Label(p.data.userName, GUILayout.Width(100f));
                                GUILayout.TextField(playerPick[p.data.slotIndex] == null ? "" : playerPick[p.data.slotIndex], GUILayout.Width(contentWidth - 150f));

                                if (GUILayout.Button("CLEAR", GUILayout.Width(70f)))
                                    playerPick[p.data.slotIndex] = "";
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.BeginVertical();
                    }
                }
                GUILayout.EndHorizontal();
                #endregion

                GUILayout.FlexibleSpace();

                #region LINE 2
                GUILayout.BeginHorizontal(GUILayout.Height(BOX_HEIGHT / 3));
                {
                    float contentWidth = BOX_WIDTH - 20f;
                    GUILayout.BeginVertical(GUILayout.Width(contentWidth / 4));
                    {
                        GUILayout.Label(" ");
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(contentWidth / 3));
                    {
                        string[] lst = strPick.Split(" ".ToCharArray(), StringSplitOptions.None);
                        GUILayout.Label("BẠN ĐANG CHỌN: " + (lst.Length - 1) + " Card.", GUILayout.Width(contentWidth / 3));
                        GUILayout.TextField(strPick);
                        if (GUILayout.Button("CLEAR"))
                            strPick = "";

                        foreach (PokerPlayerUI p in GameObject.FindObjectsOfType<PokerPlayerUI>())
                        {
                            if (GUILayout.Button("SET TO " + p.data.userName))
                            {
                                if (strPick.Trim().Split(" ".ToCharArray(), StringSplitOptions.None).Length <= 2)
                                {
                                    playerPick[p.data.slotIndex] = strPick;
                                    strPick = "";
                                }
                                else
                                {
                                    strPick = "";
                                }
                            }
                        }
                        if (GUILayout.Button("SET TO COMUNITY CARD"))
                        {
                            if (strPick.Trim().Split(" ".ToCharArray(), StringSplitOptions.None).Length <= 5)
                            {
                                comunityCard = strPick;
                                strPick = "";
                            }
                            else
                            {
                                strPick = "";
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(contentWidth / 3));
                    {
                        GUILayout.Label(" ");

                        if (GUILayout.Button("ĐÃ XONG", new GUILayoutOption[] { GUILayout.Width(contentWidth / 3 / 2), GUILayout.Height(contentWidth / 3 / 2) }))
                        {

                            Dictionary<string, int[]> handsPlayer = new Dictionary<string, int[]>();
                            List<PokerPlayerUI> playersUI = new List<PokerPlayerUI>( GameObject.FindObjectsOfType<PokerPlayerUI>());
                            playersUI.ForEach(p =>
                            {
                                string[] arrStrs = string.IsNullOrEmpty(playerPick[p.data.slotIndex]) ? new string[0] : playerPick[p.data.slotIndex].Trim().Split(" ".ToCharArray());
                                List<int> ids = new List<int>();

                                for (int i = 0; i < arrStrs.Length; i++)
                                {
                                    ids.Add(int.Parse(arrStrs[i].Trim().Split("-".ToCharArray())[1]));
                                }
                                handsPlayer.Add(p.data.userName, ids.ToArray());
                            });
                            string[] comunityCardName = string.IsNullOrEmpty(comunityCard) ? new string[0] : comunityCard.Trim().Split(" ".ToCharArray());
                            List<int> comunityCardIds = new List<int>();
                            for (int i = 0; i < comunityCardName.Length; i++)
                            {
                                
                                comunityCardIds.Add(int.Parse(comunityCardName[i].Split('-')[1]));
                            }
                            if (comunityCardIds.Count > 0)
                                handsPlayer.Add(KEY_COMUTITY_CARD, comunityCardIds.ToArray());
                            if (actionOrderHand != null)
                                actionOrderHand(handsPlayer);
                            GameObject.Destroy(gameObject);
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
                #endregion

                GUILayout.FlexibleSpace();

                #region LINE 3
                GUILayout.BeginHorizontal(GUILayout.Height(BOX_HEIGHT / 3));
                {
                    int index = 0;
                    for (int i = 0; i < 13; i++)
                    {
                        GUILayout.BeginVertical();
                        for (int j = 0; j < 4; j++)
                        {
                            int cardValue = ((int)(lstCard[index].GetRank()));
                            string str = (cardValue == 1 ? "A" : cardValue == 11 ? "J" : cardValue == 12 ? "Q" : cardValue == 13 ? "K" : cardValue.ToString()) +
                                (lstCard[index].GetSuit() == ECardSuit.Bitch ? "♠" : lstCard[index].GetSuit() == ECardSuit.Spade ? "♣" :
                                lstCard[index].GetSuit() == ECardSuit.Diamond ? "♦" : "♥") + "-" + index;

                            if (strPick.IndexOf(str) >= 0)
                                str = "";

                            if (Array.TrueForAll<string>(playerPick, p => p == null || (p != null && p.IndexOf(str) == -1)))
                            {
                                if (GUILayout.Button(str, GUILayout.Width((BOX_WIDTH - 150f) / 12f)))
                                    if (string.IsNullOrEmpty(str) == false && strPick.Split(" ".ToCharArray(), StringSplitOptions.None).Length <= 5)
                                        strPick += str + " ";
                            }
                            else
                                GUILayout.Button("", GUILayout.Width((BOX_WIDTH - 150f) / 12f));
                            index++;
                        }
                        GUILayout.EndVertical();
                    }
                }
                GUILayout.EndHorizontal();
                #endregion
            }
            GUILayout.EndArea();
        }
    }


}
