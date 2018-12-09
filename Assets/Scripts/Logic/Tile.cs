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

        neighbors = new List<Coord>();
        neighbors.Add(new Coord(coord.q + 1, coord.r + 0));
        neighbors.Add(new Coord(coord.q + 0, coord.r + 1));

        if(coord.q >0){
            neighbors.Add(new Coord(coord.q - 1, coord.r + 0));
            neighbors.Add(new Coord(coord.q - 1, coord.r + 1));    
        }
        if(coord.r > 0){
            neighbors.Add(new Coord(coord.q + 1, coord.r - 1));
            neighbors.Add(new Coord(coord.q + 0, coord.r - 1));
        }

    }

    public List<Coord> GetNeighbors(){
        return neighbors;
    }

    internal bool isNeighbour(Tile toTile)
    {
        return GetNeighbors().Contains(toTile.coord);
    }

    public override string ToString()
    {
        return "coord: " + coord + " level: " + level + " occupiatBy: "+ occupiatBy;
    }
}
