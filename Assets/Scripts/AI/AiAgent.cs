
using System;
using System.Collections.Generic;

public class AiAgent
{
    private const int WINNING_SCORE = 100;
    private const int CAPTURE_SCORE = 5;
    private const int ESCAPE_THREAT = 1;
    private const int AVOID_CAPTURE = -5;
    private const int PLACE_TILE_ON_HOME_LINE = 2;
    private const int BUILD_FROM_SAME_LEVEL = -1;
    private const int BUILD_TO_SAME_LEVEL = -1;
    private const int CLOSER_TO_GOAL = 2;
    private const int MOVE_TO_BUILD_SCORE = 1;
    private const int NEXT_TO_STONE = 1;
    private const int BLOCK_OPPONENT = 2;

    private Player player;
    private Random random;

    public AiAgent(Player player){
        this.player = player;
        this.random = new Random(1);
    }

    public ScoredMove Minmax(Board board, int depth, bool maximizingPlayer)
    {
        if (depth == 0)
        {
            return GetNextMove(board);
        }

        if (maximizingPlayer)
        {
            return Max(board, depth);
        }
        else
        {
            return Min(board, depth);
        }

    }

    public ScoredMove Max(Board board, int depth)
    {
        int value = int.MinValue;
        ScoredMove bestMove = null;
        foreach (ScoredMove possibleMove in GetPossibleMoves(board))
        {
            if (bestMove == null)
            {
                bestMove = possibleMove;
                value = possibleMove.score;
            }
            Board boardAfterMove = ExecuteMove(board, possibleMove);
            value = Math.Max(value, Minmax(boardAfterMove, depth - 1, false).score);

        }
        return bestMove;
    }

    private ScoredMove Min(Board board, int depth)
    {
        int value = int.MaxValue;
        ScoredMove bestMove = null;
        foreach (ScoredMove possibleMove in GetPossibleMoves(board))
        {
            if (bestMove == null)
            {
                bestMove = possibleMove;
            }
            Board boardAfterMove = ExecuteMove(board, possibleMove);
            Math.Min(value, Minmax(boardAfterMove, depth - 1, true).score);
        }
        return bestMove;
    }

    private Board ExecuteMove(Board board, ScoredMove possibleMove)
    {
        if(possibleMove.buildFrom == null)
        {
            board = BoardStateModifier.Capture(board, possibleMove.captureStone);
        }
        else
        {
            board = BoardStateModifier.build(board, possibleMove.buildFrom, possibleMove.buildTo);
        }

        if (possibleMove.moveFrom == null)
        {
            board = BoardStateModifier.Capture(board, possibleMove.captureStone);
        }
        else
        {
            Tile moveFrom = board.tiles[possibleMove.moveFrom.coord.q, possibleMove.moveFrom.coord.r];
            Tile moveTo = board.tiles[possibleMove.moveTo.coord.q, possibleMove.moveTo.coord.r];
            board = BoardStateModifier.moveStone(board, moveFrom, moveTo);
        }
        return board;
    }

    public List<ScoredMove> GetPossibleMoves(Board board)
    {
        List<ScoredMove> scoredMoves = new List<ScoredMove>();
        scoredMoves.AddRange(CaptureMoves(board));
        scoredMoves.AddRange(BuildMoves(board));
        return scoredMoves;
    }

    public ScoredMove GetNextMove(Board board){
        List<ScoredMove> scoredMoves = GetPossibleMoves(board);
        scoredMoves.Sort((ScoredMove x, ScoredMove y) => y.score.CompareTo(x.score));
        return scoredMoves.ToArray()[0];

    }

    private List<Tile> CapturableTiles(Tile[,] tiles, int size, Player currentPlayer)
    {
        List<Tile> captureable= new List<Tile>();

        foreach (Tile tile in tiles)
        {
            if (tile == null)
            {
                continue;
            }
            if (tile.occupiatBy == Rules.GetOpponent(currentPlayer) && Rules.CanCapture(tiles, size, currentPlayer, tile))
            {
                captureable.Add(tile);
            }
        }
        return captureable;
    }

    private List<ScoredMove> CaptureMoves(Board board)
    {
        List<ScoredMove> captureMoves = new List<ScoredMove>();
        foreach (Tile tile in CapturableTiles(board.tiles, board.size, board.currentPlayer))
        {
            // Find best Move
            Board boardAfterCapture = BoardStateModifier.Capture(board, tile);
            Move stoneMove = GetBestMove(boardAfterCapture.tiles, board.currentPlayer,board.size, null);
            if(stoneMove != null){
                captureMoves.Add(ScoredMove.Builder()
                    .withCaptureStep(tile)
                    .withMoveStep(stoneMove.from, stoneMove.to)
                    .addToScore(CAPTURE_SCORE)
                    .addToScore(stoneMove.score)
                    .build());
            }  
        }
        return captureMoves;
    }

