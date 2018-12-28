using System.Collections.Generic;

[System.Serializable]
public class Tile
{

    public Coord coord { get; private set; }
    public TileLevel level { get; private set; }
    public Player? occupiatBy { get; private set; }
    private List<Coord> neighbors;

    public Tile( Coord coord, TileLevel level = TileLevel.GROUND, Player? occupiatBy = null)
    {
        this.coord = coord;
        this.level = level;
        this.occupiatBy = occupiatBy;
    }

   
    public override string ToString()
    {
        return "coord: " + coord + " level: " + level + " occupiatBy: "+ occupiatBy;
    }
}
