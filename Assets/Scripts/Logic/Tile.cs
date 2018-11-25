using System.Collections.Generic;

public class Tile
{

    public Coord coord { get; private set; }
    public TileLevel level { get; set; }
    public bool occupiat { get; set; }
    public Player? occupiatBy { get; set; }
    private List<Coord> neighbors;

    public Tile( Coord coord, TileLevel level = TileLevel.GROUND, bool occupiat = false)
    {
        this.coord = coord;
        this.level = level;
        this.occupiat = occupiat;

        neighbors = new List<Coord>();
        neighbors.Add(new Coord(coord.q + 1, coord.r + 0));
        neighbors.Add(new Coord(coord.q + 1, coord.r - 1));
        neighbors.Add(new Coord(coord.q + 0, coord.r - 1));
        neighbors.Add(new Coord(coord.q - 1, coord.r + 0));
        neighbors.Add(new Coord(coord.q - 1, coord.r + 1));
        neighbors.Add(new Coord(coord.q + 0, coord.r + 1));

    }

    public List<Coord> GetNeighbors(){
        return neighbors;
    }
}
