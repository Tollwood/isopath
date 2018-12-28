
using System;

public class Move: IComparable 
{
    public Tile from { get; private set; }
    public Tile to { get; private set; }
    public int score { get; private set; }

    public Move( Tile from, Tile to, int score){
        this.from = from;
        this.to = to;
        this.score = score;
    }

    public override string ToString()
    {
        return "from " + from + " to " + to;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        Move otherMove = obj as Move;
        if (otherMove != null)
            return this.score.CompareTo(otherMove.score);
        else
            throw new ArgumentException("Object is not a Move");
    }
}