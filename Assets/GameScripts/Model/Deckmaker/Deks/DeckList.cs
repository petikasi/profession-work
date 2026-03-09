using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameScripts.Model.Deckmaker.Deks
{
    [Serializable]
    public class DeckList
    {
        [SerializeField] public List<Deck> decks = new();


    }
}
