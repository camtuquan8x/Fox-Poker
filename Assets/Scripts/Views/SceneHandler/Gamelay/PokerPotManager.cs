using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PokerPotManager : MonoBehaviour
{

    #region UnityEditor
    public UITable tablePot;
    public GameObject topLeftPosition,topRightPosition;
    #endregion
    List<PokerPotItem> pots = new List<PokerPotItem>();

    public void UpdatePot(List<Puppet.Poker.Datagram.ResponseUpdatePot.DataPot> datas) 
    {
        StartCoroutine(_UpdatePot(datas));
    }
    IEnumerator _UpdatePot(List<Puppet.Poker.Datagram.ResponseUpdatePot.DataPot> datas)
    {
        foreach (Puppet.Poker.Datagram.ResponseUpdatePot.DataPot item in datas)
        {
            PokerPotItem currentPot = pots.Find(p => item.id == p.Pot.id);
            if (currentPot != null)
            {
                currentPot.SetValue(item);
            }
            else
            {
                PokerPotItem potNew = PokerPotItem.Create(item);
                switch (pots.Count)
                {
                    case 6:
                        potNew.transform.parent = topLeftPosition.transform;
                        break;
                    case 7:
                        potNew.transform.parent = topRightPosition.transform;
                        break;
                    default :
                        potNew.transform.parent = tablePot.transform;
                        break;

                }
                potNew.transform.localScale = Vector3.one;
                potNew.transform.localPosition = Vector3.zero;              
                pots.Add(potNew);
            }
        }
        yield return new WaitForEndOfFrame();
        tablePot.Reposition();
    }

    public void DestroyAllPot() 
    {
        foreach (PokerPotItem item in pots)
            GameObject.Destroy(item.gameObject);
        pots.Clear();
    }
}
