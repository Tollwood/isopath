using System;
using System.Collections;
using UnityEngine;


public class AiExecutor : MonoBehaviour {

    private Game game;

    private BoardFactory boardFactory;

    private AiAgent aiClimber;
    private AiAgent aiDigger;
    private GameState knownGameState;
    private bool thinking = false;

    private float timeToThink = .5f;

    private void Start()
    {
        game = FindObjectOfType<Game>();
        boardFactory = FindObjectOfType<BoardFactory>();
        knownGameState = GameState.UNKOWN;
    }

    private void Update()
    {
        if (GameState.UNKOWN == knownGameState || knownGameState != game.gameState){
            knownGameState = game.gameState;
            UpdateUIPlayer();
            thinking = false;
        }

        if(knownGameState == GameState.GAME_OVER){
            return;
        }
        if (game.board != null)
        {
            if (aiClimber != null && game.board.currentPlayer == Player.CLIMBER && !thinking)
            {
                thinking = true;
                StartCoroutine("AiMove", aiClimber);
            }
            if (aiDigger != null && game.board.currentPlayer == Player.DIGGER && !thinking)
            {
                thinking = true;
                StartCoroutine("AiMove", aiDigger);
            }
        }
        }

    // TODO replace with events
    private void UpdateUIPlayer()
    {

        switch(knownGameState){
            case GameState.AI_PLAYING:
                aiClimber = new AiAgent(Player.CLIMBER);
                aiDigger = new AiAgent(Player.DIGGER);
                break;
            case GameState.PLAYING:
                // wait for last ai move to finish
                if (!game.board.settings.aiClimber)
                {
                    aiClimber = null;
                }
                if (!game.board.settings.aiDigger)
                {
                    aiDigger = null;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator AiMove(AiAgent agent){
        yield return new WaitForSeconds(timeToThink);
        if(thinking){
            ScoredMove nextMove = agent.GetNextMove(game.board);
            if(nextMove.captureStone != null && nextMove.buildFrom == null)
            {
                CaptureStone(nextMove);
                yield return new WaitForSeconds(timeToThink);
            }
            else
            {
                build(nextMove);
                yield return new WaitForSeconds(timeToThink);
            }

            if (thinking)
            {
                if (nextMove.captureStone != null && nextMove.moveTo == null)
                {
                    CaptureStone(nextMove);
                }
                else {
                    MoveStone(nextMove);
                }
            }
        }
        thinking = false;
    }

    private void CaptureStone(ScoredMove nextMove)
    {
        boardFactory.CaptureStone(nextMove.captureStone);
    }

    private void build(ScoredMove move)
    {
        Debug.Log(move);
        boardFactory.placeHexagon(move.buildFrom, move.buildTo);
    }

    private void MoveStone(ScoredMove move)
    {
        Debug.Log(move);
        Stone stone = boardFactory.findStoneByTile(move.moveFrom);
        Hexagon toHex = (Hexagon)boardFactory.findHexagonByTile(move.moveTo);
        if(toHex != null)
        {
            boardFactory.placeStone(stone, toHex);
        }
    }

}
