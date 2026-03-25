using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameScripts.Model.Game.GameController;
using Assets.GameScripts.ViewModel.Graphic;
using UnityEngine;
using UnityEngine.UI;

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
            factionSprites = PictureLoder.Instance.GETFACTIONPICTURES;
        }



        private void OnEnable()
        {
            DeckManagerController.Instance.RefreshDecklist += ShowPage;
            DeckManagerController.Instance.Selecteddeck += ShowModifyAndRenameButton;
            nextButton.onClick.AddListener(NextPage);
            previousButton.onClick.AddListener(PreviousPage);
            modifyButton.gameObject.SetActive(false);
            renameButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
            ShowPage();
        }

        private void OnDisable()
        {
            DeckManagerController.Instance.RefreshDecklist -= ShowPage;
            DeckManagerController.Instance.Selecteddeck -= ShowModifyAndRenameButton;
            nextButton.onClick.RemoveListener(NextPage);
            previousButton.onClick.RemoveListener(PreviousPage);
            ClearPage();
            HideModifyAndRenameButton();

        }

        public void ShowPage()
        {

            ClearPage();

            int startIndex = currentPage * itemsPerPage;
            int endIndex = Mathf.Min(startIndex + itemsPerPage, DeckManagerController.Instance.DeckList.decks.Count);
            var pageDecks = DeckManagerController.Instance.DeckList.decks
            .Skip(startIndex)
            .Take(itemsPerPage)
            .ToList();

            foreach (var deck in pageDecks)
            {
                GameObject deckpanel = Instantiate(itemPrefab, gridParent);
                MultiDeckPanel deckpanelUI = deckpanel.GetComponent<MultiDeckPanel>();
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
                child.gameObject.SetActive(false);
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

        private void ShowModifyAndRenameButton() 
        {
            modifyButton.gameObject.SetActive(true);
            renameButton.gameObject.SetActive(true);
            deleteButton.gameObject.SetActive(true);
        }

        private void HideModifyAndRenameButton()
        {
            modifyButton.gameObject.SetActive(false);
            renameButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
        }


    }
}
