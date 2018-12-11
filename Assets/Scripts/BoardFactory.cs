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

    public float xqOffSet = 1.7320508f;
    public float zrOffset = 1.5f;

    public Transform uiContainer;

    Board board;

    private void Awake()
    {
        uiContainer = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0))).transform;
        uiContainer.name = "boardElements";
    }

    internal Board create(GameConfig gameConfig)
    {
        
        board = new Board(gameConfig.size, BoardStateModifier.ResetTiles(gameConfig.size));

        foreach (Tile tile in board.tiles)
        {
            if(tile == null){
                continue;
            }
            if (tile.level == TileLevel.UNDERGROUND)
            {
                addHexagon(tile.coord, GetPositionForHexagon(tile.coord,TileLevel.UNDERGROUND),TileLevel.UNDERGROUND);
                addStone(tile.coord,TileLevel.UNDERGROUND,GetPositionForStone(tile.coord,TileLevel.UNDERGROUND));
            }
            if (tile.level == TileLevel.GROUND)
            {
                addHexagon(tile.coord, GetPositionForHexagon(tile.coord, TileLevel.UNDERGROUND),TileLevel.UNDERGROUND);
                addHexagon(tile.coord, GetPositionForHexagon(tile.coord, TileLevel.GROUND),TileLevel.GROUND);
            }
            if (tile.level == TileLevel.HILL)
            {
                addHexagon(tile.coord, GetPositionForHexagon(tile.coord, TileLevel.UNDERGROUND),TileLevel.UNDERGROUND);
                addHexagon(tile.coord, GetPositionForHexagon(tile.coord, TileLevel.GROUND),TileLevel.GROUND);
                addHexagon(tile.coord, GetPositionForHexagon(tile.coord, TileLevel.HILL),TileLevel.HILL);
                addStone(tile.coord,tile.level, GetPositionForStone(tile.coord,tile.level));
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

    private GameObject addHexagon(Coord coord, Vector3 position, TileLevel tileLevel)
    {
        GameObject hexagonGo = Instantiate(hexagonPrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)),uiContainer);
        Hexagon hexagon = hexagonGo.GetComponent<Hexagon>();

        hexagon.coord = coord;
        hexagon.SetOriginalMaterial(LevelToMaterial(tileLevel));
        hexagon.hoverMaterial = hoverMaterial;
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

        GameObject stoneGo = Instantiate(stonePrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)), uiContainer);
        Stone stone = stoneGo.GetComponent<Stone>();
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
        throw new Exception("Draggable for tile " + tile + " not found");
    }

    public Vector3 GetPositionForStone(Coord coord, TileLevel tileLevel){
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
        return new Vector3( xqOffSet * (coord.q +  coord.r - 4.279f)- coord.r, stoneOffset, zrOffset * (coord.r -3) );
    }

    public Vector3 GetPositionForHexagon(Coord coord, TileLevel tileLevel)
    {
        return new Vector3( xqOffSet  * (coord.q +  coord.r -4.279f) - coord.r, GetTileLevelOffset(tileLevel), zrOffset * (coord.r - 3 ) );
    }

    public Board Restart(GameConfig gameConfig){
        foreach (Transform child in uiContainer)
        {
            Destroy(child.gameObject);
        }
        return create(gameConfig);
    }

    public Draggable findByTile(Tile tile){
        foreach (Transform child in uiContainer)
        {
            Hexagon hexagon = child.GetComponent<Hexagon>();
            if(hexagon != null && hexagon.GetCoord().Equals(tile.coord) && child.position.y == GetTileLevelOffset(tile.level)){
                return hexagon;
            }
        }
        throw new Exception("Draggable for tile " + tile + " not found");
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
}
