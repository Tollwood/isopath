
using System;
using System.Collections.Generic;

public class AiAgent
{
    private Player player;
    private Random random;

    public AiAgent(Player player){
        this.player = player;
        this.random = new Random(1);
    }

    public Moves GetNextMove(Board board){
        return BestMove(board);
    }

    public ScoredTile[] allPossibleHexToBuildFrom(Board board)
    {
        List<ScoredTile> result = new List<ScoredTile>();
        foreach (Tile tile in board.tiles)
        {
            if (tile == null)
            {
                continue;
            }
            if (Rules.canMoveTile(board, tile.coord))
            {
                int score = 0;
                score += hasStoneAsNeighbor(board,tile,true);
                score += fromEnemysHomeLine(board, tile);
                result.Add(new ScoredTile(tile, score));
            }
        }
        result.Sort((ScoredTile x, ScoredTile y) => y.score.CompareTo(x.score));
        return result.ToArray();
    }

    private int fromEnemysHomeLine(Board board, Tile tile)
    {
        if (tile.occupiatBy == null)
        {
            int homelineR = Rules.homeLine(Rules.GetOpponent(player), board.size);
            if(tile.coord.r == homelineR)
            {
                return 1;
            }
        }
        return 0;
    }

    private int hasStoneAsNeighbor(Board board, Tile tile, bool takeStone)
    {
        int score = 0;
        foreach (Tile neighbor in Rules.GetNeighbors(board, tile))
        {
            if(player == Player.DIGGER)
            {
                if (this.player == neighbor.occupiatBy && takeStone)
                {
                    score++;
                }
            }
            else
            {
                if (this.player == neighbor.occupiatBy && !takeStone)
                {
                    score++;
                }
            }

        }
        return score;
    }

    internal Moves BestMove(Board board)
    {
        Move buildMove = bestBuild(board);
        Tile[,] tilesAfterBuild = BoardStateModifier.build(board.tiles, buildMove.from, buildMove.to);
        Board boardAfterBuild = new Board(board.size, tilesAfterBuild, Step.MOVE, board.currentPlayer, board.settings);
        Move stoneMove = GetValidStoneMoves(boardAfterBuild)[0];
        return new Moves(buildMove, stoneMove, board.currentPlayer);
    }

    private static Move[] GetValidStoneMoves(Board board)
    {
        List<Move> validMoves = new List<Move>();

        foreach (Tile fromTile in board.tiles)
        {
            if (fromTile != null && fromTile.occupiatBy == board.currentPlayer)
            {
                foreach (Tile tile in Rules.GetNeighbors(board, fromTile))
                {
                    if (Rules.canMoveStone(board, fromTile, tile))
                    {
                        int score = 0;
                        score += CloserToGoal(board, fromTile, tile);
                        validMoves.Add(new Move(fromTile, tile, score));
                    }
                }
            }
        }
        validMoves.Sort((Move x, Move y) => y.score.CompareTo(x.score));
        return validMoves.ToArray();
    }

    private static int CloserToGoal(Board board, Tile fromTile, Tile toTile)
    {

        Player opponent = Rules.GetOpponent(board.currentPlayer);
        int goalR = Rules.homeLine(opponent, board.size); //0 or 6
        if (Math.Abs(goalR - toTile.coord.r) < Math.Abs(goalR - fromTile.coord.r))
        {
            return 1;
        }
        return 0;
    }

    public ScoredTile[] allPossibleHexToBuildTo(Board board)
    {
        List<ScoredTile> result = new List<ScoredTile>();
        foreach (Tile tile in board.tiles)
        {
            if (tile == null)
            {
                continue;
            }
            if (tile.coord.r != Rules.homeLine(board.currentPlayer, board.size) && tile.level != TileLevel.HILL && tile.occupiatBy == null)
            {
                int score = 0;
                score += hasStoneAsNeighbor(board,tile,false);
                result.Add(new ScoredTile(tile, score));
            }
        }
        return result.ToArray();
    }

    public Move bestBuild(Board board)
    {
        ScoredTile[] allFrom = allPossibleHexToBuildFrom(board);
        ScoredTile[] allTo = allPossibleHexToBuildTo(board);


        Tile fromTile = allFrom[0].tile;
        Tile toTile = allTo[0].tile;
        while (!Rules.canBuild(board, fromTile, toTile))
        {
            fromTile = allFrom[random.Next(allFrom.Length - 1)].tile;
            toTile = allTo[random.Next(allTo.Length - 1)].tile;
        }
        return new Move(fromTile, toTile, 1);
    }

}