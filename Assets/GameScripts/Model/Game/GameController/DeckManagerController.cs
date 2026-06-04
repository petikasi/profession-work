using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameScripts.Model.Deckmaker.Deks;
using UnityEngine;
using GameScripts.Persistence;
using Assets.GameScripts.Persistence.RemoveDeck;

namespace Assets.GameScripts.Model.Game.GameController
{
    public class DeckManagerController : MonoBehaviour
    {
        public DeckList DeckList { get; private set; }
        public Deck SelectedDeck { get; private set; }
        public static DeckManagerController Instance { get; private set; }

        public event Action OnRefreshDecklist;
        public event Action OnSelectedDeck;

        void Awake()
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
                OnRefreshDecklist?.Invoke();
            }

        }

        public void RemoveSelectedDeck() 
        {
            if (DeckList.decks.Contains(SelectedDeck)) 
            {
                DeckList.decks.Remove(SelectedDeck);
                DeckRemoving.DeleteDeck(SelectedDeck.ID);
                OnRefreshDecklist?.Invoke();
            }
        }

        public void Choosendeck(Deck d) 
        {
            SelectedDeck = d;
            OnSelectedDeck?.Invoke();
        }
        public void AddSelectedDeckToDeckBuilder() 
        {
            DeckBuilderController.Instance.ReciveDeckToModif(SelectedDeck);

        }

        
    }
}
