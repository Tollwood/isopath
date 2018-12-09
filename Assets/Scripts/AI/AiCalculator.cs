
using System;
using System.Collections.Generic;

public class AiCalculator
{
    public static Tile[] allPossibleHexToBuildFrom(Board board){
        List<Tile> result = new List<Tile>();
        foreach(Tile tile in board.tiles){
            if(tile == null){
                continue;
            }
            if(Rules.canMoveTile(board,tile.coord)){
                result.Add(tile);
            }    
        }
        return result.ToArray();
    }

    public static Tile[] allPossibleHexToBuildTo(Board board)
    {
        List<Tile> result = new List<Tile>();
        foreach (Tile tile in board.tiles)
        {
            if(tile == null){
                continue;
            }
            if (tile.coord.q != homeLine(board) && tile.level != TileLevel.HILL && tile.occupiatBy == null)
            {
                result.Add(tile);
            }
        }
        return result.ToArray();
    }

    public static Move calcBuild(Board board){
        Tile[] allFrom = allPossibleHexToBuildFrom(board);
        Tile[] allTo = allPossibleHexToBuildTo(board);

        Random random = new Random(1);
        Tile fromTile = allFrom[random.Next(allFrom.Length)]; 
        Tile toTile = allFrom[random.Next(allTo.Length)];
        while(fromTile.Equals(toTile)){
            toTile = allTo[random.Next(allTo.Length)];
        }
        while(!Rules.canBuild(board, fromTile, toTile)){
            fromTile = allFrom[random.Next(allFrom.Length)];
            toTile = allFrom[random.Next(allTo.Length)];
        }

        return new Move(fromTile, toTile, board.currentPlayer);
    }


    private static int homeLine(Board board){
        int multiply = board.currentPlayer == Player.DIGGER ? -1 : 1;
        return (board.size -1 ) * multiply;
    }
}