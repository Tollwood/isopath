
using System;
using System.Collections.Generic;

public class AiAgent
{
    private const int CAPTURE_SCORE = 3;
    private const int ESCAPE_THREAT = 5;
    private const int PLACE_TILE_ON_HOME_LINE = 2;

    private Player player;
    private Random random;

    public AiAgent(Player player){
        this.player = player;
        this.random = new Random(1);
    }

    public ScoredMove GetNextMove(Board board){
        // Get All capture an enemy and Move Stone
        List<ScoredMove> scoredMoves = new List<ScoredMove>();
        scoredMoves.AddRange(CaptureMoves(board));
        scoredMoves.AddRange(BuildMoves(board));

        scoredMoves.Sort((ScoredMove x, ScoredMove y) => y.score.CompareTo(x.score));
        return scoredMoves.ToArray()[0];

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
            Move stoneMove = GetBestMove(boardAfterCapture.tiles, player,board.size);
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
                        List<Move> allStoneMoves = GetValidStoneMoves(tilesAfterBuild,player,board.size);
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
        return score;
    }

    private int OnEnemyHomeLine(int size, Tile tile)
    {
        int score = 0;
        int homeLineR = Rules.homeLine(Rules.GetOpponent(player), size);
        if (homeLineR == tile.coord.r)
        {
            score += PLACE_TILE_ON_HOME_LINE;
        }
        return score;
    }

    private int CalcScoreForBuildFromTile(Board board, Tile tile)
    {
        int score = 0;
        score += hasStoneAsNeighbor(board, tile, true);
        score += fromEnemysHomeLine(board, tile);
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

    private static Move GetBestMove(Tile[,] tiles, Player currentPlayer,int size)
    {
        return GetValidStoneMoves(tiles,currentPlayer,size).ToArray()[0];
    }

    private static List<Move> GetValidStoneMoves(Tile[,] tiles, Player currentPlayer,int size)
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
                        score += CloserToGoal(currentPlayer, size, fromTile, toTile);
                        score += EscapeThreat(tiles, size, fromTile, toTile, currentPlayer);
                        validMoves.Add(new Move(fromTile, toTile, score));
                    }
                }
            }
        }
        validMoves.Sort((Move x, Move y) => y.score.CompareTo(x.score));
        return validMoves;
    }

    private static int EscapeThreat(Tile[,] tiles, int size, Tile fromTile, Tile tile, Player currentPlayer)
    {
        int score = 0;
        bool fromUnderThreat = Rules.UnderThreat(tiles, size, fromTile,currentPlayer);
        bool toUnderThreat = Rules.UnderThreat(tiles, size, fromTile, currentPlayer);
        if(fromUnderThreat && !toUnderThreat)
        {
            score += ESCAPE_THREAT;
        }
        return score;
    }

    private static int CloserToGoal(Player currentPlayer, int size, Tile fromTile, Tile toTile)
    {
        Player opponent = Rules.GetOpponent(currentPlayer);
        int goalR = Rules.homeLine(opponent, size); //0 or 6
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
}