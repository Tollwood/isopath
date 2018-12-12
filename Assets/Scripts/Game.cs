using UnityEngine;

[RequireComponent(typeof(Selector))]
[RequireComponent(typeof(BoardFactory))]
public class Game : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject gameInfo;
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

    public void OnPause()
    {
        mainMenu.SetActive(true);
        gameInfo.SetActive(false);
        gameState = GameState.PAUSED;
    }

    public void OnPlay()
    {
        mainMenu.SetActive(false);
        gameInfo.SetActive(true);
        gameState = GameState.PLAYING;
    }

    public void OnNewGame()
    {
        mainMenu.SetActive(false);
        gameInfo.SetActive(true);
        cameraOrbit.shouldOrbit = false;
        cameraOrbit.Reset();
        board = boardFactory.Restart(gameConfig);
        gameState = GameState.PLAYING;
    }

    public void OnMainMenu()
    {
        board = boardFactory.Restart(gameConfig);
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
}
