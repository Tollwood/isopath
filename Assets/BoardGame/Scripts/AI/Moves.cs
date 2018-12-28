
public class Moves
{
    public Move buildMove { get; private set; }
    public Move stoneMove { get; private set; }
    public Player player { get; private set; }

    public Moves( Move buildMove, Move stoneMove,Player player){
        this.buildMove = buildMove;
        this.stoneMove = stoneMove;
        this.player = player;
    }

    public override string ToString()
    {
        return player +" build " + buildMove + " and moves stone " + stoneMove;
    }
}