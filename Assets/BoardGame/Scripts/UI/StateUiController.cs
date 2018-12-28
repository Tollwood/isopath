using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StateUiController : MonoBehaviour {

    private TextMeshProUGUI text;
    private Game game;
    // Use this for initialization
	void Start () {
        text = GetComponent<TextMeshProUGUI>();
        game = FindObjectOfType<Game>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = game.board.currentStep + " - " + game.board.currentPlayer;
        Player? winner = Rules.CheckWinningCondition(game.board);
        if(winner != null){
            text.text = winner + " won the game";
        }
	}

}