    private List<ScoredMove> BuildMoves(Board board)
    {
        List<ScoredMove> scoredMoves = new List<ScoredMove>();

        foreach(Tile buildFrom in board.tiles)
        {
            if (buildFrom == null) continue;

            if (Rules.canMoveTile(board, buildFrom.coord))
            {
                int buildFromScore = CalcScoreForBuildFromTile(board, buildFrom);
                //for each possible BuildTo 
                foreach (Tile buildTo in board.tiles)
                {
                    if (buildTo == null) continue;
                    if (Rules.CanBuild(board,buildFrom,buildTo)){
                        int buildToScore = CalcScoreForBuildToTile(board,buildTo);
                        // get All build / Caputre moves
                        Tile[,] tilesAfterBuild = BoardStateModifier.build(board.tiles, buildFrom, buildTo);
                        foreach(Tile capturable in CapturableTiles(tilesAfterBuild,board.size, board.currentPlayer))
                        {
                            scoredMoves.Add(ScoredMove.Builder()
                                .withBuildStep(buildFrom,buildTo)
                                .withCaptureStep(capturable)
                                .addToScore(CAPTURE_SCORE)
                                .addToScore(buildFromScore)
                                .addToScore(buildToScore)
                                .build());
                        }

                        // get All build / stonemove moves
                        List<Move> allStoneMoves = GetValidStoneMoves(tilesAfterBuild,board.currentPlayer,board.size, buildTo);
                        foreach(Move scoredStoneMove in allStoneMoves)
                        {
                            scoredMoves.Add(ScoredMove.Builder()
                                .withBuildStep(buildFrom, buildTo)
                                .withMoveStep(scoredStoneMove.from, scoredStoneMove.to)
                                .addToScore(scoredStoneMove.score)
                                .addToScore(buildFromScore)
                                .addToScore(buildToScore)
                                .build());
                        }
                    }
                }
            }
        }
        return scoredMoves;
    }

    private int CalcScoreForBuildToTile(Board board, Tile buildTo)
    {
        int score = 0;
        score += hasStoneAsNeighbor(board, buildTo, false, board.currentPlayer);
        score += OnEnemyHomeLine(board.size, buildTo, board.currentPlayer);
        score += BuildToSameLevel(buildTo, board.currentPlayer);
        //score += BlockOpponent(board.tiles, board.size, buildTo,true);
        return score;
    }

    private int BlockOpponent(Tile[,] tiles, int size, Tile tile, bool buildingTo, Player currentPlayer)
    {
        int score = 0;
        foreach(Tile neighbor in Rules.GetNeighbors(tiles, size, tile))
        {
            if(neighbor.occupiatBy == Rules.GetOpponent(currentPlayer))
            {
                //CLIMBER
                if (buildingTo && currentPlayer == Player.CLIMBER && tile.level != TileLevel.HILL)
                {
                    int distance = Math.Abs(tile.coord.r - Rules.homeLine(currentPlayer, size));
                    score += size / distance * BLOCK_OPPONENT;
                }
                //DIGGER
                else if (!buildingTo && currentPlayer == Player.DIGGER &&tile.level != TileLevel.UNDERGROUND)
                {
                    int distance = Math.Abs(tile.coord.r - Rules.homeLine(currentPlayer, size));
                    score += size  / distance * BLOCK_OPPONENT;
                }
            }
        }
        return score;

    }

    private int OnEnemyHomeLine(int size, Tile tile, Player currentPlayer)
    {
        int score = 0;
        int homeLineR = Rules.homeLine(Rules.GetOpponent(currentPlayer), size);
        if (homeLineR == tile.coord.r && currentPlayer == Player.CLIMBER)
        {
            score += PLACE_TILE_ON_HOME_LINE;
        }
        else if(homeLineR == tile.coord.r && currentPlayer == Player.DIGGER)
        {
            score -= PLACE_TILE_ON_HOME_LINE;
        }
        return score;
    }

    private int CalcScoreForBuildFromTile(Board board, Tile tile)
    {
        int score = 0;
        score += hasStoneAsNeighbor(board, tile, true, board.currentPlayer);
        score += fromEnemysHomeLine(board, tile, board.currentPlayer);
        score += BuildFromSameLevel(tile, board.currentPlayer);
        if (board.currentPlayer == Player.DIGGER)
        {
            score += NextToStone(board.tiles, tile,board.size, board.currentPlayer) ? NEXT_TO_STONE : 0;
        }
        //score += BlockOpponent(board.tiles, board.size, tile, false);
        return score;
    }

    private int BuildFromSameLevel(Tile tile, Player currentPlayer)
    {
        int score = 0;
        if(currentPlayer == Player.CLIMBER && Rules.GetRequiredTileLevel(currentPlayer) == tile.level)
        {
            score += BUILD_FROM_SAME_LEVEL;
        }
        return score;
    }

