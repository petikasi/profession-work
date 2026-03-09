using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameScripts.Model.Deckmaker.Deks;
using UnityEngine;
using GameScripts.Persistence;

namespace Assets.GameScripts.Model.Game.GameController
{
    public class DeckManagerController : MonoBehaviour
    {
        public DeckList DeckList { get; private set; }
        public Deck SelectedDeck { get; private set; }
        public static DeckManagerController Instance { get; private set; }

        public event Action RefreshDecklist;
        public event Action Selecteddeck;

        void Start()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DeckList = DeckSaving.LoadDeckList();
            Debug.Log(DeckList.decks.Count);
            

        }

        public void AddDeckToDeckList(Deck d)
        {
            if (!DeckList.decks.Contains(d))
            {
                DeckList.decks.Add(d);
                RefreshDecklist?.Invoke();
            }

        }


        public void RemoveDeckToDeckList(Deck d)
        {
            if (DeckList.decks.Contains(d))
            {

            }

        }

        public void Choosendeck(Deck d) 
        {
            SelectedDeck = d;
            Selecteddeck?.Invoke();
        }
    }
}
