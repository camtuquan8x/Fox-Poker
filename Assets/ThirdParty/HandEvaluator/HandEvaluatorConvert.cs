using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Poker.Models;

namespace HoldemHand
{
    public class HandEvaluatorConvert
	{
        public static string ConvertPokerCardToString(PokerCard card) {
            string suit = "";
            switch (card.GetSuit()) { 
                case Puppet.ECardSuit.Bitch :
                    suit = "c";
                    break;
                case Puppet.ECardSuit.Diamond :
                    suit = "d";
                    break;
                case Puppet.ECardSuit.Heart :
                    suit = "h";
                    break;
                case Puppet.ECardSuit.Spade :
                    suit = "s";
                    break;
            }
            return card.GetRank()+suit;
        }
        public static string ConvertPokerCardsToString(List<PokerCard> cards) {
            string hand = "";
            foreach (PokerCard item in cards)
            {
                hand += ConvertPokerCardToString(item) + " ";
            }
            return hand;
        }
	}
}
