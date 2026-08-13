using Assets.GameScripts.Model.Game.Enums;

namespace Assets.GameScripts.Model.Game.Player
{
    public class PlayerModel
    {
        public  PlayerEnum Player { get; private set; }
        public string  Name { get; private set; }
        public Deck SelectedDeck { get; private set; }

        public GamePhaseEnum CurrentGamePhase{  get; set; }

        public bool IsActive { get; private set; }
        
        public PlayerModel(PlayerEnum player,Deck deck) 
        {

            this.Player = player;
            this.SelectedDeck = deck;
            IsActive = false;
        }

        public PlayerModel(PlayerEnum player, Deck deck, GamePhaseEnum currentgamephase)
        {

            this.Player = player;
            this.SelectedDeck = deck;
            this.CurrentGamePhase = currentgamephase;
            IsActive = false;
        }


        public bool Isactive => CurrentGamePhase == GamePhaseEnum.Movement;


    }
}
