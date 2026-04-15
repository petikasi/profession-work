using System.Collections.Generic;
using Assets.GameScripts.Model.Game.GameController;
using Assets.GameScripts.ViewModel.DeckEditor;
using Assets.GameScripts.ViewModel.Graphic;
using Assets.GameScripts.ViewModel.PrefabsAndAbstracts;
using UnityEngine;


namespace Assets.GameScripts.ViewModel.SinglePlayer
{
    public class SinglePlayerDeckList : DeckListerAbstract<Deck>
    {

        private List<FactionSprites> factionSprites;

        private void Awake()
        {
            factionSprites = PictureLoder.Instance.GETFACTIONPICTURES;
        }



        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
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

    }

}
