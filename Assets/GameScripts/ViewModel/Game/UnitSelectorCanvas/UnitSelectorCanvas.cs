using Assets.GameScripts.Model.Game.GameControllerFolder;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas
{


    public class UnitSelectorCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject unitCountPanelPrefab;
        [SerializeField] private Transform unitCountPanelParent;
        [SerializeField] private Button startButton;

        private List<UnitCountpanel> unitPanels = new();
        private int allUnitCount;

        void Start()
        {
            GenerateUnitSelectionUI();
            AddEventToStart();
            UpdateStartButtonState();


        }

        private void GenerateUnitSelectionUI()
        {
  
            foreach (var panel in unitPanels)
            {
                if (panel != null) panel.OnUnitCountChanged -= HandleUnitCountChanged;
            }

            foreach (Transform child in unitCountPanelParent)
            {
                Destroy(child.gameObject);
            }
            unitPanels.Clear();

            // 2. Pakli és frakció lekérése
            var selectedDeck = DeckLoaderController.Instance.SelectedDeck;

            if (selectedDeck == null)
            {
                Debug.LogError("UnitCanvas: Nem található kiválasztott pakli!");
                return;
            }

            // Kezdõ egységszám eltárolása
            allUnitCount = selectedDeck.Count;

            FactionsEnum currentFaction = selectedDeck.FactionsGet;

            foreach (UnitTypesEnum unitType in Enum.GetValues(typeof(UnitTypesEnum)))
            {
                int totalCountOfThisUnit = selectedDeck.GetCountofUnits(unitType);

                if (totalCountOfThisUnit <= 0) continue;

                GameObject newCardObj = Instantiate(unitCountPanelPrefab, unitCountPanelParent);
                newCardObj.name = $"UnitCard_{currentFaction}_{unitType}";

                UnitCountpanel newPanel = newCardObj.GetComponent<UnitCountpanel>();

                if (newPanel != null)
                {
                    newPanel.Initiate(unitType, currentFaction, totalCountOfThisUnit);

                    // Feliratkozunk a panel eseményére!
                    newPanel.OnUnitCountChanged += HandleUnitCountChanged;

                    unitPanels.Add(newPanel);
                }
                else
                {
                    Debug.LogError($"Hiba: A '{unitCountPanelPrefab.name}' prefabon nincs UnitCountpanel szkript!");
                }
            }

            Debug.Log($"Sikeresen legenerálva és eltárolva {unitPanels.Count} darab egyedi egységtípus kártya. Összes lehelyezendõ egység: {allUnitCount}");
        }

        /// <summary>
        /// Ez a függvény fut le minden alkalommal, amikor egy egységet leraknak vagy levesznek a pályáról.
        /// </summary>
        private void HandleUnitCountChanged(bool isDecreased)
        {
            if (isDecreased)
            {
                allUnitCount--; // Egység felkerült a pályára
            }
            else
            {
                allUnitCount++; // Egységet visszavettek a pályáról
            }

            Debug.Log($"Még lehelyezésre váró egységek száma: {allUnitCount}");

            UpdateStartButtonState();
        }

        /// <summary>
        /// Aktiválja a Start gombot, ha az összes egység felkerült a pályára (allUnitCount == 0).
        /// </summary>
        private void UpdateStartButtonState()
        {
            if (startButton != null)
            {
                if (allUnitCount == 0)
                {
                    startButton.gameObject.SetActive(true);
                }
                else
                {
                    startButton.gameObject.SetActive(false);
                }

            }
        }

        private void OnDestroy()
        {
            // Biztonsági leiratkozás az objektum megsemmisülésekor
            foreach (var panel in unitPanels)
            {
                if (panel != null) panel.OnUnitCountChanged -= HandleUnitCountChanged;
            }
        }

        private void AddEventToStart() 
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(()
                => {
                    if (startButton != null)
                    {
                        
                    }
                }
            );
        }

    }
}
