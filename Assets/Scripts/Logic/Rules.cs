using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Rules {
    public static int THREAT_THRESHHOLD = 1;

    public static Player? CheckWinningCondition(Board board)
    {
        int diggerHomeLine = Rules.homeLine(Player.DIGGER, board.size);
        int climberHomeLine = Rules.homeLine(Player.CLIMBER, board.size);
        int climberStoneCount = 0;
        int diggerStoneCount = 0;
        foreach (Tile tile in board.tiles)
        {
            if (tile == null) continue;
            if (tile.occupiatBy == Player.DIGGER)
            {
                diggerStoneCount++;
            }
            if (tile.occupiatBy == Player.CLIMBER)
            {
                climberStoneCount++;
            }
            if (tile.coord.r == climberHomeLine && tile.occupiatBy == Player.DIGGER)
            {
                return Player.DIGGER;
            }
            if (tile.coord.r == diggerHomeLine && tile.occupiatBy == Player.CLIMBER)
            {
                return Player.CLIMBER;
            }
        }
        if(diggerStoneCount == 0) return Player.CLIMBER;
        if (climberStoneCount == 0) return Player.DIGGER;
        return null;
    }

    public static Player GetOpponent(Player player)
    {
        return player == Player.DIGGER ? Player.CLIMBER : Player.DIGGER;
    }

    public static int CountNeighborsOccupiatBy( Tile[,] tiles, int size, Tile tile, Player occupiant)
    {
        int count = 0;
        foreach(Tile neighborTile in Rules.GetNeighbors(tiles, size,tile)){
            
            if( neighborTile != null && neighborTile.occupiatBy == occupiant){
                count++;
            }
        }
        // TODO consider Teleport
        //int teleportQ = board.size - 1;
        //if(tile.coord.Equals(new Coord(teleportQ , 0))){
        //    Tile neighborTile = board.tiles[-1 * teleportQ, 0];
        //    if (neighborTile != null && neighborTile.occupiatBy == enemy)
        //    {
        //        count++;
        //    }
        //}
        //if (tile.coord.Equals(new Coord( -1 * teleportQ, 0)))
        //{
        //    Tile neighborTile = board.tiles[teleportQ, 0];
        //    if (neighborTile != null && neighborTile.occupiatBy == enemy)
        //    {
        //        count++;
        //    }
        //}
        return count;
    }

    public static List<Tile> GetNeighbors(Tile[,] tiles, int size, Tile fromTile)
    {
        Coord coord = fromTile.coord;
        List<Tile> neighbors = new List<Tile>();

        Coord newCoordTL = new Coord(coord.q + 1, coord.r + 0);
        Coord newCoordTR = new Coord(coord.q + 0, coord.r + 1);
        Coord newCoordR = new Coord(coord.q - 1, coord.r + 0);
        Coord newCoordL = new Coord(coord.q - 1, coord.r + 1);
        Coord newCoordBL = new Coord(coord.q + 1, coord.r - 1);
        Coord newCoordBR = new Coord(coord.q + 0, coord.r - 1);

        if (IsValidNeighbor(size, newCoordTL)) neighbors.Add(tiles[newCoordTL.q, newCoordTL.r]);
        if (IsValidNeighbor(size, newCoordTR)) neighbors.Add(tiles[newCoordTR.q, newCoordTR.r]);
        if (IsValidNeighbor(size, newCoordR)) neighbors.Add(tiles[newCoordR.q, newCoordR.r]);
        if (IsValidNeighbor(size, newCoordL)) neighbors.Add(tiles[newCoordL.q, newCoordL.r]);
        if (IsValidNeighbor(size, newCoordBL)) neighbors.Add(tiles[newCoordBL.q, newCoordBL.r]);
        if (IsValidNeighbor(size, newCoordBR)) neighbors.Add(tiles[newCoordBR.q, newCoordBR.r]);

        return neighbors;
    }

    private static bool IsValidNeighbor(int size, Coord coord)
    {
        int val = size - 1;
        int sum = coord.q + coord.r;
        // sum must be at least boardSize -1;
        // sum must not be bigger than  (boardsize -1) * 3
        // neither q nor r must be less than 0;
        // q must not be bigger than (boardsize -1) * 2
        return sum >= val && sum <= val * 3 && coord.q >= 0 && coord.r >= 0 && coord.q <= val * 2 && coord.r <= val * 2;
    }

    internal static bool canMoveStoneAnyWhere(Board board, Coord fromCoord, Coord toCoord)
    {
        Tile fromTile = board.tiles[fromCoord.q, fromCoord.r];
        Tile toTile = board.tiles[toCoord.q,toCoord.r];
        // should not modify state
        Rules.CanBuild(board, fromTile, toTile);
        bool result = canMoveStoneAnyWhere(board, board.currentPlayer);
        // should not modify state
        Rules.CanBuild(board, toTile, fromTile);
        return result;

    }

    public static bool CanCapture(Tile[,] tiles, int size, Player player,  Tile captureFrom)
    {
        return CountNeighborsOccupiatBy(tiles, size, captureFrom, player) > THREAT_THRESHHOLD;
    }

   

    public static  bool canMoveStoneAnyWhere(Board board, Player player){
        TileLevel expectedTileLevel = player == Player.CLIMBER ? TileLevel.HILL : TileLevel.UNDERGROUND;
        foreach(Tile tile in board.tiles){
            if(tile == null){
                continue;
            }
            if(tile.occupiatBy == player){
                foreach(Tile potentialTile in Rules.GetNeighbors(board.tiles, board.size, tile)){
                    if (potentialTile != null && potentialTile.occupiatBy == null && potentialTile.level == expectedTileLevel)
                    {
                        return true;
                    }    
                }
            }
        }
        return false;
    }

    public static bool canMoveStone(Tile[,] tiles, Player currentPlayer, Step currentStep, int size, Coord coord)
    {
        Tile tile = tiles[coord.q, coord.r];
        if (currentStep == Step.MOVE && tile.occupiatBy == currentPlayer){
            // activate once ai knows how to teleport
            if(CanTeleport(tiles, currentPlayer,size, tile)){
                 return true;
            }
            foreach (Tile neighbor in Rules.GetNeighbors(tiles, size, tile))
            {
                bool moveOneStep = neighbor != null && neighbor.occupiatBy == null && neighbor.level == GetRequiredTileLevel(currentPlayer);
                if (moveOneStep) return true;
            }  
        }
        return false;
    }

    private static bool CanTeleport(Tile[,] tiles, Player currentPlayer, int size, Tile tile)
    {
        int val = size - 1;
        List<Coord> teleports = new List<Coord>();
        teleports.Add(new Coord(val, 0));
        teleports.Add(new Coord(val *2, 0));

        teleports.Add(new Coord(0, val));
        teleports.Add(new Coord(val *2, val));

        teleports.Add(new Coord(0,val * 2));
        teleports.Add(new Coord(val,val * 2));

        if (!teleports.Contains(tile.coord))
        {
            return false;
        }

        Coord teleportCoord = teleports
            .Where((Coord coord) => coord.r == tile.coord.r)
            .Where((Coord coord) => coord.q != tile.coord.q)
            .Single();

        Tile teleportTile = tiles[teleportCoord.q,teleportCoord.r];
        TileLevel requiredLevel = GetRequiredTileLevel(currentPlayer);
        if(teleportTile != null && teleportTile.occupiatBy == null && teleportTile.level == requiredLevel ){
            return true;
        }
        return false;
    }

    internal static bool UnderThreat(Tile[,] tiles, int size, Tile tile, Player currentPlayer)
    {
        return tile.occupiatBy == currentPlayer && CountNeighborsOccupiatBy(tiles, size, tile, GetOpponent(currentPlayer)) >= THREAT_THRESHHOLD;
    }

    public static bool IsThreaten(Board board, Player player)
    {
        Player opponent = GetOpponent(board.currentPlayer);
        foreach (Tile tile in board.tiles)
        {
            if (UnderThreat(board.tiles, board.size, tile, board.currentPlayer))
            {
                return true;
            }
        }
        return false;
    }


    public static TileLevel GetRequiredTileLevel(Player player)
    {
        return player == Player.DIGGER ? TileLevel.UNDERGROUND : TileLevel.HILL;
    }

    public static  bool canMoveStone(Tile[,] tiles,Player currentPlayer, int size, Tile fromTile, Tile toTile)
    {
        bool currentPlayerMoving = fromTile.occupiatBy == currentPlayer;
        TileLevel toLevel = GetRequiredTileLevel(currentPlayer);
        bool isNeighbor = Rules.GetNeighbors(tiles, size, fromTile).Contains(toTile);
        bool canTeleport = CanTeleport(tiles,currentPlayer,size, fromTile);
        if (currentPlayerMoving && toTile.occupiatBy == null && toTile.level == toLevel && (isNeighbor || canTeleport)){
            return true;
        }
        return false;
    }

    public static  bool teleport(int size, Tile fromTile, Tile toTile)
    {
        int val = size - 1;
        Coord topLeftCoord = new Coord(val, 0);
        Coord topRightCoord = new Coord(val * 2, 0);

        Coord rightCoord = new Coord(val * 2, val);
        Coord leftCoord = new Coord(0, val);

        Coord bottomRightCoord = new Coord(val, val * 2);
        Coord bottomLeftCoord = new Coord(0, val * 2);

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

    public static bool canMoveTile(Board board, Coord coord)
    {
        Tile tile = board.tiles[coord.q,coord.r];
        int homeRowR = Rules.homeLine(board.currentPlayer,board.size);
        bool isHomeRow = tile.coord.r == homeRowR;
        return board.currentStep == Step.BUILD && tile.occupiatBy == null && !isHomeRow && tile.level != TileLevel.UNDERGROUND; 
    }


    public static Tile coordToTile(Dictionary<Coord, Tile> tiles, Coord coord)
    {
        Tile tile = null;
        tiles.TryGetValue(coord, out tile);
        return tile;
    }

    public static bool CanBuild(Board board, Tile from, Tile to)
    {

        Tile[,] modifiedTiles = BoardStateModifier.build(board.tiles, from, to);
        Board modifiedBoard = new Board(board.size, modifiedTiles, board.currentStep, board.currentPlayer, board.settings);
        bool canMoveAnyWhere = Rules.canMoveStoneAnyWhere(modifiedBoard, board.currentPlayer);
        bool sameTile = from.coord.Equals(to.coord);
        // TODO placing tile is only valid iv player move afterwards

        bool unoccupiat = from.occupiatBy == null && to.occupiatBy == null;

        bool minLevelFromTile = (from.level == TileLevel.HILL || from.level == TileLevel.GROUND);

        int homeRowR = Rules.homeLine(board.currentPlayer, board.size);
        bool toHomeRow = to.coord.r == homeRowR;
        bool fromHomeRow = from.coord.r == homeRowR;
        bool buildOnHill = to.level == TileLevel.HILL;
        return !sameTile && unoccupiat && minLevelFromTile && !buildOnHill && !toHomeRow && !fromHomeRow && canMoveAnyWhere;
    }

    public static int homeLine(Player player, int boardSize)
    {
        int multiply = player == Player.DIGGER ? 0 : 2;
        return (boardSize - 1) * multiply;
    }
}
