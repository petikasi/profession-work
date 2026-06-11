using UnityEngine;
using Assets.GameScripts.Model.Game.Enums;
using Assets.GameScripts.Model.Game.Player;

public class TurnModel
{
    private int _num=0;
    private PlayerModel _activePlayer;
    private bool gamestarted=false;

    public int GetNum
    {
        get => _num;
    }
    public bool Getgamestarted
    {
        get => gamestarted;
    }



    public TurnModel(PlayerModel startingPlayer)
    {
        _activePlayer = startingPlayer;
    }

    public void SetActivePayer(PlayerModel model) 
    {

        _activePlayer = model;
        _num++;
    }

    public void SetGameStarted()
    {
        gamestarted = true;
    }
}
