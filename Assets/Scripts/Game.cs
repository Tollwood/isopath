using UnityEngine;

[RequireComponent(typeof(Selector))]
[RequireComponent(typeof(BoardFactory))]
public class Game : MonoBehaviour {
    
    public GameConfig gameConfig = new GameConfig();

    public Board board { get; private set; }

    private BoardFactory boardFactory;

    private CameraOrbit cameraOrbit;
    public GameState gameState { get; private set; }
    private Selector selector;


    void Awake () {
        selector = GetComponent<Selector>();
        boardFactory = GetComponent<BoardFactory>();
	}

    private void Start()
    {
        cameraOrbit = FindObjectOfType<CameraOrbit>();
        board = boardFactory.create(gameConfig);
        gameState = GameState.AI_PLAYING;
    }

   
    public void build(Tile from, Tile to){
        Tile[,] tiles = BoardStateModifier.build( board.tiles, from, to);
        board = new Board(board.size, tiles, Step.MOVE, board.currentPlayer);
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

    internal void MoveStone(Tile fromTile, Tile toTile)
    {
        board = BoardStateModifier.moveStone(board, fromTile, toTile);
    }

    // Buttons
    public void Restart()
    {
        board = boardFactory.Restart(gameConfig);
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
        board = boardFactory.Restart(gameConfig);
        gameState = GameState.PLAYING;
    }
}
