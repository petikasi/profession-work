using System;
using UnityEngine;
using GameScripts.Persistence;
using Assets.GameScripts.Model.Game.GameController;
public class DeckBuilderController : MonoBehaviour
{
    public Deck DeckInBuilding { get; private set; }

    public static DeckBuilderController Instance { get; private set; }
    public event Action OnFactionChanged;
    public event Action<int> OnMoneyChanged;
    void Awake()
    {
        DeckInBuilding = new Deck();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void ChangeFaction(Factions f) 
    {
        Debug.Log(f.ToString());
        DeckInBuilding.ChangeFaction(f);
        OnFactionChanged.Invoke();
        OnMoneyChanged.Invoke(DeckInBuilding.GETMONEY);
    }

    public void InitializeSave()
    {
        Debug.Log($"Begin Saving as {DeckInBuilding.NAME}");
        DeckSaving.SaveDeck(DeckInBuilding);
        DeckManagerController.Instance.AddDeckToDeckList(DeckInBuilding);
        DeckInBuilding = new Deck();
    }

    public void Add(UnitTypes unit)
    {
        if (DeckInBuilding.GETMONEY >= DeckInBuilding.GetPreisofUnit(unit))
        {
            DeckInBuilding.Add(unit);
            OnMoneyChanged.Invoke(DeckInBuilding.GETMONEY);
        }

    }
    public void Remove(UnitTypes unit)
    {
        if (DeckInBuilding.GetHoleListUnit().Contains(unit))
        {
            DeckInBuilding.Remove(unit);
            OnMoneyChanged.Invoke(DeckInBuilding.GETMONEY);

        }
    }


}
