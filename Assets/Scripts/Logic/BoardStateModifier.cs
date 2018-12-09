using System;


public class BoardStateModifier
{
    
    public static Tile[,] ResetTiles(int size){
         int offSet = size - 1;
        int minR = 0;
        int maxR =  offSet * 2;
        // +2 to avoid index out of bound on neighbors
        Tile[,] tiles = new Tile[offSet * 2+2 ,offSet *2+2];

        for (int r = minR; r <= maxR; r++ ){
                
            int minQ = 0;
            int maxQ =  offSet * 2;

            if(r <=offSet){
                minQ =  maxR - offSet - r;    
            }
            if (r >= offSet)
            {
                maxQ = offSet * 3 - r  ;
            }
            for (int q = minQ; q <= maxQ; q++)
            {
            Coord coord = new Coord(q, r);
            tiles[q,r] = new Tile(coord);
            }   
        }
     
        setDigger(tiles,size);
        setClimber(tiles, size);
        return tiles;
    }

    public static  Tile[,] build(Tile[,] tiles, Tile from, Tile to)
    {
        Tile[,] copyOfTiles = new Tile[tiles.GetLength(0), tiles.GetLength(1)];
        Array.Copy(tiles, copyOfTiles, tiles.Length);

        TileLevel fromLevel = from.level;
        if (from.level == TileLevel.HILL)
        {
            fromLevel = TileLevel.GROUND;
        }
        else if (from.level == TileLevel.GROUND)
        {
            fromLevel = TileLevel.UNDERGROUND;
        }
        copyOfTiles[from.coord.q,from.coord.r] = new Tile(from.coord, fromLevel, from.occupiatBy);

        TileLevel toLevel = to.level;
        if (to.level == TileLevel.GROUND)
        {
            toLevel = TileLevel.HILL;
        }
        else if (to.level == TileLevel.UNDERGROUND)
        {
            toLevel = TileLevel.GROUND;
        }
        copyOfTiles[to.coord.q,to.coord.r] = new Tile(to.coord, toLevel, to.occupiatBy);

        return copyOfTiles;
    }

    public static Board Capture(Board board, Coord coord)
    {

        if(!Rules.CanCapture(board,coord)){
            throw new Exception("Trying to caputre but its not allowed");
        }
        Tile[,] copyOfTiles = new Tile[board.tiles.GetLength(0), board.tiles.GetLength(1)];
        Array.Copy(board.tiles, copyOfTiles, board.tiles.Length);


        Tile tile = board.tiles[coord.q,coord.r];
        Tile newTile = new Tile(tile.coord, tile.level, null);
        copyOfTiles[coord.q,coord.r] = newTile;

        Step nextStep = Step.MOVE;
        Player nextPlayer = board.currentPlayer;

        if (board.currentStep != Step.BUILD)
        {
            nextStep = Step.BUILD;
            nextPlayer = board.currentPlayer == Player.DIGGER ? Player.CLIMBER : Player.DIGGER;
        }
        return new Board(board.size,copyOfTiles,nextStep,nextPlayer);
    }

    public static Board moveStone(Board board, Tile from, Tile to)
    {
        if (!Rules.canMoveStone(board, from, to))
        {
            throw new Exception("Trying to move stone but its not allowed");
        }
        Tile[,] copyOfTiles = new Tile[board.tiles.GetLength(0), board.tiles.GetLength(1)];
        Array.Copy(board.tiles, copyOfTiles, board.tiles.Length);

        copyOfTiles[to.coord.q,to.coord.r] = new Tile(to.coord, to.level, from.occupiatBy);
        copyOfTiles[from.coord.q,from.coord.r] = new Tile(from.coord, from.level, null);

        Player nextPlayer = board.currentPlayer == Player.DIGGER ? Player.CLIMBER : Player.DIGGER;
        
        return new Board(board.size,copyOfTiles,Step.BUILD,nextPlayer);
    }

    private static void setClimber(Tile[,] tiles, int size)
    {
        for (int q = size - 1; q < size + size - 1; q++)
        {
            setPlayerOnTile(tiles, Player.CLIMBER, new Coord(q, 0));
        }
    }

    private static void setDigger(Tile[,] tiles, int size)
    {

        int maxR = (size - 1) * 2;
        for (int q = 0; q < size; q++)
        {
            setPlayerOnTile(tiles, Player.DIGGER, new Coord(q, maxR));
        }
    }

    private static void setPlayerOnTile(Tile[,] tiles, Player player, Coord coord)
    {
        Tile tile = tiles[coord.q, coord.r];
        TileLevel level = player == Player.CLIMBER ? TileLevel.HILL : TileLevel.UNDERGROUND;
        Tile newTile = new Tile(tile.coord, level, player);
        tiles[coord.q, coord.r] = newTile;
    }
}
