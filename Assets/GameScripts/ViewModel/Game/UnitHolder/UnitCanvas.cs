using Assets.GameScripts.Model.Game.GameController;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.GameScripts.ViewModel.Game.UnitHolder
{


    public class UnitCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject unitCountPanelPrefab;
        [SerializeField] private Transform unitCountPanelParent;

        // Itt tároljuk a legenerált kártyákat
        private List<UnitCountpanel> unitPanels = new();

        void Start()
        {
            GenerateUnitSelectionUI();
        }

        private void GenerateUnitSelectionUI()
        {
            // 1. UI takarítás az újragenerálás elõtt
            foreach (Transform child in unitCountPanelParent)
            {
                Destroy(child.gameObject);
            }
            unitPanels.Clear();

            // 2. Pakli és frakció lekérése
            var selectedDeck = DeckManagerController.Instance.SelectedDeck;

            if (selectedDeck == null)
            {
                Debug.LogError("UnitCanvas: Nem található kiválasztott pakli!");
                return;
            }

            List<UnitTypes> unitsInDeck = selectedDeck.GetHoleListUnit();
            Factions currentFaction = selectedDeck.FactionsGet;

            foreach (UnitTypes unitType in Enum.GetValues(typeof(UnitTypes)))
            {

                // Megkérdezzük a paklitól, hogy ebbõl a típusból összesen hány darab van
                int totalCountOfThisUnit = selectedDeck.GetCountofUnits(unitType);

                // Biztonsági ellenõrzés: ha valamiért mégis 0 vagy negatív lenne, nem gyártunk kártyát
                if (totalCountOfThisUnit <= 0) continue;

                GameObject newCardObj = Instantiate(unitCountPanelPrefab, unitCountPanelParent);
                newCardObj.name = $"UnitCard_{currentFaction}_{unitType}";

                UnitCountpanel newPanel = new();
                if (newPanel == null)
                {
                    newPanel = newCardObj.GetComponentInChildren<UnitCountpanel>();
                }

                if (newPanel != null)
                {
                    // így a kártyán a teljes mennyiség fog megjelenni!
                    newPanel.Initiate(unitType, currentFaction, totalCountOfThisUnit);

                    // Eltároljuk a listában a kártyát
                    unitPanels.Add(newPanel);
                }
                else
                {
                    Debug.LogError($"Hiba: A '{unitCountPanelPrefab.name}' prefabon nincs UnitCountpanel szkript!");
                }
            }

            Debug.Log($"Sikeresen legenerálva és eltárolva {unitPanels.Count} darab egyedi egységtípus kártya.");
        }
    }
}
