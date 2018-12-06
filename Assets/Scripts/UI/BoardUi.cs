using UnityEngine;

public class BoardUi : MonoBehaviour{
    
    private Game game;
    private BoardFactory boardFactory;

    private void Start()
    {
        game = FindObjectOfType<Game>();
        boardFactory = FindObjectOfType<BoardFactory>();
    }

    public bool placeStone(Stone stone, Hexagon to)
    {
        Tile fromTile = game.board.coordToTile(stone.coord);
        Tile toTile = game.board.coordToTile(to.coord);
        if (Rules.moveStone(game.board, fromTile, toTile))
        {
            Vector3 toPosition = boardFactory.GetPositionForStone(to.coord, to.getTile().level);
            stone.transform.position = toPosition;
            stone.coord = to.coord;
            game.board.currentStep = Step.BUILD;
            Rules.nextPlayer(game.board);
            return true;
        }
        return false;
    }

    public bool placeHexagon(Hexagon from, Hexagon to)
    {
        Tile fromTile = game.board.coordToTile(from.coord);
        Tile toTile = game.board.coordToTile(to.coord);
        if (Rules.build(game.board, fromTile, toTile))
        {
            if (Rules.canMoveStoneAnyWhere(game.board))
            {
                Vector3 toPosition = boardFactory.GetPositionForHexagon(to.coord, to.getTile().level);
                from.transform.position = toPosition;
                from.coord = to.coord;
                from.SetOriginalMaterial(boardFactory.LevelToMaterial(toTile.level));
                game.board.currentStep = Step.MOVE;
                return true;
            }
            else
            {
                Rules.build(game.board, toTile, fromTile);
            }
        }
        return false;
    }

    internal void MouseOver(Transform element, bool dragging )
    {
        Hexagon to = element.GetComponent<Hexagon>();
        TileLevel notAllowed = dragging ? TileLevel.HILL : TileLevel.UNDERGROUND;
        if (to != null && Rules.canMoveTile(game.board, to.coord) && to.getTile().level != notAllowed )
        {
            to.Highlight();
        }
    }

}
