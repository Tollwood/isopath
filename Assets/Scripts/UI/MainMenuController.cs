using UnityEngine;

public class MainMenuController : MonoBehaviour {

    public GameObject newGameBttn;
    public GameObject continueGameBttn;
    public GameObject restartGameBttn;

    private Game game;

    private void Start()
    {
        game = FindObjectOfType<Game>();
    }

    private void Update()
    {
        newGameBttn.SetActive(game.gameState != GameState.PAUSED);
        continueGameBttn.SetActive(game.gameState == GameState.PAUSED);
        restartGameBttn.SetActive(game.gameState == GameState.PAUSED);
    }
}
