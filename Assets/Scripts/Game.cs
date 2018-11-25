using UnityEngine;

[RequireComponent(typeof(Selector))]
[RequireComponent(typeof(BoardFactory))]
public class Game : MonoBehaviour {

    private BoardFactory boardFactory;
    public Board board { get; private set; }
    private Selector selector;
	
    void Awake () {
        selector = GetComponent<Selector>();
        boardFactory = GetComponent<BoardFactory>();
        board = boardFactory.create();
	}
}
