using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameScripts.Model.Game.Enums;
using Assets.GameScripts.Model.Game.GameControllerFolder;
using Assets.GameScripts.Model.Game.Player;
using Assets.GameScripts.ViewModel.Game.UnitHolder;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.GameScripts.Model.Game.GameControllerFolder
{
    public class GameController : MonoBehaviour
    {


        public static GameController Instance { get; private set; }
        public (UnitTypesEnum Unit, FactionsEnum Faction) UnitandFaction { get; private set; }
        public PlayerModel OwnPlayer { get; private set; }
        public PlayerModel EnemyPlayer { get; private set; }
        private bool hasSelectedUnit = false;
        private UnitCountpanel selectedCardScript;
        private List<BaseUnit> units = new();
        public event Action OnEveryUnitPlaced;
        int unitCount;

        public PlayerModel GetPlayer 
        {
            get => OwnPlayer;
        }


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            OwnPlayer = new PlayerModel(PlayerEnum.White, DeckLoaderController.Instance.SelectedDeck);
            if (DeckLoaderController.Instance.SelectedDeck != null)
            {
                unitCount = OwnPlayer.Deck.Count;
                Debug.Log($"A kiválasztott pakliban lévõ egységek száma: {unitCount}");
            }
            else
            {
                Debug.LogWarning("Még nincs kiválasztva semmilyen pakli!");
            }
        }

        public void UnitandFactionSet(UnitTypesEnum unit, FactionsEnum fac, UnitCountpanel cardScript)
        {
            UnitandFaction = (unit, fac);

            selectedCardScript = cardScript;
            hasSelectedUnit = true;

            Debug.Log($"GameController: Egység kijelölve: {fac} - {unit}");
        }
        public void CallPlacing(int x, int z, bool isRightClick = false)
        {
            if (isRightClick)
            {
                TryRemoveUnitAt(x, z);
                return;
            }

            if (hasSelectedUnit)
            {
                HandlePlacing(x, z);
            }
        }

        private void HandlePlacing(int x, int z)
        {
            if (selectedCardScript == null) return;

            if (z >= 5)
            {
                Debug.LogWarning("Csak az 5. sorig megengedett a lehelyezés!");
                return;
            }

            if (GetUnitAt(x, z) != null)
            {
                Debug.LogWarning("Ezen a mezõn már áll egy egység!");
                return;
            }

            if (BoardLayout.Instance != null)
            {
                // Átadjuk a selectedCardScript-et is a BoardLayoutnak!
                BaseUnit unit = BoardLayout.Instance.PlaceUnitAtTile(x, z, UnitandFaction.Faction, UnitandFaction.Unit, selectedCardScript);

                if (unit != null)
                {
                    units.Add(unit);
                    selectedCardScript.DecreaseCount();
                    ClearSelection();
                    unitCount--;
                    Debug.Log(unitCount);
                    if (unitCount == 0)
                    {

                        OnEveryUnitPlaced.Invoke();
                    }
                }
            }
        }

        private void TryRemoveUnitAt(int x, int z)
        {
            BaseUnit unitToRemove = GetUnitAt(x, z);

            if (unitToRemove != null)
            {

                if (unitToRemove.MyCardPanel != null)
                {
                    unitToRemove.MyCardPanel.IncreaseCount();
                    unitCount++;
                }

                // 2. Töröljük a belsõ listánkból
                units.Remove(unitToRemove);

                // 3. Elpusztítjuk a 3D modellt a pályáról
                Destroy(unitToRemove.gameObject);

                Debug.Log($"Egység sikeresen visszavéve a ({x}, {z}) koordinátáról.");
            }
        }


        public BaseUnit GetUnitAt(int x, int z)
        {
            return units.FirstOrDefault(u => u != null && u.TileX == x && u.TileZ == z);
        }

        private void ClearSelection()
        {
            hasSelectedUnit = false;
            selectedCardScript = null;
        }


    }
}
