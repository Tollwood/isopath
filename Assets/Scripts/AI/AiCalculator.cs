
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

    internal static Moves BestMove(Board board, int seed)
    {
        Move buildMove = bestBuild(board, seed);
        Tile[,] tilesAfterBuild = BoardStateModifier.build(board.tiles, buildMove.from, buildMove.to);
        Board boardAfterBuild = new Board(board.size, tilesAfterBuild, Step.MOVE, board.currentPlayer);
        Move stoneMove = moveStone(boardAfterBuild, seed);
        return new Moves(buildMove, stoneMove,board.currentPlayer);
    }

    internal static Move moveStone(Board board, int seed)
    {
        Random random = new Random(seed);
        Move[] validMoves = GetValidStoneMoves(board);
        return validMoves[0];
    }

    private static Move[] GetValidStoneMoves(Board board)
    {
        List<Move> validMoves = new List<Move>();

        foreach (Tile fromTile in board.tiles)
        {
            if (fromTile != null && fromTile.occupiatBy == board.currentPlayer)
            {
                foreach (Coord coord in fromTile.GetNeighbors())
                {
                    if (coord.q >= board.tiles.GetLength(0) || coord.r >= board.tiles.GetLength(1))
                    {
                        continue;
                    }
                    Tile toTile = board.tiles[coord.q, coord.r];
                    if (toTile != null && Rules.canMoveStone(board, fromTile, toTile))
                    {
                        int score = 1;
                        score = CloserToGoal(board, fromTile,toTile,score);
                        validMoves.Add(new Move(fromTile, toTile, score));
                    }
                }
            }
        }
        validMoves.Sort();
        return validMoves.ToArray();
    }

    private static int CloserToGoal(Board board, Tile fromTile, Tile toTile,int score)
    {
        Player opponent = Rules.GetOpponent(board.currentPlayer);
        int goalR = homeLine(opponent, board.size); // 0 or 6
        if (Math.Abs(goalR - toTile.coord.r) < Math.Abs(goalR - fromTile.coord.r))
        {
            score += 1;
        }
        return score;
    }

    public static Tile[] allPossibleHexToBuildTo(Board board)
    {
        List<Tile> result = new List<Tile>();
        foreach (Tile tile in board.tiles)
        {
            if(tile == null){
                continue;
            }
            if (tile.coord.r != homeLine(board.currentPlayer,board.size) && tile.level != TileLevel.HILL && tile.occupiatBy == null)
            {
                result.Add(tile);
            }
        }
        return result.ToArray();
    }

    public static Move bestBuild(Board board, int seed){
        Tile[] allFrom = allPossibleHexToBuildFrom(board);
        Tile[] allTo = allPossibleHexToBuildTo(board);

        Random random = new Random(seed);
        Tile fromTile = allFrom[random.Next(allFrom.Length-1)];
        Tile toTile = allTo[random.Next(allTo.Length-1)];
        while(toTile.Equals(fromTile)){
            toTile = allTo[random.Next(allTo.Length-1)];
        }
        while(!Rules.canBuild(board, fromTile, toTile)){
            fromTile = allFrom[random.Next(allFrom.Length-1)];
            toTile = allTo[random.Next(allTo.Length-1)];
        }
        return new Move(fromTile, toTile,1);
    }

    private static int homeLine(Player player, int boardSize){
        int multiply = player == Player.DIGGER ? 0 : 2;
        return (boardSize -1 ) * multiply;
    }
}