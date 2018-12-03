using UnityEngine;

public class BoardUi : MonoBehaviour{
    
    private Board board;
    private BoardFactory boardFactory;

    private void Start()
    {
        board = FindObjectOfType<Game>().board;
        boardFactory = FindObjectOfType<BoardFactory>();
    }

    public bool placeStone(Stone stone, Hexagon to)
    {
        Tile fromTile = board.coordToTile(stone.coord);
        Tile toTile = board.coordToTile(to.coord);
        if (Rules.moveStone(board, fromTile, toTile))
        {
            Vector3 toPosition = boardFactory.GetPositionForStone(to.coord, to.getTile().level);
            stone.transform.position = toPosition;
            stone.coord = to.coord;
            board.currentStep = Step.BUILD;
            Rules.nextPlayer(board);
            return true;
        }
        return false;
    }

    public bool placeHexagon(Hexagon from, Hexagon to)
    {
        Tile fromTile = board.coordToTile(from.coord);
        Tile toTile = board.coordToTile(to.coord);
        if (Rules.build(board, fromTile, toTile))
        {
            if (Rules.canMoveStoneAnyWhere(board))
            {
                Vector3 toPosition = boardFactory.GetPositionForHexagon(to.coord, to.getTile().level);
                from.transform.position = toPosition;
                from.coord = to.coord;
                from.SetOriginalMaterial(boardFactory.LevelToMaterial(toTile.level));
                board.currentStep = Step.MOVE;
                return true;
            }
            else
            {
                Rules.build(board, toTile, fromTile);
            }
        }
        return false;
    }

    internal void MouseOver(Transform element, bool dragging )
    {
        Hexagon to = element.GetComponent<Hexagon>();
        TileLevel notAllowed = dragging ? TileLevel.HILL : TileLevel.UNDERGROUND;
        if (to != null && Rules.canMoveTile(board, to.coord) && to.getTile().level != notAllowed )
        {
            to.Highlight();
        }
    }

}
