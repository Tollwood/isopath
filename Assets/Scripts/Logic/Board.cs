using System;

[System.Serializable]
public class Board
{
    public Settings settings {get; private set;}
    public Tile[,] tiles { get; private set; }
    public Player currentPlayer { get; private set; }
    public Step currentStep { get; private set;}
    public int size { get; private set; }

    public Board(Settings settings, Tile[,] tiles){
        this.settings = settings;
        this.size = settings.size;
        this.currentPlayer = Player.DIGGER;
        this.currentStep = Step.BUILD;
        this.tiles = tiles;

    }

    public Board(int size, Tile[,] tiles, Step currentStep, Player currentPlayer)
    {
        this.size = size;
        this.tiles = tiles;
        this.currentStep= currentStep;
        this.currentPlayer = currentPlayer;
    }
}