    private int BuildToSameLevel(Tile tile, Player currentPlayer)
    {
        int score = 0;
        if (currentPlayer == Player.DIGGER && Rules.GetRequiredTileLevel(currentPlayer) == tile.level)
        {
            score += BUILD_TO_SAME_LEVEL;
        }
        return score;
    }

    private int fromEnemysHomeLine(Board board, Tile tile, Player currentPlayer)
    {
        if (tile.occupiatBy == null)
        {
            int homelineR = Rules.homeLine(Rules.GetOpponent(currentPlayer), board.size);
            if(tile.coord.r == homelineR)
            {
                return 1;
            }
        }
        return 0;
    }

    private int hasStoneAsNeighbor(Board board, Tile tile, bool takeStone, Player currentPlayer)
    {
        int score = 0;
        foreach (Tile neighbor in Rules.GetNeighbors(board.tiles, board.size, tile))
        {
            if(currentPlayer == Player.DIGGER)
            {
                if (currentPlayer== neighbor.occupiatBy && takeStone)
                {
                    score++;
                }
            }
            else
            {
                if (currentPlayer == neighbor.occupiatBy && !takeStone)
                {
                    score++;
                }
            }

        }
        return score;
    }

    private static Move GetBestMove(Tile[,] tiles, Player currentPlayer,int size, Tile buildTo)
    {
        List<Move> validMoves = GetValidStoneMoves(tiles, currentPlayer, size, buildTo);

        if (validMoves.Count == 0) return null;
        return validMoves.ToArray()[0];
    }

    private static List<Move> GetValidStoneMoves(Tile[,] tiles, Player currentPlayer,int size, Tile buildTo)
    {
        List<Move> validMoves = new List<Move>();

        foreach (Tile fromTile in tiles)
        {
            // board.currentPlayer
            if (fromTile != null && fromTile.occupiatBy == currentPlayer)
            {
                foreach (Tile toTile in Rules.GetNeighbors(tiles, size, fromTile))
                {
                    if (Rules.canMoveStone(tiles,currentPlayer, size, fromTile, toTile))
                    {
                        int score = 0;
                        score += CloserToGoal(currentPlayer, size, fromTile, toTile) ? CLOSER_TO_GOAL :0;
                        score += EscapeThreat(tiles, size, fromTile, toTile, currentPlayer)? ESCAPE_THREAT : 0;
                        score += AvoidCapture(tiles,size,toTile,currentPlayer) ? AVOID_CAPTURE : 0;
                        score += Win(size, toTile, currentPlayer) ? WINNING_SCORE: 0;
                        if(currentPlayer == Player.CLIMBER)
                        {
                            score += NextToStone(tiles, toTile, size, currentPlayer)? NEXT_TO_STONE : 0;
                        }


                        score += MoveToBuild(buildTo, toTile) ? MOVE_TO_BUILD_SCORE : 0;
                        validMoves.Add(new Move(fromTile, toTile, score));
                    }
                }
            }
        }
        validMoves.Sort((Move x, Move y) => y.score.CompareTo(x.score));
        return validMoves;
    }

    private static bool BlockOppenentOnHomeLine()
    {
        throw new NotImplementedException();
    }

    private static bool NextToStone(Tile[,] tiles, Tile toTile, int size, Player currentPlayer)
    {
        foreach (Tile tile in Rules.GetNeighbors(tiles, size, toTile))
        {
            if (tile.occupiatBy == currentPlayer)
            {
                return true;
            }
        }
        return false;
    }

    private static bool MoveToBuild(Tile buildTo, Tile toTile)
    {
        return buildTo != null && toTile.coord.Equals(buildTo.coord);
    }

    private static bool Win(int size, Tile toTile, Player currentPlayer)
    {
        return Rules.homeLine(Rules.GetOpponent(currentPlayer), size) == toTile.coord.r;
    }

    private static bool AvoidCapture(Tile[,] tiles, int size, Tile tile, Player currentPlayer)
    {
        return Rules.CountNeighborsOccupiatBy(tiles, size, tile, Rules.GetOpponent(currentPlayer)) > Rules.THREAT_THRESHHOLD;
    }

    private static bool EscapeThreat(Tile[,] tiles, int size, Tile fromTile, Tile tile, Player currentPlayer)
    {
        bool fromUnderThreat = Rules.UnderThreat(tiles, size, fromTile,currentPlayer);
        bool toUnderThreat = Rules.UnderThreat(tiles, size, tile, currentPlayer);
        return fromUnderThreat && !toUnderThreat;
    }

    private static bool CloserToGoal(Player currentPlayer, int size, Tile fromTile, Tile toTile)
    {
        int goalR = Rules.homeLine(Rules.GetOpponent(currentPlayer), size); //0 or 6
        return Math.Abs(goalR - toTile.coord.r) < Math.Abs(goalR - fromTile.coord.r);
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
                score += hasStoneAsNeighbor(board,tile,false, board.currentPlayer);
                result.Add(new ScoredTile(tile, score));
            }
        }
        return result.ToArray();
    }
}