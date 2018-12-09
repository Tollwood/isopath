using System;
using UnityEngine;

[RequireComponent(typeof(Selector))]
[RequireComponent(typeof(BoardFactory))]
public class Game : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject gameInfo;
    public GameConfig gameConfig = new GameConfig();

    public Board board { get; private set; }

    private BoardFactory boardFactory;
    private BoardUi boardUi;

    private CameraOrbit cameraOrbit;
    public bool isPlaying { get; private set; }
    private Selector selector;

    private AiAgent aiClimber;
    private AiAgent aiDigger;

    void Awake () {
        selector = GetComponent<Selector>();
        boardFactory = GetComponent<BoardFactory>();
        boardUi = GetComponent<BoardUi>();
	}

    private void Start()
    {
        cameraOrbit = FindObjectOfType<CameraOrbit>();
        if(gameConfig.aiClimber){
            aiClimber = new AiAgent(Player.CLIMBER);
        }
        if (gameConfig.aiDigger)
        {
            aiDigger = new AiAgent(Player.DIGGER);
        }
        board = boardFactory.create(gameConfig);
        isPlaying = false;
    }

    private void Update()
    {
        if(board != null){
            if (aiClimber != null && board.currentStep == Step.BUILD && board.currentPlayer == Player.CLIMBER)
            {
                Move move = aiClimber.build(board);
                build(move);
            }
            if (aiDigger != null && board.currentStep == Step.BUILD && board.currentPlayer == Player.DIGGER)
            {
                Move move = aiDigger.build(board);
                build(move);
            }    
        }
    }

    public void OnPause()
    {
        mainMenu.SetActive(true);
        gameInfo.SetActive(false);
    }

    public void OnPlay()
    {
        mainMenu.SetActive(false);
        gameInfo.SetActive(true);
    }

    public void OnNewGame()
    {
        mainMenu.SetActive(false);
        gameInfo.SetActive(true);
        cameraOrbit.shouldOrbit = false;
        cameraOrbit.Reset();
        isPlaying = true;
    }

    public void OnRestart()
    {
        board = boardFactory.Restart(gameConfig);
        isPlaying = true;
    }

    public void build(Tile from, Tile to){
        Tile[,] tiles = BoardStateModifier.build( board.tiles, from, to);
        board = new Board(board.size, tiles, Step.MOVE, board.currentPlayer);
    }

    private void build(Move move)
    {
        Debug.Log(move);
        Hexagon fromHex = (Hexagon)boardFactory.findByTile(move.from);
        Hexagon toHex = (Hexagon)boardFactory.findByTile(move.to);
        boardUi.placeHexagon(fromHex, toHex);
    }

    internal void MoveStone(Tile fromTile, Tile toTile)
    {
        board = BoardStateModifier.moveStone(board, fromTile, toTile);
    }
}
