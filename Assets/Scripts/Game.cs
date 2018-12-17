using System;
using UnityEngine;

[RequireComponent(typeof(Selector))]
[RequireComponent(typeof(BoardFactory))]
public class Game : MonoBehaviour {
    
    public Board board { get; private set; }
    public Settings settings { get; private set; }
    private BoardFactory boardFactory;

    private CameraOrbit cameraOrbit;
    public GameState gameState { get; private set; }
    private Selector selector;
    private SettingsController settingsController;
    private MenuController menuController;

    void Awake () {
        selector = GetComponent<Selector>();
        boardFactory = GetComponent<BoardFactory>();
        settings = new Settings(4, false, false);
        string defaultSettings = JsonUtility.ToJson(settings);
        settings = (Settings)JsonUtility.FromJson(PlayerPrefs.GetString("Settings", defaultSettings), settings.GetType());
	}

    private void Start()
    {
        cameraOrbit = FindObjectOfType<CameraOrbit>();
        menuController = FindObjectOfType<MenuController>();
        board = boardFactory.create(settings);
        gameState = GameState.AI_PLAYING;
    }

   
    public void build(Tile from, Tile to){
        Tile[,] tiles = BoardStateModifier.build( board.tiles, from, to);
        board = new Board(board.size, tiles, Step.MOVE, board.currentPlayer,board.settings);
    }

    internal void MoveStone(Tile fromTile, Tile toTile)
    {
        board = BoardStateModifier.moveStone(board, fromTile, toTile);
        GameOverCheck();
    }

    internal void CaptureStone(Tile captureStone)
    {
        board = BoardStateModifier.Capture(board, captureStone);
        GameOverCheck();
    }


    private void GameOverCheck()
    {
        if (Rules.CheckWinningCondition(board) != null)
        {
            gameState = GameState.GAME_OVER;
            menuController.OnGameOverMenu();
        }
    }

    // Buttons
    public void Restart()
    {
        board = boardFactory.Restart(settings);
        gameState = GameState.AI_PLAYING;
    }


    public void OnPause()
    {
        gameState = GameState.PAUSED;
    }

    public void OnPlay()
    {
        gameState = GameState.PLAYING;
    }

    public void OnNewGame()
    {
        cameraOrbit.Reset();
        board = boardFactory.Restart(settings);
        gameState = GameState.PLAYING;
    }
}
