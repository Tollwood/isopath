using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Rules {
    private static int THREAT_THRESHHOLD = 2;

    public static void nextPlayer(Board board)
    {
        if(board.currentPlayer == Player.DIGGER) {
            board.currentPlayer = Player.CLIMBER;
        } else {
            board.currentPlayer = Player.DIGGER;
        }
    }

    public static bool IsThreaten(Board board, Player player)
    {
        Player enemy = player == Player.DIGGER ? Player.CLIMBER : Player.DIGGER;
        foreach(Tile tile in board.tiles.Values){
            if(tile.occupiatBy == player && CountEnemyNeighbors(board,tile, enemy ) >= THREAT_THRESHHOLD){
                return true;
            }
        }
        return false;
    }

    private static int CountEnemyNeighbors( Board board, Tile tile, Player enemy)
    {
        int count = 0;
        foreach(Coord neighborCoord in tile.GetNeighbors()){
            Tile neighborTile = board.coordToTile(neighborCoord);
            if( neighborTile != null && neighborTile.occupiatBy == enemy){
                count++;
            }
        }
        int teleportQ = board.boardSize - 1;
        if(tile.coord.Equals(new Coord(teleportQ , 0))){
            Tile neighborTile = board.coordToTile(new Coord(-1 * teleportQ,0));
            if (neighborTile != null && neighborTile.occupiatBy == enemy)
            {
                count++;
            }
        }
        if (tile.coord.Equals(new Coord( -1 * teleportQ, 0)))
        {
            Tile neighborTile = board.coordToTile(new Coord(teleportQ, 0));
            if (neighborTile != null && neighborTile.occupiatBy == enemy)
            {
                count++;
            }
        }
        return count;
    }

    public static bool CanCapture(Board board, Coord coord)
    {
        return CountEnemyNeighbors(board, board.coordToTile(coord), board.currentPlayer) >= THREAT_THRESHHOLD;
    }

    public static bool Capture(Board board, Coord coord)
    {
        if(CanCapture(board,coord)){
            Tile tile = board.coordToTile(coord);
            tile.occupiat = false;
            tile.occupiatBy = null;
            if(board.currentStep == Step.BUILD){
                board.currentStep = Step.MOVE;
            }
            else {
                board.currentStep = Step.BUILD;
                nextPlayer(board);
            }
            return true;
        }
        return false;
    }

    public static Player? CheckWinningCondition(Board board)
    {
        int diggerR = -1 * (board.boardSize - 1);
        int climberR =  board.boardSize - 1;
        foreach (Tile tile in board.tiles.Values)
        {
            if (tile.coord.r == diggerR&& tile.occupiatBy == Player.DIGGER)
            {
                return Player.DIGGER;
            }
            if (tile.coord.r == climberR && tile.occupiatBy == Player.CLIMBER)
            {
                return Player.CLIMBER;
            }
        }
        return null;
    }

    public static  bool canMoveStoneAnyWhere(Board board){
        TileLevel expectedTileLevel = board.currentPlayer == Player.CLIMBER ? TileLevel.HILL : TileLevel.UNDERGROUND;
        foreach(Tile tile in board.tiles.Values){
            if(tile.occupiatBy == board.currentPlayer){
                foreach(Coord coord in tile.GetNeighbors()){
                    Tile potentialTile = board.coordToTile(coord);
                    if (potentialTile != null && !potentialTile.occupiat && potentialTile.level == expectedTileLevel)
                    {
                        return true;
                    }    
                }
            }
        }
        return false;
    }

    public static bool canMoveStone(Board board, Coord coord)
    {
        Tile tile;
        if (board.currentStep == Step.MOVE && board.tiles.TryGetValue(coord, out tile))
        {
            if(tile.occupiatBy == board.currentPlayer){
                foreach (Coord neighborCoord in tile.GetNeighbors())
                {
                    Tile neighbor = board.coordToTile(neighborCoord);
                    if (neighbor != null && (Rules.canMoveStone(board, tile, neighbor)|| CanTeleport(board, tile)) )
                    {
                        return true;
                    }
                }    

            }
        }
        return false;
    }

    private static bool CanTeleport(Board board, Tile tile)
    {
        int val = board.boardSize - 1;
        List<Coord> teleports = new List<Coord>();
        teleports.Add(new Coord(-1 * val, val));
        teleports.Add(new Coord(0, val));

        teleports.Add(new Coord(val, 0));
        teleports.Add(new Coord(-1 * val, 0));

        teleports.Add(new Coord(0, -1 * val));
        teleports.Add(new Coord(val, -1 * val));

        if (!teleports.Contains(tile.coord))
        {
            return false;
        }

        Coord teleportCoord = teleports
            .Where((Coord coord) => coord.r == tile.coord.r)
            .Where((Coord coord) => coord.q != tile.coord.q)
            .Single();

        Tile teleportTile = board.coordToTile(teleportCoord);
        TileLevel requiredLevel = board.currentPlayer == Player.DIGGER ? TileLevel.UNDERGROUND : TileLevel.HILL;
        if(teleportTile != null && teleportTile.occupiat == false && teleportTile.level == requiredLevel ){
            return true;
        }
        return false;
    }

    public static  bool canMoveStone(Board board, Tile fromTile, Tile toTile)
    {
        bool currentPlayerMoving = fromTile.occupiatBy == board.currentPlayer;
        TileLevel toLevel = board.currentPlayer == Player.DIGGER ? TileLevel.UNDERGROUND : TileLevel.HILL;
        if (currentPlayerMoving && !toTile.occupiat && toTile.level == toLevel && (fromTile.isNeighbour(toTile) || teleport(board, fromTile,toTile))){
            return true;
        }
        return false;
    }

    public static  bool teleport(Board board, Tile fromTile, Tile toTile)
    {
        int max = board.boardSize - 1;
        Coord topRightCoord = new Coord(-1 * max, max);
        Coord topLeftCoord = new Coord(0, max);
        // midle
        Coord rightCoord = new Coord(max, 0);
        Coord leftCoord = new Coord(-1 * max, 0);

        //bottom
        Coord bottomRightCoord = new Coord(max, -1 * max);
        Coord bottomLeftCoord = new Coord(0, -1 * max);

        return matchTeleport(topRightCoord, topLeftCoord, fromTile, toTile)
        ||
            matchTeleport(rightCoord, leftCoord, fromTile, toTile)
        ||
            matchTeleport(bottomRightCoord, bottomLeftCoord, fromTile, toTile);
    }

    private static bool matchTeleport(Coord rightCoord, Coord leftCoord, Tile fromTile, Tile toTile)
    {
        return (fromTile.coord.Equals(rightCoord)  || fromTile.coord.Equals(leftCoord)) && (toTile.coord.Equals(rightCoord) || toTile.coord.Equals(leftCoord));
    }

    public static  bool moveStone(Board board, Tile from, Tile to)
    {
        if (canMoveStone(board, from,to)){
            to.occupiat = true;
            to.occupiatBy = from.occupiatBy;
            from.occupiat = false;
            from.occupiatBy = null;
            return true;
        }
        return false;
    }

    public static bool canMoveTile(Board board, Coord coord)
    {
        Tile tile;
        if (board.tiles.TryGetValue(coord, out tile))
        {
            int homeRowR = board.currentPlayer == Player.DIGGER ? board.boardSize - 1 : -1 * (board.boardSize - 1);
            bool isHomeRow = tile.coord.r == homeRowR;
            return board.currentStep == Step.BUILD && !tile.occupiat && !isHomeRow;
        }
        return false;
    }

    public static bool build(Board board, Tile from, Tile to){

        bool sameTile = from.coord.Equals(to.coord);
        // TODO placing tile is only valid iv player move afterwards

        bool unoccupiat = !from.occupiat && !to.occupiat;

        bool minLevelFromTile = (from.level == TileLevel.HILL || from.level == TileLevel.GROUND);

        int homeRowR = board.currentPlayer == Player.DIGGER ? board.boardSize - 1 : -1 * (board.boardSize - 1);
        bool toHomeRow = to.coord.r == homeRowR;
        bool fromHomeRow = from.coord.r == homeRowR;

        if (!sameTile && unoccupiat && minLevelFromTile && to.level != TileLevel.HILL && !toHomeRow && !fromHomeRow){

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

}
