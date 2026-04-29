using System.Collections.Generic;
using Assets.GameScripts.Model.Game.GameController;
using Assets.GameScripts.ViewModel.DeckEditor;
using Assets.GameScripts.ViewModel.Graphic;
using Assets.GameScripts.ViewModel.PrefabsAndAbstracts;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.GameScripts.ViewModel.SinglePlayer
{
    public class SinglePlayerDeckList : DeckListerAbstract<Deck>
    {
        [SerializeField] protected Button playButton;
        private List<FactionSprites> factionSprites;

        private void Awake()
        {
            factionSprites = PictureLoder.Instance.GETFACTIONPICTURES;
        }



        protected override void OnEnable()
        {
            base.OnEnable();
            DeckManagerController.Instance.Selecteddeck += ShowPlayAndRenameButton;
            HidePlayAndRenameButton();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            DeckManagerController.Instance.Selecteddeck += ShowPlayAndRenameButton;
        }

        protected override List<Deck> GetItems()
        {
            return DeckManagerController.Instance.DeckList.decks;
        }

        protected override void SetupItem(GameObject obj, Deck deck)
        {
            var ui = obj.GetComponent<MultiDeckPanel>();

            var factionSprite = factionSprites
                .Find(e => e.ByFaction(deck.FactionsGet));

            ui.Initialize(deck, factionSprite?.GetSprite);
        }

        private void ShowPlayAndRenameButton() 
        {

            playButton.gameObject.SetActive(true);
        }
        private void HidePlayAndRenameButton()
        {

            playButton.gameObject.SetActive(false);
        }


    }

}
