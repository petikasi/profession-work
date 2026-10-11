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
    public event Action<bool> OnActivateEndTurnButton
    public event Action<bool> OneSideWon;

    private BaseUnit selectedUnitOnBoard;
    private List<Vector2Int> highlightedMoveTiles = new();
    private List<Vector2Int> highlightedAttackTiles = new();

    private BasicEnemyBehaviour enemyAI = new BasicEnemyBehaviour();
    private int unitCount;

    public PlayerModel GetPlayer => OwnPlayer;

    private enum UnitActionState { None, SelectingMove, SelectingAttack }
    private UnitActionState currentState = UnitActionState.None;

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
        DeselectUnit();
        OwnPlayer.CurrentGamePhase = GamePhaseEnum.Lock;
        OnActivateEndTurnButton?.Invoke(false);
        MoveALlEnemy();
    }

    public void StartPlayerTurn()
    {
        foreach (BaseUnit unit in friendlyunits)
        {
            if (unit != null) unit.Already_Moved = false;
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


    private void MoveALlEnemy()
    {
        StartCoroutine(EnemyTurnRoutine());
    }

    private System.Collections.IEnumerator EnemyTurnRoutine()
    {
        yield return new WaitForSeconds(0.4f);

        int boardWidth = BoardLayout.Instance.Width;
        int boardHeight = BoardLayout.Instance.Height;

        // Végigmegyünk az összes élõ ellenséges egységen
        foreach (BaseUnit enemy in enemyunits.ToList())
        {
            if (enemy == null || enemy.HealthPoint <= 0) continue;

            // Érvényes játékos egységek szûrése
            List<BaseUnit> livingPlayerUnits = friendlyunits.Where(u => u != null && u.HealthPoint > 0).ToList();
            if (livingPlayerUnits.Count == 0) break;

            // 1. CÉLPONT KIVÁLASZTÁSA a külsõ osztály metódusával
            BaseUnit target = enemyAI.ChooseTargetBasedOnDistance(enemy, livingPlayerUnits);
            if (target == null) continue;

            int currentDistance = GetManhattanDistance(enemy.TileX, enemy.TileZ, target.TileX, target.TileZ);

            // 2. HA MÁR HATÓTÁVBAN VAN: Támad, és nem lép
            if (currentDistance <= enemy.Range)
            {
                ExecuteAttack(enemy, target);
                yield return new WaitForSeconds(0.6f);
                continue;
            }

            // 3. HA NINCS HATÓTÁVBAN: Lép a célponthoz legközelebbi szabad mezõre
            Vector2Int bestMoveTile = FindBestMoveTileTowards(enemy, target, boardWidth, boardHeight);

            if (bestMoveTile.x != enemy.TileX || bestMoveTile.y != enemy.TileZ)
            {
                BoardLayout.Instance.MoveUnitOnBoard(enemy, bestMoveTile.x, bestMoveTile.y);
                yield return new WaitForSeconds(0.6f); // Megvárjuk a lépés animációját
            }

            // 4. LÉPÉS UTÁNI TÁMADÁS: Ha az új pozícióból már eléri a célt, megtámadja
            int newDistance = GetManhattanDistance(enemy.TileX, enemy.TileZ, target.TileX, target.TileZ);
            if (newDistance <= enemy.Range)
            {
                ExecuteAttack(enemy, target);
                yield return new WaitForSeconds(0.6f);
            }
        }

        yield return new WaitForSeconds(0.4f);

        // Az összes ellenség lépése és támadása lezárult -> játékos köre indul
        StartPlayerTurn();
    }

    private Vector2Int FindBestMoveTileTowards(BaseUnit bot, BaseUnit target, int boardWidth, int boardHeight)
    {
        Vector2Int bestTile = new Vector2Int(bot.TileX, bot.TileZ);
        int minDistance = GetManhattanDistance(bot.TileX, bot.TileZ, target.TileX, target.TileZ);
        int speed = bot.MovementSpeed;

        for (int x = bot.TileX - speed; x <= bot.TileX + speed; x++)
        {
            for (int z = bot.TileZ - speed; z <= bot.TileZ + speed; z++)
            {
                if (IsWithinBounds(x, z, boardWidth, boardHeight))
                {
                    int moveCost = GetManhattanDistance(bot.TileX, bot.TileZ, x, z);

                    if (moveCost <= speed)
                    {
                        BaseUnit unitAtTile = GetUnitAt(x, z);

                        // Csak üres mezõre vagy a saját jelenlegi helyére léphet
                        if (unitAtTile == null || unitAtTile == bot)
                        {
                            int distToTarget = GetManhattanDistance(x, z, target.TileX, target.TileZ);
                            if (distToTarget < minDistance)
                            {
                                minDistance = distToTarget;
                                bestTile = new Vector2Int(x, z);
                            }
                        }
                    }
                }
            }
        }

        return bestTile;
    }

    private void PlaceAllEnemy()
    {
        if (BoardLayout.Instance == null)
        {
            Debug.LogError("[UserGameController] BoardLayout.Instance NULL, nem lehet lehelyezni az ellenségeket!");
            return;
        }

        DeckManager deckManager = new DeckManager();
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

    // ==========================================
    // MOZGÁSI ÉS HARCI INTERAKCIÓK
    // ==========================================

    public void HandleMovementPhaseClick(int tileX, int tileZ)
    {
        BaseUnit clickedUnit = GetUnitAt(tileX, tileZ);

        // 1. ÁLLAPOT: Nincs még kiválasztva senki VAGY új saját egységre kattintunk
        if (currentState == UnitActionState.None)
        {
            if (clickedUnit != null && friendlyunits.Contains(clickedUnit))
            {
                SelectUnit(clickedUnit);
            }
            return;
        }

        // Ha a játékos egy másik saját egységre kattint a mozgás elõtt, váltsunk át arra
        if (currentState == UnitActionState.SelectingMove && clickedUnit != null && friendlyunits.Contains(clickedUnit))
        {
            SelectUnit(clickedUnit);
            return;
        }

        // 2. ÁLLAPOT: Mozgás vagy helyben támadás kiválasztása
        if (currentState == UnitActionState.SelectingMove)
        {
            // Opció A: Azonnali támadás (egy helyben állva egy piros mezõre kattintva)
            if (highlightedAttackTiles.Contains(new Vector2Int(tileX, tileZ)) && clickedUnit != null && enemyunits.Contains(clickedUnit))
            {
                ExecuteAttack(selectedUnitOnBoard, clickedUnit);
                FinishCurrentUnitTurn();
                return;
            }

            // Opció B: Lépés egy zöld mezõre
            if (highlightedMoveTiles.Contains(new Vector2Int(tileX, tileZ)) && clickedUnit == null)
            {
                BoardLayout.Instance.MoveUnitOnBoard(selectedUnitOnBoard, tileX, tileZ);

                // Átlépünk a támadási fázisba az új pozícióból
                currentState = UnitActionState.SelectingAttack;
                ShowAttackTargetsFromPosition(tileX, tileZ, selectedUnitOnBoard.Range);
                return;
            }
        }

        // 3. ÁLLAPOT: Támadás a sikeres lépés után
        if (currentState == UnitActionState.SelectingAttack)
        {
            if (highlightedAttackTiles.Contains(new Vector2Int(tileX, tileZ)) && clickedUnit != null && enemyunits.Contains(clickedUnit))
            {
                ExecuteAttack(selectedUnitOnBoard, clickedUnit);
            }

            // Akár támadtunk, akár üres mezõre/máshová kattintottunk, a kör véget ér az egységnek
            FinishCurrentUnitTurn();
        }
    }

    private void SelectUnit(BaseUnit unit)
    {
        if (unit.Already_Moved)
        {
            Debug.Log($"{unit.name} már mozgott ebben a körben!");
            return;
        }

        DeselectUnit();
        selectedUnitOnBoard = unit;
        currentState = UnitActionState.SelectingMove;

        CalculateAndShowInitialRanges(selectedUnitOnBoard.TileX, selectedUnitOnBoard.TileZ, selectedUnitOnBoard.MovementSpeed, selectedUnitOnBoard.Range);
    }

    private void CalculateAndShowInitialRanges(int startX, int startZ, int moveSpeed, int attackRange)
    {
        highlightedMoveTiles.Clear();
        highlightedAttackTiles.Clear();
        List<Vector2Int> friendlyTiles = new List<Vector2Int>();

        int boardWidth = BoardLayout.Instance.Width;
        int boardHeight = BoardLayout.Instance.Height;

        // Zöld (Mozgási) és Kék (Baráti) mezõk keresése
        for (int x = startX - moveSpeed; x <= startX + moveSpeed; x++)
        {
            for (int z = startZ - moveSpeed; z <= startZ + moveSpeed; z++)
            {
                if (IsWithinBounds(x, z, boardWidth, boardHeight) && GetManhattanDistance(startX, startZ, x, z) <= moveSpeed)
                {
                    BaseUnit unitOnTile = GetUnitAt(x, z);
                    if (unitOnTile == null)
                    {
                        highlightedMoveTiles.Add(new Vector2Int(x, z));
                    }
                    else if (friendlyunits.Contains(unitOnTile))
                    {
                        friendlyTiles.Add(new Vector2Int(x, z));
                    }
                }
            }
        }

        // Piros (Azonnal támadható ellenségek) keresése
        for (int x = startX - attackRange; x <= startX + attackRange; x++)
        {
            for (int z = startZ - attackRange; z <= startZ + attackRange; z++)
            {
                int dist = GetManhattanDistance(startX, startZ, x, z);
                if (IsWithinBounds(x, z, boardWidth, boardHeight) && dist > 0 && dist <= attackRange)
                {
                    BaseUnit unitOnTile = GetUnitAt(x, z);
                    if (unitOnTile != null && enemyunits.Contains(unitOnTile))
                    {
                        highlightedAttackTiles.Add(new Vector2Int(x, z));
                    }
                }
            }
        }

        if (BoardLayout.Instance != null)
        {
            BoardLayout.Instance.HighlightMovementTiles(highlightedMoveTiles, friendlyTiles, highlightedAttackTiles);
        }
    }

    private void ShowAttackTargetsFromPosition(int currentX, int currentZ, int attackRange)
    {
        highlightedMoveTiles.Clear();
        highlightedAttackTiles.Clear();

        int boardWidth = BoardLayout.Instance.Width;
        int boardHeight = BoardLayout.Instance.Height;

        for (int x = currentX - attackRange; x <= currentX + attackRange; x++)
        {
            for (int z = currentZ - attackRange; z <= currentZ + attackRange; z++)
            {
                int dist = GetManhattanDistance(currentX, currentZ, x, z);
                if (IsWithinBounds(x, z, boardWidth, boardHeight) && dist > 0 && dist <= attackRange)
                {
                    BaseUnit unitOnTile = GetUnitAt(x, z);
                    if (unitOnTile != null && enemyunits.Contains(unitOnTile))
                    {
                        highlightedAttackTiles.Add(new Vector2Int(x, z));
                    }
                }
            }
        }

        if (highlightedAttackTiles.Count == 0)
        {
            Debug.Log("Lépés után nincs ellenség hatótávban, az egység köre lezárult.");
            FinishCurrentUnitTurn();
            return;
        }

        if (BoardLayout.Instance != null)
        {
            // Csak a piros ellenséges célpontokat rajzoljuk ki
            BoardLayout.Instance.HighlightMovementTiles(null, null, highlightedAttackTiles);
        }
    }

    private void ExecuteAttack(BaseUnit attacker, BaseUnit target)
    {
        attacker.AttackEnemy(target);

        if (target.HealthPoint <= 0)
        {
            enemyunits.Remove(target);
        }
    }

    private void FinishCurrentUnitTurn()
    {
        if (selectedUnitOnBoard != null)
        {
            selectedUnitOnBoard.Already_Moved = true;
        }
        DeselectUnit();
    }

    public void DeselectUnit()
    {
        selectedUnitOnBoard = null;
        currentState = UnitActionState.None;
        highlightedMoveTiles.Clear();
        highlightedAttackTiles.Clear();

        if (BoardLayout.Instance != null)
        {
            BoardLayout.Instance.ClearHighlightVisuals();
        }
    }

    private bool IsWithinBounds(int x, int z, int width, int height)
    {
        return x >= 0 && x < width && z >= 0 && z < height;
    }

    private int GetManhattanDistance(int x1, int z1, int x2, int z2)
    {
        return Mathf.Abs(x1 - x2) + Mathf.Abs(z1 - z2);
    }

    // ==========================================
    // LELYSZÍNI LEHELYEZÉS ÉS VISSZAVÉTEL
    // ==========================================

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
                unit.Player = PlayerEnum.White;
                friendlyunits.Add(unit);
                selectedCardScript.DecreaseCount();
                ClearSelection();
                unitCount--;
                OnEveryUnitPlaced?.Invoke(unitCount == 0);
            }
        }
    }

    private void TryRemoveUnitAt(int x, int z)
    {
        BaseUnit unitToRemove = GetUnitAt(x, z);

        if (unitToRemove != null && friendlyunits.Contains(unitToRemove))
        {
            if (unitToRemove.MyCardPanel != null)
            {
                unitToRemove.MyCardPanel.IncreaseCount();
                unitCount++;
            }

            friendlyunits.Remove(unitToRemove);
            Destroy(unitToRemove.gameObject);
        }
    }

    public BaseUnit GetUnitAt(int x, int z)
    {
        BaseUnit baseunit = friendlyunits.FirstOrDefault(u => u != null && u.TileX == x && u.TileZ == z);
        if (baseunit != null) return baseunit;

        return enemyunits.FirstOrDefault(u => u != null && u.TileX == x && u.TileZ == z);
    }

    private void ClearSelection()
    {
        hasSelectedUnit = false;
        selectedCardScript = null;
    }
}