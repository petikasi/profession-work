using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameScripts.Model.Game.Enums;
using Assets.GameScripts.Model.Game.GameController;
using Assets.GameScripts.Model.Game.Player;
using Assets.GameScripts.ViewModel.Game.UnitHolder;
using UnityEngine;
using UnityEngine.Rendering;

public class GameController : MonoBehaviour
{


    public static GameController Instance { get; private set; }
    public (UnitTypesEnum Unit, FactionsEnum Faction) UnitandFaction { get;  private set; }
    public PlayerModel Players { get; private set; }
    private bool hasSelectedUnit = false;
    private UnitCountpanel selectedCardScript;
    private List<BaseUnit> units = new();
    public event Action OnEveryUnitPlaced;
    int unitCount;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Players = new PlayerModel(PlayerEnum.White,DeckManagerController.Instance.SelectedDeck);
        if (DeckManagerController.Instance.SelectedDeck != null)
        {
             unitCount = DeckManagerController.Instance.SelectedDeck.Count;
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

        // Ellenõrizzük, hogy az adott helyen áll-e már egység, nehogy egymásra rakjuk õket
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
                if (unitCount==0) 
                {

                    OnEveryUnitPlaced.Invoke();
                }
            }
        }
    }

    private void TryRemoveUnitAt(int x, int z)
    {
        // Megkeressük az egységet a listánkból a koordinátái alapján
        BaseUnit unitToRemove = GetUnitAt(x, z);

        if (unitToRemove != null)
        {
            // 1. Ha megvan az egység kártyája, visszanöveljük a darabszámot a UI-on
            if (unitToRemove.MyCardPanel != null)
            {
                unitToRemove.MyCardPanel.IncreaseCount(); // Ezt a metódust mindjárt hozzáadjuk a kártyához!
                unitCount++;
            }

            // 2. Töröljük a belsõ listánkból
            units.Remove(unitToRemove);

            // 3. Elpusztítjuk a 3D modellt a pályáról
            Destroy(unitToRemove.gameObject);

            Debug.Log($"Egység sikeresen visszavéve a ({x}, {z}) koordinátáról.");
        }
    }
    /// <summary>
    /// Segédfüggvény, ami megkeresi az egységet a koordináták alapján
    /// </summary>
    private BaseUnit GetUnitAt(int x, int z)
    {
        return units.FirstOrDefault(u => u != null && u.TileX == x && u.TileZ == z);
    }
    private void ClearSelection()
    {
        hasSelectedUnit = false;
        selectedCardScript = null;
    }


}
