using UnityEngine;

public class GameOverMenu : MonoBehaviour {

    public GameObject winningStone;
    public float rotationSpeed = 25f;
    Game game;
    BoardFactory boardFactory;
    MenuController menuController;

	// Use this for initialization
	void Start () {
        game = FindObjectOfType<Game>();
        boardFactory = FindObjectOfType<BoardFactory>();
        menuController = FindObjectOfType<MenuController>();
        winningStone.SetActive(true);
        if (Player.DIGGER == Rules.CheckWinningCondition(game.board))
        {
            winningStone.GetComponent<Renderer>().material = boardFactory.digger;
        }
        else
        {
            winningStone.GetComponent<Renderer>().material = boardFactory.climber;
        }
	}

    private void Update()
    {
        winningStone.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }


    public void onMainMenu(){
        winningStone.SetActive(false);
        menuController.OnMainMenu();
    }
}
