
using UnityEngine;

public class Stone : MonoBehaviour, Draggable{

    public  Material hoverMaterial;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;
    public Coord coord;
    private Game game;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        originalMaterial = meshRenderer.material;
        game = FindObjectOfType<Game>();
    }

    internal void OnMouseEnter()
    {
        if(Rules.canMoveStone(game.board,coord)){
            meshRenderer.material = hoverMaterial;
        }
    }

    internal void OnMouseExit()
    {
        meshRenderer.material = originalMaterial;
    }

    public void SetOriginalMaterial(Material originalMaterial){
        this.originalMaterial = originalMaterial;
        meshRenderer.material = originalMaterial;
    }

    public bool isDraggable()
    {
        return Rules.canMoveStone(game.board,coord);
    }

    public Tile getTile(){
        return game.board.tiles[coord.q,coord.r];
    }

    public Coord GetCoord()
    {
        return coord;
    }
}
