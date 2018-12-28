
using System;

public class ScoredTile
{
    public Tile tile { get; private set; }
    public int score { get; private set; }

    public ScoredTile( Tile tile, int score){
        this.tile = tile;
        this.score = score;
    }
}