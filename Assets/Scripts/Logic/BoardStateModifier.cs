using System;


public class BoardStateModifier
{
    
    public static Tile[,] ResetTiles(int size){
         int offSet = size - 1;
        int minR = 0;
        int maxR =  offSet * 2;
        //  +2 to avoid index out of bound on neighbors
        Tile[,] tiles = new Tile[size * 2 , size *2];

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

    public static Board NewBoard(int size)
    {
        Settings settings = new Settings(size, false, false);
        Tile[,] tiles = ResetTiles(size);
        return new Board(settings, tiles);
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

    public static Board build(Board board, Tile from, Tile to)
    {
        Tile[,] tiles = build(board.tiles, from, to);
        return new Board(board.size, tiles, Step.MOVE, board.currentPlayer, board.settings);
    }

    public static Board Capture(Board board, Tile tile)
    {
        Tile[,] copyOfTiles = new Tile[board.tiles.GetLength(0), board.tiles.GetLength(1)];
        Array.Copy(board.tiles, copyOfTiles, board.tiles.Length);


        Tile newTile = new Tile(tile.coord, tile.level, null);
        copyOfTiles[tile.coord.q,tile.coord.r] = newTile;

        Step nextStep = Step.MOVE;
        Player nextPlayer = board.currentPlayer;

        if (board.currentStep != Step.BUILD)
        {
            nextStep = Step.BUILD;
            nextPlayer = board.currentPlayer == Player.DIGGER ? Player.CLIMBER : Player.DIGGER;
        }
        return new Board(board.size,copyOfTiles,nextStep,nextPlayer,board.settings);
    }

    public static Board moveStone(Board board, Tile from, Tile to)
    {
        if (!Rules.canMoveStone(board.tiles,board.currentPlayer,board.size, from, to))
        {
            throw new Exception("Trying to move stone but its not allowed");
        }
        Tile[,] copyOfTiles = new Tile[board.tiles.GetLength(0), board.tiles.GetLength(1)];
        Array.Copy(board.tiles, copyOfTiles, board.tiles.Length);

        copyOfTiles[to.coord.q,to.coord.r] = new Tile(to.coord, to.level, from.occupiatBy);
        copyOfTiles[from.coord.q,from.coord.r] = new Tile(from.coord, from.level, null);

        Player nextPlayer = board.currentPlayer == Player.DIGGER ? Player.CLIMBER : Player.DIGGER;
        
        return new Board(board.size,copyOfTiles,Step.BUILD,nextPlayer,board.settings);
    }

    private static void setClimber(Tile[,] tiles, int size)
    {
        int maxQ = (size - 1);
        int minQ = 0;
        int homeLine = Rules.homeLine(Player.CLIMBER, size);
        for (int q = minQ; q <= maxQ; q++)
        {
            setPlayerOnTile(tiles, Player.CLIMBER, new Coord(q, homeLine));
        }
    }

    private static void setDigger(Tile[,] tiles, int size)
    {
        int maxQ = (size - 1) * 2;
        int minQ = (size - 1);
        int homeLine = Rules.homeLine(Player.DIGGER,size);
        for (int q = minQ; q <= maxQ; q++)
        {
            setPlayerOnTile(tiles, Player.DIGGER, new Coord(q, homeLine));
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
