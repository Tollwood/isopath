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
        yield return new WaitForSeconds(2f);
        if(thinking){
            Moves nextMove = agent.GetNextMove(game.board);
            build(nextMove.buildMove);
            yield return new WaitForSeconds(2f);
            if (thinking)
            {
                MoveStone(nextMove.stoneMove);
            }
        }
        thinking = false;
    }

    private void build(Move move)
    {
        Debug.Log(move);
        boardFactory.placeHexagon(move.from, move.to);
    }

    private void MoveStone(Move move)
    {
        Debug.Log(move);
        Stone stone = (Stone)boardFactory.findStoneByTile(move.from);
        Hexagon toHex = (Hexagon)boardFactory.findByTile(move.to);
        boardFactory.placeStone(stone, toHex);
    }

}
