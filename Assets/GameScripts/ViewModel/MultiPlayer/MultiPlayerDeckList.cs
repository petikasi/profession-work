using System.Collections.Generic;
using Assets.GameScripts.Model.Game.GameController;
using Assets.GameScripts.ViewModel.DeckEditor;
using Assets.GameScripts.ViewModel.Graphic;
using Assets.GameScripts.ViewModel.PrefabsAndAbstracts;
using UnityEngine;


namespace Assets.GameScripts.ViewModel.MutliPlayer 
{
    public class MultiPlayerDeckList : DeckListerAbstract<Deck>
    {

        private List<FactionSprites> factionSprites;

        private void Awake()
        {
            factionSprites = PictureLoder.Instance.GETFACTIONPICTURES;
        }



        protected override void OnEnable()
        {
            base.OnEnable();

            DeckManagerController.Instance.RefreshDecklist += base.CreatePage;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            DeckManagerController.Instance.RefreshDecklist -= base.CreatePage;

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
