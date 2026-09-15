using Assets.GameScripts.Model.Game.Enums;
using Assets.GameScripts.Model.Game.GameControllerFolder;
using Assets.GameScripts.Model.Game.Board;
using Assets.GameScripts.Model.Game.Player;
using Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.GameScripts.Model.Game.Players.BasicEnemy;
using UnityEngine.InputSystem.iOS;

public class UserGameController : MonoBehaviour
{
    public static UserGameController Instance { get; private set; }
    public (UnitTypesEnum Unit, FactionsEnum Faction) UnitandFaction { get; private set; }
    public PlayerModel OwnPlayer { get; private set; }
    private bool hasSelectedUnit = false;
    private UnitCountpanel selectedCardScript;
    private List<BaseUnit> friendlyunits = new();
    private List<BaseUnit> enemyunits = new();

    public event Action<bool> OnEveryUnitPlaced;
    public event Action OnDestroyUnececeryView;
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
        OnActivateEndTurnButton?.Invoke(false);
    }

    public void StartPlayerTurn()
    {

        foreach (BaseUnit unit in friendlyunits) 
        {
            unit.Already_Moved = false;
        
        }
        OwnPlayer.CurrentGamePhase = GamePhaseEnum.Movement;
        OnActivateEndTurnButton?.Invoke(true);
    }



    public void SetPlayerToActive()
    {
        PlaceAllEnemy();
        OwnPlayer.CurrentGamePhase = GamePhaseEnum.Movement;

        if (BoardLayout.Instance != null)
        {
            BoardLayout.Instance.RemoveZoneVisuals();
        }

        OnActivateEndTurnButton?.Invoke(true);
        OnDestroyUnececeryView?.Invoke();

    }

    private void PlaceAllEnemy()
    {
        if (BoardLayout.Instance == null)
        {
            Debug.LogError("[UserGameController] BoardLayout.Instance NULL, nem lehet lehelyezni az ellenségeket!");
            return;
        }

        List<(FactionsEnum Faction, UnitTypesEnum UnitType)> enemyDeck = DeckManager.GetRandomDeck();

        if (enemyDeck == null || enemyDeck.Count == 0)
        {
            Debug.LogWarning("[UserGameController] Az ellenséges pakli üres!");
            return;
        }

        List<Vector2Int> availableTiles = new List<Vector2Int>();
        int boardWidth = BoardLayout.Instance.Width;
        int boardHeight = BoardLayout.Instance.Height;
        int enemyZoneStart = boardHeight - BoardLayout.Instance.PLAYER_ZONE;

        for (int x = 0; x < boardWidth; x++)
        {
            for (int z = enemyZoneStart; z < boardHeight; z++)
            {

                if (GetUnitAt(x, z) == null)
                {
                    availableTiles.Add(new Vector2Int(x, z));
                }
            }
        }

        for (int i = 0; i < availableTiles.Count; i++)
        {
            Vector2Int temp = availableTiles[i];
            int randomIndex = UnityEngine.Random.Range(i, availableTiles.Count);
            availableTiles[i] = availableTiles[randomIndex];
            availableTiles[randomIndex] = temp;
        }


        int placeCount = Mathf.Min(enemyDeck.Count, availableTiles.Count);

        for (int i = 0; i < placeCount; i++)
        {
            var unitData = enemyDeck[i];
            Vector2Int tile = availableTiles[i];

            BaseUnit spawnedEnemy = BoardLayout.Instance.PlaceUnitAtTile(
                tile.x,  
                tile.y,
                unitData.Faction,
                unitData.UnitType,
                null
            );

            if (spawnedEnemy != null)
            {
                spawnedEnemy.Player = PlayerEnum.Black;
                enemyunits.Add(spawnedEnemy);
            }
        }

        Debug.Log($"[UserGameController] Sikeresen lehelyezve {enemyunits.Count} ellenséges egység.");
    }

    private void MoveEnemy()
    {


    }

    public void TryAttack(BaseUnit attacker, int targetX, int targetZ)
    {
        BaseUnit targetUnit = GetUnitAt(targetX, targetZ);

        if (attacker != null && targetUnit != null)
        {
            // Manhattan távolság ellenõrzése a Range (hatótáv) alapján
            int distance = Mathf.Abs(attacker.TileX - targetX) + Mathf.Abs(attacker.TileZ - targetZ);

            if (distance <= attacker.Range)
            {
                attacker.AttackEnemy(targetUnit);
                attacker.Already_Moved = true;
            }
            else
            {
                Debug.LogWarning("A célpont kívül esik a hatótávon!");
            }
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
                friendlyunits.Add(unit);
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
        List<Vector2Int> friendlyTiles = new List<Vector2Int>();
        List<Vector2Int> enemyTiles = new List<Vector2Int>();

        int boardWidth = BoardLayout.Instance.Width;
        int boardHeight = BoardLayout.Instance.Height;

        for (int x = startX - moveSpeed; x <= startX + moveSpeed; x++)
        {
            for (int z = startZ - moveSpeed; z <= startZ + moveSpeed; z++)
            {
                if (x >= 0 && x < boardWidth && z >= 0 && z < boardHeight)
                {
                    int distance = Mathf.Abs(startX - x) + Mathf.Abs(startZ - z);

                    if (distance <= moveSpeed)
                    {
                        BaseUnit unitOnTile = GetUnitAt(x, z);
                        Debug.Log(enemyunits.Count);
                        // 1. Zöld: Üres mezõ
                        if (unitOnTile == null)
                        {
                            highlightedTiles.Add(new Vector2Int(x, z));
                        }
                        // 2. Kék: Saját/Szövetséges egység (beleértve a saját kezdõpozícióját is)
                        else if (friendlyunits.Contains(unitOnTile))
                        {
                            friendlyTiles.Add(new Vector2Int(x, z));
                        }
                        // 3. Piros: Ellenséges
                        else if (enemyunits.Contains(unitOnTile))
                        {
                            enemyTiles.Add(new Vector2Int(x, z));
                        }
                    }
                }
            }
        }

 
        if (BoardLayout.Instance != null)
        {
            BoardLayout.Instance.HighlightMovementTiles(highlightedTiles, friendlyTiles, enemyTiles);
        }
    }

    private void TryAtackUnit() 
    { 
    
    
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

        if (selectedUnitOnBoard.Already_Moved)
        {
            Debug.LogWarning($"{selectedUnitOnBoard.name} egység már mozgott ebben a körben!");
            return;
        }

        BaseUnit targetUnit = GetUnitAt(targetX, targetZ);

        if (targetUnit != null && enemyunits.Contains(targetUnit))
        {
            TryAttack(selectedUnitOnBoard, targetX, targetZ);
            DeselectUnit();
            return;
        }

        Vector2Int targetPos = new Vector2Int(targetX, targetZ);
        if (!highlightedTiles.Contains(targetPos))
        {
            Debug.LogWarning($"A célmezõ ({targetX}, {targetZ}) kívül esik a(z) {selectedUnitOnBoard.name} mozgási tartományán!");
            return;
        }

        if (BoardLayout.Instance != null)
        {
            BoardLayout.Instance.MoveUnitOnBoard(selectedUnitOnBoard, targetX, targetZ);
            selectedUnitOnBoard.Already_Moved = true; // Kijelöljük, hogy a körben már mozgott
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

            friendlyunits.Remove(unitToRemove);
            Destroy(unitToRemove.gameObject);

            Debug.Log($"Egység sikeresen visszavéve a ({x}, {z}) koordinátáról.");
        }
    }

    public BaseUnit GetUnitAt(int x, int z)
    {
        BaseUnit baseunit;

        baseunit = friendlyunits.FirstOrDefault(u => u != null && u.TileX == x && u.TileZ == z);
        if (null != baseunit) return baseunit;
        baseunit = enemyunits.FirstOrDefault(u => u != null && u.TileX == x && u.TileZ == z);
        return baseunit;
    }

    private void ClearSelection()
    {
        hasSelectedUnit = false;
        selectedCardScript = null;
    }
}