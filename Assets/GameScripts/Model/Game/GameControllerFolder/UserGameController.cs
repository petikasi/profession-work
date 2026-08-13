using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameScripts.Model.Game.Enums;
using Assets.GameScripts.Model.Game.GameControllerFolder;
using Assets.GameScripts.Model.Game.Player;
using Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas;
using UnityEngine;

namespace Assets.GameScripts.Model.Game.GameControllerFolder
{
    public class UserGameController : MonoBehaviour
    {
        public static UserGameController Instance { get; private set; }
        public (UnitTypesEnum Unit, FactionsEnum Faction) UnitandFaction { get; private set; }
        public PlayerModel OwnPlayer { get; private set; }
        private bool hasSelectedUnit = false;
        private UnitCountpanel selectedCardScript;
        private List<BaseUnit> units = new();

        public event Action<bool> OnEveryUnitPlaced;
        public event Action OnDestroyUICanvas;

        private BaseUnit selectedUnitOnBoard;
        private int unitCount;

        public PlayerModel GetPlayer => OwnPlayer;

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
                unitCount = OwnPlayer.SelectedDeck.Count;
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

        public void SetPlayerToActive()
        {
            if (unitCount == 0) 
            {
                OnDestroyUICanvas?.Invoke();
                OwnPlayer.CurrentGamePhase = GamePhaseEnum.Movement;
            }
  
        }

        private void HandlePlacing(int x, int z)
        {
            if (selectedCardScript == null) return;

            if (OwnPlayer.CurrentGamePhase != GamePhaseEnum.Deployment) return;

            if (z >= BoardLayout.Instance.PLAYER_ZONE) return;

            if (GetUnitAt(x, z) != null) return;

            if (BoardLayout.Instance != null)
            {
                BaseUnit unit = BoardLayout.Instance.PlaceUnitAtTile(x, z, UnitandFaction.Faction, UnitandFaction.Unit, selectedCardScript);

                if (unit != null)
                {
                    units.Add(unit);
                    selectedCardScript.DecreaseCount();
                    ClearSelection();
                    unitCount--;
                    if (unitCount == 0)
                    {
                        OnEveryUnitPlaced?.Invoke(true);
                    }
                    else 
                    {
                        OnEveryUnitPlaced?.Invoke(true);

                    }
                }
            }
        }


        public void SelectUnitToMove(int x, int z)
        {
            // Javítva: Movvment -> Movement
            if (OwnPlayer.CurrentGamePhase != GamePhaseEnum.Movement) return;

            BaseUnit unitAtTile = GetUnitAt(x, z);

            if (unitAtTile != null)
            {
                selectedUnitOnBoard = unitAtTile;
                Debug.Log($"Egység kijelölve mozgatásra: {selectedUnitOnBoard.name} a ({x}, {z}) koordinátán.");
            }
        }

        public void MovingUnit(int targetX, int targetZ)
        {
            // Javítva: Movvment -> Movement
            if (OwnPlayer.CurrentGamePhase != GamePhaseEnum.Movement)
            {
                Debug.LogWarning("Nem a mozgási fázisban vagy!");
                return;
            }

            if (selectedUnitOnBoard == null)
            {
                Debug.LogWarning("Nincs kijelölve egyetlen egység sem a mozgatáshoz!");
                return;
            }

            if (GetUnitAt(targetX, targetZ) != null)
            {
                Debug.LogWarning($"A cél csempe ({targetX}, {targetZ}) már foglalt!");
                return;
            }

            if (BoardLayout.Instance != null)
            {
                // Átadjuk az egységet és a célkoordinátákat a BoardLayoutnak
                BoardLayout.Instance.MoveUnitOnBoard(selectedUnitOnBoard, targetX, targetZ);

                Debug.Log($"A(z) {selectedUnitOnBoard.name} sikeresen elmozdult a ({targetX}, {targetZ}) mezõre.");

                // Kijelölés törlése a sikeres lépés után
                DeselectUnit();
            }
        }

        public void DeselectUnit()
        {
            selectedUnitOnBoard = null;
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

                units.Remove(unitToRemove);
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