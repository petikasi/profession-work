using System;
using UnityEngine;
using GameScripts.Persistence;
namespace Assets.GameScripts.Model.Game.GameControllerFolder
{
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

        public void ChangeFaction(FactionsEnum f)
        {
            Debug.Log(f.ToString());
            DeckInBuilding.ChangeFaction(f);
            OnFactionChanged.Invoke();
            OnMoneyChanged.Invoke(DeckInBuilding.GETMONEY);
        }

        public void ReciveDeckToModif(Deck deck)
        {
            DeckInBuilding = deck;
            OnFactionChanged?.Invoke();
            Debug.Log($"Money: {DeckInBuilding.GETMONEY}");
            OnMoneyChanged?.Invoke(DeckInBuilding.GETMONEY);
        }

        public void InitializeSave()
        {
            Debug.Log($"Begin Saving as {DeckInBuilding.NAME}");
            if (DeckLoaderController.Instance.DeckList.SearchByID(DeckInBuilding.ID))
            {
                DeckLoaderController.Instance.DeckList.ReplaceDeckWithModifiedDack(DeckInBuilding);
                DeckSaving.SaveDeckList(DeckLoaderController.Instance.DeckList);

            }
            else
            {
                DeckSaving.SaveDeck(DeckInBuilding);
                DeckLoaderController.Instance.AddDeckToDeckList(DeckInBuilding);

            }
            DeckInBuilding = new();

        }

        public void Add(UnitTypesEnum unit)
        {
            if (DeckInBuilding.GETMONEY >= DeckInBuilding.GetPreisofUnit(unit))
            {
                DeckInBuilding.Add(unit);
                OnMoneyChanged.Invoke(DeckInBuilding.GETMONEY);
            }

        }
        public void Remove(UnitTypesEnum unit)
        {
            if (DeckInBuilding.GetHoleListUnit().Contains(unit))
            {
                DeckInBuilding.Remove(unit);
                OnMoneyChanged.Invoke(DeckInBuilding.GETMONEY);

            }
        }
        public void SetDeckToEmpty()
        {
            DeckInBuilding = new();
        }

    }
}
