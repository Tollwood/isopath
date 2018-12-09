
public class Move
{
    public Tile from { get; private set; }
    public Tile to { get; private set; }
    public Player player { get; private set; }

    public Move( Tile from, Tile to, Player player){
        this.from = from;
        this.to = to;
        this.player = player;
    }

    public override string ToString()
    {
        return player +"moved from " + from + " to " + to;
    }
}