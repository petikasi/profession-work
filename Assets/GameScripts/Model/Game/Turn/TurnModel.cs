using UnityEngine;
using Assets.GameScripts.Model.Game.Enums;

public class TurnModel
{
    private int _num;
    private PlayerEnum _activePlayer;

    public int Num
    {
        get => _num;
        set => _num = value;
    }

    public PlayerEnum ActivePlayer
    {
        get => _activePlayer;
        set => _activePlayer = value;
    }

    public TurnModel(int startTurnNum, PlayerEnum startingPlayer)
    {
        _num = startTurnNum;
        _activePlayer = startingPlayer;
    }
}