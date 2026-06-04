using Assets.GameScripts.Model.Game.Enums;

namespace Assets.GameScripts.Model.Game.Player
{
    public class PlayerModel
    {
        public  PlayerEnum Player { get; private set; }
        public string  Name { get; private set; }
        public Deck Deck { get; private set; }

        public bool IsActive { get; private set; }
        
        public PlayerModel(PlayerEnum player,Deck deck) 
        {

            this.Player = player;
            this.Deck = deck;
            IsActive = false;
        }


    }
}
