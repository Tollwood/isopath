using System;
using UnityEngine;

public class BoardFactory : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public GameObject stonePrefab;
    public Material hill;
    public Material ground;
    public Material underground;
    public Material climberHex;
    public Material climber;
    public Material digger;
    public Material hoverMaterial;

    public float scale = .12f;
    private float xqOffSet = 1.7320508f;
    private float zrOffset = 1.5f;

    private Transform uiContainer;

    private Game game;

    private void Awake()
    {
        xqOffSet = 1.7320508f;
        zrOffset = 1.5f;
}

    internal Board create(Settings settings, Game newGame)
    {
        if (uiContainer == null)
        {
            this.game = newGame;
            uiContainer = new GameObject().transform;
            uiContainer.parent = game.transform;
            uiContainer.name = "boardElements";
            uiContainer.localPosition = new Vector3(0, 0, 0);
        }
        Board board = new Board(settings, BoardStateModifier.ResetTiles(settings.size));

        foreach (Tile tile in board.tiles)
        {
            if(tile == null){
                continue;
            }
            if (tile.level == TileLevel.UNDERGROUND)
            {
                addHexagon(tile, GetPositionForHexagon(board.size,tile.coord,TileLevel.UNDERGROUND));
                addStone(tile.coord,TileLevel.UNDERGROUND,GetPositionForStone(board.size, tile.coord,tile.level));
            }
            if (tile.level == TileLevel.GROUND)
            {
                addHexagon(new Tile(tile.coord,TileLevel.UNDERGROUND,tile.occupiatBy), GetPositionForHexagon(board.size, tile.coord, TileLevel.UNDERGROUND));
                addHexagon(tile, GetPositionForHexagon(board.size, tile.coord, TileLevel.GROUND));
            }
            if (tile.level == TileLevel.HILL)
            {
                addHexagon(new Tile(tile.coord, TileLevel.UNDERGROUND, tile.occupiatBy), GetPositionForHexagon(board.size, tile.coord, TileLevel.UNDERGROUND));
                addHexagon(new Tile(tile.coord, TileLevel.GROUND, tile.occupiatBy), GetPositionForHexagon(board.size, tile.coord, TileLevel.GROUND));
                addHexagon(tile, GetPositionForHexagon(board.size, tile.coord, TileLevel.HILL));
                addStone(tile.coord,tile.level, GetPositionForStone(board.size, tile.coord,tile.level));
            }
        }
        return board;
    }

    public Material LevelToMaterial(TileLevel level)
    {
        Material newMaterial;
        switch (level)
        {
            case TileLevel.HILL:
                newMaterial =  hill;
                break;
            case TileLevel.GROUND:
                newMaterial = ground;
                break;
            default:
                newMaterial = underground;
                break;
        }
        return newMaterial;
    }

    private GameObject addHexagon(Tile tile, Vector3 position)
    {
        GameObject hexagonGo = Instantiate(hexagonPrefab,uiContainer);
        hexagonGo.transform.localPosition = position;
        Hexagon hexagon = hexagonGo.GetComponent<Hexagon>();
        hexagonGo.transform.localScale = new Vector3(scale, scale, scale);
        hexagon.SetOriginalMaterial(LevelToMaterial(tile.level));
        hexagon.hoverMaterial = hoverMaterial;
        hexagon.tile = tile;
        return hexagonGo;
    }

    private GameObject addStone(Coord coord, TileLevel tileLevel, Vector3 position)
    {
        Material stoneMaterial;
        switch (tileLevel)
        {
            case TileLevel.HILL:
                stoneMaterial = climber;
                break;
            default:
                stoneMaterial = digger;
                break;
        }

        GameObject stoneGo = Instantiate(stonePrefab, uiContainer);
        stoneGo.transform.localPosition = position;
        Stone stone = stoneGo.GetComponent<Stone>();
        stoneGo.transform.localScale = new Vector3(scale, scale, scale);
        stone.SetOriginalMaterial(stoneMaterial);
        stone.hoverMaterial = hoverMaterial;

        stone.coord = coord;
        return stoneGo;
    }

    internal Stone findStoneByTile(Tile tile)
    {
        foreach (Transform child in uiContainer)
        {
            Stone stone = child.GetComponent<Stone>();
            if (stone != null && stone.GetCoord().Equals(tile.coord)){
                return stone;
            }
        }
        return null;
    }

    public Vector3 GetPositionForStone(int size, Coord coord, TileLevel tileLevel){
        float stoneOffset;
        switch (tileLevel)
        {
            case TileLevel.HILL:
                stoneOffset = 0.6f;
                break;
            default:
                stoneOffset = 0.13f;
                break;
        }
        float x = xqOffSet * (coord.q - (size - 1) + (coord.r - (size - 1))) - (coord.r - (size - 1));
        return new Vector3( x * scale, stoneOffset * scale, (zrOffset * (coord.r -(size-1)) )* scale);
    }

    public Vector3 GetPositionForHexagon(int size, Coord coord, TileLevel tileLevel)
    {
        return new Vector3(  (xqOffSet  * (coord.q - (size - 1) + (coord.r - (size - 1))) - (coord.r - (size - 1))) * scale, (GetTileLevelOffset(tileLevel)) * scale, (zrOffset * (coord.r - (size-1) )) * scale);
    }

    public Board Restart(Settings settings){
        foreach (Transform child in uiContainer)
        {
            Destroy(child.gameObject);
        }
        return create(settings,game);
    }

    public Hexagon findHexagonByTile(Tile tile){
        foreach (Transform child in uiContainer)
        {
            Hexagon hexagon = child.GetComponent<Hexagon>();  
            if(hexagon != null && hexagon.GetCoord().Equals(tile.coord) && hexagon.tile.level == tile.level){
                return hexagon;
            }
        }
        return null;
    }

    private float GetTileLevelOffset(TileLevel tileLevel)
    {
        float tileLevelOffSet;
        switch (tileLevel)
        {
            case TileLevel.HILL:
                tileLevelOffSet = 0.3f;
                break;
            case TileLevel.GROUND:
                tileLevelOffSet = 0.1f;
                break;
            default:
                tileLevelOffSet = 0f;
                break;
        }
        return tileLevelOffSet;
    }

    internal void CaptureStone(Tile captureStone)
    {
        if(Rules.CanCapture(game.board.tiles, game.board.size, captureStone))
        {
            game.CaptureStone(captureStone);
            Stone stone = findStoneByTile(captureStone);
            if(stone != null)
            {
                Destroy(stone.gameObject);
            }
        }
    }

    public bool placeStone(Stone stone, Hexagon to)
    {
        Tile fromTile = game.board.tiles[stone.coord.q, stone.coord.r];
        Tile toTile = game.board.tiles[to.tile.coord.q, to.tile.coord.r];
        if (Rules.canMoveStone(game.board.tiles, game.board.currentPlayer, game.board.size, fromTile, toTile))
        {
            game.MoveStone(fromTile, toTile);
            Vector3 toPosition = GetPositionForStone(game.board.size, to.tile.coord, to.tile.level);
            stone.transform.localPosition = toPosition;
            stone.transform.localScale = new Vector3(scale, scale, scale);
            stone.coord = to.tile.coord;
            return true;
        }
        return false;
    }

    public bool placeHexagon(Hexagon fromHex, Tile to)
    {
        if (Rules.CanBuild(game.board, fromHex.tile, to))
        {
            game.build(fromHex.tile, to);
            Tile newTo = game.board.tiles[to.coord.q, to.coord.r];
            Vector3 toPosition = GetPositionForHexagon(game.board.size, newTo.coord,newTo.level);
            fromHex.transform.localPosition = toPosition;
            fromHex.transform.localScale = new Vector3(scale, scale, scale);
            fromHex.tile = newTo;
            fromHex.SetOriginalMaterial(LevelToMaterial(newTo.level));
            return true;
        }
        return false;
    }

    public bool placeHexagon(Tile from, Tile to)
    {
        Hexagon fromHex = (Hexagon)findHexagonByTile(from);
        if (fromHex != null)
        {
            return placeHexagon(fromHex, to);
        }
        return false;
    }

    internal void MouseOver(Transform element, bool dragging)
    {
        Hexagon hex = element.GetComponent<Hexagon>();
        TileLevel notAllowed = dragging ? TileLevel.HILL : TileLevel.UNDERGROUND;
        if (hex != null && Rules.canMoveTile(game.board, hex.tile.coord) && hex.tile.level != notAllowed)
        {
            hex.Highlight();
        }
    }
}
