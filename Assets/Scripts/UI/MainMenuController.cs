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
            newGameBttn.SetActive(!game.isPlaying);
            continueGameBttn.SetActive(game.isPlaying);
            restartGameBttn.SetActive(game.isPlaying);
    }
}
