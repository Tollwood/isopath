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
    public int size;

    public float xqOffSet = 1.7320508f;
    public float zrOffset = 1.5f;

    internal Board create()
    {
        Board board = new Board(size);

        foreach (Coord coord in board.tiles.Keys)
        {
            Tile tile;
            board.tiles.TryGetValue(coord, out tile);
            if (tile.level == TileLevel.UNDERGROUND)
            {
                addHexagon(coord, GetPositionForHexagon(coord,TileLevel.UNDERGROUND),TileLevel.UNDERGROUND);
                addStone(coord,TileLevel.UNDERGROUND,GetPositionForStone(coord,TileLevel.UNDERGROUND));
            }
            if (tile.level == TileLevel.GROUND)
            {
                addHexagon(coord, GetPositionForHexagon(coord, TileLevel.UNDERGROUND),TileLevel.UNDERGROUND);
                addHexagon(coord, GetPositionForHexagon(coord, TileLevel.GROUND),TileLevel.GROUND);
            }
            if (tile.level == TileLevel.HILL)
            {
                addHexagon(coord, GetPositionForHexagon(coord, TileLevel.UNDERGROUND),TileLevel.UNDERGROUND);
                addHexagon(coord, GetPositionForHexagon(coord, TileLevel.GROUND),TileLevel.GROUND);
                addHexagon(coord, GetPositionForHexagon(coord, TileLevel.HILL),TileLevel.HILL);
                addStone(coord,tile.level, GetPositionForStone(coord,tile.level));
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
        GameObject hexagonGo = Instantiate(hexagonPrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)));
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

        GameObject stoneGo = Instantiate(stonePrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)));
        Stone stone = stoneGo.GetComponent<Stone>();
        stone.SetOriginalMaterial(stoneMaterial);
        stone.hoverMaterial = hoverMaterial;
        stone.coord = coord;
        return stoneGo;
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
        return new Vector3(coord.q * xqOffSet +  coord.r, stoneOffset, zrOffset * coord.r);
    }

    public Vector3 GetPositionForHexagon(Coord coord, TileLevel tileLevel)
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
        return new Vector3(coord.q * xqOffSet +  coord.r, tileLevelOffSet, zrOffset * coord.r);
    }
}
