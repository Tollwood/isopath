using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Rules {
    private static int THREAT_THRESHHOLD = 2;

    public static bool IsThreaten(Board board, Player player)
    {
        Player opponent = GetOpponent(board.currentPlayer);
        foreach(Tile tile in board.tiles){
            if(tile.occupiatBy == player && CountEnemyNeighbors(board,tile, opponent ) >= THREAT_THRESHHOLD){
                return true;
            }
        }
        return false;
    }

    public static Player GetOpponent(Player player)
    {
        return player == Player.DIGGER ? Player.CLIMBER : Player.DIGGER;
    }

    private static int CountEnemyNeighbors( Board board, Tile tile, Player enemy)
    {
        int count = 0;
        foreach(Coord neighborCoord in tile.GetNeighbors()){
            if (neighborCoord.q >= board.tiles.GetLength(0) || neighborCoord.r >= board.tiles.GetLength(1))
            {
                continue;
            }
            Tile neighborTile = board.tiles[neighborCoord.q,neighborCoord.r];
            if( neighborTile != null && neighborTile.occupiatBy == enemy){
                count++;
            }
        }
        int teleportQ = board.size - 1;
        if(tile.coord.Equals(new Coord(teleportQ , 0))){
            Tile neighborTile = board.tiles[-1 * teleportQ, 0];
            if (neighborTile != null && neighborTile.occupiatBy == enemy)
            {
                count++;
            }
        }
        if (tile.coord.Equals(new Coord( -1 * teleportQ, 0)))
        {
            Tile neighborTile = board.tiles[teleportQ, 0];
            if (neighborTile != null && neighborTile.occupiatBy == enemy)
            {
                count++;
            }
        }
        return count;
    }

    internal static bool canMoveStoneAnyWhere(Board board, Coord fromCoord, Coord toCoord)
    {
        Tile fromTile = board.tiles[fromCoord.q, fromCoord.r];
        Tile toTile = board.tiles[toCoord.q,toCoord.r];
        // should not modify state
        Rules.canBuild(board, fromTile, toTile);
        bool result = canMoveStoneAnyWhere(board.tiles, board.currentPlayer);
        // should not modify state
        Rules.canBuild(board, toTile, fromTile);
        return result;

    }

    public static bool CanCapture(Board board, Coord coord)
    {
        return CountEnemyNeighbors(board, board.tiles[coord.q,coord.r], board.currentPlayer) >= THREAT_THRESHHOLD;
    }

    public static Player? CheckWinningCondition(Board board)
    {
        int diggerR = -1 * (board.size - 1);
        int climberR =  board.size - 1;
        foreach (Tile tile in board.tiles)
        {
            if(tile == null) continue;
            
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

    public static  bool canMoveStoneAnyWhere(Tile[,] tiles, Player player){
        TileLevel expectedTileLevel = player == Player.CLIMBER ? TileLevel.HILL : TileLevel.UNDERGROUND;
        foreach(Tile tile in tiles){
            if(tile == null){
                continue;
            }
            if(tile.occupiatBy == player){
                foreach(Coord coord in tile.GetNeighbors()){
                    Tile potentialTile = tiles[coord.q,coord.r];
                    if (potentialTile != null && potentialTile.occupiatBy == null && potentialTile.level == expectedTileLevel)
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
        Tile tile = board.tiles[coord.q, coord.r];
        if (board.currentStep == Step.MOVE && tile.occupiatBy == board.currentPlayer){
            // activate once ai knows how to teleport
            //if(CanTeleport(board, tile)){
            //    return true;
            //}
            foreach (Coord neighborCoord in tile.GetNeighbors())
            {
                if(neighborCoord.q >= board.tiles.GetLength(0) || neighborCoord.r >= board.tiles.GetLength(1)){
                    continue;
                }
                Tile neighbor = board.tiles[neighborCoord.q,neighborCoord.r];

                bool moveOneStep = neighbor != null && neighbor.occupiatBy == null && neighbor.level == GetRequiredTileLevel(board.currentPlayer);

                if (moveOneStep)
                {
                    return true;
                }
            }  
        }
        return false;
    }

    private static bool CanTeleport(Board board, Tile tile)
    {
        int val = board.size - 1;
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

        Tile teleportTile = board.tiles[teleportCoord.q,teleportCoord.r];
        TileLevel requiredLevel = GetRequiredTileLevel(board.currentPlayer);
        if(teleportTile != null && teleportTile.occupiatBy == null && teleportTile.level == requiredLevel ){
            return true;
        }
        return false;
    }

    private static TileLevel GetRequiredTileLevel(Player player)
    {
        return player == Player.DIGGER ? TileLevel.UNDERGROUND : TileLevel.HILL;
    }

    public static  bool canMoveStone(Board board, Tile fromTile, Tile toTile)
    {
        bool currentPlayerMoving = fromTile.occupiatBy == board.currentPlayer;
        TileLevel toLevel = GetRequiredTileLevel(board.currentPlayer);
        if (currentPlayerMoving && toTile.occupiatBy == null && toTile.level == toLevel && (fromTile.isNeighbour(toTile) || teleport(board.size, fromTile,toTile))){
            return true;
        }
        return false;
    }

    public static  bool teleport(int size, Tile fromTile, Tile toTile)
    {
        int val = size - 1;
        Coord topLeftCoord = new Coord(val, 0);
        Coord topRightCoord = new Coord(val * 2, 0);

        // midle
        Coord rightCoord = new Coord(val * 2, val);
        Coord leftCoord = new Coord(0, val * 2);

        //bottom
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
        int homeRowR = board.currentPlayer == Player.DIGGER ? board.size - 1 : -1 * (board.size - 1);
        bool isHomeRow = tile.coord.r == homeRowR;
        return board.currentStep == Step.BUILD && tile.occupiatBy == null && !isHomeRow && tile.level != TileLevel.UNDERGROUND;
    }


    public static Tile coordToTile(Dictionary<Coord, Tile> tiles, Coord coord)
    {
        Tile tile = null;
        tiles.TryGetValue(coord, out tile);
        return tile;
    }

    public static bool canBuild(Board board, Tile from, Tile to)
    {

        Tile[,] modifiedTiles = BoardStateModifier.build(board.tiles, from, to);
        bool canMoveAnyWhere = Rules.canMoveStoneAnyWhere(modifiedTiles, board.currentPlayer);
        bool sameTile = from.coord.Equals(to.coord);
        // TODO placing tile is only valid iv player move afterwards

        bool unoccupiat = from.occupiatBy == null && to.occupiatBy == null;

        bool minLevelFromTile = (from.level == TileLevel.HILL || from.level == TileLevel.GROUND);

        int homeRowR = board.currentPlayer == Player.DIGGER ? 0 : 2 * (board.size - 1);
        bool toHomeRow = to.coord.r == homeRowR;
        bool fromHomeRow = from.coord.r == homeRowR;
        bool buildOnHill = to.level == TileLevel.HILL;
        return !sameTile && unoccupiat && minLevelFromTile && !buildOnHill && !toHomeRow && !fromHomeRow && canMoveAnyWhere;
    }
}
