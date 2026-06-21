using System.Collections.Generic;
using System.Linq;
using Assets.GameScripts.Model.Game.GameControllerFolder;
using Assets.GameScripts.ViewModel.CameraAndGrapichs.Grapic;
using Assets.GameScripts.ViewModel.PrefabsAndAbstracts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.DeckEditor
{
    public  class DeckViewScroller : DeckListerAbstract<Deck>
    {
        [SerializeField] Button modifyButton;
        [SerializeField] Button deleteButton;

        private List<FactionSprites> factionSprites;

        private  void Awake()
        {
            factionSprites = PictureLoder.Instance.GETFACTIONPICTURES;
        }



        protected override void OnEnable()
        {
            base.OnEnable();
            
            DeckLoaderController.Instance.OnRefreshDecklist += base.CreatePage;
            DeckLoaderController.Instance.OnSelectedDeck += ShowModifyAndRenameButton;

            HideModifyAndRenameButton();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            DeckLoaderController.Instance.OnRefreshDecklist -= base.CreatePage;
            DeckLoaderController.Instance.OnSelectedDeck -= ShowModifyAndRenameButton;

            HideModifyAndRenameButton();

        }

        protected override List<Deck> GetItems()
        {
            return DeckLoaderController.Instance.DeckList.decks;
        }

        protected override void SetupItem(GameObject obj, Deck deck)
        {
            var ui = obj.GetComponent<MultiDeckPanel>();

            var factionSprite = factionSprites
                .Find(e => e.ByFaction(deck.FactionsGet));

            ui.Initialize(deck, factionSprite?.GetSprite);
        }

        private void ShowModifyAndRenameButton()
        {
            modifyButton.gameObject.SetActive(true);
            deleteButton.gameObject.SetActive(true);
        }

        private void HideModifyAndRenameButton()
        {
            modifyButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
        }


    }
}
