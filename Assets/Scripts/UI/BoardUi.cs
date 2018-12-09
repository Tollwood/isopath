using System;
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
        Tile fromTile = game.board.tiles[stone.coord.q,stone.coord.r];
        Tile toTile = game.board.tiles[to.coord.q,to.coord.r];
        if (Rules.canMoveStone(game.board,fromTile, toTile))
        {
            game.MoveStone(fromTile, toTile);
            Vector3 toPosition = boardFactory.GetPositionForStone(to.coord, to.getTile().level);
            stone.transform.position = toPosition;
            stone.coord = to.coord;
            return true;
        }
        return false;
    }

    public bool placeHexagon(Hexagon from, Hexagon to)
    {
        Tile fromTile = game.board.tiles[from.coord.q,from.coord.r];
        Tile toTile = game.board.tiles[to.coord.q,to.coord.r];
        if ( Rules.canBuild(game.board, fromTile, toTile) )
        {
                game.build(fromTile, toTile);
                Vector3 toPosition = boardFactory.GetPositionForHexagon(to.coord, to.getTile().level);
                from.transform.position = toPosition;
                from.coord = to.coord;
                from.SetOriginalMaterial(boardFactory.LevelToMaterial(game.board.tiles[toTile.coord.q,toTile.coord.r].level));
                return true;
        }
        return false;
    }

    internal void MouseOver(Transform element, bool dragging )
    {
        Hexagon hex = element.GetComponent<Hexagon>();
        TileLevel notAllowed = dragging ? TileLevel.HILL : TileLevel.UNDERGROUND;
        if (hex != null && Rules.canMoveTile(game.board, hex.coord) && hex.getTile().level != notAllowed )
        {
            hex.Highlight();
        }
    }

}
