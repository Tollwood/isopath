
using UnityEngine;

public class Stone : MonoBehaviour, Draggable{

    public  Material hoverMaterial;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;
    public Coord coord;
    private Board board;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        originalMaterial = meshRenderer.material;
        board = FindObjectOfType<Game>().board;
    }

    internal void OnMouseEnter()
    {
        if(Rules.canMoveStone(board,coord)){
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
        return Rules.canMoveStone(board,coord);
    }

    public Tile getTile(){
        return board.coordToTile(coord);
    }
}
