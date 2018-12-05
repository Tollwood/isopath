using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StateUiController : MonoBehaviour {

    private TextMeshProUGUI text;
    private Board board;
    // Use this for initialization
	void Start () {
        text = GetComponent<TextMeshProUGUI>();
        board = FindObjectOfType<Game>().board;
	}
	
	// Update is called once per frame
	void Update () {
        text.text = board.currentStep + " - " + board.currentPlayer;
        Player? winner = Rules.CheckWinningCondition(board);
        if(winner != null){
            text.text = winner + " won the game";
        }
	}

}
