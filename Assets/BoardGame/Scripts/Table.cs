using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour {

    Game game;
    int currentSize = 0;

	void Start () {
        game = FindObjectOfType<Game>();	
	}
	
	void Update () {
        if( game != null && game.board != null && game.board.settings != null && currentSize != game.board.settings.size){
            currentSize = game.board.settings.size;
            switch (currentSize){
                case 3:
                    transform.localPosition = new Vector3(0, -6.3f, 0);
                    transform.localScale = new Vector3(0.12f, 0.1f, 0.12f);
                    break;
                case 4:
                    transform.localPosition = new Vector3(0, -9.47f, 0);
                    transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    break;
                case 5:
                    transform.localPosition = new Vector3(0, -12.59f, 0);
                    transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    break;
                default:
                    transform.localPosition = new Vector3(0, -12.59f, 0);
                    transform.localScale = new Vector3(0.23f, 0.2f, 0.23f);
                    break;
            }
        }
        	
	}
}
