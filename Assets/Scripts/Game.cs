using UnityEngine;

[RequireComponent(typeof(Selector))]
[RequireComponent(typeof(BoardFactory))]
public class Game : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject gameInfo;

    public Board board { get; private set; }

    private BoardFactory boardFactory;
    private CameraOrbit cameraOrbit;
    public bool isPlaying { get; private set; }
    private Selector selector;


    void Awake () {
        selector = GetComponent<Selector>();
        boardFactory = GetComponent<BoardFactory>();
	}

    private void Start()
    {
        cameraOrbit = FindObjectOfType<CameraOrbit>();
        board = boardFactory.create();
        isPlaying = false;
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
        board = boardFactory.Restart();
        isPlaying = true;
    }
}
