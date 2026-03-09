using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameScripts.Model.Deckmaker.Deks;
using Assets.GameScripts.Model.Game.GameController;
using GameScripts.Model.Units;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

namespace Assets.GameScripts.ViewModel.DeckEditor
{
    public  class DeckViewScroller :MonoBehaviour
    {
        [SerializeField] GameObject itemPrefab;
        [SerializeField] Transform gridParent;
        [SerializeField] Button nextButton;
        [SerializeField] Button previousButton;
        [SerializeField] Button modifyButton;
        [SerializeField] Button renameButton;
        [SerializeField] Button deleteButton;
        private List<FactionSprites> factionSprites;

        public int itemsPerPage = 9;

        private int currentPage = 0;

        private void Awake()
        {
            factionSprites = new List<FactionSprites>();
            AddSpritesToList();
        }



        private void OnEnable()
        {
            DeckManagerController.Instance.RefreshDecklist += ShowPage;
            DeckManagerController.Instance.Selecteddeck += SetModifyAndRenameButton;
            nextButton.onClick.AddListener(NextPage);
            previousButton.onClick.AddListener(PreviousPage);
            modifyButton.gameObject.SetActive(false);
            renameButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
            ShowPage();
        }

        private void OnDisable()
        {
            DeckManagerController.Instance.RefreshDecklist += ShowPage;
            DeckManagerController.Instance.Selecteddeck += SetModifyAndRenameButton;
            nextButton.onClick.RemoveListener(NextPage);
            previousButton.onClick.RemoveListener(PreviousPage);
            ClearPage();

        }

        public void ShowPage()
        {

            int startIndex = currentPage * itemsPerPage;
            int endIndex = Mathf.Min(startIndex + itemsPerPage, DeckManagerController.Instance.DeckList.decks.Count);
            var pageDecks = DeckManagerController.Instance.DeckList.decks
            .Skip(startIndex)
            .Take(itemsPerPage)
            .ToList();

            foreach (var deck in pageDecks)
            {
                GameObject deckpanel = Instantiate(itemPrefab, gridParent);
                DeckPanel deckpanelUI = deckpanel.GetComponent<DeckPanel>();
                FactionSprites factionsprite= factionSprites.Find(e => e.ByFaction(deck.FactionsGet));
                deckpanelUI.Initialize(deck,factionsprite.GetSprite);
                        


            }

            previousButton.gameObject.SetActive(currentPage > 0);
            nextButton.gameObject.SetActive(endIndex < DeckManagerController.Instance.DeckList.decks.Count);
            
        }

        public void ClearPage() 
        {

            foreach (Transform child in gridParent)
            {
                Destroy(child.gameObject);
            }

        }

        private void NextPage()
        {
            currentPage++;
            ShowPage();
        }

        private void PreviousPage()
        {
            currentPage--;
            ShowPage();
        }

        private void SetModifyAndRenameButton() 
        {
            modifyButton.gameObject.SetActive(true);
            renameButton.gameObject.SetActive(true);
            deleteButton.gameObject.SetActive(true);
        }
        private void AddSpritesToList()
        {

            foreach (Factions fac in Enum.GetValues(typeof(Factions)))
            {

                string key = $"FactionPictures/{fac}";
                Sprite sprite = Resources.Load<Sprite>(key);
                if (sprite == null)
                {
                    Debug.LogWarning($"Sprite for {fac} not found at key: {key}");
                }
                factionSprites.Add(
                    new FactionSprites(fac, sprite)
                );

                Debug.Log($"{fac} faction finished loading into memory");
            }

        }


    }
}
