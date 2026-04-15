using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameScripts.Persistence;
using UnityEngine;

namespace Assets.GameScripts.Model.Deckmaker.Deks
{
    [Serializable]
    public class DeckList
    {
        [SerializeField] public List<Deck> decks = new();


        public bool SearchByID(string id) 
        {
            foreach (var deck in decks)
            {
                if (deck.ID == id)
                {
                    return true;
                
                }
                
            }
            

            return false;
        }

        public void ReplaceDeckWithModifiedDack(Deck decktoreplace)
        {
            int index = decks.FindIndex(d => d.ID == decktoreplace.ID);

            if (index != -1)
            {
                decks[index] = decktoreplace;
            }
        }
    }
}
