using System;
using System.Collections.Generic;
using Assets.GameScripts.Model.Game.GameController;
using Assets.GameScripts.ViewModel.Game.UnitHolder;
using UnityEngine;
using UnityEngine.Rendering;

public class GameController : MonoBehaviour
{


    public static GameController Instance { get; private set; }

    private List<BaseUnit> units = new();
    public (UnitTypes Unit, Factions Faction) UnitandFaction { get;  private set; }
    private bool hasSelectedUnit = false;
    private UnitCountpanel selectedCardScript;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UnitandFactionSet(UnitTypes unit, Factions fac, UnitCountpanel cardScript)
    {
        UnitandFaction = (unit, fac);

        selectedCardScript = cardScript;
        hasSelectedUnit = true;

        Debug.Log($"GameController: Egység kijelölve: {fac} - {unit}");
    }
    public void CallPlacing (int x, int z)
    {

        if (!hasSelectedUnit)
        {
            Debug.Log("Nincs kijelölt egység!");
            return;
        }

        if (z >= 5)
        {
            Debug.LogWarning($"Ide nem rakhatsz le egységet! Csak az 5. sorig megengedett. (Kattintott sor: {z})");
            return;
        }

        if (BoardLayout.Instance != null)
        {
            BaseUnit unit = BoardLayout.Instance.PlaceUnitAtTile(x, z, UnitandFaction.Faction, UnitandFaction.Unit);

            if (unit != null)
            {
                units.Add(unit);

                // Sikeres lerakás után levonunk egyet a kártya számlálójából
                if (selectedCardScript != null)
                {
                    selectedCardScript.DecreaseCount();
                }

                // Sikeres lehelyezés után töröljük a kijelölést, hogy ne lehessen spammelni
                ClearSelection();
            }
        }
        else
        {
            Debug.LogError("GameController: Nem található BoardLayout a jelenetben!");
        }

    }
    private void ClearSelection()
    {
        hasSelectedUnit = false;
        selectedCardScript = null;
    }


}
