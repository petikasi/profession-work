using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.GameScripts.Model.Game.Players.BasicEnemy;

namespace Assets.GameScripts.Model.Game.Players.BasicEnemy
{
    public class DeckManager 
    {

        public static List<(FactionsEnum Faction, UnitTypesEnum UnitType)> GetRandomDeck()
        {
            List<List<(FactionsEnum Faction, UnitTypesEnum UnitType)>> allDecks = new()
        {
            BasicDecksHolder.HumanBalancedDeck,
            BasicDecksHolder.HumanSiegeDeck,
            BasicDecksHolder.OrksHordeDeck,
            BasicDecksHolder.OrksShamanDeck,
            BasicDecksHolder.ElvenRangedDeck,
            BasicDecksHolder.ElvenAncientDeck,
            BasicDecksHolder.HellRushDeck,
            BasicDecksHolder.HellControlDeck
        };

            int randomIndex = Random.Range(0, allDecks.Count);

            return allDecks[randomIndex];
        }
    }

}