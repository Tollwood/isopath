
using System;
using System.Linq;
using System.Collections.Generic;

public class AiAgent
{
    private const int WINNING_SCORE = 100;
    private const int CAPTURE_SCORE = 3;
    private const int ESCAPE_THREAT = 5;
    private const int AVOID_CAPTURE = -5;
    private const int PLACE_TILE_ON_HOME_LINE = 2;
    private const int BUILD_FROM_SAME_LEVEL = -1;
    private const int BUILD_TO_SAME_LEVEL = -1;
    private const int CLOSER_TO_GOAL = 1;
    private const int MOVE_TO_BUILD_SCORE = 1;
    private const int BLOCK_OPPONENT = 2;
    private Player player;
    private Random random;

    public AiAgent(Player player){
        this.player = player;
        this.random = new Random(new Random().Next());
    }

    public ScoredMove GetNextMove(Board board){
        // Get All capture an enemy and Move Stone
        List<ScoredMove> scoredMoves = new List<ScoredMove>();
        scoredMoves.AddRange(CaptureMoves(board));
        scoredMoves.AddRange(BuildMoves(board));

        scoredMoves.Sort((ScoredMove x, ScoredMove y) => y.score.CompareTo(x.score));
        int maxScore = scoredMoves.ToArray()[0].score;
        ScoredMove[] bestMoves = scoredMoves.Where((move) => move.score == maxScore).ToArray();
        return bestMoves[random.Next(bestMoves.Length-1)];

    }

    private List<Tile> CapturableTiles(Tile[,] tiles, int size)
    {
        List<Tile> captureable= new List<Tile>();

        foreach (Tile tile in tiles)
        {
            if (tile == null)
            {
                continue;
            }
            if (tile.occupiatBy == Rules.GetOpponent(player) && Rules.CanCapture(tiles, size, player, tile))
            {
                captureable.Add(tile);
            }
        }
        return captureable;
    }

    private List<ScoredMove> CaptureMoves(Board board)
    {
        List<ScoredMove> captureMoves = new List<ScoredMove>();
        foreach (Tile tile in CapturableTiles(board.tiles, board.size))
        {
            // Find best Move
            Board boardAfterCapture = BoardStateModifier.Capture(board, tile);
            Move stoneMove = GetBestMove(boardAfterCapture.tiles, player,board.size, null);
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
                        foreach(Tile capturable in CapturableTiles(tilesAfterBuild,board.size))
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
                        List<Move> allStoneMoves = GetValidStoneMoves(tilesAfterBuild,player,board.size, buildTo);
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
        score += hasStoneAsNeighbor(board, buildTo, false);
        score += OnEnemyHomeLine(board.size, buildTo);
        score += BuildToSameLevel(buildTo);
        score += BlockOpponent(board.tiles, board.size, buildTo,true);
        return score;
    }

    private int BlockOpponent(Tile[,] tiles, int size, Tile tile, bool buildingTo)
    {
        int score = 0;
        foreach(Tile neighbor in Rules.GetNeighbors(tiles, size, tile))
        {
            if(neighbor.occupiatBy == Rules.GetOpponent(player))
            {
                //CLIMBER
                if (buildingTo && player == Player.CLIMBER && tile.level != TileLevel.HILL)
                {
                    score += BLOCK_OPPONENT;
                    // get distance to homeLine
                    // the close the more critical     
                }
                //DIGGER
                else if (!buildingTo && player == Player.DIGGER &&tile.level != TileLevel.UNDERGROUND)
                {
                    score += BLOCK_OPPONENT;
                }
            }
        }
        return score;

    }

    private int OnEnemyHomeLine(int size, Tile tile)
    {
        int score = 0;
        int homeLineR = Rules.homeLine(Rules.GetOpponent(player), size);
        if (homeLineR == tile.coord.r && player == Player.CLIMBER)
        {
            score += PLACE_TILE_ON_HOME_LINE;
        }
        else if(homeLineR == tile.coord.r && player == Player.DIGGER)
        {
            score -= PLACE_TILE_ON_HOME_LINE;
        }
        return score;
    }

    private int CalcScoreForBuildFromTile(Board board, Tile tile)
    {
        int score = 0;
        score += hasStoneAsNeighbor(board, tile, true);
        score += fromEnemysHomeLine(board, tile);
        score += BuildFromSameLevel(tile);
        score += BlockOpponent(board.tiles, board.size, tile, false);
        return score;
    }

    private int BuildFromSameLevel(Tile tile)
    {
        int score = 0;
        if(player == Player.CLIMBER && Rules.GetRequiredTileLevel(player) == tile.level)
        {
            score += BUILD_FROM_SAME_LEVEL;
        }
        return score;
    }

    private int BuildToSameLevel(Tile tile)
    {
        int score = 0;
        if (player == Player.DIGGER && Rules.GetRequiredTileLevel(player) == tile.level)
        {
            score += BUILD_TO_SAME_LEVEL;
        }
        return score;
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
        foreach (Tile neighbor in Rules.GetNeighbors(board.tiles, board.size, tile))
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

    private static Move GetBestMove(Tile[,] tiles, Player currentPlayer,int size, Tile buildTo)
    {
        return GetValidStoneMoves(tiles,currentPlayer,size, buildTo).ToArray()[0];
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
                        score += MoveToBuild(buildTo, toTile) ? MOVE_TO_BUILD_SCORE : 0;
                        validMoves.Add(new Move(fromTile, toTile, score));
                    }
                }
            }
        }
        validMoves.Sort((Move x, Move y) => y.score.CompareTo(x.score));
        return validMoves;
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
                score += hasStoneAsNeighbor(board,tile,false);
                result.Add(new ScoredTile(tile, score));
            }
        }
        return result.ToArray();
    }
}