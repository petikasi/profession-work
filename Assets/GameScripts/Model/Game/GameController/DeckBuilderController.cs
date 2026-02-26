using System;
using UnityEngine;
using GameScripts.Persistence;
using Assets.GameScripts.Model.Deckmaker.Deks;
public class DeckBuilderController : MonoBehaviour
{

    
    public Deck CurrentDeck { get; private set; }

    public static DeckBuilderController Instance { get; private set; }
    public event Action<Factions> OnFactionChanged;
    public event Action<int> OnMoneyChanged;
    public DeckList decklist;
    void Awake()
    {
        CurrentDeck = new Deck();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        decklist = DeckSaving.LoadDeck();
        Debug.Log(decklist.decks.Count);

    }

    public void ChangeFaction(Factions f) 
    {
        Debug.Log(f.ToString());
        CurrentDeck.ChangeFaction(f);
        OnFactionChanged.Invoke(f);
        OnMoneyChanged.Invoke(CurrentDeck.GETMONEY);
    }

    public void InitializeSave()
    {
        Debug.Log($"Begin Saving as {DeckBuilderController.Instance.CurrentDeck.NAME}");
        DeckSaving.SavingDecks(CurrentDeck,decklist);


    }

    public void Add(UnitTypes unit)
    {
        if (CurrentDeck.GETMONEY >= CurrentDeck.GetPreisofUnit(unit))
        {
            CurrentDeck.Add(unit);
            OnMoneyChanged.Invoke(CurrentDeck.GETMONEY);
        }

    }
    public void Remove(UnitTypes unit)
    {
        if (CurrentDeck.GetHoleListUnit().Contains(unit))
        {
            CurrentDeck.Remove(unit);
            OnMoneyChanged.Invoke(CurrentDeck.GETMONEY);

        }
    }


}
