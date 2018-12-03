using System.Collections.Generic;

[System.Serializable]
public class Board {

    public Dictionary<Coord,Tile> tiles;
    public Player currentPlayer = Player.DIGGER;
    public Step currentStep = Step.BUILD;
    public int boardSize = 4;
    public bool dirty { get; set;}

    public Board(int boardSize){
        this.boardSize = boardSize;
        ResetGame();
    }

    private void ResetGame(){
        ResetTiles();

        dirty = true;
    }

    private void ResetTiles()
    {
        int minR = -1 * boardSize + 1;
        int maxR = boardSize - 1;
        tiles = new Dictionary<Coord, Tile>();

        for (int r = maxR; r >= minR; r-- ){
                
                int minQ = -1 * boardSize + 1;
                int maxQ = boardSize - 1;
                if(r <=0){
                    minQ = -1 * (boardSize - 1 + r);    
                }
                if (r >= 0)
                {
                    maxQ = boardSize - 1 - r;
                }
                for (int q = minQ; q <= maxQ; q++)
                {
                Coord coord = new Coord(q, r);
                tiles.Add(coord, new Tile(coord));
                }   
        }
     
       setDigger();
       setClimber();
    }

    private void setClimber()
    {
        for (int i = 0; i < boardSize; i++){
            setPlayerOnTile(Player.CLIMBER, new Coord(i , -1 * boardSize + 1));
        }
    }

    private void setDigger()
    {
        for (int i = 0; i < boardSize; i++)
        {
            setPlayerOnTile(Player.DIGGER, new Coord(-1 * i, boardSize - 1));
        }
    }

    private void setPlayerOnTile(Player player, Coord coord)
    {
        Tile tile = coordToTile(coord);
        tile.level = player == Player.CLIMBER ? tile.level = TileLevel.HILL : tile.level = TileLevel.UNDERGROUND;
        tile.occupiatBy = player;
        tile.occupiat = true;
    }



    public Tile coordToTile(Coord coord){
        Tile tile = null;
        tiles.TryGetValue(coord, out tile);
        return tile;
    }
}
