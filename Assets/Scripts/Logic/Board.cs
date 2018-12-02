using System;
using System.Collections.Generic;

[System.Serializable]
public class Board {

    public Dictionary<Coord,Tile> tiles;
    public Player currentPlayer;
    public Step currentStep;
    public int boardSize = 4;
    public bool dirty { get; set;}

    public Board(int boardSize){
        this.boardSize = boardSize;
        ResetGame();
    }

    private void ResetGame(){
        ResetTiles();
        currentPlayer = Player.DIGGER;
        currentStep = Step.BUILD;
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
        if (player == Player.CLIMBER)
        {
            tile.level = TileLevel.HILL;
        }
        else
        {
            tile.level = TileLevel.UNDERGROUND;
        }
        tile.occupiatBy = player;
        tile.occupiat = true;
    }

    public bool canMoveTile(Coord coord){
        Tile tile;
        if(this.tiles.TryGetValue(coord, out tile)){
            return this.currentStep == Step.BUILD && !tile.occupiat;    
        }
        return false;
    }

    internal void nextPlayer()
    {
        if(currentPlayer == Player.DIGGER) {
            currentPlayer = Player.CLIMBER;
        } else {
            currentPlayer = Player.DIGGER;
        }
    }

    public bool canMoveStoneAnyWhere(){
        TileLevel expectedTileLevel = currentPlayer == Player.CLIMBER ? TileLevel.HILL : TileLevel.UNDERGROUND;
        foreach(Tile tile in tiles.Values){
            if(tile.occupiatBy == currentPlayer){
                foreach(Coord coord in tile.GetNeighbors()){
                    Tile potentialTile = coordToTile(coord);
                    if (potentialTile != null && !potentialTile.occupiat && potentialTile.level == expectedTileLevel)
                    {
                        return true;
                    }    
                }
            }
        }
        return false;
    }

    public bool canMoveStone(Coord coord)
    {
        Tile tile;
        if (this.tiles.TryGetValue(coord, out tile))
        {
            return this.currentStep == Step.MOVE && tile.occupiat && tile.occupiatBy == currentPlayer;
        }
        return false;
    }

    public bool canMoveStone(Tile fromTile, Tile toTile)
    {
        bool currentPlayerMoving = fromTile.occupiatBy == currentPlayer;
        TileLevel toLevel = currentPlayer == Player.DIGGER ? TileLevel.UNDERGROUND : TileLevel.HILL;
        if (currentPlayerMoving && !toTile.occupiat && toTile.level == toLevel){
            return true;
        }
        return false;
    }

    public bool moveStone(Tile from, Tile to)
    {
        if (canMoveStone(from,to)){
            to.occupiat = true;
            to.occupiatBy = from.occupiatBy;
            from.occupiat = false;
            from.occupiatBy = null;
            return true;
        }
        return false;
    }
    public bool build(Tile from, Tile to){

        // TODO placing tile on the same place again is not a move
        // TODO placing tile is only valid iv player move afterwards
        bool unoccupiat = !from.occupiat && !to.occupiat;
        bool minLevelFromTile = (from.level == TileLevel.HILL || from.level == TileLevel.GROUND);
        if (unoccupiat && minLevelFromTile && to.level != TileLevel.HILL){

            if(from.level == TileLevel.HILL){
                from.level = TileLevel.GROUND;
            }else if (from.level == TileLevel.GROUND){
                from.level = TileLevel.UNDERGROUND;
            }
            if (to.level == TileLevel.GROUND){
                to.level = TileLevel.HILL;
            } else if (to.level == TileLevel.UNDERGROUND){
                to.level = TileLevel.GROUND;
            }
            return true;
        }
        return false;


    }

    public Tile coordToTile(Coord coord){
        Tile tile;
        tiles.TryGetValue(coord, out tile);
        return tile;
    }
}

