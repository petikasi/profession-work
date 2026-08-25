using Assets.GameScripts.Model.Game.Enums;
using Assets.GameScripts.Model.Game.GameControllerFolder;
using Assets.GameScripts.Model.Game.Board;
using Assets.GameScripts.Model.Game.Player;
using Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public event Action<bool> OnActivateEndTurnButton;

    private BaseUnit selectedUnitOnBoard;
    private List<Vector2Int> highlightedTiles = new(); // Eltároljuk az érvényes lépési mezõket
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

    public void EndPlayerTurn() 
    {

        OwnPlayer.CurrentGamePhase = GamePhaseEnum.Lock;
    }

    public void StartPlayerTurn()
    {

        foreach (BaseUnit unit in units) 
        {
            unit.Already_Moved = false;
        
        }
        OwnPlayer.CurrentGamePhase = GamePhaseEnum.Movement;
    }



    public void SetPlayerToActive()
    {
        OwnPlayer.CurrentGamePhase = GamePhaseEnum.Movement;

        if (BoardLayout.Instance != null)
        {
            BoardLayout.Instance.RemoveZoneVisuals();
        }

        OnActivateEndTurnButton?.Invoke(true);
        OnDestroyUICanvas?.Invoke();

        GC.Collect();
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
                    OnEveryUnitPlaced?.Invoke(false);
                }
            }
        }
    }

    // ==========================================
    // MOZGÁSI FÁZIS LOGIKA ÉS ZÓNA MEGJELENÍTÉS
    // ==========================================

    public void SelectUnitToMove(int x, int z)
    {
        if (OwnPlayer.CurrentGamePhase != GamePhaseEnum.Movement) return;

        DeselectUnit();

        BaseUnit unitAtTile = GetUnitAt(x, z);

        if (unitAtTile != null)
        {

          
            selectedUnitOnBoard = unitAtTile;

            if (unitAtTile.Already_Moved!)
            {
                Debug.Log($" {selectedUnitOnBoard.name} már mozgott");
                return;
            }


            Debug.Log($"Egység kijelölve mozgatásra: {selectedUnitOnBoard.name} | Sebeség: {selectedUnitOnBoard.MovementSpeed}");

            // Kiszámoljuk és kijelöljük a mozgási tartományt
            CalculateAndShowMovementRange(x, z, selectedUnitOnBoard.MovementSpeed);
        }
    }

    private void CalculateAndShowMovementRange(int startX, int startZ, int moveSpeed)
    {


        highlightedTiles.Clear();

        int boardWidth = BoardLayout.Instance.Width;
        int boardHeight = BoardLayout.Instance.Height;

        for (int x = startX - moveSpeed; x <= startX + moveSpeed; x++)
        {
            for (int z = startZ - moveSpeed; z <= startZ + moveSpeed; z++)
            {
                // Pálya határain belüli ellenõrzés
                if (x >= 0 && x < boardWidth && z >= 0 && z < boardHeight)
                {
                    // Manhattan-távolság kiszámítása: |x1 - x2| + |z1 - z2|
                    int distance = Mathf.Abs(startX - x) + Mathf.Abs(startZ - z);

                    if (distance <= moveSpeed)
                    {
                        // Csak üres mezõre lehet lépni (vagy a saját kezdõpozíciójára)
                        if (GetUnitAt(x, z) == null || (x == startX && z == startZ))
                        {
                            highlightedTiles.Add(new Vector2Int(x, z));
                        }
                    }
                }
            }
        }

        // Szólunk a BoardLayout-nak, hogy jelenítse meg a zónákat
        if (BoardLayout.Instance != null)
        {
            BoardLayout.Instance.HighlightMovementTiles(highlightedTiles);
        }
    }

    public void MovingUnit(int targetX, int targetZ)
    {
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

        Vector2Int targetPos = new Vector2Int(targetX, targetZ);
        if (!highlightedTiles.Contains(targetPos))
        {
            Debug.LogWarning($"A célmezõ ({targetX}, {targetZ}) kívül esik a(z) {selectedUnitOnBoard.name} mozgási tartományán!");
            return;
        }
        if (selectedUnitOnBoard.Already_Moved!) 
        {
            Debug.LogWarning($"{selectedUnitOnBoard.name} uni már mozgott");
            return;
        }
        if (BoardLayout.Instance != null)
        {
            BoardLayout.Instance.MoveUnitOnBoard(selectedUnitOnBoard, targetX, targetZ);

            Debug.Log($"A(z) {selectedUnitOnBoard.name} sikeresen elmozdult a ({targetX}, {targetZ}) mezõre.");

            DeselectUnit();
        }
    }

    public void DeselectUnit()
    {
        selectedUnitOnBoard = null;
        highlightedTiles.Clear();

        if (BoardLayout.Instance != null)
        {
            BoardLayout.Instance.ClearHighlightVisuals();
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