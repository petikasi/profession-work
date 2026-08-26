using Assets.GameScripts.Model.Game.Enums;
using Assets.GameScripts.Model.Game.GameControllerFolder;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas
{
    public class UnitSelectorCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject unitCountPanelPrefab;
        [SerializeField] private Transform unitCountPanelParent;
        [SerializeField] private Button randButton;

        private List<UnitCountpanel> unitPanels = new();
        private int allUnitCount;

        private void OnEnable()
        {
            if (UserGameController.Instance != null)
            {
                UserGameController.Instance.OnDestroyUnececeryView += DestroyUnitPanels;
            }
        }

        private void OnDisable()
        {
            if (UserGameController.Instance != null)
            {
                UserGameController.Instance.OnDestroyUnececeryView -= DestroyUnitPanels;
            }

            // Tisztítsuk meg a feliratkozásokat a megelõzés érdekében
            foreach (var panel in unitPanels)
            {
                if (panel != null)
                {
                    panel.OnUnitCountChanged -= HandleUnitCountChanged;
                }
            }
        }

        void Start()
        {
            GenerateUnitSelectionUI();

            if (randButton != null)
            {
                randButton.onClick.RemoveAllListeners();
                randButton.onClick.AddListener(() =>
                {
                    if (UserGameController.Instance != null)
                    {
                        UserGameController.Instance.StartPlayerTurn();
                    }
                });
            }
        }

        private void DestroyUnitPanels()
        {
            Debug.Log("[UnitSelectorCanvas] Egységválasztó panelek, szülõ konténer és a script eltávolítása...");

            // 1. Töröljük a gyermek kártyákat
            foreach (var unitPanel in unitPanels)
            {
                if (unitPanel != null && unitPanel.gameObject != null)
                {
                    Destroy(unitPanel.gameObject);
                }
            }
            unitPanels.Clear();

            // 3. Töröljük a szülõ Panel GameObject-jét (amiben a kártyák voltak)
            /*if (unitCountPanelParent != null)
            {
                Destroy(unitCountPanelParent.gameObject);
            }*/

            // 4. Eltávolítjuk magát a UnitSelectorCanvas script komponenst a Canvasról
           //Destroy(this);
        }

        private void GenerateUnitSelectionUI()
        {
            foreach (var panel in unitPanels)
            {
                if (panel != null) panel.OnUnitCountChanged -= HandleUnitCountChanged;
            }

            if (unitCountPanelParent != null)
            {
                foreach (Transform child in unitCountPanelParent)
                {
                    Destroy(child.gameObject);
                }
            }
            unitPanels.Clear();

            var selectedDeck = DeckLoaderController.Instance.SelectedDeck;

            if (selectedDeck == null)
            {
                Debug.LogError("UnitCanvas: Nem található kiválasztott pakli!");
                return;
            }

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
                    newPanel.OnUnitCountChanged += HandleUnitCountChanged;
                    unitPanels.Add(newPanel);
                }
                else
                {
                    Debug.LogError($"Hiba: A '{unitCountPanelPrefab.name}' prefabon nincs UnitCountpanel szkript!");
                }
            }

            Debug.Log($"Sikeresen legenerálva {unitPanels.Count} darab kártya panel.");
        }

        private void HandleUnitCountChanged(bool isDecreased)
        {
            if (isDecreased)
            {
                allUnitCount--;
            }
            else
            {
                allUnitCount++;
            }

            Debug.Log($"Még lehelyezésre váró egységek száma: {allUnitCount}");
        }
    }
}